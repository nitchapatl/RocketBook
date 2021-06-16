using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketBook.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Book = new Book();
        }

        public Book Book { get; set; }
    }
}
