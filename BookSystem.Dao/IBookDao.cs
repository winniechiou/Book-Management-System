using System.Collections.Generic;
using BookSystem.Model;

namespace BookSystem.Dao
{
    public interface IBookDao
    {
        void DeleteBookById(int BookID);
        string findBook(int bookID);
        string findBookKeeper(int bookID);
        List<BookSearchArg> GetBookByCondtioin(BookSearchArg arg);
        List<Books> GetBookLendRecords(int BookID);
        BookUpdate GetEditById(int BookID);
        void InsertBook(Books book);
        void UpdateById(BookUpdate arg);
    }
}