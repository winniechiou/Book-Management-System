using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Security.Application;

namespace BookSystem.Dao
{
    public class BookDao : IBookDao //繼承IBookDao(a interface)
    {

        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()

        {
            return BookSystem.Common.ConfigTool.GetDBConnectionString("DBConn");
        }



        /// <summary>
        /// 新增書籍
        /// </summary>
        public void InsertBook(BookSystem.Model.Books book)
        {
            string sql = @"INSERT INTO BOOK_DATA(
                            BOOK_NAME, 
                            BOOK_AUTHOR, 
                            BOOK_PUBLISHER, 
                            BOOK_NOTE, 
                            BOOK_BOUGHT_DATE, 
                            BOOK_CLASS_ID,
                            BOOK_STATUS
						 )
						VALUES
						( 
                            @BOOK_NAME, 
                            @BOOK_AUTHOR, 
                            @BOOK_PUBLISHER, 
                            @BOOK_NOTE, 
                            @BOOK_BOUGHT_DATE, 
                            @BOOK_CLASS_ID,
                            @BOOK_STATUS

						)";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", book.BOOK_NAME));
                cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", book.BOOK_AUTHOR));
                cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", book.BOOK_PUBLISHER));
                cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", book.BOOK_NOTE));
                cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", book.BOOK_BOUGHT_DATE));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", book.BOOK_CLASS_ID));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", "A"));
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    cmd.ExecuteNonQuery();//可以知道影響幾筆(int)
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }



        /// <summary>
        /// 依照條件取得書籍資料(搜尋)
        /// </summary>

        public List<BookSystem.Model.BookSearchArg> GetBookByCondtioin(BookSystem.Model.BookSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"select 
                                BD.BOOK_ID as BookID,
                                BC.BOOK_CLASS_NAME as BookClassName,
                                BC.BOOK_CLASS_ID as BookClassID,
                                BD.BOOK_NAME as BookName,
                                CONVERT(varchar(12),BD.BOOK_BOUGHT_DATE, 23) as BoughtDate,
                                BD.BOOK_STATUS as BookStatus,
                                BCD.CODE_NAME as StatusName,
                                M.USER_ENAME as UserName,
                                M.USER_ID as UserID,
                                M.USER_CNAME as UserCName
                                FROM BOOK_DATA AS BD
                                LEFT JOIN MEMBER_M AS M ON BD.BOOK_KEEPER = M.USER_ID
                                INNER JOIN BOOK_CLASS AS BC ON BD.BOOK_CLASS_ID = BC.BOOK_CLASS_ID
                                INNER JOIN BOOK_CODE AS BCD ON BD.BOOK_STATUS=BCD.CODE_ID  
                                where BCD.CODE_TYPE='BOOK_STATUS' AND 
                                            (UPPER(BD.BOOK_NAME) LIKE UPPER('%' + @BOOK_NAME + '%')or @BOOK_NAME='') AND
                                             (BC.BOOK_CLASS_ID = @BOOK_CLASS_ID OR @BOOK_CLASS_ID='')AND
                                             (M.USER_ID = @USER_ID OR @USER_ID='')AND
                                             (BD.BOOK_STATUS = @BOOK_STATUS OR @BOOK_STATUS='')
                                ORDER BY BoughtDate DESC";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", arg.BOOK_NAME == null ? string.Empty : arg.BOOK_NAME));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", arg.BOOK_CLASS_ID == null ? string.Empty : arg.BOOK_CLASS_ID));
                cmd.Parameters.Add(new SqlParameter("@USER_ID", arg.USER_ID == null ? string.Empty : arg.USER_ID));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", arg.BOOK_STATUS == null ? string.Empty : arg.BOOK_STATUS));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
            }
            return this.MapBookDataToList(dt);
        }



        /// <summary>
        /// 刪除前 確認書本是否有借閱者
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public String findBookKeeper(int bookID)
        {
            string bookKepper = "";
            string sql = @" select BOOK_KEEPER from BOOK_DATA where BOOK_ID=@bookID";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookID", bookID));
                bookKepper = Convert.ToString(cmd.ExecuteScalar());
            }
            return bookKepper;
        }



        /// <summary>
        /// 刪除前 確認書本是否存在
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public String findBook(int bookID)
        {
            string bookid = "";//跟傳入值命名不能一樣
            string sql = @"  select BOOK_ID from BOOK_DATA where BOOK_ID=@bookID";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookID", bookID));
                bookid = Convert.ToString(cmd.ExecuteScalar());
            }
            return bookid;
        }



        /// <summary>
        /// 刪除客戶
        /// </summary>
        public void DeleteBookById(int BookID)
        {
            try
            {
                string sql = "Delete from BOOK_DATA Where BOOK_ID = @BookID";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@BookID", BookID));
                    SqlTransaction tran = conn.BeginTransaction();
                    cmd.Transaction = tran;
                    try
                    {
                        cmd.ExecuteNonQuery();//可以知道影響幾筆(int)
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 取得欲修改書籍資料
        /// </summary>
        public BookSystem.Model.BookUpdate GetEditById(int BookID)
        {
            DataTable dt = new DataTable();
            string sql = @"select 
                                BD.BOOK_ID as BookID,
                                BD.BOOK_NAME as BookName,
                                BD.BOOK_AUTHOR as Author,
                                BD.BOOK_PUBLISHER as Publisher,
                                BD.BOOK_NOTE as Note,
                                CONVERT(VARCHAR,BD.BOOK_BOUGHT_DATE,111) as BoughDate,
                                BC.BOOK_CLASS_NAME as BookClassName,
                                BC.BOOK_CLASS_ID as BookClassID,
                                BCD.CODE_NAME as StatusName, 
                                BCD.CODE_ID as StatusID,
                                M.USER_ENAME as UserName,
                                M.USER_ID as UserID
                        FROM BOOK_DATA AS BD
                                LEFT JOIN MEMBER_M AS M ON BD.BOOK_KEEPER = M.USER_ID
                                INNER JOIN BOOK_CLASS AS BC ON BD.BOOK_CLASS_ID = BC.BOOK_CLASS_ID
                                INNER JOIN BOOK_CODE AS BCD ON BD.BOOK_STATUS = BCD.CODE_ID
                        WHERE BD.BOOK_ID =@BookID and BCD.CODE_TYPE='BOOK_STATUS';";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookID", BookID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
            }
            return this.MapBookDataForUpdateToList(dt);
        }



        /// <summary>
        /// 修改書籍資料
        /// </summary>
        public void UpdateById(BookSystem.Model.BookUpdate arg)
        {
            string sql = @"UPDATE BOOK_DATA
                            SET
                                BOOK_NAME =@BOOK_NAME,
                                BOOK_AUTHOR =@BOOK_AUTHOR,
                                BOOK_PUBLISHER =@BOOK_PUBLISHER,
                                BOOK_NOTE =@BOOK_NOTE,
                                BOOK_BOUGHT_DATE =@BOOK_BOUGHT_DATE,
                                BOOK_CLASS_ID=@BOOK_CLASS_ID,
                                BOOK_STATUS=@BOOK_STATUS_ID,
                                BOOK_KEEPER=@KEEPER_ID
                            WHERE
                                BOOK_ID=@BOOK_ID;";

            string sqlAboutInsertLendRecord = @"INSERT INTO BOOK_LEND_RECORD(				
                                                      BOOK_ID, 
                                                    KEEPER_ID, 
                                                    LEND_DATE,
                                                    CRE_DATE,
                                                    MOD_DATE
                                                     )VALUES(
                                                    @BOOK_ID,
                                                    @KEEPER_ID,
                                                    @LEND_DATE,
                                                    @CRE_DATE,
                                                    @MOD_DATE) ";

            if (arg.KEEPER_ID != null)
            {
                sql = sql + sqlAboutInsertLendRecord;
            }
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", arg.BOOK_NAME));
                cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", arg.BOOK_AUTHOR));
                cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", arg.BOOK_PUBLISHER));
                cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", arg.BOOK_NOTE));
                cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", arg.BOOK_BOUGHT_DATE));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", arg.BOOK_CLASS_ID));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS_ID", arg.BOOK_STATUS_ID));
                cmd.Parameters.Add(new SqlParameter("@KEEPER_ID", arg.KEEPER_ID == null ? string.Empty : arg.KEEPER_ID));
                cmd.Parameters.Add(new SqlParameter("@BOOK_ID", arg.BOOK_ID));
                cmd.Parameters.Add(new SqlParameter("@LEND_DATE", DateTime.Now.ToShortDateString()));
                cmd.Parameters.Add(new SqlParameter("@CRE_DATE", DateTime.Now.ToShortDateString()));
                cmd.Parameters.Add(new SqlParameter("@MOD_DATE", DateTime.Now.ToShortDateString()));
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    cmd.ExecuteNonQuery();//可以知道影響幾筆(int)
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }



        /// <summary>
        /// 取得借閱明細
        /// </summary>
        public List<BookSystem.Model.Books> GetBookLendRecords(int BookID)
        {
            DataTable dt = new DataTable();
            string sql = @" SELECT BLR.BOOK_ID as BookID,
                                    CONVERT(VARCHAR,BLR.LEND_DATE,111) as LendDate,
                                    BLR.KEEPER_ID as KeeperID,
                                    M.USER_ENAME as UserEName,
                                    M.USER_CNAME as UserCName
                                 FROM BOOK_LEND_RECORD AS BLR 
                                 INNER JOIN MEMBER_M AS M ON BLR.KEEPER_ID=M.USER_ID
                                 WHERE BLR.BOOK_ID=@BOOK_ID
                                 ORDER BY BLR.LEND_DATE DESC;";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_ID", BookID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
            }
            return this.MapBookLendRecordToList(dt);
        }



        /// <summary>
        /// Map資料進List
        /// </summary>
        private List<BookSystem.Model.BookSearchArg> MapBookDataToList(DataTable employeeData)
        {
            List<BookSystem.Model.BookSearchArg> result = new List<BookSystem.Model.BookSearchArg>();
            foreach (DataRow row in employeeData.Rows)
            {
                result.Add(new BookSystem.Model.BookSearchArg()
                {
                    BOOK_ID = row["BookID"].ToString(),
                    BOOK_CLASS_NAME = row["BookClassName"].ToString(),
                    BOOK_NAME = row["BookName"].ToString(),
                    BOOK_BOUGHT_DATE = row["BoughtDate"].ToString(),
                    BOOK_STATUS = row["BookStatus"].ToString(),
                    USER_ENAME = row["UserName"].ToString(),
                    USER_CNAME = row["UserName"].ToString(),
                    USER_ID = row["UserID"].ToString(),
                    BOOK_STATUS_NAME = row["StatusName"].ToString()
                });
            }
            return result;
        }
        


        /// <summary>
        /// Map資料
        /// </summary>
        private BookSystem.Model.BookUpdate MapBookDataForUpdateToList(DataTable BookData)
        {
            BookSystem.Model.BookUpdate result = new BookSystem.Model.BookUpdate();
            foreach (DataRow row in BookData.Rows)
            {
                result.BOOK_ID = (int)row["BookID"];
                //result.BOOK_NAME = row["BookName"].ToString();
                result.BOOK_NAME = Microsoft.Security.Application.Encoder.HtmlEncode(row["BookName"].ToString());
                //result.BOOK_AUTHOR = row["Author"].ToString();
                result.BOOK_AUTHOR = Microsoft.Security.Application.Encoder.HtmlEncode(row["Author"].ToString());
                result.BOOK_PUBLISHER = row["Publisher"].ToString();
                result.BOOK_NOTE = row["Note"].ToString();
                result.BOOK_BOUGHT_DATE = row["BoughDate"].ToString();
                result.BOOK_CLASS_NAME = row["BookClassName"].ToString();
                result.BOOK_CLASS_ID = row["BookClassID"].ToString();
                result.BOOK_STATUS = row["StatusName"].ToString();
                result.BOOK_STATUS_ID = row["StatusID"].ToString();
                result.USER_ENAME = row["UserName"].ToString();
                result.KEEPER_ID = row["UserID"].ToString();
            }
            return result;
        }



        /// <summary>
        /// Map資料進Model
        /// </summary>
        private List<BookSystem.Model.Books> MapBookLendRecordToList(DataTable BookData)
        {
            List<BookSystem.Model.Books> result = new List<BookSystem.Model.Books>();
            foreach (DataRow row in BookData.Rows)
            {
                result.Add(new BookSystem.Model.Books()
                {
                    BOOK_ID = row["BookID"].ToString(),
                    LEND_DATE = row["LendDate"].ToString(),
                    KEEPER_ID = row["KeeperID"].ToString(),
                    USER_ENAME = row["UserEName"].ToString(),
                    USER_CNAME = row["UserCName"].ToString(),
                });
            }
            return result;
        }

    }
}
