using System.Collections.Generic;
using System;
using System.Linq;
using UnecontLogConverter.ViewModels;
using System.Threading.Tasks;
using UnecontLogConverter.Entities;
using UnecontLogConverter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UnecontLogConverter.Infrastructure;
using System.IO;
using AutoMapper;
using System.Net.Http;
using UnecontLogConverter.Helpers;
using System.Security.Policy;

namespace UnecontLogConverter.Services
{
    public class LogService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAppSettingsService _appSettingsService;

        public LogService(AppDbContext context, IAppSettingsService appSettingsService, IMapper mapper)
        {
            _context = context;
            _appSettingsService = appSettingsService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> TransformLog(string parameter, bool saveToFile = false, bool saveToDatabase = false)
        {
            try
            {
                List<string> inputLog = null;
                IEnumerable<string> result = null;

                if (Validations.IsValidUrl(parameter))
                {
                    try
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var content = await httpClient.GetStringAsync(parameter);

                            inputLog = new List<string>(content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!ex.Data.Contains("origin"))
                            ex.Data.Add("origin", "LogService.TransformLog.GetUrlContent");
                        throw ex;
                    }
                }
                else if (Validations.IsValidGuid(parameter))
                {
                    var log = await GetLogAsync(parameter);

                    if (log != null)
                        inputLog = log.Content;
                    else
                        return null;
                }
                else
                    throw new InvalidDataException(parameter);

                if (inputLog != null && inputLog.Any())
                {
                    if (saveToFile)
                    {
                        var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                        if (!Directory.Exists(logsDirectory))
                        {
                            Directory.CreateDirectory(logsDirectory);
                        }

                        var path = Path.Combine(logsDirectory, $"log_{System.Guid.NewGuid()}.txt");

                        System.IO.File.WriteAllLines(path, FormatAgoraLogs(ConvertLog(inputLog)));
                        result = new List<string> { path };
                    }

                    if (saveToDatabase)
                    {
                        var log = new Log
                        {
                            Content = inputLog
                        };
                        _context.Logs.Add(log);

                        var logTransformed = _mapper.Map<LogTransformed>(log);
                        logTransformed.TransformedContent = Convert(inputLog);
                        logTransformed.Version = _appSettingsService.GetVersion();
                        logTransformed.Fields = "provider http-method status-code uri-path time-taken response-size cache-status";

                        _context.LogsTransformed.Add(logTransformed);

                        await _context.SaveChangesAsync();
                    }
                }
                else
                    throw new InvalidDataException(parameter);


                if (!saveToFile)
                    result = FormatAgoraLogs(ConvertLog(inputLog));

                return result;
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("origin"))
                    ex.Data.Add("origin", "LogService.TransformLog");
                throw ex;
            }
        }

        public async Task<PaginatedResult<LogListViewModel>> GetAllLogsAsync(DateTime? startDate, DateTime? endDate, bool orderByDateAsc, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var logs = _context.Logs.AsQueryable();

                if (startDate.HasValue)
                    logs = logs.Where(log => log.CreatedAt >= startDate.Value);

                if (endDate.HasValue)
                    logs = logs.Where(log => log.CreatedAt <= endDate.Value);

                logs = orderByDateAsc
                    ? logs.OrderBy(log => log.CreatedAt)
                    : logs.OrderByDescending(log => log.CreatedAt);

                logs = logs
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var result = new PaginatedResult<LogListViewModel>
                {
                    Data = _mapper.Map<IEnumerable<LogListViewModel>>(logs.ToList()),
                    TotalRecords = logs.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return result;
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("origin"))
                    ex.Data.Add("origin", "LogService.GetAllLogsAsync");
                throw ex;
            }
        }

        public async Task<PaginatedResult<LogListViewModel>> GetAllTransformedLogsAsync(DateTime? startDate, DateTime? endDate, bool orderByDateAsc, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var logs = _context.LogsTransformed.AsQueryable();

                if (startDate.HasValue)
                    logs = logs.Where(log => log.CreatedAt >= startDate.Value);

                if (endDate.HasValue)
                    logs = logs.Where(log => log.CreatedAt <= endDate.Value);

                logs = orderByDateAsc
                    ? logs.OrderBy(log => log.CreatedAt)
                    : logs.OrderByDescending(log => log.CreatedAt);

                logs = logs
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var result = new PaginatedResult<LogListViewModel>
                {
                    Data = _mapper.Map<IEnumerable<LogListViewModel>>(logs.ToList()),
                    TotalRecords = logs.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return result;
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("origin"))
                    ex.Data.Add("origin", "LogService.GetAllTransformedLogsAsync");
                throw ex;
            }
        }

        public async Task<LogFullViewModel> GetLogWithTransformationAsync(string logId)
        {
            try
            {
                var log = await _context.Logs
                .Include(l => l.TransformedLog)
                .FirstOrDefaultAsync(l => l.Id == logId);

                if (log == null)
                    return null;

                return new LogFullViewModel
                {
                    Log = log.Content,
                    TransformedLog = FormatAgoraLogs(log.TransformedLog).ToList(),
                };
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("origin"))
                    ex.Data.Add("origin", "LogService.GetLogWithTransformationAsync");
                throw ex;
            }
        }

        public async Task<LogViewModel> GetLogAsync(string logId)
        {
            try
            {
                var log = await _context.Logs
                .FirstOrDefaultAsync(l => l.Id == logId);

                if (log == null)
                    return null;

                return _mapper.Map<LogViewModel>(log);
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("origin"))
                    ex.Data.Add("origin", "LogService.GetLogAsync");
                throw ex;
            }
        }

        public async Task<LogTransformedViewModel> GetLogTransformationAsync(string logId)
        {
            try
            {
                var log = await _context.LogsTransformed
                .FirstOrDefaultAsync(l => l.Id == logId);

                if (log == null)
                    return null;

                return _mapper.Map<LogTransformedViewModel>(log);
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("origin"))
                    ex.Data.Add("origin", "LogService.GetLogTransformationAsync");
                throw ex;
            }
        }

        private List<string> Convert(List<string> inputLog)
        {
            try
            {
                var TransformedContent = new List<string>();

                foreach (var line in inputLog)
                {
                    var parts = line.Split('|');

                    TransformedContent.Add($"{_appSettingsService.GetProvider()} {parts[3].Split(' ')[0].Trim('"')} {int.Parse(parts[1])} {parts[3].Split(' ')[1]} {(int)Math.Round(decimal.Parse(parts[4]))} {int.Parse(parts[0])} {TransformCacheStatus(parts[2])}");
                }

                return TransformedContent;
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("origin"))
                    ex.Data.Add("origin", "LogService.Convert");
                throw ex;
            }
        }

        private string TransformCacheStatus(string cacheStatus)
        {
            return cacheStatus == "INVALIDATE" ? "REFRESH_HIT" : cacheStatus;
        }

        public static IEnumerable<string> FormatAgoraLogs(LogTransformed logs)
        {
            var header = new[]
            {
            $"#Version: {logs.Version}",
            $"#Date: {logs.CreatedAt:dd/MM/yyyy HH:mm:ss}",
            $"#Fields: {logs.Fields}"
        };
            var body = logs.TransformedContent;

            return header.Concat(body);
        }

        public IEnumerable<LogEntry> ConvertLog(List<string> inputLog)
        {
            return inputLog.Select(line =>
            {
                var parts = line.Split('|');
                return new LogEntry
                {
                    HttpMethod = parts[3].Split(' ')[0].Trim('"'),
                    StatusCode = int.Parse(parts[1]),
                    UriPath = parts[3].Split(' ')[1],
                    TimeTaken = (int)Math.Round(decimal.Parse(parts[4])),
                    ResponseSize = int.Parse(parts[0]),
                    CacheStatus = TransformCacheStatus(parts[2])
                };
            });
        }

        private IEnumerable<string> FormatAgoraLogs(IEnumerable<LogEntry> logs)
        {
            var header = new[]
            {
            $"#Version: {_appSettingsService.GetVersion()}",
            $"#Date: {System.DateTime.Now:dd/MM/yyyy HH:mm:ss}",
            "#Fields: provider http-method status-code uri-path time-taken response-size cache-status"
        };
            var body = logs.Select(log =>
                $"{log.Provider} {log.HttpMethod} {log.StatusCode} {log.UriPath} {log.TimeTaken} {log.ResponseSize} {log.CacheStatus}");
            return header.Concat(body);
        }
    }
}