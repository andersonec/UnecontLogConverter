using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnecontLogConverter.Entities;

namespace UnecontLogConverter.Infrastructure.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate();

            // Caso existam informações importantes que devem ser salvas no banco, basta tratar aqui

            // Exemplo:

            if (!context.Logs.Any())
            {
                var log = new Log
                {
                    Content = null,
                    CreatedAt = DateTime.Now,
                };

                // Em casos reais, a linha abaixo não estaria comentada
                // context.Logs.Add(log);
            }

            context.SaveChanges();
        }
    }
}
