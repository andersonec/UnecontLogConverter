using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using UnecontLogConverter.Entities;
using UnecontLogConverter.Infrastructure;
using UnecontLogConverter.Infrastructure.Data;
using UnecontLogConverter.Services;
using Xunit;

namespace UnecontTests
{
    public partial class LogServiceTests : _TestsBase
    {
        private readonly LogService _logService;
        private readonly AppDbContext _dbContext;

        public LogServiceTests()
        {
            _logService = CreateLogService();
            _dbContext = CreateDbContext();
        }

        [Fact]
        public async Task TransformLog_ValidUrl_ReturnsFormattedLogs()
        {
            // Arrange
            var mockUrl = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";

            // Act
            var result = await _logService.TransformLog(mockUrl);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Count());
        }

        [Fact]
        public async Task TransformLog_ValidGuid_FindsLogAndReturnsFormattedLogs()
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
            var result = await _logService.TransformLog(mockGuid);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Count());
        }

        [Fact]
        public async Task TransformLog_InvalidParameter_ThrowsInvalidDataException()
        {
            // Arrange
            var invalidParameter = "invalid_data";

            // Act & Assert
            await Assert.ThrowsAsync<InvalidDataException>(() => _logService.TransformLog(invalidParameter));
        }

        [Fact]
        public async Task TransformLog_SaveToFile_CreatesLogFile()
        {
            // Act
            var result = await _logService.TransformLog("https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt", saveToFile: true);

            // Assert
            Assert.NotNull(result);
            Assert.True(File.Exists(result.First()));

            // Cleanup
            File.Delete(result.First());
        }

        [Fact]
        public async Task TransformLog_SaveToDatabase_SavesTransformedLogs()
        {
            // Act
            var result = await _logService.TransformLog("https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt", saveToDatabase: true);

            // Assert
            _dbContext.Logs.Any();
        }
    }
}
