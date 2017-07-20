using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecwi.DAL.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [StringLength(50)]
        public string AuthorName { get; set; }
    }
}
