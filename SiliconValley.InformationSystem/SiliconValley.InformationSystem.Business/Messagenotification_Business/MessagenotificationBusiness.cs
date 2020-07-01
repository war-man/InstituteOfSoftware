using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Messagenotification_Business
{
    /// <summary>
    /// 发布通知业务类
    /// </summary>
   public class MessagenotificationBusiness:BaseBusiness<Messagenotification>
    {
        //员工表
        EmployeesInfoManage infoBusiness = new EmployeesInfoManage();
        //通知关系业务类
        BaseBusiness<MessagenoEmployeesInfo> MessagenoEmployeesInfoBusiness = new BaseBusiness<MessagenoEmployeesInfo>();
        //通知视图
        BaseBusiness<MessagenotificationView> MessagenotificationViewBusiness = new BaseBusiness<MessagenotificationView>();
        //当前登陆人
        Base_UserModel user = Base_UserBusiness.GetCurrentUser();
        /// <summary>
        /// 获取当前登陆人需要显示的信息
        /// </summary>
        /// <returns></returns>
        public List<MessagenotificationView> DateList()
        {
            List<MessagenotificationView> messagenotificationViews = new List<MessagenotificationView>();
         var x=  MessagenotificationViewBusiness.GetList().Where(a => a.NotifierEmployeeId == user.EmpNumber && a.Readornot == false).ToList();
            foreach (var item in x)
            {
                if (item.Duedate!=null)
                {
                    if (item.Duedate>DateTime.Now)
                    {
                        messagenotificationViews.Add(item);
                    }

                }
                else
                {
                    messagenotificationViews.Add(item);
                }
            }
            return messagenotificationViews;
        }
        /// <summary>
        /// 发布通知消息
        /// </summary>
        /// <param name="Duedate">到期日期</param>
        /// <param name="Conten">内容</param>
        /// <param name="NotifierEmployeeId">通知人（为null则通知全部员工，否则字符串拼接）</param>
        /// <returns></returns>
        public object AddMessagenoti(string Duedate, string Conten,string NotifierEmployeeId)
        {
            AjaxResult retus = null;
            try
            {
           
                retus = new SuccessResult();
                retus.Success = true;
                Messagenotification messagenotification = new Messagenotification();
                messagenotification.Content = Conten;
                messagenotification.Addtime = DateTime.Now;
               
                if (!string.IsNullOrEmpty(Duedate))
                {
                    messagenotification.Duedate = Convert.ToDateTime(Duedate);
                }
                messagenotification.PublisherEmployeeId = user.EmpNumber;
                this.Insert(messagenotification);
                var id = this.GetList().Where(a => a.Addtime.ToUniversalTime().ToString() == messagenotification.Addtime.ToUniversalTime().ToString()).FirstOrDefault().id;
                List<MessagenoEmployeesInfo> MessagenoEmployeesInfolist = new List<MessagenoEmployeesInfo>();
                if (NotifierEmployeeId!=null)
                {
                    var NotifierEmployeeIds = NotifierEmployeeId.Length - 1;

                    string[] x = NotifierEmployeeIds.ToString().Split(',');
                    foreach (var item in x)
                    {
                        MessagenoEmployeesInfo messagenoEmployeesInfo = new MessagenoEmployeesInfo();
                        messagenoEmployeesInfo.NotifierEmployeeId = item;
                        messagenoEmployeesInfo.MessagenoID = id;
                        messagenoEmployeesInfo.Readornot = false;
                        messagenoEmployeesInfo.AddTimn = DateTime.Now;
                        MessagenoEmployeesInfolist.Add(messagenoEmployeesInfo);
                    }
                }
                else
                {
                    var emp = infoBusiness.GetList();

                    foreach (var item in emp)
                    {
                        MessagenoEmployeesInfo messagenoEmployeesInfo = new MessagenoEmployeesInfo();
                        messagenoEmployeesInfo.NotifierEmployeeId = item.EmployeeId;
                        messagenoEmployeesInfo.MessagenoID = id;
                        messagenoEmployeesInfo.Readornot = false;
                        messagenoEmployeesInfo.AddTimn = DateTime.Now;
                        MessagenoEmployeesInfolist.Add(messagenoEmployeesInfo);
                    }
                }
                MessagenoEmployeesInfoBusiness.Insert(MessagenoEmployeesInfolist);
                retus.Msg = "发布成功";
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;
        }

        public object Date(int page, int limit)
        {
         var list=   MessagenotificationViewBusiness.GetList().Select(a=>new { a.id,a.PublisherName,a.Duedate,a.Content,a.Addtime}).ToList();
            var dataList = list.OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = list.Count,
                data = dataList
            };
            return data;
        }

        public object Messageread(int id)
        {
            AjaxResult retus = null;
            try
            {
                var x = MessagenoEmployeesInfoBusiness.GetEntity(id);
                x.AddTimn = DateTime.Now;
                x.Readornot = true;
                MessagenoEmployeesInfoBusiness.Update(x);
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "操作成功";
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            return retus;
        }
    }
}
