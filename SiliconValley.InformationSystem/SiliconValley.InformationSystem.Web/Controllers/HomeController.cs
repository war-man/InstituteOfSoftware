using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Util;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
namespace SiliconValley.InformationSystem.Web.Controllers
{
    public class users
    {
        public int ID { get; set; }

        public string name { get; set; }

        public int age  { get; set; }

        public DateTime birthday { get; set; }
    }
    //[CheckLogin]
    public class HomeController : Controller
    {
      // /Home/DataSetToExcel
        public ActionResult Index()
        {
            // Business.Base_SysManage.Base_UserBusiness buser = new Business.Base_SysManage.Base_UserBusiness();
            // var list= buser.GetList();
            //return View(list);
            Business.Base_SysManage.Base_SysRoleBusiness brole = new Business.Base_SysManage.Base_SysRoleBusiness();
            var list = brole.GetList();
            return View(list);
        }

        public ActionResult About()
        {

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TEXT()
        {
            return View();
        }

      

        public ActionResult test()
        {
            return View();
        }
        public ActionResult DataSetToExcel()
        {
          // string ss= ConfigHelper.GetConnectionString("BaseDb");

            //SqlConnection con = new SqlConnection("server=.;database=exsil;uid=sa;pwd=123;");
            //string sql = "select * from users";
            //con.Open();
            //SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            //DataSet ds = new DataSet();
            //adapter.Fill(ds);

           // DataTable user = ds.Tables[0];

            List<users> users = new List<users>();
            users.Add(new users() { ID = 1, name = "唐敏", age = 19, birthday = "2000-08-24".ToDateTime() });
            users.Add(new users() { ID = 1, name = "唐da敏", age = 19, birthday = "2000-08-24".ToDateTime() });
            users.Add(new users() { ID = 1, name = "唐xiao敏", age = 19, birthday = "2000-08-24".ToDateTime() });
            users.Add(new users() { ID = 1, name = "唐mm敏", age = 19, birthday = "2000-08-24".ToDateTime() });
            DataTable user = users.ToDataTable<users>();
            var userbaty = AsposeOfficeHelper.DataTableToExcelBytes(user);

           
            int rowNumber = user.Rows.Count;
            int columnNumber = user.Columns.Count;
            int colIndex = 0;
            if (rowNumber == 0)
            {

                return Content("d"); ;
            }
            

            //建立Excel对象 
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            
            excel.Application.Workbooks.Add(true);
            //excel.Application.Workbooks.Open()
            excel.Visible = true;//是否打开该Excel文件 

            //读取用户字段
            string jsonfile = Server.MapPath("/Config/user.json");

            System.IO.StreamReader file = System.IO.File.OpenText(jsonfile);
            //加载问卷
            JsonTextReader reader = new JsonTextReader(file);

            //转化为JObject
            JObject ojb = (JObject)JToken.ReadFrom(reader);

            var jj= ojb["user"].ToString();

            JObject jo = (JObject)JsonConvert.DeserializeObject(jj);
            //生成字段名称 
            foreach (DataColumn col in user.Columns)
            {
                colIndex++;
                excel.Cells[1, colIndex] = jo[col.ColumnName] ;
            }

            //填充数据 
            for (int c = 1; c <=rowNumber; c++)
            {

                for (int j = 0; j < columnNumber; j++)
                {
                    excel.Cells[c + 1, j + 1] = user.Rows[c-1].ItemArray[j];
                }
            }

            return Content("ds");
        }

        public ActionResult DATATEST()
        {

          

           var  ss= Request.Files[0].FileName;

            Request.Files[0].SaveAs(Server.MapPath("/uploadXLSXfile/"+ss));

            //insert(ss);
            DataTable table = AsposeOfficeHelper.ReadExcel(Server.MapPath(@"\uploadXLSXfile\" + ss));

            //SessionHelper.Session["data"] = table;
            //SessionHelper.Session["url"] = ss;


            List<Base_User> userlist = new List<Base_User>();

            foreach (DataRow item in table.Rows)
            {
                Base_User user = new Base_User();
                user.RealName = item["姓名"].ToString();
                user.Birthday = DateTime.Parse(item["生日"].ToString());

                userlist.Add(user);
            }

            return View("show",userlist);
        }


        public ActionResult show()
        {
            DataTable table = SessionHelper.Session["data"] as DataTable;

            List<Base_User> userlist = new List<Base_User>();

            foreach (DataRow item in table.Rows)
            {
                Base_User user = new Base_User();
                user.RealName = item["Name"].ToString();
                user.Birthday =DateTime.Parse( item["birthday"].ToString());

                userlist.Add(user);
            }


            return View(userlist);


        }

        public ActionResult insert()
        {
            string filename= SessionHelper.Session["url"].ToString();
            DataTable table= AsposeOfficeHelper.ReadExcel(Server.MapPath(@"\uploadXLSXfile\" + filename));

            SqlConnection con = new SqlConnection("server=.;database=exsil;uid=sa;pwd=123;");

            con.Open();
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con);

            sqlBulkCopy.DestinationTableName = "users";

            sqlBulkCopy.BatchSize = table.Rows.Count;
            sqlBulkCopy.ColumnMappings.Add("序号", "ID");
            sqlBulkCopy.ColumnMappings.Add("姓名", "name");
            sqlBulkCopy.ColumnMappings.Add("年龄", "age");
            sqlBulkCopy.ColumnMappings.Add("生日", "birthday");

            sqlBulkCopy.WriteToServer(table);

            return Content("ok");
        }


    }
}