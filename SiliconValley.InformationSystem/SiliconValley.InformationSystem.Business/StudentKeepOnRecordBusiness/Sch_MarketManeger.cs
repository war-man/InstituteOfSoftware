using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness
{
   public class Sch_MarketManeger : BaseBusiness<Sch_Market>
    {

       //添加
       public bool AddData(List<Sch_Market>  list)
        {
            bool s = false;
            try
            {
                this.Insert(list);
                s = true;
            }
            catch (Exception)
            {

                
            }
            return s;
        }

      /// <summary>
      /// 编辑数据
      /// </summary>
      /// <param name="nes"></param>
      /// <returns></returns>
        public AjaxResult MyUpdate(Sch_Market data)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                this.Update(data);
                a.Success = true;
                a.Msg = "操作成功！！";
            }
            catch (Exception)
            {
                a.Success = false;
                a.Msg = "系统错误，请重试";
            }

            return a;
        }

    
        public AjaxResult MyUpdate(List<Sch_Market> data)
        {
            AjaxResult a = new AjaxResult();

            try
            {
                this.Update(data);
                a.Success = true;
            }
            catch (Exception)
            {

                a.Success = false;
            }

            return a;
        }

        /// <summary>
        /// 根据姓名、电话号码获取备案数据
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public Sch_Market Find(string Name,string Phone)
        {
           List<Sch_Market> list= this.GetListBySql<Sch_Market>("select top 1 * from Sch_Market where StudentName = '" + Name + "' and Phone = '" + Phone + "'");

            return list.Count > 0 ? list[0] : null;
        }

    }
}
