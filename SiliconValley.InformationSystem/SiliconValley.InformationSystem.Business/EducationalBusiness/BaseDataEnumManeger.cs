using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class BaseDataEnumManeger:BaseBusiness<BaseDataEnum>
    {
        /// <summary>
        /// 根据主键或名称查询数据
        /// </summary>
        /// <param name="name">主键或名称（名称不能模糊查询）</param>
        /// <param name="key">true--按照主键查询，false---按照名称查询</param>
        /// <returns></returns>
        public BaseDataEnum GetSingData(string name,bool key)
        {
            BaseDataEnum b_new = new BaseDataEnum();
            if (key)
            {
                int id = Convert.ToInt32(name);
                b_new = this.GetEntity(id);
            }
            else
            {
                b_new= this.GetList().Where(b => b.Name == name).FirstOrDefault();
            }
            return b_new;
        }

        /// <summary>
        /// 根据父Id找子数据
        /// </summary>
        /// <param name="farther_Id">父Id</param>
        /// <returns></returns>
        public List<BaseDataEnum> GetChildData(int farther_Id)
        {
           return this.GetList().Where(b => b.fatherId == farther_Id).ToList();
        }
        /// <summary>
        /// 获取所有有效的父级数据
        /// </summary>
        /// <returns></returns>
        public List<BaseDataEnum> GetFartherData()
        {
           return  this.GetList().Where(b => b.IsDelete == false && b.fatherId == 0).ToList();
        }
        /// <summary>
        /// 根据父级名称找子集
        /// </summary>
        /// <param name="FartherName">父级名称</param>
        /// <returns></returns>
        public List<BaseDataEnum> GetsameFartherData(string FartherName)
        {
           BaseDataEnum find_fater= this.GetList().Where(b => b.Name == FartherName && b.fatherId == 0 && b.IsDelete==false).FirstOrDefault();
            if (find_fater==null)
            {
               return new List<BaseDataEnum>();
            }
            else
            {
                //根据父级Id找到子集
               return this.GetList().Where(g => g.fatherId == find_fater.Id).ToList();
            }
        }
    }
}
