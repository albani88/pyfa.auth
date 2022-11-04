using pyfa.auth.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using pyfa.auth.libs;

namespace pyfa.auth.libs
{
 
    public class lPgsqlMapping
    {
        private BaseController bc = new BaseController();
        private TokenController tc = new TokenController();
        private lDataLayer ld = new lDataLayer();
        private lConvert lc = new lConvert();
        private lPgsql pgsql = new lPgsql();

        internal List<dynamic> Genspuserinfobyemail(string email)
        {
            var strQry = "";
            List<dynamic> retObjectV = new List<dynamic>();

            strQry += " \r\n";
            strQry += " select id as NIK \r\n";
            strQry += " ,level_id \r\n";
            strQry += " ,a.lvl_Name as level_name  \r\n";
            strQry += " ,b.Kd_Company as company_id \r\n";
            strQry += " ,b.Nm_Company as company_name \r\n";
            strQry += " ,c.Kd_Divisi as division_id ";
            strQry += " ,c.Nm_Divisi as division_name \r\n";
            strQry += " ,d.Kd_Dept as dept_id \r\n";
            strQry += " ,d.Nm_Dept as dept_name  \r\n";
            strQry += " ,STUFF( \r\n";
            strQry += " (   SELECT ',' + CONVERT(NVARCHAR(20), Kd_Sales_Group)  \r\n";
            strQry += " FROM M_User_Sales_Group \r\n";
            strQry += " WHERE [user_id] = u.id  \r\n";
            strQry += " FOR xml path('') \r\n";
            strQry += " ) \r\n";
            strQry += " , 1 \r\n";
            strQry += " , 1 \r\n";
            strQry += " , '' \r\n";
            strQry += " ) as sales_group \r\n";
            strQry += " ,STUFF( \r\n";
            strQry += " (   SELECT ',' + CONVERT(NVARCHAR(20), Kd_Prop) \r\n";
            strQry += " FROM M_User_Propinsi  \r\n";
            strQry += " WHERE [user_id] = u.id \r\n";
            strQry += " FOR xml path('') \r\n";
            strQry += " ) \r\n";
            strQry += " , 1  \r\n";
            strQry += " , 1 \r\n";
            strQry += " , ''  \r\n";
            strQry += " ) as kd_prop \r\n";
            strQry += " from m_user u \r\n";
            strQry += " left join  \r\n";
            strQry += " (  \r\n";
            strQry += "     select lvl_Name,lvl_Id from M_Level_User \r\n";
            strQry += " )a on lvl_Id = u.level_id \r\n";
            strQry += " left join  \r\n";
            strQry += " (  \r\n";
            strQry += "     select Nm_Company,Kd_Company from M_Company  \r\n";
            strQry += " )b on b.Kd_Company = u.Kd_Company\r\n";
            strQry += " left join  \r\n";
            strQry += " (  \r\n";
            strQry += "     select Nm_Divisi,Kd_Divisi,Kd_Perusahaan from M_Divisi  \r\n";
            strQry += " )c on c.Kd_Divisi = u.Kd_Divisi and c.Kd_Perusahaan = u.Kd_Company\r\n";
            strQry += " left join  \r\n";
            strQry += " ( \r\n";
            strQry += "     select Nm_Dept,Kd_Dept,Kd_Perusahaan,Kd_Divisi  from M_Dept  \r\n";
            strQry += " )d on d.Kd_Dept = u.Kd_Dept and d.Kd_Perusahaan = u.Kd_Company and d.Kd_Divisi = u.Kd_Divisi \r\n";
            strQry += " where email = '" + email + "' \r\n";


            retObjectV = this.executeStringQuery(strQry);
           // var  result1 = JObject.Parse(jaReturn);
            return retObjectV;
        }

        internal List<dynamic> executeStringQuery(string qryStr)
        {
           
            var jaReturn = new JArray();
            List<dynamic> retObject = new List<dynamic>();
           
            retObject = pgsql.execStringQuerywithresult(qryStr);
            //jaReturn = lc.convertDynamicToJArray(retObject);
           
            return retObject;
        }

    }
}
