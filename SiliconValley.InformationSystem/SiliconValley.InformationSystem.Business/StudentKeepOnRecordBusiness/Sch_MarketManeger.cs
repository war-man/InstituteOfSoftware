using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    
       public List<Sch_Market> GetAll(int limt,int page)
        {
            List<Sch_Market> ALL = this.GetListBySql<Sch_Market>("select * from Sch_Market").ToList();

            return ALL;
        }
    }
}
