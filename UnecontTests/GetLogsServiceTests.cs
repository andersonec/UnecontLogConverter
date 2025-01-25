using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnecontLogConverter.Entities;
using UnecontLogConverter.Infrastructure;
using UnecontLogConverter.Infrastructure.Data;
using UnecontLogConverter.Services;
using Xunit;

namespace UnecontTests
{
    public class GetLogsServiceTests : _TestsBase
    {
        private readonly LogService _logService;
        private readonly AppDbContext _dbContext;
        private readonly IAppSettingsService _mockAppSettingsService;

        public GetLogsServiceTests()
        {
            _logService = CreateLogService();
            _dbContext = CreateDbContext();
            _mockAppSettingsService = CreateAppSettings();
        }

        [Fact]
        public async Task GetLog_ValidGuid_FindsLog()
        {
            // Arrange
            var mockGuid = Guid.NewGuid().ToString();

            _dbContext.Logs.Add(
                new Log
                {
                    Id = mockGuid,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ContentSerialized = $"[\"MINHA CDN GET 200 /robots.txt 100 312 HIT\",\"MINHA CDN POST 200 /myImages 319 101 MISS\",\"MINHA CDN GET 404 /not-found 143 199 MISS\",\"MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT\"]"
                });
            _dbContext.SaveChanges();


            // Act
            var result = await _logService.GetLogAsync(mockGuid);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetLog_ValidGuid_NotFindsLog()
        {
            // Act
            var result = await _logService.GetLogAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetLog_InvalidGuid_FindsLog()
        {
            // Act
            var result = await _logService.GetLogAsync("invalid_guid");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetLogTransformationAsync_ValidGuid_FindsLog()
        {
            // Arrange
            var mockGuid = Guid.NewGuid().ToString();

            _dbContext.LogsTransformed.Add(
                new LogTransformed
                {
                    Id = mockGuid,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    Fields = "provider http-method status-code uri-path time-taken response-size cache-status",
                    LogId = Guid.NewGuid().ToString(),
                    Version = _mockAppSettingsService.GetVersion(),
                    TransformedContentSerialized = $"[\"MINHA CDN GET 200 /robots.txt 100 312 HIT\",\"MINHA CDN GET 404 /not-found 143 199 MISS\",\"MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT\"]"
                });
            _dbContext.SaveChanges();


            // Act
            var result = await _logService.GetLogTransformationAsync(mockGuid);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetLogTransformationAsync_ValidGuid_NotFindsLog()
        {
            // Act
            var result = await _logService.GetLogTransformationAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetLogTransformationAsync_InvalidGuid_FindsLog()
        {
            // Act
            var result = await _logService.GetLogTransformationAsync("invalid_guid");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetLogWithTransformationAsync_ValidGuid_FindsLog()
        {
            var LogMockGuid = Guid.NewGuid().ToString();

            _dbContext.Logs.Add(
                new Log
                {
                    Id = LogMockGuid,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ContentSerialized = $"[\"MINHA CDN GET 200 /robots.txt 100 312 HIT\",\"MINHA CDN POST 200 /myImages 319 101 MISS\",\"MINHA CDN GET 404 /not-found 143 199 MISS\",\"MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT\"]"
                });
            _dbContext.SaveChanges();


            // Arrange
            var LogTransformedMockGuid = Guid.NewGuid().ToString();

            _dbContext.LogsTransformed.Add(
                new LogTransformed
                {
                    Id = LogTransformedMockGuid,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    Fields = "provider http-method status-code uri-path time-taken response-size cache-status",
                    LogId = LogMockGuid,
                    Version = _mockAppSettingsService.GetVersion(),
                    TransformedContentSerialized = $"[\"MINHA CDN GET 200 /robots.txt 100 312 HIT\",\"MINHA CDN GET 404 /not-found 143 199 MISS\",\"MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT\"]"
                });
            _dbContext.SaveChanges();


            // Act
            var result = await _logService.GetLogWithTransformationAsync(LogMockGuid);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetLogWithTransformationAsync_ValidGuid_NotFindsLog()
        {
            // Act
            var result = await _logService.GetLogWithTransformationAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetLogWithTransformationAsync_InvalidGuid_FindsLog()
        {
            // Act
            var result = await _logService.GetLogWithTransformationAsync("invalid_guid");

            // Assert
            Assert.Null(result);
        }
    }
}
