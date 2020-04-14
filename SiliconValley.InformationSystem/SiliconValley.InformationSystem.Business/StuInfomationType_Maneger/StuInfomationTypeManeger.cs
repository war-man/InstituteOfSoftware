using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.StuInfomationType_Maneger
{
   public class StuInfomationTypeManeger:BaseBusiness<StuInfomationType>
    {
        public StuInfomationType SerchSingleData(string id,bool IsKey)
        {
            StuInfomationType s = new StuInfomationType();
            if (IsKey)
            {
                //是主键
                int Id = Convert.ToInt32(id);
                s=this.GetEntity(Id);
            }
            else
            {
                //不是主键
                s= this.GetList().Where(ss => ss.Name == id).FirstOrDefault();
            }           
            return s;
        }

        //这个方法是用于通过名字来查询信息来源Id的
        public StuInfomationType GetNameSearchId(string name)
        {
           return this.GetList().Where(i => i.Name == name).FirstOrDefault();
            
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="new_s"></param>
        /// <returns></returns>
        public AjaxResult Add_Data(StuInfomationType new_s)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                StuInfomationType find= this.GetNameSearchId(new_s.Name);
                if (find!=null)
                {
                    a.Success = false;
                    a.Msg = "该信息来源已存在！！";
                }
                else
                {
                    a.Success = true;
                    a.Msg = "添加成功! !";
                    this.Insert(new_s);
                }
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = "数据错误，请刷新重试！！";
            }
            return a;
        }
    }
}
