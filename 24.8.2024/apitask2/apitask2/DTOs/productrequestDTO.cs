﻿using apitask2.Models;

namespace apitask2.DTOs
{
    public class productrequestDTO
    {

        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public string? Price { get; set; }

        public int? CategoryId { get; set; }

        public IFormFile? ProductImage { get; set; }
    }
}
