using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.StuSatae_Maneger
{
   public class StuStateManeger:BaseBusiness<StuStatus>
    {
        /// <summary>
        /// 这是根据名称找状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AjaxResult GetStu(string name)
        {
            AjaxResult a = new AjaxResult();
            StuStatus find_s= this.GetList().Where(s => s.StatusName == name).FirstOrDefault();
            if (find_s!=null)
            {
                a.Data = find_s;
                a.Success = true;
            }
            else
            {
                a.Success = false;
            }
            return a;
        }        
        /// <summary>
        /// 通过主键找值
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public AjaxResult GetIdGiveName(string id,bool IsKey)
        {
            AjaxResult new_a = new AjaxResult();           
            if (IsKey)
            {
                //主键
                int Id = Convert.ToInt32(id);
                StuStatus s= this.GetEntity(Id);
                if (s!=null)
                {
                    new_a.Success = true;
                    new_a.Data = s;
                }
                else
                {
                    new_a.Success = false;
                }
                
            }
            else
            {
                //通过名称查询
                 StuStatus s1= this.GetList().Where(s => s.StatusName == id).FirstOrDefault();
                if (s1!=null)
                {
                    new_a.Data = s1;
                    new_a.Success = true;
                }
                else
                {
                    new_a.Success = false;
                }
            }                       
                return new_a;            
        }


        public AjaxResult Add_Data(StuStatus new_s)
        {
            AjaxResult a = new AjaxResult();
            try
            {
               StuStatus find= this.GetList().Where(s => s.StatusName == new_s.StatusName).FirstOrDefault();
                if (find==null)
                {
                    this.Insert(new_s);
                    a.Msg = "添加成功！！";
                    a.Success = true;
                }
                else
                {
                    a.Success = false;
                    a.Msg = "已有该状态名称！！";
                }
                
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = "数据错误，请刷新重试!!";
            }

            return a;
        }
    }
}
