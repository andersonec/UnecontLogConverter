using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System;

namespace UnecontLogConverter.ViewModels
{
    public class LogViewModel
    {
        public string Id { get; set; }

        public List<string> Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
