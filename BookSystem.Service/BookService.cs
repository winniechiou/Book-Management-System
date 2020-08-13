using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSystem.Service
{
    public class BookService:IBookService
    {
        private BookSystem.Dao.IBookDao bookDao { get; set; }

        public void InsertBook(BookSystem.Model.Books book)
        {
            bookDao.InsertBook(book);
        }

        public List<BookSystem.Model.BookSearchArg> GetBookByCondtioin(BookSystem.Model.BookSearchArg arg)
        {
            return bookDao.GetBookByCondtioin(arg);
        }

        public String findBookKeeper(int bookID)
        {
            return bookDao.findBookKeeper(bookID);
        }

        public String findBook(int bookID)
        {
            return bookDao.findBook(bookID);
        }

        public void DeleteBookById(int BookID)
        {
            bookDao.DeleteBookById(BookID);
        }

        public BookSystem.Model.BookUpdate GetEditById(int BookID)
        {
            return bookDao.GetEditById(BookID);
        }

        public void UpdateById(BookSystem.Model.BookUpdate arg)
        {
            bookDao.UpdateById(arg);
        }

        public List<BookSystem.Model.Books> GetBookLendRecords(int BookID)
        {
            return bookDao.GetBookLendRecords(BookID);
        }

        }
}
