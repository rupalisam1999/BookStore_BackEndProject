using BusinessLayer.Interfaces;
using DatabaseLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        IBookBL bookBL;
        public BookController(IBookBL bookBL)
        {
            this.bookBL = bookBL;
        }
        [HttpPost("AddBook")]
        public IActionResult AddBook(BookModel book)
        {
            try
            {
                var bookData = this.bookBL.AddBook(book);
                if (bookData != null)
                {
                    return this.Ok(new { success = true, message = "Book Added  successfull", res = bookData });
                }
                return this.Ok(new { success = true, message = "Book Already Exists" });
            }
            catch (Exception ex)
            {
                return this.Ok(new { success = false, message = ex.Message });
            }

        }

        [HttpPost("UpdateBook/{BookId}")]
        public IActionResult UpdateBooks(int BookId, BookModel updateBook)
        {
            try
            {
                var result = this.bookBL.UpdateBooks(BookId, updateBook);
                if (result==true)
                {
                    return this.Ok(new { success = true, message = $"Book updated Successfully ", response = updateBook });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = result });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        [HttpGet("getbookbyBookId/{BookId}")]
        public IActionResult GetBookByBookId(int BookId)
        {
            try
            {
                var result = this.bookBL.GetBookByBookId(BookId);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Books is  Displayed Successfully by BookId ", response = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = $"Book id not exists " });
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        [HttpGet("getallbook")]
        public IActionResult GetAllBooks()
        {
            try
            {
                var result = this.bookBL.GetAllBooks();
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"All Books Displayed Successfully ", response = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = $"Books are not there " });
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [HttpDelete("deletebook/{BookId}")]
        public IActionResult DeleteBook(int BookId)
        {
            try
            {
                var result = this.bookBL.DeleteBook(BookId);
                if (result==true)
                {
                    return this.Ok(new { success = true, message = $"Book deleted Successfully ", response = BookId });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = result });
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
