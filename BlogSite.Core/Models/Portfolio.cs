using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSite.Core.Models
{
    public class Portfolio : BaseEntity
    {
        public string Title { get; set; }
        public string? ImagePath { get; set; }  // <-- şəkilin yolu
        [NotMapped]
        public IFormFile? ImageFile { get; set; }  // <-- HTTP POST üçün
        public string ProjectUrl { get; set; }
    }
}
