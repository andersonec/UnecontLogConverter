using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UnecontLogConverter.Entities;
using UnecontLogConverter.Infrastructure.Data;
using UnecontLogConverter.Infrastructure;
using UnecontLogConverter.Services;

namespace UnecontTests
{
    public class _TestsBase
    {
        private readonly Mock<IAppSettingsService> _mockAppSettingsService;
        private readonly AppDbContext _dbContext;
        private readonly LogService _logService;
        private readonly Mapper _mapper;

        public _TestsBase()
        {
            // Configura o banco em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);

            // Mock do IAppSettingsService
            _mockAppSettingsService = new Mock<IAppSettingsService>();
            _mockAppSettingsService.Setup(x => x.GetProvider()).Returns("Tests");
            _mockAppSettingsService.Setup(x => x.GetVersion()).Returns("1.0_test");

            _mapper = new Mapper(Helpers.mapperConfiguration);

            // Instancia o serviço com os mocks e o DbContext real
            _logService = new LogService(_dbContext, _mockAppSettingsService.Object, _mapper);

            // Popula o banco com dados de teste
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _dbContext.Logs.AddRange(new[]
            {
                new Log {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ContentSerialized = $"[\"MINHA CDN GET 200 /robots.txt 100 312 HIT\",\"MINHA CDN POST 200 /myImages 319 101 MISS\",\"MINHA CDN GET 404 /not-found 143 199 MISS\",\"MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT\"]"
                },

                new Log {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now.AddDays(-1),
                    ContentSerialized = $"[\"MINHA CDN POST 200 /myImages 319 101 MISS\",\"MINHA CDN GET 404 /not-found 143 199 MISS\",\"MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT\"]"
                },

                new Log {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now ,
                    ContentSerialized = $"[\"MINHA CDN GET 200 /robots.txt 100 312 HIT\",\"MINHA CDN POST 200 /myImages 319 101 MISS\",\"MINHA CDN GET 404 /not-found 143 199 MISS\"]"
                }
            });

            _dbContext.LogsTransformed.AddRange(new[]
            {
                new LogTransformed {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now.AddDays(-2),
                    Fields = "provider http-method status-code uri-path time-taken response-size cache-status",
                    LogId = Guid.NewGuid().ToString(),
                    Version = _mockAppSettingsService.Object.GetVersion(),
                    TransformedContentSerialized = $"[\"MINHA CDN GET 200 /robots.txt 100 312 HIT\",\"MINHA CDN GET 404 /not-found 143 199 MISS\",\"MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT\"]"
                },

                new LogTransformed {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Fields = "provider http-method status-code uri-path time-taken response-size cache-status",
                    LogId = Guid.NewGuid().ToString(),
                    Version = _mockAppSettingsService.Object.GetVersion(),
                    TransformedContentSerialized = $"[\"MINHA CDN GET 200 /robots.txt 100 312 HIT\",\"MINHA CDN POST 200 /myImages 319 101 MISS\",\"MINHA CDN GET 404 /not-found 143 199 MISS\",\"MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT\"]"
                },

                new LogTransformed {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now ,
                    Fields = "provider http-method status-code uri-path time-taken response-size cache-status",
                    LogId = Guid.NewGuid().ToString(),
                    Version = _mockAppSettingsService.Object.GetVersion(),
                    TransformedContentSerialized = $"[\"MINHA CDN POST 200 /myImages 319 101 MISS\",\"MINHA CDN GET 404 /not-found 143 199 MISS\",\"MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT\"]"
                }
            });

            _dbContext.SaveChanges();
        }

        protected LogService CreateLogService()
        {
            return _logService;
        }

        protected AppDbContext CreateDbContext()
        {
            return _dbContext;
        }

        protected IAppSettingsService CreateAppSettings()
        {
            return _mockAppSettingsService.Object;
        }
    }
}
