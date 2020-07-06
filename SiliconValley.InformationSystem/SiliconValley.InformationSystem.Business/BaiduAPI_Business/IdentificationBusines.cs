

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiliconValley.InformationSystem.Entity.ViewEntity;

namespace SiliconValley.InformationSystem.Business.BaiduAPI_Business
{
    /// <summary>
    /// 身份证识别 帮助类
    /// </summary>
    public static class IdentificationBusines
    {
        /// <summary>
        /// 提取身份证正面信息
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static IdCard GetFrontInfo(string jsonText)
        {
            var jObject = (JObject)JsonConvert.DeserializeObject(jsonText);
            var Address = jObject["words_result"].ToObject<JObject>()["住址"].ToObject<JObject>()["words"].ToString();
            var CardNumber = jObject["words_result"].ToObject<JObject>()["公民身份号码"].ToObject<JObject>()["words"].ToString();
            var Birthday = jObject["words_result"].ToObject<JObject>()["出生"].ToObject<JObject>()["words"].ToString();
            var Name = jObject["words_result"].ToObject<JObject>()["姓名"].ToObject<JObject>()["words"].ToString();
            var Gender = jObject["words_result"].ToObject<JObject>()["性别"].ToObject<JObject>()["words"].ToString();
            var Nation = jObject["words_result"].ToObject<JObject>()["民族"].ToObject<JObject>()["words"].ToString();

            IdCard idCard = new IdCard();
            idCard.Address = Address;
            idCard.Birthday = Birthday;
            idCard.CardNumber = CardNumber;
            idCard.Gender = Gender;
            idCard.Name = Name;
            idCard.Nation = Nation;


            return idCard;

        }
    }
}
