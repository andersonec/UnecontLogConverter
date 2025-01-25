using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace UnecontLogConverter.Entities
{
    public class Log
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ContentSerialized { get; set; } // Para armazenar no banco de dados

        [NotMapped]
        public List<string> Content
        {
            get => string.IsNullOrEmpty(ContentSerialized)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(ContentSerialized);
            set => ContentSerialized = JsonSerializer.Serialize(value, new JsonSerializerOptions());
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual LogTransformed TransformedLog { get; set; }
    }
}