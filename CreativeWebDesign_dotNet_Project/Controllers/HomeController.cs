﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
/*using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.SpreadSheet;*/
using System.IO;
using System.Data;
using System.Configuration;
using ClosedXML.Excel;
using Newtonsoft.Json.Linq;
//..



namespace CreativeWebDesign_dotNet_Project.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult PostDemographics(string organization, string streetAddress, string state, string number,
            string password, string role, string city, string zipCode, string email, string gender, string mariatalStatus, string age,
            string occupation, string education, double timeOnLandingPage)
        {
            CreativeWebDesigndotNetProject_dbEntities db = new CreativeWebDesigndotNetProject_dbEntities();
            UserRecord user = new UserRecord();
            //List<UserActivity> activities = user.UserActivities.Select(x => x).ToList<UserActivity>();
            
            string uID = Guid.NewGuid().ToString();
            ViewBag.userId = uID;
            user.Id = uID;
            user.TimeLoggedIn = DateTime.Now;
            user.Age = age;
            user.City = city;
            user.Email = email;
            user.Gender = gender;
            user.MaritalStatus = mariatalStatus;
            user.Occupation = occupation;
            user.Organization = organization;
            user.Password = password;
            user.PhoneNumber = number;
            user.RoleInOrganization = role;
            user.State = state;
            user.StreetAddress = streetAddress;
            user.ZipCode = zipCode;
            user.Education = education;
            //figure out time on landing page
            string[] split = timeOnLandingPage.ToString().Split('.');   
            string minutes = split[0];
            string seconds = "0";
            try
            {
                double secDouble = Double.Parse(("." + split[1])) * 60;
                seconds = Math.Round(secDouble).ToString();
            }
            catch{}
            user.TimeOnLandingPage = minutes + " min, " + seconds + " sec";
            db.UserRecords.Add(user);
            db.SaveChanges();

           
            return RedirectToAction("UserMatrix", new {userId = uID });
        }

        
        public ActionResult UserMatrix(string userId)
        {
            ViewBag.userId = userId;
            return View();
        }

        

        [HttpPost]
        public ActionResult PostUserMatrix(string userActivity, string userID, double timeOnSurvey, string finalChoice)
        {
            CreativeWebDesigndotNetProject_dbEntities db = new CreativeWebDesigndotNetProject_dbEntities();
            //if (db.UserActivities.Where(x => x.UserId == userID).Count() == 0)
            
                JObject uaObj = JObject.Parse(userActivity);
                string[] split = timeOnSurvey.ToString().Split('.');
                string minutes = split[0];
                string seconds = "0";
                try
                {
                    double secDouble = Double.Parse(("." + split[1])) * 60;
                    seconds = Math.Round(secDouble).ToString();
                }
                catch { }
                db.UserRecords.Find(userID).TimeOnMatrix = minutes + " min, " + seconds + "sec";
                db.UserRecords.Find(userID).TimeLoggedOff = DateTime.Now;
                db.UserRecords.Find(userID).FinalChoice = finalChoice;

                foreach (var item in uaObj["userActivity"])
                {
                    try
                    {
                        UserActivity ua = new UserActivity();
                        ua.UserId = userID;
                        ua.UserRecord = db.UserRecords.Find(userID);

                        ua.OrderClicked = item["orderClicked"].ToString();
                        try
                        {
                            double timeOnE = Double.Parse(item["timeSpentOnElement"].ToString());
                            ua.TimeSpentOnElement = Math.Round(timeOnE, 1).ToString();
                        }
                        catch
                        {
                            ua.TimeSpentOnElement = "";
                        }
                        ua.ElementClicked = item["elementClicked"].ToString();
                        ua.ElementType = item["elementType"].ToString();
                        ua.ElementValue = item["elementValue"].ToString();

                        ua.Id = Guid.NewGuid().ToString();
                        db.UserActivities.Add(ua);
                        db.SaveChanges();
                        

                    }
                    catch { }
                    
                    
                    

                }
                try
                {

                    List<UserActivity> list = db.UserActivities.Where(x => x.UserId == userID && (x.ElementType).Trim() == "checkBox").ToList<UserActivity>();
               
                    if (list.Last().ToString() != db.UserRecords.Find(userID).FinalChoice.Trim().ToString())
                    {
                        UserActivity fca = new UserActivity();
                        fca.UserId = userID;
                        fca.TimeSpentOnElement = "";
                        fca.ElementType = "checkBox";
                        fca.OrderClicked = (db.UserActivities.Where(x => x.UserId == userID).Count() + 1).ToString();
                        fca.ElementValue = db.UserRecords.Find(userID).FinalChoice.Trim().ToString();
                        fca.ElementClicked = db.UserRecords.Find(userID).FinalChoice.Trim().ToString() + "CheckBox";
                        db.UserActivities.Add(fca);
                        db.SaveChanges();
                    }
                }
                catch { }
            
            //UserRecord user = db.UserRecords.Find(userID);
            /*ErrorLog log = new ErrorLog();
            log.Id = db.ErrorLogs.Count() + 1;
            log.UserId = userID;
            string uaString = uaObj.ToString();

            log.ActivityArray = String.Join("", uaString.Split('"', '\\', '\r', '\n', '[', ']', '{', '}'));
            db.ErrorLogs.Add(log);
            db.SaveChanges();
           int actLength = db.UserActivities.Where(x => x.UserId == userID).Count();
           if(actLength == 0)
           {
               db.UserActivities.Select(x => x.UserId == userID);
               var ye = "d";
               if(uaObj.Count != 0)
               {
                   
               }
           }*/

            return Content("");
        }
        

        [HttpGet]
        public ActionResult SurveyCompleted()
        {
            return Content("Survey Completed");
        }

        public ActionResult DatabaseSpreadSheet()
        {
            CreativeWebDesigndotNetProject_dbEntities db = new CreativeWebDesigndotNetProject_dbEntities();

            /*Workbook workbook = new Workbook();
            Worksheet spreadSheet = new Worksheet("My worksheet");
            spreadSheet.Cells[0, 0] = new Cell("cell1");
            workbook.Worksheets.Add(spreadSheet);
            //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            string constr = ConfigurationManager.ConnectionStrings["database"].ConnectionString;*/
            
            XLWorkbook wb = new XLWorkbook();
           
            //x
            
            List<UserRecord> userRecords = db.UserRecords.Where(x => x.UserActivities.Count()!= 0).ToList<UserRecord>();
            userRecords = userRecords.OrderBy(x => x.TimeLoggedIn).ToList<UserRecord>();
            //List<UserRecord> userRecords = db.UserRecords.Select(x => x).ToList<UserRecord>();
            //
            for (int i = 0; i < userRecords.Count(); i++)
            {
                var worksheet = wb.AddWorksheet("User Record " + i.ToString());
                int j = 1;
                //Add User Info to Excel Sheet
                worksheet.Cell("A" + j.ToString()).Value = "User ID";
                worksheet.Cell("A" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("B" + j.ToString()).Value = "Marital Status";
                worksheet.Cell("B" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("C" + j.ToString()).Value = "Occupation";
                worksheet.Cell("C" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("D" + j.ToString()).Value = "Organization";
                worksheet.Cell("D" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("E" + j.ToString()).Value = "Password";
                worksheet.Cell("E" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("F" + j.ToString()).Value = "Phone Number";
                worksheet.Cell("F" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("G" + j.ToString()).Value = "Role In Organization";
                worksheet.Cell("G" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("H" + j.ToString()).Value = "State";
                worksheet.Cell("H" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("I" + j.ToString()).Value = "Street Address";
                worksheet.Cell("I" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("J" + j.ToString()).Value = "Time Logged In";
                worksheet.Cell("J" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("K" + j.ToString()).Value = "Time Logged Off";
                worksheet.Cell("K" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("L" + j.ToString()).Value = "Time On Matrix";
                worksheet.Cell("L" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("M" + j.ToString()).Value = "Time On Landing Page";
                worksheet.Cell("M" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("N" + j.ToString()).Value = "Zip Code";
                worksheet.Cell("N" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("O" + j.ToString()).Value = "Gender";
                worksheet.Cell("O" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("P" + j.ToString()).Value = "Email";
                worksheet.Cell("P" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("Q" + j.ToString()).Value = "Education";
                worksheet.Cell("Q" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("R" + j.ToString()).Value = "City";
                worksheet.Cell("R" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("S" + j.ToString()).Value = "Age";
                worksheet.Cell("S" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("T" + j.ToString()).Value = "Final Choice";
                worksheet.Cell("T" + j.ToString()).Style.Font.Bold = true;
                j++;

                worksheet.Cell("A" + j.ToString()).Value = userRecords[i].Id;
                worksheet.Cell("B" + j.ToString()).Value = userRecords[i].MaritalStatus;
                worksheet.Cell("C" + j.ToString()).Value = userRecords[i].Occupation;
                worksheet.Cell("D" + j.ToString()).Value = userRecords[i].Organization;
                worksheet.Cell("E" + j.ToString()).Value = userRecords[i].Password;
                worksheet.Cell("F" + j.ToString()).Value = userRecords[i].PhoneNumber;
                worksheet.Cell("G" + j.ToString()).Value = userRecords[i].RoleInOrganization;
                worksheet.Cell("H" + j.ToString()).Value = userRecords[i].State;
                worksheet.Cell("I" + j.ToString()).Value = userRecords[i].StreetAddress;
                worksheet.Cell("J" + j.ToString()).Value = userRecords[i].TimeLoggedIn;
                worksheet.Cell("K" + j.ToString()).Value = userRecords[i].TimeLoggedOff;
                worksheet.Cell("L" + j.ToString()).Value = userRecords[i].TimeOnMatrix;
                worksheet.Cell("M" + j.ToString()).Value = userRecords[i].TimeOnLandingPage;
                worksheet.Cell("N" + j.ToString()).Value = userRecords[i].ZipCode;
                worksheet.Cell("O" + j.ToString()).Value = userRecords[i].Gender;
                worksheet.Cell("P" + j.ToString()).Value = userRecords[i].Email;
                worksheet.Cell("Q" + j.ToString()).Value = userRecords[i].Education;
                worksheet.Cell("R" + j.ToString()).Value = userRecords[i].City;
                worksheet.Cell("S" + j.ToString()).Value = userRecords[i].Age;
                try
                {
                    if (userRecords[i].FinalChoice.Trim() == "")
                    {
                        worksheet.Cell("T" + j.ToString()).Value = "None";
                    }
                    else
                    {
                        worksheet.Cell("T" + j.ToString()).Value = userRecords[i].FinalChoice;
                    }
                }
                catch { worksheet.Cell("T" + j.ToString()).Value = "None"; }
                j = j+2;
                //Add User Activity to Excel Sheet
                
                List<UserActivity> ua = userRecords[i].UserActivities.ToList<UserActivity>();
                ua = ua.OrderBy(x => Int32.Parse(x.OrderClicked)).ToList<UserActivity>();

                worksheet.Cell("A" + j.ToString()).Value = "User's Activities:";
                worksheet.Cell("A" + j.ToString()).Style.Font.Bold = true;
                j++;
                worksheet.Cell("A" + j.ToString()).Value = "Element Clicked";
                worksheet.Cell("A" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("B" + j.ToString()).Value = "Time On Window(sec)";
                worksheet.Cell("B" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("C" + j.ToString()).Value = "Order Clicked";
                worksheet.Cell("C" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("D" + j.ToString()).Value = "Element Type";
                worksheet.Cell("D" + j.ToString()).Style.Font.Bold = true;
                worksheet.Cell("E" + j.ToString()).Value = "Element Value";
                worksheet.Cell("E" + j.ToString()).Style.Font.Bold = true;
                for (int k = 0; k < ua.Count(); k++)
                {
                    j++;
                    worksheet.Cell("A" + j.ToString()).Value = ua[k].ElementClicked;
                    worksheet.Cell("B" + j.ToString()).Value = ua[k].TimeSpentOnElement +"/";
                    
                    worksheet.Cell("C" + j.ToString()).Value = ua[k].OrderClicked +"/";
                    worksheet.Cell("D" + j.ToString()).Value = ua[k].ElementType;
                    worksheet.Cell("E" + j.ToString()).Value = ua[k].ElementValue + "/";
                    
                }

                for (int l = 1; l < 25; l++)
                {
                    worksheet.Column(l).AdjustToContents();
                }
                
            }
            
            
            
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            /*using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM UserRecords"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            
                            sda.Fill(dt);
                            XLWorkbook wb = new XLWorkbook();
                            wb.Worksheets.Add(dt, "User Records");

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }
                    
                }

            }*/

            return Content("");
        }

        public ActionResult FinishedSurvey()
        {
            return View();
        }

    }
}