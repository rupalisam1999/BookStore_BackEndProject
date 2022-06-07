using DatabaseLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Services
{
   public class BookRL:IBookRL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection sqlConnection;
        string ConnectionString;
        public BookRL(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("BookStore_DB");
        }

        public BookModel AddBook(BookModel book)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (connection)
                {
                    SqlCommand cmd = new SqlCommand("spAddBook", connection);


                    cmd.CommandType = CommandType.StoredProcedure;
                   

                    
                    cmd.Parameters.AddWithValue("@BookName", book.BookName);
                    cmd.Parameters.AddWithValue("@AuthorName", book.AuthorName);
                    cmd.Parameters.AddWithValue("@TotalRating", book.TotalRating);
                    cmd.Parameters.AddWithValue("@RatingCount", book.RatingCount);
                    cmd.Parameters.AddWithValue("@OriginalPrice", book.OriginalPrice);
                    cmd.Parameters.AddWithValue("@DiscountPrice", book.DiscountPrice);
                    cmd.Parameters.AddWithValue("@BookDetails", book.BookDetails);
                    cmd.Parameters.AddWithValue("@BookImage", book.BookImage);
                    cmd.Parameters.AddWithValue("@BookQuantity", book.BookQuantity);


                    connection.Open();
                    cmd.ExecuteNonQuery();

                    return book;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool UpdateBooks(int BookId, BookModel updateBook)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (connection)
                {
                    SqlCommand cmd = new SqlCommand("spUpdateBook", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", updateBook.BookId);
                    cmd.Parameters.AddWithValue("@BookName", updateBook.BookName);
                    cmd.Parameters.AddWithValue("@AuthorName", updateBook.AuthorName);
                    cmd.Parameters.AddWithValue("@TotalRating", updateBook.TotalRating);
                    cmd.Parameters.AddWithValue("@RatingCount", updateBook.RatingCount);
                    cmd.Parameters.AddWithValue("@OriginalPrice", updateBook.OriginalPrice);
                    cmd.Parameters.AddWithValue("@DiscountPrice", updateBook.DiscountPrice);
                    cmd.Parameters.AddWithValue("@BookDetails", updateBook.BookDetails);
                    cmd.Parameters.AddWithValue("@BookImage", updateBook.BookImage);
                    cmd.Parameters.AddWithValue("@BookQuantity", updateBook.BookQuantity);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    return true;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BookModel> GetBookByBookId(int BookId)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (sqlConnection)
                {
                    List<BookModel> book = new List<BookModel>();
                    SqlCommand cmd = new SqlCommand("spGetBookByBookId", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", BookId);
                    connection.Open();
                    SqlDataReader fetch = cmd.ExecuteReader();
                    if (fetch.HasRows)
                    {
                        while (fetch.Read())
                        {
                            BookModel data = new BookModel();

                            data.BookName = fetch["BookName"].ToString();
                            data.AuthorName = fetch["AuthorName"].ToString();
                            data.TotalRating = Convert.ToInt32(fetch["TotalRating"]);
                            data.RatingCount = Convert.ToInt32(fetch["RatingCount"]);
                            data.OriginalPrice = Convert.ToInt32(fetch["OriginalPrice"]);
                            data.DiscountPrice = Convert.ToInt32(fetch["DiscountPrice"]);
                            data.BookDetails = fetch["BookDetails"].ToString();
                            data.BookImage = fetch["BookImage"].ToString();
                            data.BookQuantity = Convert.ToInt32(fetch["BookQuantity"]);
                            book.Add(data);
                        }
                        return book;
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
               connection.Close();
            }
        }
        public List<BookModel> GetAllBooks()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (connection)
                {
                    List<BookModel> book = new List<BookModel>();
                    SqlCommand cmd = new SqlCommand("spGetAllBook", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    SqlDataReader fetch = cmd.ExecuteReader();
                    if (fetch.HasRows)
                    {
                        while (fetch.Read())
                        {
                            BookModel data = new BookModel();
                            data.BookName = fetch["BookName"].ToString();
                            data.AuthorName = fetch["AuthorName"].ToString();
                            data.TotalRating = Convert.ToInt32(fetch["TotalRating"]);
                            data.RatingCount = Convert.ToInt32(fetch["RatingCount"]);
                            data.OriginalPrice = Convert.ToInt32(fetch["OriginalPrice"]);
                            data.DiscountPrice = Convert.ToInt32(fetch["DiscountPrice"]);
                            data.BookDetails = fetch["BookDetails"].ToString();
                            data.BookImage = fetch["BookImage"].ToString();
                            data.BookQuantity = Convert.ToInt32(fetch["BookQuantity"]);
                            book.Add(data);
                        }
                        return book;
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }

        }
        public bool DeleteBook(int BookId)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
                using (connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteBook", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", BookId);
                    connection.Open();
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return true;


                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }

        }

    }
}

