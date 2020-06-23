using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaiduBce;
using BaiduBce.Auth;
using BaiduBce.Services.Bos;

namespace DotnetSample
{
    internal class BosClientSample
    {
        private static void Main(string[] args)
        {
            const string accessKeyId = "a43996ac0c6d40c69d3ebb47127909e9"; // 用户的Access Key ID
            const string secretAccessKey = "2cfdf8b1f0e548f28cafcfd1aafc9226"; // 用户的Secret Access Key
            const string endpoint = "http://guiguxinxihua.bd.bcebos.com/";

            // 初始化一个BosClient
            BceClientConfiguration config = new BceClientConfiguration();
            config.Credentials = new DefaultBceCredentials(accessKeyId, secretAccessKey);
            config.Endpoint = endpoint;

            // 设置HTTP最大连接数为10
            config.ConnectionLimit = 10;

            // 设置TCP连接超时为5000毫秒
            config.TimeoutInMillis = 5000;

            // 设置读写数据超时的时间为50000毫秒
            config.ReadWriteTimeoutInMillis = 50000;

            BosClient client = new BosClient(config);
            
        }
    }
}
