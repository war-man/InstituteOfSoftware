using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Shortmessage_Business
{
    //短信模板业务类型
  public  class ShortmessageBusiness:BaseBusiness<Shortmessage>
    {
        //短信模板类型
        BaseBusiness<ShortmessageType> ShortmessageTypeBusiness = new BaseBusiness<ShortmessageType>();
        /// <summary>
        /// 短信模板操作,直接调用即可实现覆盖添加等
        /// </summary>
        /// <param name="TypeName">模板类型名称(学费催费)</param>
        /// <param name="content">模板内容</param>
        /// <returns></returns>
        public AjaxResult EntiShortmessage(string TypeName, string content)
        {
            AjaxResult result = null;
            try
            {
                result = new SuccessResult();
              
               var fineshort= ShortmessageTypeBusiness.GetList().Where(a=>a.IsDelete==false&&a.TypeName==TypeName).FirstOrDefault();
                if (fineshort!=null)
                {
                    EntiShortmessages(fineshort.id, content);
                }
                else
                {
                    ShortmessageType shortmessageType = new ShortmessageType();
                    shortmessageType.IsDelete = false;
                    shortmessageType.TypeName = TypeName;
                    ShortmessageTypeBusiness.Insert(shortmessageType);
                    BusHelper.WriteSysLog("数据添加(短信模板类型)", Entity.Base_SysManage.EnumType.LogType.添加数据);
                    fineshort = ShortmessageTypeBusiness.GetList().Where(a => a.IsDelete == false && a.TypeName == TypeName).FirstOrDefault();
                    EntiShortmessages(fineshort.id, content);
                }
                result.Msg = "保存成功";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result = new ErrorResult();
                result.Msg = "服务器错误";
                result.Success = false;
                result.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return result;
           
        }
        /// <summary>
        /// 短信模板保存
        /// </summary>
        /// <param name="Typeid">类型id</param>
        /// <param name="content">模板内容</param>
        public void EntiShortmessages(int Typeid,string content)
        {
            var x = this.GetList().Where(a => a.IsDelete == false && a.TypeID == Typeid).FirstOrDefault();
            if (x == null)
            {
                Shortmessage shortmessage = new Shortmessage();
                shortmessage.IsDelete = false;
                shortmessage.content = content;
                shortmessage.AddDate = DateTime.Now;
                shortmessage.TypeID = Typeid;
                this.Insert(shortmessage);
                BusHelper.WriteSysLog("数据添加(短信模板数据)", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            else
            {
                x.content = content;
                this.Update(x);
                BusHelper.WriteSysLog("数据编辑(短信模板数据)", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
        }
        /// <summary>
        /// 通过短信模板类型名称获取模板内容
        /// </summary>
        /// <param name="TypeName">类型名称</param>
        /// <returns></returns>
        public Shortmessage FineShortmessage(string TypeName)
        {
           var x= ShortmessageTypeBusiness.GetList().Where(a => a.IsDelete == false && a.TypeName == TypeName).FirstOrDefault();
            if (x!=null)
            {
               return this.GetList().Where(a => a.IsDelete == false && a.TypeID == x.id).FirstOrDefault();
            }
            else
            {
                return new Shortmessage();
            }
          
        }
    }
}
