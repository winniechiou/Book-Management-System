using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 using Microsoft.Security.Application;
namespace BookSystem.Controllers
{
    public class BookController : Controller
    {
        private Service.ICodeService codeService { get; set; }
        private Service.IBookService bookService { get; set; }
        
        public ActionResult Index() {
            return View();
        }
        
        public ActionResult UpdateBook()
        {
            return View();
        }
        


        /// <summary>
        /// 生成DropDownListDataForClass
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDropDownListDataForClass()
        {
            
            List<SelectListItem> listAboutBookClass = codeService.GetBookClass();
            return Json(listAboutBookClass);
        }


        /// <summary>
        /// 生成DropDownListDataForUser
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDropDownListDataForUser()
        {
            List<SelectListItem> listAboutUser = codeService.GetUser();
            return Json(listAboutUser);
        }


        /// <summary>
        /// 生成DropDownListDataForStatus
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDropDownListDataForStatus()
        {
            List<SelectListItem> listAboutStatus = codeService.GetStatus("BOOK_STATUS");
            return Json(listAboutStatus);
        }


        /// <summary>
        /// 新增書籍
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertBookInView(BookSystem.Model.Books book)
        {
            bookService.InsertBook(book);
            return Json(true);
        }


        /// <summary>
        /// 搜尋書籍
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult SearchBook(BookSystem.Model.BookSearchArg arg)
        {
            List<BookSystem.Model.BookSearchArg> searchResult = bookService.GetBookByCondtioin(arg);
            return Json(searchResult);
        }


        /// <summary>
        /// AutoComplete 取得書名
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult BookNameAutoComplete(BookSystem.Model.BookSearchArg arg)
        {
            List<BookSystem.Model.BookSearchArg> searchResult = bookService.GetBookByCondtioin(arg);
            List<string> bookNameData = new List<string>();
            foreach (var value  in searchResult)
            {
                bookNameData.Add(value.BOOK_NAME);
            }
            return Json(bookNameData);
        }


        /// <summary>
        /// 借閱紀錄
        /// </summary>
        /// <returns></returns>
        /// 前面在html寫DATA的時候 變數名稱要跟這邊的一樣
        [HttpPost()]
        public JsonResult LendBookRecord(int bookID)
        {
            if (bookService.findBook(bookID) != null) {
                List<BookSystem.Model.Books> lendBookRecord = bookService.GetBookLendRecords(bookID);
                return Json(lendBookRecord); }
            else {
                string alert = "BookIDNotExist";
                return Json(alert);
            }
        }
        

        
        /// <summary>
        /// 刪除書籍
        /// </summary>
        [HttpPost()]
        public JsonResult DeleteBook(int BookID)
        {
            string checkStatus = "";
            if (bookService.findBook(BookID) != "") {
                //書存在

                if (bookService.findBookKeeper(BookID) != "") {
                    //借書人存在 不可刪除
                    checkStatus = "existKeeper";
                    return this.Json(checkStatus);
                }
                else {
                    //借書人不存在 可以刪除
                    checkStatus = "canBeDelete";
                    bookService.DeleteBookById(BookID);
                    return this.Json(checkStatus);
                }
            } else {
                //書不存在 不可刪除
                checkStatus = "bookNotExist";
                return this.Json(checkStatus);
            }

        }


        /// <summary>
        /// 取得欲修改書籍資料or明細
        /// </summary>
        [HttpPost()]
        public JsonResult GetBookData(int bookID)
        {
            BookSystem.Model.BookUpdate bookData = new BookSystem.Model.BookUpdate();
            if (bookService.findBook(bookID) !="")
            {
                bookData = bookService.GetEditById(bookID);
                return Json(bookData);
            }
            else {
                string alert = "BookIDNotExist";
                return Json(alert);
            }
        }

        
        /// <summary>
        /// 修改書籍
        /// </summary>
        [HttpPost()]
        public JsonResult UpdateBook(BookSystem.Model.BookUpdate book)
        {
            bookService.UpdateById(book);
            return Json(true);
        }



        
    }
}