using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    /// <summary>
    /// 班主任值班记录业务类
    /// </summary>
   public class ClassTeacherKeepManeger:BaseBusiness<ClassTeacherKeep>
    {
        RedisCache redisCache = new RedisCache();
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<ClassTeacherKeep> GetAllClassTeacherKeep()
        {
           
            List<ClassTeacherKeep> get_ClassTeacherKeep_list = new List<ClassTeacherKeep>();
            get_ClassTeacherKeep_list = Reconcile_Com.redisCache.GetCache<List<ClassTeacherKeep>>("ClassTeacherKeepList");
            if (get_ClassTeacherKeep_list == null || get_ClassTeacherKeep_list.Count == 0)
            {
                get_ClassTeacherKeep_list = this.GetList();
                Reconcile_Com.redisCache.SetCache("ClassTeacherKeepList", get_ClassTeacherKeep_list);
            }
            return get_ClassTeacherKeep_list;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="new_t"></param>
        /// <returns></returns>
        public AjaxResult Add_data(ClassTeacherKeep new_t)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                this.Insert(new_t);
                redisCache.RemoveCache("ClassTeacherKeepList");
                a.Success = true;
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }

            return a;
        }
        /// <summary>
        /// 集合添加数据
        /// </summary>
        /// <param name="new_t"></param>
        /// <returns></returns>
        public bool LIST_add(List<ClassTeacherKeep> new_t)
        {
            bool IsSuccess = false;
            try
            {
                foreach (ClassTeacherKeep item in new_t)
                {
                    this.Add_data(item);
                }
                IsSuccess = true;
            }
            catch (Exception )
            {
                IsSuccess = false;
            }
            return IsSuccess;
        }
    }
}
