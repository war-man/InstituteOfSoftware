using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Util;
using System.Data;
using System.Text;
using System.IO;
using SiliconValley.InformationSystem.Business.Base_SysManage;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class AttendanceStatisticsController : Controller
    {
        RedisCache rc = new RedisCache();
        //考勤统计
        // GET: Personnelmatters/AttendanceStatistics
        public ActionResult AttendanceStatisticsIndex()
        {
            AttendanceInfoManage msrmanage = new AttendanceInfoManage();
            if (msrmanage.GetADInfoData().Where(s => s.IsDel == false).Count() > 0)
            {
                var time = msrmanage.GetADInfoData().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
                string mytime = DateTime.Parse(time.ToString()).Year + "年" + DateTime.Parse(time.ToString()).Month + "月";
                ViewBag.yearandmonth = mytime;

                var deserveday = msrmanage.GetADInfoData().Where(s => s.IsDel == false).FirstOrDefault().DeserveToRegularDays;
                ViewBag.DeserveToRegularDays = deserveday;
            }

            return View();
        }
        //获取考勤数据
        public ActionResult GetCheckingInData(int page, int limit, string AppCondition)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var attlist = attmanage.GetADInfoData().Where(s => s.IsDel == false).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string ename = str[0];
                string deptname = str[1];
                string pname = str[2];
                string empstate = str[3];

                attlist = attlist.Where(e => empmanage.GetInfoByEmpID(e.EmployeeId).EmpName.Contains(ename)).ToList();
                if (!string.IsNullOrEmpty(deptname))
                {
                    attlist = attlist.Where(e => empmanage.GetDeptByEmpid(e.EmployeeId).DeptId == int.Parse(deptname)).ToList();
                }
                if (!string.IsNullOrEmpty(pname))
                {
                    attlist = attlist.Where(e => empmanage.GetInfoByEmpID(e.EmployeeId).PositionId == int.Parse(pname)).ToList();
                }
                if (!string.IsNullOrEmpty(empstate))
                {
                    attlist = attlist.Where(e => empmanage.GetInfoByEmpID(e.EmployeeId).IsDel == bool.Parse(empstate)).ToList();
                }

            }
            var newlist = attlist.OrderBy(s => s.AttendanceId).Skip((page - 1) * limit).Take(limit).ToList();
            var mylist = from e in newlist
                         select new
                         {
                             #region 获取值
                             e.AttendanceId,
                             e.EmployeeId,
                             empName = empmanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             empDept = empmanage.GetDeptByEmpid(e.EmployeeId).DeptName,
                             empPosition = empmanage.GetPositionByEmpid(e.EmployeeId).PositionName,
                             empIsDel = empmanage.GetInfoByEmpID(e.EmployeeId).IsDel,
                             e.YearAndMonth,
                             e.ToRegularDays,
                             e.LeaveDays,
                             e.WorkAbsentNum,
                             e.WorkAbsentRecord,
                             e.OffDutyAbsentNum,
                             e.OffDutyAbsentRecord,
                             NoClockTotalNum = e.WorkAbsentNum + e.OffDutyAbsentNum,
                             e.TardyNum,
                             e.TardyRecord,
                             e.LeaveEarlyNum,
                             e.LeaveEarlyRecord,
                             e.Remark,
                             e.IsDel,
                             e.DeserveToRegularDays,
                             e.NoClockWithhold,
                             e.TardyWithhold,
                             e.LeaveWithhold
                             #endregion

                         };

            var newobj = new
            {
                code = 0,
                msg = "",
                count = attlist.Count(),
                data = mylist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 年月份及应到勤天数的改变
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangeTimeandDays()
        {
            AttendanceInfoManage msrmanage = new AttendanceInfoManage();
            if (msrmanage.GetADInfoData().Where(s => s.IsDel == false).Count() > 0)
            {
                var time = msrmanage.GetADInfoData().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
                string mytime = DateTime.Parse(time.ToString()).Year + "-" + DateTime.Parse(time.ToString()).Month;
                ViewBag.time = mytime;

                var deserveday = msrmanage.GetADInfoData().Where(s => s.IsDel == false).FirstOrDefault().DeserveToRegularDays;
                ViewBag.days = deserveday;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChangeTimeandDays(string CurrentTime, int ShouldComeDays)
        {
            var AjaxResultxx = new AjaxResult();
            AttendanceInfoManage msrmanage = new AttendanceInfoManage();
            try
            {
                var attlist = msrmanage.GetADInfoData().Where(s => s.IsDel == false).ToList();
                for (int i = 0; i < attlist.Count(); i++)
                {
                    attlist[i].YearAndMonth = Convert.ToDateTime(CurrentTime);
                    attlist[i].DeserveToRegularDays = ShouldComeDays;
                    msrmanage.Update(attlist[i]);
                    rc.RemoveCache("InRedisATDData");
                    AjaxResultxx = msrmanage.Success();
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = msrmanage.Error(ex.Message);
            }

            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAttendanceInfo(int id)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            var att = attmanage.GetEntity(id);
            return View(att);
        }
        public ActionResult GetAttById(int id)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var att = attmanage.GetEntity(id);
            var newobj = new
            {
                #region 考勤表赋值
                att.AttendanceId,
                att.EmployeeId,
                empName = emanage.GetInfoByEmpID(att.EmployeeId).EmpName,
                esex = emanage.GetInfoByEmpID(att.EmployeeId).Sex,
                dname = emanage.GetDeptByEmpid(att.EmployeeId).DeptName,
                pname = emanage.GetPositionByEmpid(att.EmployeeId).PositionName,
                att.ToRegularDays,
                att.LeaveDays,
                att.WorkAbsentNum,
                att.WorkAbsentRecord,
                att.OffDutyAbsentNum,
                att.OffDutyAbsentRecord,
                att.TardyNum,
                att.TardyRecord,
                att.LeaveEarlyNum,
                att.LeaveEarlyRecord,
                att.Remark,
                att.YearAndMonth,
                att.DeserveToRegularDays,
                att.IsDel,
                #endregion
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditAttendanceInfo(AttendanceInfo att)
        {
            var AjaxResultxx = new AjaxResult();
            AttendanceInfoManage atmanage = new AttendanceInfoManage();
            try
            {
                var a = atmanage.GetEntity(att.AttendanceId);
                att.YearAndMonth = a.YearAndMonth;
                att.DeserveToRegularDays = a.DeserveToRegularDays;
                att.IsDel = a.IsDel;
                att.EmployeeId = a.EmployeeId;
                atmanage.Update(att);
                rc.RemoveCache("InRedisATDData");
                AjaxResultxx = atmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = atmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchImport()
        {
            return View();
        }

        //获取Excle文件中的值
        public List<AttendanceInfo> GetExcelFunction()
        {
            string namef = SessionHelper.Session["filename"].ToString();//获取要读取的Excel文件名称
            System.Data.DataTable t = AsposeOfficeHelper.ReadExcel(namef, false);//从Excel文件拿值
            List<AttendanceInfo> atdlist = new List<AttendanceInfo>();
            //if (t.Rows[0][0].ToString() == "姓名" && t.Rows[0][1].ToString() == "性别" && t.Rows[0][2].ToString() == "电话" && t.Rows[0][3].ToString() == "学校" && t.Rows[0][4].ToString() == "家庭住址" && t.Rows[0][5].ToString() == "区域" && t.Rows[0][6].ToString() == "信息来源" && t.Rows[0][7].ToString() == "学历" && t.Rows[0][8].ToString() == "备案人" && t.Rows[0][9].ToString() == "备注")
            //{
            //    //需要转型
            //    for (int i = 1; i < (t.Rows.Count); i++)
            //    {
            //        StudentPutOnRecord create_s = new StudentPutOnRecord();
            //        create_s.StuName = t.Rows[i][0].ToString();
            //        create_s.StuSex = t.Rows[i][1].ToString() == "女" ? false : true;
            //        create_s.StuPhone = t.Rows[i][2].ToString();
            //        create_s.StuSchoolName = t.Rows[i][3].ToString();//学校
            //        create_s.StuAddress = t.Rows[i][4].ToString();//家庭住址
            //        create_s.StuIsGoto = false;
            //        create_s.StuStatus_Id = 1;
            //        new_listStudent.Add(create_s);
            //    }
            //    return new_listStudent;
            //}
            //else
            //{
            //    return new_listStudent;
            //}
            return atdlist;
        }

        //处理文件上传的方法
        //public ActionResult IntoFunction()
        //{
        //    StringBuilder ProName = new StringBuilder();
        //    try
        //    {
        //        HttpPostedFileBase file = Request.Files["file"];
        //        string fname = Request.Files["file"].FileName; //获取上传文件名称（包含扩展名）
        //        string f = Path.GetFileNameWithoutExtension(fname);//获取文件名称
        //        string name = Path.GetExtension(fname);//获取扩展名
        //        string pfilename = AppDomain.CurrentDomain.BaseDirectory + "uploadXLSXfile/ConsultUploadfile/";//获取当前程序集下面的uploads文件夹中的excel文件夹目录
        //        //获取当前上传的操作人
        //        string UserName = Base_UserBusiness.GetCurrentUser().UserName;
        //        string completefilePath = f + DateTime.Now.ToString("yyyyMMddhhmmss") + UserName + name;//将上传的文件名称转变为当前项目名称
        //        ProName.Append(Path.Combine(pfilename, completefilePath));//合并成一个完整的路径;
        //        file.SaveAs(ProName.ToString());//上传文件   
        //        SessionHelper.Session["filename"] = ProName.ToString();
        //        List<MyExcelClass> studentlist = GetExcel();
        //        if (studentlist.Count > 0)//如果拿到值说明文件格式是可以读取的
        //        {
        //            var jsondata = new
        //            {
        //                code = "",
        //                msg = "ok",
        //                data = studentlist,
        //            };
        //            return Json(jsondata, JsonRequestBehavior.AllowGet);
        //        }
        //        else //该文件格式不正确
        //        {
        //            var jsondata = new
        //            {
        //                code = "",
        //                msg = "文件格式错误",
        //                data = "",
        //            };
        //            DeleteFile();//如果格式不符合规范则删除上传的文件
        //            return Json(jsondata, JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        BusHelper.WriteSysLog(ee.Message, EnumType.LogType.上传文件);
        //        return Json("no", JsonRequestBehavior.AllowGet);
        //    }

        //}

        //将文件中的内容写入到数据库中
        //public ActionResult IntoServer()
        //{
        //    Excel_Entity = new ExcelHelper();
        //    try
        //    {
        //        List<MyExcelClass> equally_list = Repeatedly(true); //两个数据集合之间比较取交集(挑出相同的)
        //        if (equally_list.Count > 0)
        //        {
        //            //将未重复的数据添加到数据库中
        //            List<MyExcelClass> equally_list2 = Repeatedly(false);
        //            //添加数据
        //            bool mis = AddExcelToServer(equally_list2);
        //            if (mis)
        //            {
        //                //有重复的值                                
        //                //获取当前年月日
        //                string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + equally_list[1].EmployeesInfo_Id + "ErrorExcel.xls";
        //                string path = "~/uploadXLSXfile/ConsultUploadfile/ConflictExcel/" + filename;
        //                SessionHelper.Session["filename2"] = path;
        //                //获取表头数据
        //                string jsonfile = Server.MapPath("/Config/MyExcelClass.json");
        //                System.IO.StreamReader file = System.IO.File.OpenText(jsonfile);
        //                JsonTextReader reader = new JsonTextReader(file);
        //                //转化为JObject
        //                JObject ojb = (JObject)JToken.ReadFrom(reader);

        //                var jj = ojb["MyExcelClass"].ToString();

        //                JObject jo = (JObject)JsonConvert.DeserializeObject(jj);
        //                DataTable user = equally_list.ToDataTable<MyExcelClass>();
        //                //生成字段名称 
        //                List<string> Head = new List<string>();
        //                foreach (DataColumn col in user.Columns)
        //                {
        //                    Head.Add(jo[col.ColumnName].ToString());
        //                }
        //                bool s = Excel_Entity.DaoruExcel(equally_list, Server.MapPath(path), Head);//将有重复的数据写入Excel表格中
        //                if (s)
        //                {
        //                    //成功写入,两个数据进行对比
        //                    //获取已备案的数据集合
        //                    var datajson = new
        //                    {
        //                        resut = "okk",
        //                        msg = "其他数据已导入,以下是有冲突的数据",
        //                        old = SercherStudent(equally_list),
        //                        news = equally_list
        //                    };
        //                    //string number = "13204961361";
        //                    //string smsText = "备案提示:已备案成功，但是有重复数据";
        //                    //string t = PhoneMsgHelper.SendMsg(number, smsText);
        //                    return Json(datajson, JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    //写入出错
        //                    DeleteFile();
        //                    var datajson = new
        //                    {
        //                        resut = "no",
        //                        msg = "系统错误,请重试！！！",

        //                    };
        //                    return Json(datajson, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else
        //            {
        //                var datajson = new
        //                {
        //                    resut = "no",
        //                    msg = "系统错误,请重试！！！",

        //                };
        //                return Json(datajson, JsonRequestBehavior.AllowGet);
        //            }

        //        }
        //        else
        //        {
        //            //没有重复的值
        //            List<MyExcelClass> nochongfu_list = GetExcel();
        //            if (AddExcelToServer(nochongfu_list))
        //            {
        //                var datajson = new
        //                {
        //                    resut = "ok",
        //                };
        //                //通知备案人备案成功
        //                //string number = "13204961361";
        //                //string smsText = "备案提示:已备案成功";
        //                //string t = PhoneMsgHelper.SendMsg(number, smsText);

        //                return Json(datajson, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                var datajson = new
        //                {
        //                    resut = "no",
        //                    msg = "系统错误,请重试！！！",

        //                };
        //                return Json(datajson, JsonRequestBehavior.AllowGet);
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        BusHelper.WriteSysLog(s_Entity.Enplo_Entity.GetEntity(UserName).EmpName + "Excel大批量导入数据时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
        //        var datajson = new
        //        {
        //            resut = "no",
        //            msg = "系统错误,请重试！！！",

        //        };
        ////        return Json(datajson, JsonRequestBehavior.AllowGet);
        //    }
        //}

        /// <summary>
        /// 模板下载 
        /// </summary>
        /// <returns></returns>        
        public FileStreamResult DownFile()
        {
            string rr = Server.MapPath("/uploadXLSXfile/Template/Excle模板.xls");  //获取下载文件的路径         
            FileStream stream = new FileStream(rr, FileMode.Open);
            return File(stream, "application/octet-stream", Server.UrlEncode("ExcleTemplate.xls"));
        }

    }
}