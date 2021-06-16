using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketBook.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string DisplayCategory { get; set; }
    }
}
