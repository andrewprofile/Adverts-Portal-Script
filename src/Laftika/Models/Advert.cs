﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Laftika.Models
{
    public class Advert
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public string PhoneNumber { get; set; }
    }
}
