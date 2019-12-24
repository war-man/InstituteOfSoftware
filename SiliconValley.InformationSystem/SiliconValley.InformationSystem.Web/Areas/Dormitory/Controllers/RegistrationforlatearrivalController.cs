using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Dormitory.Controllers
{
    [CheckLogin]
    /// <summary>
    /// 晚归控制器
    /// </summary>
    public class RegistrationforlatearrivalController : Controller
    {
        private dbprosutdent_dbproheadmaster dbprosutdent_Dbproheadmaster;
        private NotreturningLateBusiness dbnotreturn;
        private InstructorListBusiness dbinstructorList;
        private NotreturningLateViewBusiness dbnotreturningLateViewBusiness;
        private ProStudentInformationViewBusiness dbproStudentInformationViewBusiness;
        // GET: Dormitory/Registrationforlatearrival
        public ActionResult RegistrationforlatearrivalIndex()
        {
            return View();
        }

        /// <summary>
        /// 加载表格数据0
        /// </summary>
        /// <returns></returns>
        public ActionResult loadtable0(int page, int limit, string name0,DateTime startime,DateTime endtime) {
            dbnotreturningLateViewBusiness = new NotreturningLateViewBusiness();
            var list0= dbnotreturningLateViewBusiness.GetNotreturningLateView(name0,startime,endtime);
            var data = list0.OrderByDescending(a => a.RegisterTime).Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = list0.Count(),
                data = data
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 加载数据表1
        /// </summary>
        /// <returns></returns>
        public ActionResult loadtable1(int page, int limit, string name1) {
            dbproStudentInformationViewBusiness = new ProStudentInformationViewBusiness();
            var list0= dbproStudentInformationViewBusiness.GetProStudentInformationViews(name1);
            var data = list0.OrderByDescending(a => a.StudentNmber).Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = list0.Count(),
                data = data
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 晚归登记
        /// </summary>
        /// <param name="notreturningLate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LateReturn(NotreturningLate notreturningLate)
        {
            AjaxResult ajaxResult = new AjaxResult();
            dbprosutdent_Dbproheadmaster = new dbprosutdent_dbproheadmaster();
            dbnotreturn = new NotreturningLateBusiness();
            dbinstructorList = new InstructorListBusiness();
            Headmaster queryheadmaster = dbprosutdent_Dbproheadmaster.GetHeadmasterByStudentNumber(notreturningLate.StudentNumber);
            notreturningLate.AddTime = DateTime.Now;
            notreturningLate.HeadMasterID = queryheadmaster.ID;


            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var query = dbinstructorList.GetInstructorByempid(user.EmpNumber);
            notreturningLate.Inspector = query.ID;

            notreturningLate.IsDelete = false;

            if (dbnotreturn.AddNotreturningLate(notreturningLate))
            {
                ajaxResult.Data = "";
                ajaxResult.Success = true;
            }
            else
            {
                ajaxResult.Data = "";
                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}