using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnecontLogConverter.Services;
using Xunit;

namespace UnecontTests
{
    public class GetAllLogsServiceTests : _TestsBase
    {
        private readonly LogService _logService;

        public GetAllLogsServiceTests()
        {
            _logService = CreateLogService();
        }

        [Fact]
        public async Task GetAllLogsAsync_ReturnsPaginatedLogs()
        {

            var result = await _logService.GetAllLogsAsync(
                startDate: new DateTime(2025, 1, 1),
                endDate: new DateTime(2025, 1, 30),
                orderByDateAsc: true,
                pageNumber: 1,
                pageSize: 2
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count()); // Página 1 com 2 itens
            Assert.Equal(2, result.TotalRecords); // Verifica total de registros
        }

        [Fact]
        public async Task GetAllLogsAsync_ReturnsEmpty_WhenNoLogsMatchFilters()
        {
            var result = await _logService.GetAllLogsAsync(
                startDate: new DateTime(2023, 1, 2),
                endDate: new DateTime(2023, 1, 3),
                orderByDateAsc: true,
                pageNumber: 1,
                pageSize: 2
            );

            Assert.NotNull(result);
            Assert.Empty(result.Data); // Nenhum log encontrado
            Assert.Equal(0, result.TotalRecords); // Total de registros é zero
        }


        [Fact]
        public async Task GetAllTransformedLogsAsync_ReturnsPaginatedLogs()
        {
            var result = await _logService.GetAllTransformedLogsAsync(
                startDate: new DateTime(2025, 1, 1),
                endDate: new DateTime(2025, 1, 30),
                orderByDateAsc: true,
                pageNumber: 1,
                pageSize: 2
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count()); // Página 1 com 2 itens
            Assert.Equal(2, result.TotalRecords); // Verifica total de registros
        }

        [Fact]
        public async Task GetAllTransformedLogsAsync_ReturnsEmpty_WhenNoLogsMatchFilters()
        {
            var result = await _logService.GetAllTransformedLogsAsync(
                startDate: new DateTime(2023, 1, 2),
                endDate: new DateTime(2023, 1, 3),
                orderByDateAsc: true,
                pageNumber: 1,
                pageSize: 2
            );

            Assert.NotNull(result);
            Assert.Empty(result.Data); // Nenhum log encontrado
            Assert.Equal(0, result.TotalRecords); // Total de registros é zero
        }
    }
}
