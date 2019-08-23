using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    public class RecordOfConversationController : Controller
    {


        private readonly ConversationBusiness db_conversation;



        public RecordOfConversationController()
        {
            db_conversation = new ConversationBusiness();
        }


        // GET: Teaching/RecordOfConversation
        public ActionResult ConversationIndex()
        {


            return View();
        }



        [HttpPost]
        public ActionResult GetConversationRecord(string begindate, string enddate, string studentname)
        {

            AjaxResult result = new AjaxResult();

            List<ConversationRecordView> resultlist = new List<ConversationRecordView>();

            try
            {
                resultlist = db_conversation.GetScreenConversationRecord(begindate, enddate, studentname);

                result.ErrorCode = 200;
                result.Data = resultlist;
                result.Msg = "成功";




            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = resultlist;
                result.Msg = ex.Message;

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetConversationrecords()
        {

            AjaxResult result = new AjaxResult();
            List<ConversationRecordView> resultlist = new List<ConversationRecordView>();
            try
            {
                var list = db_conversation.GetConversationRecords().OrderByDescending(d=>d.Time).ToList().Take(30);

               

                foreach (var item in list)
                {
                    var tempobj = db_conversation.GetConversationRecordView(item);

                    if (tempobj != null)
                        resultlist.Add(tempobj);
                }


                result.Data = resultlist;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.Data = resultlist;
                result.ErrorCode = 500;
                result.Msg = ex.Message;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 添加访谈记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Operations()
        {

            return View();

        }

    }
}