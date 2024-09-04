using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace META_DATA_API.Models.TBL
{
    public class META_USR_MST
    {
        /// <summary>
        /// 관리번호
        /// </summary>
        public string MNGT_NO { get; set; }
        /// <summary>
        /// 아이디(이메일)
        /// </summary>
        public string USR_ID { get; set; }
        /// <summary>
        /// 사업자 번호
        /// </summary>
        public string CRN { get; set; }
        /// <summary>
        /// 인증키([MNGT_NO]|[USR_ID]|[CRN]|[AUTH_STRT_YMD]|[AUTH_END_YMD])
        /// </summary>
        public string AUTH_KEY { get; set; }
        /// <summary>
        /// 인증키 사용 시작일자
        /// </summary>
        public string AUTH_STRT_YMD { get; set; }
        /// <summary>
        /// 인증키 사용 종료일자
        /// </summary>
        public string AUTH_END_YMD { get; set; }
        /// <summary>
        /// 관리자여부
        /// </summary>
        public string MAIN_YN { get; set; }
        /// <summary>
        /// 승인여부
        /// </summary>
        public string APV_YN { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string INS_USR { get; set; }
        /// <summary>
        /// 입력일자
        /// </summary>
        public string INS_YMD { get; set; }
        /// <summary>
        /// 입력시간
        /// </summary>
        public string INS_HM { get; set; }
    }
}