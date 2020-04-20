using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.MarketChair_Business
{
   public class MarketChairManeger:BaseBusiness<MarketChair>
    {
        /// <summary>
        /// 获取某年的一月至十二月的演讲次数
        /// </summary>
        /// <param name="yearName"></param>
        /// <returns></returns>
        public int[] GetMarcherCount(DateTime yearName)
        {
           List<MarketChair> marketchair_list= this.GetList().Where(m => m.ChairTime <= yearName).ToList();
            int j = 1;
            int[] ary = new int[12];
            for (int i = 0; i < 12; i++)
            {
                ary[j] = GetMonthCount(marketchair_list,j);
                j++;
            }
            return ary;
        }
        /// <summary>
        /// 获取某个月份的演讲数量
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="monthName">月份名称</param>
        /// <returns></returns>
        public int GetMonthCount(List<MarketChair> list,int monthName)
        {
            return list.Select(m => Convert.ToDateTime(m.ChairTime).Month).Where(m => m == monthName).ToList().Count;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="new_m"></param>
        /// <returns></returns>
        public AjaxResult Add_data(MarketChair new_m)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                this.Insert(new_m);
                a.Success = true;
                a.Msg = "添加成功！！！";
            }
            catch (Exception)
            {

                a.Success = false;
                a.Msg = "添加数据异常，请刷新重试！！！";
            }

            return a;
        }

    }
}
