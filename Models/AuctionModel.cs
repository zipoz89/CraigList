using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CraigList.Models
{
    public class AuctionModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Price { get; set; }

        public string Picture { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(700)]
        public string Description { get; set; }
        public DateTime EndDate { get; set; }

    }
}
