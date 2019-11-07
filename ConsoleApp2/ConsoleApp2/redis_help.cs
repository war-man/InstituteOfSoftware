using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
using Json.Net;

namespace ConsoleApp2
{
   public class redis_help
    {
        private static readonly string ConnectionString = "192.168.1.112:6379,password=123456";
        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _lock = new object();
        /// <summary>
        /// 连接对象
        /// </summary>
        private volatile IConnectionMultiplexer _connection;
        /// <summary>
        /// 数据库
        /// </summary>
        private IDatabase _db;
        public redis_help()
        {
            _connection = ConnectionMultiplexer.Connect(ConnectionString);
            _db = GetDatabase();
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        protected IConnectionMultiplexer GetConnection()
        {
            
            if (_connection != null && _connection.IsConnected)
            {
                return _connection;
            }
            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected)
                {
                    return _connection;
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                }
                _connection = ConnectionMultiplexer.Connect(ConnectionString);
            }

            return _connection;
        }
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }
            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            _db.StringSet(key, entryBytes, expiresIn);
        }
        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {

            var rValue = _db.StringGet(key);
            if (!rValue.HasValue)
            {
                return default(T);
            }

            var result = Deserialize<T>(rValue);

            return result;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return default(T);
            }
            var json = Encoding.UTF8.GetString(serializedObject);

            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 判断是否已经设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return _db.KeyExists(key);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns>byte[]</returns>
        private byte[] Serialize(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json);
        }


        /// <summary>
        /// 获取Hash值
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="field">值</param>
        /// <returns></returns>
        public virtual T GetHash<T>(string key, string field)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }
            
            var rValue = _db.HashGet(key, field, CommandFlags.None);

            //先序列化
            //var jsonRvalue = Serialize(rValue);

            //反序列化
            var result = Deserialize<T>(rValue);

            return result;
        }


        /// <summary>
        /// 设置Hash值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="field">Field 和 Value 数组</param>
        public virtual void SetHahs<T>(string key,params HashEntry[] arryField)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            //var jsonField = Serialize(field);
            //var time=TimeSpan.FromMinutes(cacheTime);
            //var  list= JsonConvert.DeserializeObject<List<T>>(jsonvalue);

           
                _db.HashSet(key,arryField);
        }

        public virtual void SetList<T>(string key, string value)
        {


        }

    }
}
