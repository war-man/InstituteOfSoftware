using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    using SiliconValley.InformationSystem.Business.BiddingRecordBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Util;

    public class BiddingRecordController : Controller
    {
        // GET: Market/BiddingRecord/GetBRData
        public ActionResult BiddingIndex()
        {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var elist = emanage.GetList();
            ViewBag.recorder = new SelectList(elist, "EmployeeId", "EmpName");
            return View();
        }
        public ActionResult GetBRData(int page, int limit,string AppCondition) {
            BiddingRecordManage brmanage = new BiddingRecordManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var brlist = brmanage.GetList().Where(b => b.IsDel == false).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string recorder = str[0];
                string BiddingOpponent = str[1];
                string Unit = str[2];
                string Keyword = str[3];
                string start_time = str[4];
                string end_time = str[5];
               
                if (!string.IsNullOrEmpty(recorder))
                {
                    brlist = brlist.Where(a => a.Recorder==recorder).ToList();
                }
                brlist = brlist.Where(a=>a.BiddingOpponent.Contains(BiddingOpponent)).ToList();
                brlist = brlist.Where(a => a.Unit.Contains(Unit)).ToList();
                brlist = brlist.Where(e => e.Keyword.Contains(Keyword)).ToList();
                if (!string.IsNullOrEmpty(start_time))
                {
                    DateTime stime = Convert.ToDateTime(start_time + " 00:00:00.000");
                    brlist = brlist.Where(a => a.ShowTime >= stime).ToList();
                }
                if (!string.IsNullOrEmpty(end_time))
                {
                    DateTime etime = Convert.ToDateTime(end_time + " 23:59:59.999");
                    brlist = brlist.Where(a => a.ShowTime <= etime).ToList();
                }
            }
            var mylist = brlist.OrderBy(n => n.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var newlist = from e in mylist
                          select new
                          {
                              #region 获取属性值 
                              e.Id,
                              recorder= emanage.GetList().Where(s=>s.EmployeeId==e.Recorder).FirstOrDefault().EmpName,
                              e.BiddingOpponent,
                              e.Url,
                              e.Unit,
                              e.Keyword,
                              e.CopywritingOriginality,
                              e.TheFirstBid,
                              e.ShowTime,
                              e.AverageClickPrice,
                              e.IsDel,
                              #endregion
                          };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = brlist.Count(),
                data = newlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

        //竞价记录添加
        public ActionResult AddBiddingRecord() {
            return View();
        }
        [HttpPost]
        public ActionResult AddBiddingRecord(BiddingRecord br) {
            BiddingRecordManage brmanage = new BiddingRecordManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                //br.Recorder=登录的人就是记录的人
                br.Recorder = "201908150004";//到时候再设置为登陆的用户
                br.IsDel = false;
                brmanage.Insert(br);
                AjaxResultxx = brmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = brmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 竞价记录删除(修改isdel为true)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BRremove(string list) {
            BiddingRecordManage brmanage = new BiddingRecordManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                string[] arr = list.Split(',');

                for (int i = 0; i < arr.Length - 1; i++)
                {
                    string id = arr[i];
                    var br = brmanage.GetEntity(int.Parse(id));
                    br.IsDel = true;
                    brmanage.Update(br);
                    AjaxResultxx = brmanage.Success();
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = brmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 竞价记录编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult EditBR(int Id) {
            BiddingRecordManage brmanage = new BiddingRecordManage();
            var br = brmanage.GetEntity(Id);
            ViewBag.id = Id;
            return View(br);
        }
        public ActionResult GetBRById(int id) {
            BiddingRecordManage brmanage = new BiddingRecordManage();
            var br= brmanage.GetEntity(id);
            return Json(br,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditBR(BiddingRecord br) {
            BiddingRecordManage brmanage = new BiddingRecordManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var b = brmanage.GetEntity(br.Id);
                br.Recorder = b.Recorder;
                br.IsDel = b.IsDel;
                brmanage.Update(br);
                AjaxResultxx = brmanage.Success();
            }
            catch (Exception ex)
            {
               AjaxResultxx= brmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

    }
}