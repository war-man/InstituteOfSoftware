﻿using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// CD类访谈记录业务类
    /// </summary>
   public class CDInterviewBusiness:BaseBusiness<CDInterview>
    {
        private ProScheduleForTrainees dbproScheduleForTrainees;
        public List<CDInterview> GetCDInterviews() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据班级号获cd类访谈记录
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<CDInterview> GetCDInterviewsByClassno(string classno)
        {
            var list = this.GetCDInterviews();
            List<CDInterview> dd = new List<CDInterview>();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            var list1 = dbproScheduleForTrainees.GetTraineesByClassNO(classno);
            foreach (var item in list)
            {
                foreach (var item1 in list1)
                {
                    if (item.StudentNO == item1.StudentID)
                    {
                        dd.Add(item);
                    }
                }
            }
            return dd;
        }
    }
}