using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
   public  class dbacc_dbstu
    {
        private AccdationinformationBusiness dbacc;
        private ProStudentInformationBusiness dbstu;

        /// <summary>
        /// 返回未居住的学生或者返回null；
        /// </summary>
        /// <returns></returns>
        public List<StudentInformation> GetUninhabitedData() {
            dbacc = new AccdationinformationBusiness();
            dbstu = new ProStudentInformationBusiness();
            List<StudentInformation> resultdata= dbstu.GetStudentInSchoolData();
            List<Accdationinformation> accdata = dbacc.GetAccdationinformations();
            if (resultdata.Count!=accdata.Count)
            {
                for (int i = resultdata.Count-1; i >=0; i--)
                {
                    foreach (var item in accdata)
                    {
                        if (resultdata.Count>0)
                        {
                            if (resultdata[i].StudentNumber == item.Studentnumber)
                            {
                                resultdata.Remove(resultdata[i]);
                                break;
                            }
                        }
                        
                    }
                }
                return resultdata;
            }
            else
            {
                return null;
            }
       
        }
    }
}
