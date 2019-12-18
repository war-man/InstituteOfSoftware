using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 企业信息业务类
    /// </summary>
    public class EnterpriseInfoBusiness:BaseBusiness<EnterpriseInfo>
    {

        /// <summary>
        /// 所有的企业信息包括合作企业集合
        /// </summary>
        /// <returns></returns>
        public List<EnterpriseInfo> GetAll() {
           return  this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 返回合作企业集合
        /// </summary>
        /// <returns></returns>
        public List<EnterpriseInfo> GetCooAll() {
            return this.GetAll().Where(a => a.IsCooper == true).ToList();
        }
        /// <summary>
        /// 返回没有合作企业集合
        /// </summary>
        /// <returns></returns>
        public List<EnterpriseInfo> GetNoCooAll() {
            return this.GetAll().Where(a => a.IsCooper == false).ToList();
        }
        /// <summary>
        /// 根据id查找该id对应的对象全部企业信息
        /// </summary>
        /// <param name="EnterID"></param>
        /// <returns></returns>
        public EnterpriseInfo GetEnterByID(int? EnterID) {
            return this.GetAll().Where(a => a.ID == EnterID).FirstOrDefault();
        }


        /// <summary>
        /// 名字是否唯一  true 可以使用 false 不能使用
        /// </summary>
        /// <returns></returns>
        public bool isonlyname(string entname,string entid) {
            bool result = false;
            //判断是否是自己
            if (!string.IsNullOrEmpty(entid))
            {
                int keyid = int.Parse(entid);
               var queryobj=  this.GetEntity(keyid);
                if (queryobj.EntName==entname)
                {
                    result = true;
                }
            }
            else
            {
                var query = this.GetInfoByentname(entname);
                if (query==null)
                {
                    return true;
                }
            }
            return result;
        }

       /// <summary>
       /// 根据公司名称返回这个公司对象
       /// </summary>
       /// <param name="entname"></param>
       /// <returns></returns>
        public EnterpriseInfo GetInfoByentname(string entname) {
           return this.GetAll().Where(a => a.EntName == entname).FirstOrDefault();
        }

        
    }
}
