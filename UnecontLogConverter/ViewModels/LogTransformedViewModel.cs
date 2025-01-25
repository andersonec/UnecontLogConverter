using System.Collections.Generic;
using System;

namespace UnecontLogConverter.ViewModels
{
    public class LogTransformedViewModel
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Version { get; set; }

        public string Fields { get; set; }

        public List<string> TransformedContent { get; set; }
    }
}
