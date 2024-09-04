using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ELVIS_META_COMMON.Query
{
    class Container_Query
    {
        string sqlstr;

        /// <summary>
        /// 메인 - 로그인
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnPort_Query()
        {
            sqlstr = "";

            sqlstr = "   SELECT MAX((SELECT  PORT_LOC_NM FROM BIG_MDM_PORT_MST WHERE PORT_CD = A. LOC_CD)) AS LOC_NM ";
            sqlstr += "       ,  LOC_CD";
            sqlstr += "    FROM BIG_MDM_TRMN_MST A ";
            sqlstr += "   GROUP BY LOC_CD  ";
            sqlstr += "   ORDER BY LOC_CD  ";

            return sqlstr;
        }

        public string Query_GetTerminalData(DataRow dr)
        {
            sqlstr = "";

            sqlstr = "   SELECT TRMN_CD ";
            sqlstr += "       , TRMN_NM";
            sqlstr += "    FROM BIG_MDM_TRMN_MST A ";
            sqlstr += "  WHERE LOC_CD = '" + dr["LOC_CD"].ToString() + "'" ;
            sqlstr += "  ORDER BY TRMN_CD  ";
            
            return sqlstr;
        }

    }
}
