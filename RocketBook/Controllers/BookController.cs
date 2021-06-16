using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketBook.data;
using RocketBook.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using RocketBook.Models.ViewModels;

namespace RocketBook.Controllers
{
    public class BookController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Book> objList = _db.Book.Include(u => u.Category);

            return View(objList);
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {

            BookVM bookVM = new BookVM()
            {
                Book = new Book(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //this is for create
                return View(bookVM);
            }
            else
            {
                bookVM.Book = _db.Book.Find(id);
                if (bookVM.Book == null)
                {
                    return NotFound();
                }
                return View(bookVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM bookVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (bookVM.Book.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + "\\images\\Books";
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    bookVM.Book.image = fileName + extension;

                    _db.Book.Add(bookVM.Book);
                }
                else
                {
                    //updating
                    var objFromDb = _db.Book.AsNoTracking().FirstOrDefault(u => u.Id == bookVM.Book.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + "\\images\\Books";
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        bookVM.Book.image = fileName + extension;
                    }
                    else
                    {
                        bookVM.Book.image = objFromDb.image;
                    }
                    _db.Book.Update(bookVM.Book);
                }


                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            bookVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(bookVM);

        }


        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Book book = _db.Book.Include(u => u.Category).FirstOrDefault(u => u.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Book.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + "\\images\\Books";
            var oldFile = Path.Combine(upload, obj.image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _db.Book.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }


    }
}
