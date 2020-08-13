using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookSystem.Models
{
    public class CodeService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        ///  取得BOOK_CLASS的部分資料(about dropdownlist)
        /// </summary>
        public List<SelectListItem> GetBookClass()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT BOOK_CLASS_NAME as CodeName, BOOK_CLASS_ID as CodeID FROM BOOK_CLASS ;";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
            }
            return this.MapData(dt);
        }

        /// <summary>
        /// DROP DOWN LIST FOR 借閱人
        /// </summary>
        public List<SelectListItem> GetUser()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT USER_ID as CodeID, USER_ENAME+'-'+USER_CNAME as CodeName FROM MEMBER_M;";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
            }
            return this.MapData(dt);
        }

      
        /// <summary>
        /// DROP DOWN LIST FOR 借閱狀態
        /// </summary>
        public List<SelectListItem> GetStatus(string Status)
        {
            DataTable dt = new DataTable();
            string sql = @"Select Distinct CODE_NAME As CodeName, CODE_ID As CodeID 
                           From BOOK_CODE 
                           Where CODE_TYPE = @Status";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Status", Status));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
            }
            return this.MapData(dt);
        }



        /// <summary>
        /// Maping BOOK CLASS資料
        /// </summary>
        private List<SelectListItem> MapData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text =  row["CodeName"].ToString(),
                    Value = row["CodeID"].ToString()
                });
            }
            return result;
        }
    }
}




