using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.FinaceBusines
{
    //费用明细
 public   class CostitemsBusiness:BaseBusiness<Costitems>
    {
        //阶段
        GrandBusiness Grandcontext = new GrandBusiness();
        //费用明目类别
        BaseBusiness<CostitemsX> cisitemesx = new BaseBusiness<CostitemsX>();
        /// <summary>
        /// 查询名目是否重复
        /// </summary>
        /// <param name="Stage">阶段id</param>
        /// <param name="Name">名目名称</param>
        /// <returns></returns>
        public int BoolName(int Stage,string Name)
        {
           return this.GetList().Where(a => a.IsDelete == false && a.Grand_id == Stage && a.Name == Name).Count();
        }
        /// <summary>
        /// 名目数据添加
        /// </summary>
        /// <param name="costitems">数据对象</param>
        /// <returns></returns>
        public AjaxResult AddCostitems(Costitems costitems)
        {  
            AjaxResult retus = null;
            try
            {
                costitems.IsDelete = false;
               
               
                 this.Insert(costitems);
             
               
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "添加数据";
                BusHelper.WriteSysLog("添加数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;
        }
        /// <summary>
        /// 验证明目表是否有重复的数据
        /// </summary>
        /// <param name="costitems"></param>
        /// <returns></returns>
        public int BoolCostitems(Costitems costitems)
        {
            costitems.Grand_id= costitems.Grand_id > 0 ? costitems.Grand_id : null;
        return this.GetList().Where(a => a.IsDelete==false&&a.Name==costitems.Name&&a.Rategory==costitems.Rategory&&a.Grand_id==costitems.Grand_id).Count();
          
        }
        /// <summary>
        /// 费用名目查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public object DateCostitems(int page, int limit)
        {
            var list = this.GetList().Where(a => a.IsDelete == false).Select(a=>new {
                a.id,a.Name,a.Amountofmoney,Stage=a.Grand_id>0? Grandcontext.GetEntity(a.Grand_id).GrandName:"暂无",
                Typex= cisitemesx.GetEntity(a.Rategory).Name
            }).ToList();
            var dataList = list.OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = list.Count,
                data = dataList
            };
            return data;
        }
        /// <summary>
        /// 查询费用类别是否重复
        /// </summary>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public int TypeName(string Name)
        {
           return cisitemesx.GetList().Where(a => a.IsDelete == false && a.Name == Name).Count();
        }
        /// <summary>
        /// 添加费用明目类型
        /// </summary>
        /// <param name="costitemsX">数据对象</param>
        /// <returns></returns>
        public AjaxResult AddType(CostitemsX costitemsX)
        {
            AjaxResult retus = null;
            try
            {
                costitemsX.IsDelete = false;
                costitemsX.AddDate = DateTime.Now;
                cisitemesx.Insert(costitemsX);
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "添加数据";
                BusHelper.WriteSysLog("添加数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;
        }
        /// <summary>
        /// 获取明目类型的所有数据
        /// </summary>
        /// <returns></returns>
        public List<CostitemsX> TypeDate()
        {
          return  cisitemesx.GetList().Where(a => a.IsDelete == false).ToList();
        }
        /// <summary>
        /// 获取名目的所有数据
        /// </summary>
        /// <returns></returns>
        public List< Costitems> costitemslist()
        {
            return this.GetList().Where(a => a.IsDelete == false).ToList();
        }

       
    }
}
