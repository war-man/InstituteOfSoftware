using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class SurveyGroupByDateView
    {
        public SurveyGroupByDateView()
        {
            data = new List<object>();
        }

        public DateTime date { get; set; }
        public List<object> data {get;set;}

        public static bool IsContains(List<SurveyGroupByDateView> sources , DateTime date)
        {
            foreach (var item in sources)
            {
                if (item.date.Year == date.Year && item.date.Month == date.Month)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
