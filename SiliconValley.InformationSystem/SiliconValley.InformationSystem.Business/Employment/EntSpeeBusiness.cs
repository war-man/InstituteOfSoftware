using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    public class EntSpeeBusiness : BaseBusiness<EntSpee>
    {
        public EntSpee GetEntSpeeByID(int ID)
        {
            return this.GetIQueryable().Where(a => a.Id == ID).FirstOrDefault();
        } 
    }
}
