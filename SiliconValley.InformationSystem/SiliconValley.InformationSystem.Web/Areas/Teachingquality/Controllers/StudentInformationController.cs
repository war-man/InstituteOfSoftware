using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.StudentBusiness;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{  //学员信息模块
    [CheckLogin]
    public class StudentInformationController : Controller
    {
        public static int cou = 0;
        public class Student { }

        private readonly StudentInformationBusiness dbtext;
        public StudentInformationController()
        {
            dbtext = new StudentInformationBusiness();
            
        }
        // GET: Teachingquality/StudentInformation
        public ActionResult Index()
        {
            return View();
        }
        //获取网络时间
        public string Date()
        {

            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;

            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse)request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                { if (h == "Date") { datetime = headerCollection[h]; } }
                return datetime;
            }

            catch (Exception) { return datetime; }
            finally
            {
                if (request != null)
                { request.Abort(); }
                if (response != null)
                { response.Close(); }
                if (headerCollection != null)
                { headerCollection.Clear(); }
            }
        }
        //月份前面加个零
        public string Month(int a)
        {

            if (a < 10)
            {
                return "0" + a;
            }
            string c = a.ToString();
            return c;
        }
        //生成学号
        public string StudentID(string IDnumber)
        {
             cou++;
            string mingci = string.Empty;
            DateTime date = Convert.ToDateTime(Date());
            string sfz = IDnumber.Substring(6,8);
             string n=  date.Year.ToString().Substring(2);//获取年份
   
            string y = Month(Convert.ToInt32(date.Month)).ToString();


            // string count = Count().ToString();
            string count = cou.ToString();
            if (count.Length<2)
                mingci = "0000" + count;
            else if (count.Length < 3)
                mingci = "000" + count;
            else if (count.Length < 4)
                mingci = "00" + count;
            else if (count.Length < 5)
                mingci = "0" + count;
            else mingci =  count;

            string xuehao = n + y + sfz+mingci;
            return xuehao;
        }
        //清除学号尾数
        public void myche()
        {
            cou = 0;
           
        }
        //学员注册
        public ActionResult Registeredtrainees()
        {
         //  var x= dbtext.GetList();
            return View();
        }
    }
}