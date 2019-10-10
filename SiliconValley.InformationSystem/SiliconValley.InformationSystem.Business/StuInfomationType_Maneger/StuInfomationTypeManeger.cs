using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;

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
    }
}
