
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
namespace SiliconValley.InformationSystem.Business.StudentManeger
{
    
    //学员信息
  public class StudentInformationManeger: BaseBusiness<StudentInformation>
    {

       
        //获取网络时间
        public string Date()
        {

            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;

            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse)request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                { if (h == "Date") { datetime = headerCollection[h]; } }
                return datetime;
            }

            catch (Exception) { return datetime; }
            finally
            {
                if (request != null)
                { request.Abort(); }
                if (response != null)
                { response.Close(); }
                if (headerCollection != null)
                { headerCollection.Clear(); }
            }
        }
    }
}
