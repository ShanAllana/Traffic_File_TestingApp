using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCCDailyTraffic.Models;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.ComponentModel;
using Spire.Xls.Collections;
using Spire.Xls;
using System.Data.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Globalization;

namespace TCCDailyTraffic.Controllers
{
    public class HomeController : Controller
    {
        string[] Result = new string[2];
        tables dtblModel = new tables();
       

        DataSet dts = new DataSet();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TCCDT()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TCCDT( FormCollection form , HttpPostedFileBase postedFile)
        {
            //if (form.Keys.Count != 0)
            //{
            // DateTime startDate = Convert.ToDateTime(form["createdStartDate"]);
            //DateTime endDate = Convert.ToDateTime(form["createdEndDate"]);
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today;
            DataTable dtbl = new DataTable();

                //Create a new data table and add new columns
                dtbl.Columns.Add(new DataColumn("StoreNumber", typeof(int)));
                dtbl.Columns.Add(new DataColumn("ActivityDate", typeof(DateTime)));
                dtbl.Columns.Add(new DataColumn("ActivityTime", typeof(TimeSpan)));
                dtbl.Columns.Add(new DataColumn("TrafficIn", typeof(int)));
                dtbl.Columns.Add(new DataColumn("TrafficOut", typeof(int)));
                dtbl.Columns.Add(new DataColumn("TrafficGroupIn", typeof(int)));
                dtbl.Columns.Add(new DataColumn("TrafficGroupOut", typeof(int)));

                //DataTable d2 = new DataTable();
                dtbl = DbHandler.getTcc(startDate, endDate);
                //d2 = DbHandler.geteDate(endDate);
                dts.Tables.Add(dtbl);
                dtblModel.dtblFirst = dts.Tables[0];
            //}

            //2nd Table
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                string filePath = string.Empty;
                string extension = string.Empty;
                filePath = path + Path.GetFileName(postedFile.FileName);
                extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);
                DataTable dtbl2 = new DataTable();

                //Create a new data table and add new columns
                dtbl2.Columns.Add(new DataColumn("StoreNumber", typeof(int)));
                dtbl2.Columns.Add(new DataColumn("ActivityDate", typeof(DateTime)));
                dtbl2.Columns.Add(new DataColumn("ActivityTime", typeof(TimeSpan)));
                dtbl2.Columns.Add(new DataColumn("TrafficIn", typeof(int)));
                dtbl2.Columns.Add(new DataColumn("TrafficOut", typeof(int)));
                dtbl2.Columns.Add(new DataColumn("TrafficGroupIn", typeof(int)));
                dtbl2.Columns.Add(new DataColumn("TrafficGroupOut", typeof(int)));

                dtbl2 = SpireXlsMethod(filePath);
                dts.Tables.Add(dtbl2);
                dtblModel.dtblSecond = dts.Tables[1];
            }

            DataSet dts2 = new DataSet();

