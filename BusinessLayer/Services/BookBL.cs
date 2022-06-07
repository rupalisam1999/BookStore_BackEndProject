using BusinessLayer.Interfaces;
using DatabaseLayer;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class BookBL : IBookBL
    {
        IBookRL bookRL;
        public BookBL(IBookRL bookRL)
        {
            this.bookRL = bookRL;
        }
        public BookModel AddBook(BookModel book)
        {
            try
            {
                return this.bookRL.AddBook(book);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        
        public bool UpdateBooks(int BookId, BookModel updateBook)
        {
            try
            {
                return this.bookRL.UpdateBooks(BookId,updateBook);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public List<BookModel> GetBookByBookId(int BookId)
        {
            try
            {
                return this.bookRL.GetBookByBookId(BookId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public List<BookModel> GetAllBooks()
        {
            try
            {
                return this.bookRL.GetAllBooks();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public bool DeleteBook(int BookId)
        {
            try
            {
                return this.bookRL.DeleteBook(BookId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
