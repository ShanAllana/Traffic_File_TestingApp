using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace TCCDailyTraffic.Models
{
    public class DbHandler
    {
        public static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

       // tables dtblModel = new tables();
        public static DataTable getTcc(DateTime startDate, DateTime endDate)
        {
            string sqlFormattedstartDate = startDate.ToString("yyyy-MM-dd 07:09:00");
            string sqlFormattedendDate = endDate.ToString("yyyy-MM-dd 23:59:00");
            
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                string query = Query.getQuery(sqlFormattedstartDate, sqlFormattedendDate);
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dtbl = new DataTable();
                sda.Fill(dtbl);
                conn.Close();

                return dtbl;
            
            
            
        }

        public static string[] userDetails()
        {
            string[] user = new string[2];
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand cmd2 = new SqlCommand("select Convert(varchar(MAX), DecryptByPassPhrase('93', password)) as password from EmployeeInformation where employeeID = 1001");
            cmd2.Connection = conn;
            SqlCommand cmd1 = new SqlCommand("select UserID from EmployeeInformation where employeeID = 1001");
            cmd1.Connection = conn;
            user[0] = (string)cmd1.ExecuteScalar();
            user[1] = (string)cmd2.ExecuteScalar();
            conn.Close();
            return user;
        }

        
        //public  void saveMismatch()
        //{
        //    DataTable FirstMismatch = new DataTable();
        //    DataTable SecondMismatch = new DataTable();
        //    FirstMismatch = dtblModel.dtsSecond.Tables[0] ;


        //    Workbook book = new Workbook();
        //    Worksheet sheetone = book.Worksheets[0];
        //    sheetone.InsertDataTable(FirstMismatch, true, 1, 1);

        //    Worksheet sheettwo = book.Worksheets[1];
        //    sheettwo.InsertDataTable(SecondMismatch, true, 1, 1);

        //    book.SaveToFile("C:\\Users\\saqib.abbasi\\Desktop\\TCCDailyTraffic (2)\\Mismatch.xls", ExcelVersion.Version97to2003);
        //    System.Diagnostics.Process.Start("C:\\Users\\saqib.abbasi\\Desktop\\TCCDailyTraffic (2)\\Mismatch.xls"); 

        //}


        //public static DataTable geteDate(DateTime endDate)
        //{
        //    string sqlFormattedendDate = endDate.ToString("yyyy-MM-dd");
        //    if (conn.State == ConnectionState.Closed)
        //        conn.Open();
        //    string query = Query.getQueryFile(sqlFormattedendDate);
        //    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
        //    DataTable dtbl1 = new DataTable();
        //    sda.Fill(dtbl1);
        //    conn.Close();
        //    return dtbl1;
        //}
    }
}