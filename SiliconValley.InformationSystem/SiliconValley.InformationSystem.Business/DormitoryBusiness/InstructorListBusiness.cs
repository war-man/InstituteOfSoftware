using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 教官业务类
    /// </summary>
    public class InstructorListBusiness:BaseBusiness<InstructorList>
    {
        /// <summary>
        /// 添加教官
        /// </summary>
        /// <param name="instructorList"></param>
        /// <returns></returns>
        public bool AddInstructorList(InstructorList instructorList) {
            bool result = false;
            try
            {
                this.Insert(instructorList);
                result = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
    }
}
