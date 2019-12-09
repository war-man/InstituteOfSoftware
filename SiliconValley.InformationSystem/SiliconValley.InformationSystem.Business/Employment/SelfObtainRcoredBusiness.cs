using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 自主就业业务类
    /// </summary>
   public class SelfObtainRcoredBusiness:BaseBusiness<SelfObtainRcored>
    {

        private QuarterBusiness dbquarter;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private EmpQuarterClassBusiness dbempQuarterClass;
        /// <summary>
        /// 获取全部可用的深数据
        /// </summary>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainRcoreds() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据学生编号获取这个自主就业的对象
        /// </summary>
        /// <param name="Studentno"></param>
        /// <returns></returns>
        public SelfObtainRcored GetSelfObtainByStudentno(string Studentno)
        {
           return this.GetSelfObtainRcoreds().Where(a => a.StudentNO == Studentno).FirstOrDefault();
        }

        /// <summary>
        ///根据这个计划id 获取这个计划中自主就业的对象
        /// </summary>
        /// <param name="QuarterID"></param>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainsByQuarterID(int QuarterID) {
            return this.GetSelfObtainRcoreds().Where(a => a.QuarterID == QuarterID).ToList();
        }


        /// <summary>
        ///根据年度获取这个年度所有的自主就业的数据
        /// </summary>
        /// <param name="paramdata"></param>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainRcoredsBy_classlist(List<ClassSchedule> paramdata)
        {
            
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            List<StudentInformation> studentlist = new List<StudentInformation>();
            foreach (var item in paramdata)
            {
                studentlist.AddRange(dbproScheduleForTrainees.GetStudentsByClassid(item.id));
            }
            var data = this.GetSelfObtainRcoreds();
        
            for (int i = data.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < studentlist.Count; j++)
                {

                    if (data[i].StudentNO != studentlist[j].StudentNumber)
                    {
                        if (j == studentlist.Count - 1)
                        {
                            data.Remove(data[i]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// 根据班级编号返回这个班级的自主就业记录
        /// </summary>dddddddd 
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainRcoredsByClassid(int classid) {
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            var data = this.GetSelfObtainRcoreds();
            var list1 = dbproScheduleForTrainees.GetTraineesByClassid(classid);
            List<SelfObtainRcored> result = new List<SelfObtainRcored>();
            for (int i = data.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < list1.Count; j++)
                {
                    if (data[i].StudentNO == list1[j].StudentID)
                    {
                        result.Add(data[i]);
                    }
                   
                }
            }
            return result;
        }

        /// <summary>
        /// 删除自主就业记录
        /// </summary>
        /// <param name="studentnumber">学生编号</param>
        /// <returns></returns>
        public bool del(string studentnumber) {
           var  aa= this.GetSelfObtainRcoreds().Where(a => a.StudentNO == studentnumber).FirstOrDefault();
            if (aa!=null)
            {
                var oldname = AppDomain.CurrentDomain.BaseDirectory + "uploadXLSXfile/SelfObtainRcoredImg/" + aa.ImgUrl;
                if (this.DeleteImgFile(oldname))
                {

                    aa.IsDel = true;
                    this.Update(aa);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public bool DeleteImgFile(string fileUrl)
        {
            try
            {

                if (System.IO.File.Exists(fileUrl))
                {
                    System.IO.File.Delete(fileUrl);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

    }
}
