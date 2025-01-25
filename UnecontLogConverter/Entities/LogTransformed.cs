using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace UnecontLogConverter.Entities
{
    public class LogTransformed
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Version { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Fields { get; set; }

        public string TransformedContentSerialized { get; set; } // Para armazenar no banco de dados

        [NotMapped]
        public List<string> TransformedContent
        {
            get => string.IsNullOrEmpty(TransformedContentSerialized)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(TransformedContentSerialized);
            set => TransformedContentSerialized = JsonSerializer.Serialize(value, new JsonSerializerOptions());
        }

        [Required]
        public string LogId { get; set; }

        [ForeignKey("LogId")]
        public virtual Log OriginalLog { get; set; }
    }
}