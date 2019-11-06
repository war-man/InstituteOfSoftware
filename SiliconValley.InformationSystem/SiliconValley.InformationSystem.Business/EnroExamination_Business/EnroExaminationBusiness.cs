using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EnroExamination_Business
{
    /// <summary>
    /// 成考管理
    /// </summary>
  public  class EnroExaminationBusiness:BaseBusiness<EnroExamination>
    {
        /// <summary>
        /// 验证成考是否有重复
        /// </summary>
        /// <param name="studentInformation">集合数据</param>
        /// <returns></returns>
        public List<StudentInformation> BoollistEnroExamination(List<StudentInformation> studentInformation)
        {
            List<StudentInformation> students = new List<StudentInformation>();

            var list = this.GetList();
            foreach (var item in studentInformation)
            {
                foreach (var item1 in list)
                {
                    if (item.StudentNumber==item1.StudentNumber)
                    {
                        students.Add(item);
                    }
                    
                }
            }
            return students;
        }
    }
}
