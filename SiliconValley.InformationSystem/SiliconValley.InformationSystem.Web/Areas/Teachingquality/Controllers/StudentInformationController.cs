using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;

using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Newtonsoft.Json;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{  //学员信息模块
    [CheckLogin]
    public class StudentInformationController : Controller
    {
        //拆班记录
        BaseBusiness<RemovalRecords> Dismantle = new BaseBusiness<RemovalRecords>();
        //备案提供方法
        StudentDataKeepAndRecordBusiness dataKeepAndRecordBusiness = new StudentDataKeepAndRecordBusiness();
        //班级表
        ClassScheduleBusiness classschedu = new ClassScheduleBusiness();
        //学员班级表
        ScheduleForTraineesBusiness Stuclass = new ScheduleForTraineesBusiness();
        //班级阶段
        BaseBusiness<Grand> MyGrand = new BaseBusiness<Grand>();
        //专业
        SpecialtyBusiness Techarcontext = new SpecialtyBusiness();

        private static int NameKeysid = 0;
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
      
       //图片
       public JsonResult img()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase f = Request.Files["file"];//最简单的获取方法
                string name = "2017.jpg";
  
                f.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Areas/Teachingquality/StuImages/"+name);//保存图片

                //这下面是返回json给前端 
                var data1 = new
                {
                    src = AppDomain.CurrentDomain.BaseDirectory + "Areas/Teachingquality/StuImages/" + name ,//服务器储存路径
                };
                var Person = new
                {
                    code = 0,//0表示成功
                    msg = "",//这个是失败返回的错误
                    data = data1
                };
                return Json(Person);//格式化为json
            }
            else
            {
                return null;
            }
        }
        //生成学号
        public string StudentID(string IDnumber)

        {
           
            string mingci = string.Empty;
            DateTime date = Convert.ToDateTime(Date());
            //当前年份
            string n = date.Year.ToString().Substring(2);//获取年份

            //学员总数
            var laststr = dbtext.Mylist("StudentInformation").Where(a => Convert.ToDateTime(a.InsitDate).Year.ToString().Substring(2).ToString() == n&& a.IsDelete != true).Count()+1;
            string sfz = IDnumber.Substring(6,8);

            string y = Month(Convert.ToInt32(date.Month)).ToString();
            // string count = Count().ToString();
            string count = laststr.ToString();
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
        
        //复杂日期截取
        public string Datetimes(DateTime InsitDate)
        {
            string[] dat =InsitDate.ToString().Split('/');
            string c = Month(int.Parse(dat[1]));
            string ri = dat[2];
            string[] ri1 = ri.Split(' ');
            string riqi = Month(int.Parse(ri1[0]));
            string data = dat[0] + "/" + c + "/" + riqi;
            return data;
        }

      
        //学员注册编辑
        public ActionResult Registeredtrainees()
        {
          
            var Dismantl = Dismantle.GetList().Where(a => a.IsDelete == false).ToList();
            var List = classschedu.GetList().Where(a => a.IsDelete == false && a.ClassStatus == false).ToList();
            foreach (var item in Dismantl)
            {
                List = List.Where(a => a.ClassNumber != item.FormerClass).ToList();
            }
            ViewBag.List = List.Select(a => new SelectListItem { Text = a.ClassNumber, Value = a.ClassNumber });
            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id)&&id!= "undefined")
            {
                
                ViewBag.Name = "编辑学员信息";
                ViewBag.StudentID = id;
                return View();

            }
            else
            {
               
                ViewBag.Name = "注册学员信息";
                ViewBag.StudentID = false;
                return View();
            }
           
        }
        //获取所有数据
        public ActionResult GetDate(int page, int limit,string Name,string Sex,string StudentNumber,string identitydocument)
        {
            //  List<StudentInformation>list=  dbtext.GetPagination(dbtext.GetIQueryable(),page,limit, dbtext)
            //List<StudentInformation> list = dbtext.GetList().Where(a=>a.IsDelete!=true).ToList();
              List<StudentInformation> list = dbtext.Mylist("StudentInformation").Where(a=>a.IsDelete!=true&&a.State==null).ToList();
            try
            {
              
                if (!string.IsNullOrEmpty(Name))
                {
                    list = list.Where(a => a.Name.Contains(Name)).ToList();
                }
                if (!string.IsNullOrEmpty(Sex))
                {
                    bool sex = Convert.ToBoolean(Sex);
                    list = list.Where(a => a.Sex == sex).ToList();
                }
                if (!string.IsNullOrEmpty(StudentNumber))
                {
                    list = list.Where(a => a.StudentNumber.Contains(StudentNumber)).ToList();
                }
                if (!string.IsNullOrEmpty(identitydocument))
                {
                    list = list.Where(a => a.identitydocument.Contains(identitydocument)).ToList();
                }

              
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据);

                
            }
            var dataList = list.OrderBy(a => a.StudentNumber).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = list.Count,
                data = dataList
            };
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        //学员备案查询赋值信息
        [HttpGet]
        public ActionResult DataKey()
        {
           ViewBag.Name = Request.QueryString["Name"];
            StudentDataKeepAndRecordBusiness dbctext = new StudentDataKeepAndRecordBusiness();

            return View();
        }
        //备案查询
        public ActionResult DataKeys()
        {
            string Name = Request.QueryString["Name"];
            List<StudentPutOnRecord> x = null;
            try
            {
                StudentDataKeepAndRecordBusiness dbctexta = new StudentDataKeepAndRecordBusiness();
                x = dbctexta.GetList().Where(a => a.StuName == Name).ToList();
                var studentlist = dbtext.StudentList();
                for (int i = x.Count-1; i >=0; i--)
                {
                    if (x.Count>0)
                    {
                        foreach (var item in studentlist)
                        {
                            if (x[i].Id==item.StudentPutOnRecord_Id)
                            {
                                x.Remove(x[i]);
                            }
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {

            BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据);
            }
         
            var data = new
            {
                code = "",
                msg = "",
                count = x.Count,
                data = x
            };
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        //查询赋值
        public  ActionResult NameKeys(int id)
        {
            NameKeysid = id;
            StudentDataKeepAndRecordBusiness dbctext = new StudentDataKeepAndRecordBusiness();
            try
            {
                var x = dbctext.GetList().Where(a => a.Id == id).FirstOrDefault();

                return Json(x, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据);
                return Json("加载数据异常", JsonRequestBehavior.AllowGet);
            }
      
        }
        //以身份证查询是否有重复学员
        public bool Isidentitydocument(string identitydocument)
        {
        
           var x= dbtext.Mylist("StudentInformation").Where(a => a.identitydocument == identitydocument&& a.IsDelete != true).ToList();
            if (x.Count>0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
        //注册学员编辑学员
        public ActionResult Enti(StudentInformation studentInformation,string List)
        {
           
            
              AjaxResult result = null;
            if (studentInformation.StudentNumber == null)
            {
                if (Isidentitydocument(studentInformation.identitydocument))
                {

                    try
                    {
                        
                        studentInformation.StudentNumber = StudentID(studentInformation.identitydocument);
                        studentInformation.InsitDate = DateTime.Now; 
                        studentInformation.Password = "000000";
                        studentInformation.StudentPutOnRecord_Id = NameKeysid;
                        studentInformation.IsDelete = false;
                        dataKeepAndRecordBusiness.ChangeStudentState(NameKeysid);
                        dbtext.Insert(studentInformation);
                        ScheduleForTrainees scheduleForTrainees = new ScheduleForTrainees();
                        scheduleForTrainees.ClassID = List;//班级名称
                        scheduleForTrainees.CurrentClass = true;
                        scheduleForTrainees.StudentID = studentInformation.StudentNumber;
                        scheduleForTrainees.AddDate = DateTime.Now;
                        Stuclass.Insert(scheduleForTrainees);
                        Stuclass.Remove("ScheduleForTrainees");
                        result = new SuccessResult();
                        result.Msg = "注册成功";
                        result.Success = true;
                        result.Data = studentInformation.StudentNumber;
                        dbtext.Remove("StudentInformation");
                        BusHelper.WriteSysLog("注册学员成功", Entity.Base_SysManage.EnumType.LogType.添加数据);

                    }
                    catch (Exception ex)
                    {
                        result = new ErrorResult();
                        result.ErrorCode = 500;
                        result.Msg = "服务器错误1";

                        BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                     
                    }
                }
                else
                {
                    result = new SuccessResult();
                    result.Success = false;
                    result.Msg = "身份证重复";
                }
            }
            else
            {
             try
                    {
                    StudentInformation x = Finds(studentInformation.StudentNumber);
                    studentInformation.Password = x.Password;
                    studentInformation.StudentPutOnRecord_Id = x.StudentPutOnRecord_Id;
                    studentInformation.InsitDate = x.InsitDate;
                   
                    dbtext.Update(studentInformation);
                    result = new SuccessResult();
                        result.Msg = "修改成功";
                        result.Success = true;
                    dbtext.Remove("StudentInformation");
                    BusHelper.WriteSysLog("修改学员信息成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                }
                    catch (Exception ex)
                    {
                        result = new ErrorResult();
                        result.ErrorCode = 500;
                        result.Msg = "服务器错误";

                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                  
                }
                }
             
           
       
            return Json( result,JsonRequestBehavior.AllowGet);
        }
        //按学号查询单个学员 find
        public StudentInformation Finds(string id)
        {
            var x = dbtext.Mylist("StudentInformation").Where(a => a.StudentNumber == id&& a.IsDelete != true).FirstOrDefault();
            return x;
        }
        //按学号查询单个学员返回json格式
        public ActionResult Select()
        {
            string stuid = Request.QueryString["stuid"];
            var a = Finds(stuid);
            var ClassID = Stuclass.Mylist("ScheduleForTrainees").Where(c => c.StudentID == a.StudentNumber && c.CurrentClass == true).First().ClassID;
            var x = new
            {
                a.Familyphone,
                StudentNumber = a.StudentNumber,//学号
                Name = a.Name,//姓名
                InsitDate = a.InsitDate,//入校时间
                Education = a.Education,//学历
                Telephone = a.Telephone,//电话
                Familyaddress = a.Familyaddress,//家庭住址
                WeChat = a.WeChat,//微信
                qq = a.qq,//qq
                BirthDate = a.BirthDate,//出生日期
                identitydocument = a.identitydocument,//身份证号码
                Reack = a.Reack,//备注
                Traine = a.Traine,//学员性格
                Hobby = a.Hobby,//兴趣爱好
                Nation = a.Nation,//民族
                Sex = a.Sex,//性别,
                Guardian=a.Guardian,//亲属
                AddDate = Stuclass.Mylist("ScheduleForTrainees").Where(c => c.StudentID == a.StudentNumber && c.CurrentClass == true).First().AddDate,//入班时间
               classa = classschedu.GetList().Where(q=> q.IsDelete == false && q.ClassStatus == false&&q.ClassNumber== ClassID).FirstOrDefault().ClassNumber//班级号
                                  //a => a.IsDelete == false && a.ClassStatus == false
            };
            
                //classab = classschedu.GetList().Where(w => w.IsDelete == false&&w.ClassNumber== Stuclass.GetList().Where(c => c.StudentID == a.StudentNumber && c.CurrentClass == false).First().ClassID && w.ClassStatus == false).FirstOrDefault().ClassNumber //班级名称
                //a => a.IsDelete == false && a.ClassStatus == false
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Viewdetails()
        {
            string stuid = Request.QueryString["id"];
            ViewBag.stuid = stuid;
            return View();
        }
        //修改密码视图
        [HttpGet]
        public ActionResult Password()
        {
            string stuid = Request.QueryString["stuid"];
            ViewBag.Password = stuid;
            return View();
        }
        //修改密码方法
        [HttpPost]
        public ActionResult Password(StudentInformation student)
        {
          var x=  dbtext.Mylist("StudentInformation").Where(a => a.StudentNumber == student.StudentNumber&& a.IsDelete != true).FirstOrDefault();
            x.Password = student.Password;
            AjaxResult result = null;
            try
            {
                dbtext.Update(x);
                result = new SuccessResult();
                result.Success = true;
                result.Msg = "修改成功";
                dbtext.Remove("StudentInformation");
                BusHelper.WriteSysLog("修改学员密码成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            catch (Exception ex)
            {

                result = new ErrorResult();
                result.ErrorCode = 500;
                result.Msg = "服务器错误";
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
              
            }
          return Json(result, JsonRequestBehavior.AllowGet);
        }
        //查询备案是否存在
        public ActionResult Beian()
        {
            string Name = Request.QueryString["Name"];
            StudentDataKeepAndRecordBusiness dbctexta = new StudentDataKeepAndRecordBusiness();
            var x = dbctexta.GetList().Where(a => a.StuName == Name).ToList();
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        //根据阶段查找专业
        public ActionResult Stage(int id)
        {
          

            return Json(dbtext.Stage(id), JsonRequestBehavior.AllowGet);
        
        }
        //学费赋值
        public ActionResult GetFeestandard(int Stage,int Major_Id)
        {
            //int Stage = Convert.ToInt32(Request.QueryString["Stage"]);
            //int Major_Id = Convert.ToInt32(Request.QueryString["Major_Id"]);
          return Json(  dbtext.GetFeestandard(Stage, Major_Id),JsonRequestBehavior.AllowGet);
        }
        //迟交信息
        public ActionResult Latetuitionfee(int id)
        {
            return Json(dbtext.Latetuitionfee(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Studenttuitionfeestandard(int id)
        {
            return Json(dbtext.Studenttuitionfeestandard(id),JsonRequestBehavior.AllowGet);
        }
        //添加学员照片规格宽144高192
        [HttpGet]
        public ActionResult AddStudentimg()
        {
            string studentid= Request.QueryString["studentid"]; ;
            ViewBag.studentid = studentid;
            ViewBag.student = JsonConvert.SerializeObject( dbtext.StuClass(studentid));
            ViewBag.Picture = dbtext.GetEntity(studentid).Picture == null ? "" : dbtext.GetEntity(studentid).Picture;
            return View();
        }
        //添加学员照片规格宽144高192
        [HttpPost]
        public ActionResult AddStudentimg(string id)
        {
        //    string studentid = Request.QueryString["studentid"];
            // Bitmap bitmap = new Bitmap(144,192);
            AjaxResult result = new AjaxResult();
            try
            {
                var fien =  Request.Files[0];
             string filename = fien.FileName;
            string Extension = Path.GetExtension(filename);
            string newfilename =id + Extension;
               
                 
                if (dbtext.StudentAddImg(id, newfilename)==true)
                {

                    result = new SuccessResult();
                    result.ErrorCode = 200;
                    string path = Server.MapPath("~/Areas/Teachingquality/studentImg/" + newfilename);
                   
                      fien.SaveAs(path);
                 
                    //  bitmap.Save(path);
                }
            }
            catch (Exception ex)
            {

                result = new SuccessResult();
                result.ErrorCode = 300;
            }


            return Json(result,JsonRequestBehavior.AllowGet);
        }
       //验证图片是否已经有了
        public int boolImg(string studentid)
        {
          return  dbtext.boolImg(studentid);
        }
      
    }
}