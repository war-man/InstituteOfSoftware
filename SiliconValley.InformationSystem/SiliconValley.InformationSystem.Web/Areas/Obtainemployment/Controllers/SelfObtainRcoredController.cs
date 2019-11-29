using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    /// <summary>
    /// 自主就业控制器
    /// </summary>
    public class SelfObtainRcoredController : Controller
    {

        private QuarterBusiness dbquarter;
        private EmpQuarterClassBusiness dbempQuarterClass;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProStudentInformationBusiness dbproStudentInformation;
        private SelfObtainRcoredBusiness dbselfObtainRcored;
        private StudentIntentionBusiness dbstudentIntention;
        private EmploymentStaffBusiness dbemploymentStaff;
        // GET: Obtainemployment/SelfObtainRcored
        public ActionResult SelfObtainRcoredIndex()
        {
            return View();
        }

        /// <summary>
        /// 加载左侧的树形
        /// </summary>
        /// <returns></returns>
        public ActionResult EstablishTree()
        {
            dbquarter = new QuarterBusiness();
            dbempQuarterClass = new EmpQuarterClassBusiness();
            //第一层
            var querydata = dbquarter.yearplan();

            //返回的结果
            resultdtree result = new resultdtree();

            //状态
            dtreestatus dtreestatus = new dtreestatus();



            //最外层的儿子数据
            List<dtreeview> childrendtreedata = new List<dtreeview>();

            for (int i = 0; i < querydata.Count; i++)
            {
                //第一层
                dtreeview seconddtree = new dtreeview();
                try
                {
                    //if (i == 0)
                    //{
                    //    seconddtree.spread = true;
                    //}
                    seconddtree.nodeId = querydata[i].Year.ToString();
                    seconddtree.context = querydata[i].YearTitle;
                    seconddtree.last = false;
                    seconddtree.parentId = "0";
                    seconddtree.level = 0;

                    //这是第二层数据
                    var Quarterslist = dbquarter.GetQuartersByYear(querydata[i].Year);
                    if (Quarterslist.Count > 0)
                    {

                        //第二层的tree数据
                        List<dtreeview> Quarterlist = new List<dtreeview>();
                        for (int j = 0; j < Quarterslist.Count; j++)
                        {


                            dtreeview Quarters = new dtreeview();
                            Quarters.nodeId = Quarterslist[j].ID.ToString();
                            Quarters.context = Quarterslist[j].QuaTitle;
                            Quarters.last = false;
                            Quarters.parentId = querydata[i].Year.ToString();
                            Quarters.level = 1;
                            //第三层数据
                            var empQuarterClasslist = dbempQuarterClass.GetEmpQuartersByYearID(Quarterslist[j].ID);
                            if (empQuarterClasslist.Count > 0)
                            {
                                //if (j == 0)
                                //{
                                //    Quarters.spread = true;
                                //}
                                //第三层tree数据
                                List<dtreeview> QuarterClasslist = new List<dtreeview>();
                                foreach (var item1 in empQuarterClasslist)
                                {
                                    dtreeview dtreeview = new dtreeview();
                                    dtreeview.nodeId = item1.ID.ToString();
                                    dtreeview.context = item1.ClassNO;
                                    dtreeview.last = true;
                                    dtreeview.parentId = Quarterslist[j].ID.ToString();
                                    dtreeview.level = 2;
                                    QuarterClasslist.Add(dtreeview);

                                }

                                Quarters.children = QuarterClasslist;
                                Quarterlist.Add(Quarters);
                            }
                            else
                            {
                                Quarters.spread = false;
                                seconddtree.last = true;
                            }

                        }
                        seconddtree.children = Quarterlist;

                    }
                    else
                    {
                        seconddtree.last = true;
                    }
                    dtreestatus.code = "200";
                    dtreestatus.message = "操作成功";
                }
                catch (Exception ex)
                {
                    dtreestatus.code = "1";
                    dtreestatus.code = "操作失败";
                    throw;
                }
                childrendtreedata.Add(seconddtree);
            }

            result.status = dtreestatus;
            result.data = childrendtreedata;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加自主就业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult add(int param0)
        {
            dbempQuarterClass = new EmpQuarterClassBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbselfObtainRcored = new SelfObtainRcoredBusiness();
            var quyeryempquarterclsss = dbempQuarterClass.GetEntity(param0);

            List<ScheduleForTrainees> queryscheduleForTrainees = dbproScheduleForTrainees.GetTraineesByClassNO(quyeryempquarterclsss.ClassNO);
            List<StudentInformation> studentlist = new List<StudentInformation>();
            foreach (var item in queryscheduleForTrainees)
            {
                studentlist.Add(dbproStudentInformation.GetEntity(item.StudentID));
            }
            var query = dbselfObtainRcored.GetSelfObtainsByQuarterID(quyeryempquarterclsss.QuarterID);
            for (int i = studentlist.Count - 1; i >= 0; i--)
            {
                foreach (var item in query)
                {
                    if (studentlist[i].StudentNumber == item.StudentNO)
                    {
                        studentlist.Remove(studentlist[i]);
                        break;
                    }
                }
            }
            var resultStudentlist = studentlist.Select(aa => new
            {
                StudentNumber = aa.StudentNumber,
                Name = aa.Name
            }).ToList();

            ViewBag.studentlist = Newtonsoft.Json.JsonConvert.SerializeObject(resultStudentlist);
            ViewBag.param0 = quyeryempquarterclsss.ClassNO;
            ViewBag.param1 = quyeryempquarterclsss.QuarterID;
            return View();
        }

        /// <summary>
        /// 添加自主就业
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult add(SelfObtainRcored param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {

                dbselfObtainRcored = new SelfObtainRcoredBusiness();
                var name = ImageUpload(param0.StudentNO);
                if (string.IsNullOrEmpty(name))
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "莫乱搞！";
                }
                else
                {
                    dbstudentIntention = new StudentIntentionBusiness();
                    var query = dbstudentIntention.GetIntention(param0.QuarterID, param0.StudentNO);
                    if (query != null)
                    {
                        query.IsDel = false;
                        dbstudentIntention.Update(query);
                    }
                    param0.ImgUrl = name;
                    param0.Date = DateTime.Now;
                    param0.EmpStaffID = 1007;
                    param0.IsDel = false;

                    dbselfObtainRcored.Insert(param0);
                    ajaxResult.Success = true;
                }

            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }


            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

   

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public string ImageUpload(string studentno)
        {
            dbproStudentInformation = new ProStudentInformationBusiness();
            var student = dbproStudentInformation.GetEntity(studentno);
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            var query = dbproScheduleForTrainees.GetTraineesByStudentNumber(studentno);
            StringBuilder ProName = new StringBuilder();
            HttpPostedFileBase file = Request.Files["Image"];
            if (file != null)
            {
                string fname = file.FileName; //获取上传文件名称（包含扩展名）
                string f = Path.GetFileNameWithoutExtension(fname);//获取文件名称
                string name = Path.GetExtension(fname);//获取扩展名
                string pfilename = AppDomain.CurrentDomain.BaseDirectory + "uploadXLSXfile/SelfObtainRcoredImg/";//获取当前程序集下面的uploads文件夹中的文件夹目录
                string completefilePath = query.ClassID + student.Name + name;//将上传的文件名称转变为当前项目名称
                ProName.Append(Path.Combine(pfilename, completefilePath));//合并成一个完整的路径;
                file.SaveAs(ProName.ToString());//上传文件
                return completefilePath;
            }

            else
            {
                return null;
            }
        }

        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="leave">1为年度，2为计划，3为班级</param>
        /// <param name="string1">学生编号</param>
        /// <param name="string2">eg:年度是2019  计划7  班级1801TA</param>
        /// <returns></returns>
        public ActionResult table00(int page, int limit, string leave, string string1, string string2)
        {
            dbselfObtainRcored = new SelfObtainRcoredBusiness();
            dbquarter = new QuarterBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            var data = new List<SelfObtainRcored>();

            switch (leave)
            {
                case "1":
                    var year = int.Parse(string2);
                    data = dbselfObtainRcored.GetSelfObtainRcoredsByYear(year);
                    break;
                case "2":
                    var quarterid = int.Parse(string2);
                    data = dbselfObtainRcored.GetSelfObtainsByQuarterID(quarterid);
                    break;
                case "3":
                    data = dbselfObtainRcored.GetSelfObtainRcoredsByClassno(string2);
                    break;
            }

            if (!string.IsNullOrEmpty(string1))
            {
                List<string> selfobtainid = string1.Split('-').ToList();
                for (int i = data.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < selfobtainid.Count; j++)
                    {
                        if (data[i].ID.ToString() != selfobtainid[j])
                        {
                            if (j == selfobtainid.Count - 1)
                            {
                                data.Remove(data[i]);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            var resultdata = data.Select(a => new
            {
                a.ID,
                a.Date,
                a.ImgUrl,
                a.Remark,
                studnetname = dbproStudentInformation.GetEntity(a.StudentNO).Name,
                empname = dbemploymentStaff.GetEmpInfoByEmpID(a.EmpStaffID).EmpName,
                title = dbquarter.GetEntity(a.QuarterID).QuaTitle,
                classno = dbproScheduleForTrainees.GetTraineesByStudentNumber(a.StudentNO).ClassID
            }).ToList();

            var data1 = resultdata.OrderByDescending(a => a.Date).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = resultdata.Count(),
                data = data1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public ActionResult edit(int param0)
        {
            dbselfObtainRcored = new SelfObtainRcoredBusiness();
            var query = dbselfObtainRcored.GetEntity(param0);
            SelfObtainRcored self = new SelfObtainRcored();
            self.ID = query.ID;
            self.ImgUrl = query.ImgUrl;
            ViewBag.obj = Newtonsoft.Json.JsonConvert.SerializeObject(self);
            return View();
        }

        [HttpPost]
        public ActionResult edit(SelfObtainRcored param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {

                dbselfObtainRcored = new SelfObtainRcoredBusiness();
                var query = dbselfObtainRcored.GetEntity(param0.ID);
                var oldname = AppDomain.CurrentDomain.BaseDirectory + "uploadXLSXfile/SelfObtainRcoredImg/" + query.ImgUrl;
                if (this.DeleteImgFile(oldname))
                {
                    var name = ImageUpload(query.StudentNO);
                    if (string.IsNullOrEmpty(name))
                    {
                        ajaxResult.Success = false;
                        ajaxResult.Msg = "莫乱搞！";
                    }
                    else
                    {
                        query.ImgUrl = name;
                        dbselfObtainRcored.Update(query);
                        ajaxResult.Success = true;
                    }
                }
                else
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "请联系信息部成员！";
                }
               

            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }


            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        public bool DeleteImgFile(string fileUrl)
        {
            try
            {
               
                if (System.IO.File.Exists(fileUrl))
                {
                    System.IO.File.Delete(fileUrl);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult del(int param0) {
            
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbselfObtainRcored = new SelfObtainRcoredBusiness();
                var query = dbselfObtainRcored.GetEntity(param0);
                var oldname = AppDomain.CurrentDomain.BaseDirectory + "uploadXLSXfile/SelfObtainRcoredImg/" + query.ImgUrl;
                if (this.DeleteImgFile(oldname))
                {
                   
                        query.IsDel = true;
                        dbselfObtainRcored.Update(query);
                        ajaxResult.Success = true;
                    
                }
                else
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "请联系信息部成员！";
                }
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}