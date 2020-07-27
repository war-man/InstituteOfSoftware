using SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness
{
    /// <summary>
    /// 备案数据日志业务类
    /// </summary>
   public class StudentbeanLogManeger:BaseBusiness<StudentbeanLog>
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool Add_data(StudentbeanLog log)
        {
            bool s = true;
            try
            {
                Insert(log);
            }
            catch (Exception)
            {
                s = false;
            }

            return s;
        }
    }
}
