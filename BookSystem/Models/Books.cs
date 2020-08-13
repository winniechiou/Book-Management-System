using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookSystem.Models
{
    /// <summary>
    /// 包含新增的項目
    /// </summary>
    public class Books
    {
        /// <summary>
        /// 書本編號
        /// </summary>
        ///[MaxLength(5)]
        [DisplayName("書本編號")]
        public  string BOOK_ID { get; set; }

        /// <summary>
        /// 書名
        /// </summary>
        [DisplayName("書名")]
        [StringLength(200)]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_NAME { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DisplayName("作者")]
        [StringLength(30)]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_AUTHOR { get; set; }

        /// <summary>
        ///出版商
        /// </summary>
        [DisplayName("出版商")]
        [StringLength(20)]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_PUBLISHER { get; set; }

        /// <summary>
        /// 內容簡介
        /// </summary>
        [DisplayName("內容簡介")]
        [StringLength(1200)]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_NOTE { get; set; }


        /// <summary>
        /// 購書日期
        /// </summary>
        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_BOUGHT_DATE { get; set; }

        /// <summary>
        /// 借閱日期
        /// </summary>
        [DisplayName("借閱日期")]
        public string LEND_DATE { get; set; }

        /// <summary>
        /// 圖書類別
        /// </summary>
        [DisplayName("圖書類別")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BOOK_CLASS_NAME { get; set; }

        [DisplayName("圖書類別ID")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_CLASS_ID { get; set; }

        ///// <summary>
        /// 借閱人ID
        /// </summary>
        [DisplayName("借閱人ID")]
        public string KEEPER_ID { get; set; }

        ///// <summary>
        /// 借閱人英文名
        /// </summary>
        [DisplayName("借閱人英文名")]
        public string USER_ENAME { get; set; }

        ///// <summary>
        /// 借閱人中文名
        /// </summary>
        [DisplayName("借閱人中文名")]
        public string USER_CNAME { get; set; }

        /// <summary>
        /// 借閱狀態
        /// </summary>
        [DisplayName("借閱狀態")]
        public string BOOK_STATUS{ get; set; }

        /// <summary>
        /// 借閱狀態ID
        /// </summary>
        [DisplayName("借閱狀態ID")]
        public string BOOK_STATUS_ID { get; set; }

    }
}