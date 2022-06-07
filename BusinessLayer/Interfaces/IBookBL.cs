using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
   public interface IBookBL
    {
        public BookModel AddBook(BookModel book);
        public bool UpdateBooks(int BookId, BookModel updateBook);
        public List<BookModel> GetBookByBookId(int BookId);
        public List<BookModel> GetAllBooks();
        public bool DeleteBook(int BookId);
    }
}
