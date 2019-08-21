using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 专业业务类
    /// </summary>
   public class SpecialtyBusiness:BaseBusiness<Specialty>
    {

        public Specialty GetSpecialtyByID(int SpecialtyId)
        {

            return this.GetSpecialties().Where(t=>t.Id==SpecialtyId ).FirstOrDefault();
        }

        public bool IsInList(List<Specialty> souces, Specialty specialty)
        {

            foreach (var item in souces)
            {
                if (item.Id == specialty.Id)
                    return true;
            }

            return false;

        }

        /// <summary>
        /// 获取全部专业
        /// </summary>
        /// <returns></returns>
        public List<Specialty> GetSpecialties()
        {

            return this.GetList().Where(d => d.IsDelete == false).ToList(); ;
        }


        /// <summary>
        /// 添加专业
        /// </summary>
        /// <param name="MajorName">专业名称</param>
        public Specialty AddMajor(string MajorName)
        {
            Specialty specialty = new Specialty();
            specialty.IsDelete = false;
            specialty.SpecialtyName = MajorName;

            this.Insert(specialty);

           return  this.GetSpecialties().OrderByDescending(d=>d.Id).FirstOrDefault();

        }



        /// <summary>
        /// 获取和名称相似的专业
        /// </summary>
        /// <param name="majorName">专业名称</param>
        /// <returns></returns>
        public List<Specialty> ContainsMajorName(string majorName)
        {

            return this.GetList().Where(d=>d.IsDelete==false && d.SpecialtyName.ToUpper().Contains(majorName.ToUpper())).ToList();

        }
    }
}