            if (dts != null)
            {

                dts2 = passDatatable();
                dts.Tables.Add(dts2.Tables[0].Copy());
                dts.Tables.Add(dts2.Tables[1].Copy());
            }
            return View(dts);
        }
        public System.Data.DataTable SpireXlsMethod(string Filename)
        {
            DataTable dtbl = new System.Data.DataTable();
           // DataTable emp = new DataTable();
                        Workbook workbook = new Workbook();
            workbook.LoadFromFile(Filename);
            Worksheet sheet = workbook.Worksheets[0]; //01:00 PM mujhe 13:00
            sheet.Columns[2].NumberFormat = "hh:mm:ss";
          // if(sheet.Columns.Count() != 7)
          // {
           //     dtbl = emp;
         // }
           // else {  
            // sheet[3, 3].Text = "";
            dtbl = sheet.ExportDataTable();
                //DataTable dtCloned = dtbl.Clone();
                //dtCloned.Columns[2].DataType = typeof(TimeSpan);
                //DataRow row = dtCloned.NewRow();

                //for (int i = 0; i < dtbl.Rows.Count; i++)
                //{


                //    for (int j = 0; j < dtbl.Columns.Count; j++)
                //    {
                //        DataColumn dtc = new DataColumn();
                //        if (j == 2)
                //        {
                //            //dtc.DataType = System.Type.GetType("System.Datetime");
                //            //dtCloned.Columns.Add(dtc);
                //            string s = dtbl.Rows[i][dtbl.Columns[j]].ToString();//12/31/1899 10:00:00 AM Tables[2].Rows[j][Model.Tables[2].Columns[i]]
                //            string FTime;
                //            FTime = s.Substring(11);
                //            DateTime dt1 = Convert.ToDateTime(FTime.ToString());
                //            string FTime2 = dt1.ToString("HH:mm:ss tt");
                //            DateTime dt = DateTime.ParseExact(FTime2, "HH:mm:ss tt", null, DateTimeStyles.None);
                //            string FTT = dt.ToString("HH:mm:ss");

                //            row[i] = FTT;//DateTime.ParseExact(FTime, "HH:mm:ss tt", CultureInfo.InvariantCulture);                    
                //        }
                //        else
                //            row[i] = dtbl.Rows[i][dtbl.Columns[j]];
                //        dtCloned.Rows.Add(row[i]);
                //    }


                // dtCloned.ImportRow(row);

                // }
           // }
                return dtbl;
            
        }

        public DataSet passDatatable()
        {
            dtblModel.dtblFirst = dts.Tables[0];
            dtblModel.dtblSecond = dts.Tables[1];
            //DataTable dtbl3 = new DataTable();

            ////Create a new data table and add new columns
            //dtbl3.Columns.Add(new DataColumn("StoreNumber", typeof(int)));
            //dtbl3.Columns.Add(new DataColumn("ActivityDate", typeof(DateTime)));
            //dtbl3.Columns.Add(new DataColumn("ActivityTime", typeof(TimeSpan)));
            //dtbl3.Columns.Add(new DataColumn("TrafficIn", typeof(int)));
            //dtbl3.Columns.Add(new DataColumn("TrafficOut", typeof(int)));
            //dtbl3.Columns.Add(new DataColumn("TrafficGroupIn", typeof(int)));
            //dtbl3.Columns.Add(new DataColumn("TrafficGroupOut", typeof(int)));

            dtblModel.dtsSecond = getDifferentRecords(dtblModel.dtblFirst, dtblModel.dtblSecond);
            


            return dtblModel.dtsSecond;
        }

        #region Compare two DataTables and return a DataTable with DifferentRecords   
        public DataSet getDifferentRecords(DataTable FirstDataTable, DataTable SecondDataTable)
        {
            //Create Empty Table   
            DataTable ResultDataTable = new DataTable("ResultDataTable");
            DataTable ResultDataExcel = new DataTable("ResultDataExcel");
            DataSet dts2 = new DataSet();

            //use a Dataset to make use of a DataRelation object   
            using (DataSet ds = new DataSet())
            {

                DataTable dtCloneOne = FirstDataTable.Clone();
                DataTable dtCloneTwo = SecondDataTable.Clone();
                dtCloneOne.Columns[0].DataType = typeof(Int32);
                dtCloneOne.Columns[1].DataType = typeof(DateTime);
                dtCloneOne.Columns[2].DataType = typeof(TimeSpan);
                dtCloneOne.Columns[3].DataType = typeof(Int32);
                dtCloneOne.Columns[4].DataType = typeof(Int32);
                dtCloneOne.Columns[5].DataType = typeof(Int32);
                dtCloneOne.Columns[6].DataType = typeof(Int32);


                foreach (DataRow row in FirstDataTable.Rows)
                {
                    dtCloneOne.ImportRow(row);
                }

                dtCloneTwo.Columns[0].DataType = typeof(Int32);
                dtCloneTwo.Columns[1].DataType = typeof(DateTime);
                dtCloneTwo.Columns[2].DataType = typeof(TimeSpan);
                dtCloneTwo.Columns[3].DataType = typeof(Int32);
                dtCloneTwo.Columns[4].DataType = typeof(Int32);
                dtCloneTwo.Columns[5].DataType = typeof(Int32);
                dtCloneTwo.Columns[6].DataType = typeof(Int32);



                foreach (DataRow row in SecondDataTable.Rows)
                {
                    dtCloneTwo.ImportRow(row);
                }
                //Add tables   
                ds.Tables.AddRange(new DataTable[] { dtCloneOne, dtCloneTwo });

                
                //Get Columns for DataRelation   
                DataColumn[] firstColumns = new DataColumn[ds.Tables[0].Columns.Count];
                for (int i = 0; i < firstColumns.Length; i++)
                {
                    firstColumns[i] = ds.Tables[0].Columns[i];
                }

                DataColumn[] secondColumns = new DataColumn[ds.Tables[1].Columns.Count];
                for (int i = 0; i < secondColumns.Length; i++)
                {
                    secondColumns[i] = ds.Tables[1].Columns[i];
                }

                //Create DataRelation   
                DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                ds.Relations.Add(r1);

                DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                ds.Relations.Add(r2);

                //Create columns for return table   
                for (int i = 0; i < dtCloneOne.Columns.Count; i++)
                {
                    ResultDataTable.Columns.Add(dtCloneOne.Columns[i].ColumnName, dtCloneOne.Columns[i].DataType);
                }

                for (int i = 0; i < dtCloneOne.Columns.Count; i++)
                {
                    ResultDataExcel.Columns.Add(dtCloneOne.Columns[i].ColumnName, dtCloneOne.Columns[i].DataType);
                }

                ////Query First Dataset
                //IEnumerable<DataRow> query1 = from userData in dtCloneOne.AsEnumerable()
                //                              select userData;
                ////Query Second Dataset
                //IEnumerable<DataRow> query2 = from userData in dtCloneTwo.AsEnumerable()
                //                              select userData;
                //DataTable userData1 = query1.CopyToDataTable();
                //DataTable userData2 = query2.CopyToDataTable();

                ////now use Except Operator to find the Data in first set and Not in second
                //var userDataFirstset = userData1.AsEnumerable().Except(userData2.AsEnumerable(),DataRowComparer.Default);

                ////Find data in second and not in First

                //var userDataSecondset = userData2.AsEnumerable().Except(userData2.AsEnumerable(), DataRowComparer.Default);

                //foreach (var row in userDataFirstset)
                //{
                //    DataRow dr = ResultDataTable.NewRow();
                //    dr[0] = row[0];
                //    dr[1] = row[1];
                //    dr[2] = row[2];
                //    dr[3] = row[3];
                //    dr[4] = row[4];
                //    dr[5] = row[5];
                //    dr[6] = row[6];

                //}

                // If FirstDataTable Row not in SecondDataTable, Add to ResultDataTable.
                
                foreach (DataRow parentrow in ds.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r1);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }

                //If SecondDataTable Row not in FirstDataTable, Add to ResultDataTable.
                foreach (DataRow parentrow in ds.Tables[1].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r2);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataExcel.LoadDataRow(parentrow.ItemArray, true);
                }

            }

            dts2.Tables.AddRange(new DataTable[] { ResultDataTable, ResultDataExcel });

            Workbook book = new Workbook();
            Worksheet sheetone = book.Worksheets[0];
            sheetone.InsertDataTable(ResultDataTable, true, 1, 1);

            Worksheet sheettwo = book.Worksheets[1];
            sheettwo.InsertDataTable(ResultDataExcel, true, 1, 1);
            sheetone.Name = "Database";
            sheettwo.Name = "ExcelFile";
            sheetone.Columns[2].NumberFormat = "hh:mm:ss";
            sheettwo.Columns[2].NumberFormat = "hh:mm:ss";
            book.SaveToFile("C:\\Users\\saqib.abbasi\\Desktop\\TCCDailyTraffic (2)\\Mismatch.xls", ExcelVersion.Version97to2003);

            FirstDataTable = ResultDataTable;
            SecondDataTable = ResultDataExcel;
            int trows = ResultDataTable.Rows.Count;
            return dts2;
        }
        #endregion

        public ActionResult Portal_Monitoring_Click()
        {
//C:\\Users\\saqib.abbasi\\Desktop\\TCCDailyTraffic (2)\\TCCDailyTraffic2\\TCCDailyTraffic\\TCCDailyTraffic\\TCCDailyTraffic\\Content
            IWebDriver drv = new ChromeDriver("C:\\Users\\saqib.abbasi\\Desktop\\TCCDailyTraffic (2)\\TCCDailyTraffic2\\TCCDailyTraffic\\TCCDailyTraffic\\TCCDailyTraffic\\Content");
            string[] userdetail = new string[2];
            userdetail = DbHandler.userDetails();


            //drv.Manage().Window.Minimize();
            drv.Url = "https://tccrocks.rebiz.com/DailyMonitoredDetail";


            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);
           
            drv.FindElement(By.Id("LoginUser_UserName")).SendKeys(userdetail[0]);

            drv.FindElement(By.Id("LoginUser_Password")).SendKeys(userdetail[1]);

            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);

            drv.FindElement(By.Id("LoginUser_LoginButton")).Click();
            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);


            drv.FindElement(By.XPath("//*[@id=" + "'mainPane_rdbFilters'" + "]/tbody/tr/td[2]/span/label")).Click();

            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);

            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);


            String StartDateFill = DateTime.Today.AddDays(-2).ToString("MM/dd/yyyy");
            //string StartDateFill = "06/26/2021";

            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);

            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);



            drv.FindElement(By.Id("mainPane_txtSDate")).Click();
            pickDate(StartDateFill, drv);
            drv.FindElement(By.Id("mainPane_txtEDate")).Click();
            pickDate(StartDateFill, drv);


            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);



            Thread.Sleep(5);

            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);


            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);




            Thread.Sleep(5);


            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);
            drv.FindElement(By.XPath("/html/body/form/div[3]/div[3]/div[1]/div[2]/div/div/div[3]/div[2]/div[1]/div[2]/div/div[5]/input")).Click();



            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);
            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);

            string DMDPercentage = drv.FindElement(By.XPath("/html/body/form/div[3]/div[3]/div[1]/div[2]/div/div/div[3]/div[2]/div[2]/div[4]/span/div/table/tbody/tr[5]/td[3]/div/div[1]/div/table/tbody/tr/td/div[1]/table/tbody/tr[3]/td[4]/div")).Text;

            Thread.Sleep(1000);



            //////////////////////////// CC Report ki monitoring percentage ///////////////


            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);

            drv.FindElement(By.XPath("/html/body/form/div[3]/header/nav/ul[1]/li[1]/a/i")).Click();



            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);

            drv.FindElement(By.XPath("/html/body/form/div[3]/div[1]/div[3]/div/ul/li[2]/ul/li[2]/ul/li[1]/a")).Click();


            drv.FindElement(By.Id("mainPane_txtSDate")).Click();
            pickDate(StartDateFill, drv);
            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);
            drv.FindElement(By.Id("mainPane_txtEDate")).Click();
            pickDate(StartDateFill, drv);


            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);

            drv.FindElement(By.XPath("/html/body/form/div[3]/div[3]/div/div[3]/div/div/div[2]/div[1]/div[6]/input")).Click();

            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);
            string CCPercentage = drv.FindElement(By.XPath("/html/body/form/div[3]/div[3]/div/div[3]/div/div/div[2]/div[2]/div[2]/div/table/tbody/tr[1]/td[10]/span")).Text;

            Result[0] = DMDPercentage;
            Result[1] = CCPercentage;

            Session["Result0"] = Result[0];
            Session["Result1"] = Result[1];

            ////MessageBox.Show("Monitoring percentage in DMD Report : " + DMDPercentage + "\n   CC Report: " + CCPercentage + "\n Back-End DB: " + DMDPercentage + "\n Hence Traffic File is Eligible to be send");
            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);

            drv.FindElement(By.XPath("/html/body/form/div[3]/header/nav/ul[2]/li[2]/a/h6/span")).Click();

            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);

            drv.FindElement(By.XPath("/html/body/form/div[3]/header/nav/ul[2]/li[2]/ul/li[4]/input")).Click();

            drv.Close();

            return RedirectToAction("TCCDT");

        }

        public static void pickDate(String date, IWebDriver driver)
        {

            String dtMonth, dtDay, dtYear;

            int i = 1, j = 1;

            dtMonth = date.Substring(0, 2);
            dtDay = date.Substring(3, 2);

            dtYear = date.Substring(6);

            String[] monthString = new String[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("datepicker-switch")).Click();
            Thread.Sleep(1000);
            // driver.findElement(By.className("datepicker-switch")).click();
            driver.FindElement(By.XPath("/html/body/div[4]/div[2]/table/thead/tr[2]/th[2]")).Click();

            Thread.Sleep(1000);
            driver.FindElement(By.XPath("/html/body/div[4]/div[3]/table/tbody/tr/td/*[contains(text(), '" + dtYear + "')]")).Click();

            Thread.Sleep(1000);

            driver.FindElement(By.XPath("/html/body/div[4]/div[2]/table/tbody/tr/td/*[contains(text(), '" + monthString[Convert.ToInt16(dtMonth) - 1] + "')]")).Click();

            Thread.Sleep(2000);

            String checkDate = null;

            if (Convert.ToInt16(dtDay) >= Convert.ToInt16(20))
            {
                i = 3;
            }

            Boolean w = true;
            while (w)
            {
                for (j = 1; j <= 7; j++)
                {
                    checkDate = driver.FindElement(By.XPath("/html/body/div[4]/div[1]/table/tbody/tr[" + i + "]/td[" + j + "]"))
                            .Text;
                    if (Convert.ToInt16(checkDate) == Convert.ToInt16(dtDay))
                    {
                        driver.FindElement(By.XPath("/html/body/div[4]/div[1]/table/tbody/tr[" + i + "]/td[" + j + "]")).Click();
                        w = false;
                        i = 1;
                        j = 1;
                        break;
                    }
                }
                i++;
                j = 1;
            }
        }

       
    }
}