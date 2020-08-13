using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSystem.Model
{
    public class BookSearchArg
    {

        /// / <summary>
        // / 包含查詢欄位的項目
        // / </summary>
        [DisplayName("書名")]
        public string BOOK_ID { get; set; }
        [DisplayName("書名")]
        [StringLength(200)]
        public string BOOK_NAME { get; set; }
        [DisplayName("圖書類別")]
        public string BOOK_CLASS_NAME { get; set; }
        [DisplayName("圖書類別")]
        public string BOOK_CLASS_ID { get; set; }
        [DisplayName("借閱人")]
        public string USER_ENAME { get; set; }
        [DisplayName("借閱人中文名")]
        public string USER_CNAME { get; set; }
        [DisplayName("借閱人")]
        public string USER_ID { get; set; }
        [DisplayName("借閱狀態")]
        public string BOOK_STATUS { get; set; }
        [DisplayName("借閱狀態")]
        public string BOOK_STATUS_NAME { get; set; }
        [DisplayName("購書日期")]
        public string BOOK_BOUGHT_DATE { get; set; }
    }
}
