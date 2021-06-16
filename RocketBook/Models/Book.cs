using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RocketBook.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [Display(Name = "Book Title")]
        public string Title { get; set; }

        public string Description { get; set; }
        public string Author { get; set; }
        public string PublishDate { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Please enter correct price.")]
        public double Price { get; set; }

        public string image { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }
}
