using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class ClassroomManeger:BaseBusiness<Classroom>
    {
        /// <summary>
        /// 根据主键或名称查单条数据
        /// </summary>
        /// <param name="name">Id或名称</param>
        /// <param name="key">true--按主键查找,false---按名称查找</param>
        /// <returns></returns>
        public Classroom GetSingData(string name,bool key)
        {
            Classroom new_c = new Classroom();
            if (key)
            {
                //根据主键查询
               int id= Convert.ToInt32(name);
                new_c= this.GetEntity(id);
            }
            else
            {
                new_c= this.GetList().Where(c => c.ClassroomName == name).FirstOrDefault();
            }

            return new_c;
        }

        /// <summary>
        /// 获取有效数据
        /// </summary>
        /// <returns></returns>
        public List<Classroom> GetExitsData()
        {
           return this.GetList().Where(c => c.IsDelete == false).ToList();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="new_c"></param>
        /// <returns></returns>
        public bool My_add(Classroom new_c)
        {
            try
            {
                this.Insert(new_c);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="new_c"></param>
        /// <returns></returns>
        public bool My_update(Classroom new_c)
        {
            try
            {
                Classroom find_c = this.GetEntity(new_c.Id);
                find_c.ClassroomName = new_c.ClassroomName;
                find_c.Count = new_c.Count;
                find_c.Rmark = new_c.Rmark;
                this.Update(find_c);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool My_Delete(int id)
        {         
            try
            {
                Classroom find_c= this.GetEntity(id);
                find_c.IsDelete = find_c.IsDelete==true?false:true;
                this.Update(find_c);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 查找名字相同的数据
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool FindSameName(string Name)
        {
           Classroom find_c= GetSingData(Name, false);
            if (find_c==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 获取XX校区有效的教室
        /// </summary>
        /// <param name="id">校区Id</param>
        /// <returns></returns>
        public List<Classroom> GetAddreeClassRoom(int id)
        {
           return GetExitsData().Where(c => c.BaseData_Id == id).ToList();
        }

        /// <summary>
        /// 获取所有有效教室
        /// </summary>
        /// <returns></returns>
        public List<Classroom> GetEffectiveClass()
        {
            return GetExitsData().Where(c => c.IsDelete==false).ToList();
        }

    }
}
