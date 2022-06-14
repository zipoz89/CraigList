using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CraigList.Models
{
    public class CreateAuctionModel
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(700)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Picture { get; set; }
    }
}
