using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PushServe.Util
{
    public class RedisHelper
    {
        #region -- connInfo --

        /// <summary>
        /// redis连接池
        /// </summary>
        private static Dictionary<string, ConnectionMultiplexer> redisClientPoolDic = new Dictionary<string, ConnectionMultiplexer>();

        /// <summary>
        /// 并发锁
        /// </summary>
        private static Dictionary<string, object> initLockDic = new Dictionary<string, object>();

        /// <summary>
        /// 初始化锁
        /// </summary>
        private static object initLock = new object();

        /// <summary>
        /// redis连接池 不同连接路径获取不一样的连接池
        /// </summary>
        private static ConnectionMultiplexer RedisClientPool(string path)
        {
            ConnectionMultiplexer result = null;
            if (!redisClientPoolDic.ContainsKey(path))
            {
                if (!initLockDic.ContainsKey(path))
                {
                    lock (initLock)
                    {
                        if (!initLockDic.ContainsKey(path))
                        {
                            initLockDic.Add(path, new object());
                        }
                    }
                }
                lock (initLockDic[path])
                {
                    if (!redisClientPoolDic.ContainsKey(path))
                    {
                        result = ConnectionMultiplexer.Connect(path);
                        redisClientPoolDic.Add(path, result);
                    }
                    else
                    {
                        result = redisClientPoolDic[path];
                    }
                }
            }
            else
            {
                result = redisClientPoolDic[path];
            }
            return result;
        }

        /// <summary>
        /// 获取redis客户端
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetRedisClient(string path)
        {
            return RedisClientPool(path).GetDatabase();
        }
        #endregion

        #region -- Item --
        /// <summary> 
        /// 设置单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <param name="timeSpan"></param> 
        /// <returns></returns> 
        public static bool Item_Set<T>(string path, string key, T t)
        {
            return GetRedisClient(path).StringSet(key, JsonUtil<T>.Serialize(t));
        }

        /// <summary> 
        /// 设置单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <param name="timeSpan"></param> 
        /// <returns></returns> 
        public static bool Item_Set<T>(string path, string key, T t, TimeSpan timespan)
        {
            return GetRedisClient(path).StringSet(key, JsonUtil<T>.Serialize(t), timespan);
        }

        /// <summary> 
        /// 获取单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static T Item_Get<T>(string path, string key)
        {
            T result = default(T);
            RedisValue value = GetRedisClient(path).StringGet(key);
            if (value.HasValue)
            {
                result = JsonUtil<T>.Deserialize(value);
            }
            return result;
        }

        /// <summary> 
        /// 获取单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static string Item_Get(string path, string key)
        {
            string result = GetRedisClient(path).StringGet(key).ToString();
            return result;
        }

        /// <summary> 
        /// 移除单体 
        /// </summary> 
        /// <param name="key"></param> 
        public static bool Item_Remove(string path, string key)
        {
            return GetRedisClient(path).KeyDelete(key);
        }

        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Inc(string path, string key)
        {
            return GetRedisClient(path).StringIncrement(key);
        }


        /// <summary>
        /// key 是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyExists(string path, string key)
        {
            return GetRedisClient(path).KeyExists(key);
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="oldKey"></param>
        /// <param name="newKey"></param>
        /// <returns></returns>
        public static bool KeyRename(string path, string oldKey, string newKey)
        {
            if (GetRedisClient(path).KeyExists(oldKey))
            {
                return GetRedisClient(path).KeyRename(oldKey, newKey);
            }
            return false;
        }

        /// <summary>
        /// 设置过期
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="ts"></param>
        public static void KeyExpire(string path, string key, TimeSpan ts)
        {
            GetRedisClient(path).KeyExpire(key, ts);
        }

        #endregion

        #region -- List --

        public static long List_RightPush<T>(string path, string key, T t)
        {
            return GetRedisClient(path).ListRightPush(key, JsonUtil<T>.Serialize(t));
        }

        public static long List_LeftPush<T>(string path, string key, T t)
        {
            return GetRedisClient(path).ListLeftPush(key, JsonUtil<T>.Serialize(t));
        }

        public static long List_InsertAfter<T>(string path, string key, T value, T target)
        {
            return GetRedisClient(path).ListInsertAfter(key, JsonUtil<T>.Serialize(target), JsonUtil<T>.Serialize(value));
        }

        public static long List_InsertBefore<T>(string path, string key, T value, T target)
        {
            return GetRedisClient(path).ListInsertBefore(key, JsonUtil<T>.Serialize(target), JsonUtil<T>.Serialize(value));
        }

        public static T List_GetByIndex<T>(string path, string key, long index)
        {
            RedisValue value = GetRedisClient(path).ListGetByIndex(key, index);
            if (!value.IsNull)
            {
                return JsonUtil<T>.Deserialize(value);
            }
            return default(T);
        }

        public static void List_SetByIndex<T>(string path, string key, long index, T value)
        {
            GetRedisClient(path).ListSetByIndex(key, index, JsonUtil<T>.Serialize(value));
        }

        public static long List_Remove<T>(string path, string key, T t)
        {
            return GetRedisClient(path).ListRemove(key, JsonUtil<T>.Serialize(t));
        }

        public static long List_Count(string path, string key)
        {
            return GetRedisClient(path).ListLength(key);
        }

        public static List<T> List_GetRange<T>(string path, string key, int start, int count)
        {
            var list = GetRedisClient(path).ListRange(key, start, start + count - 1);
            List<T> result = new List<T>();
            if (list != null && list.Length > 0)
            {
                foreach (var item in list)
                {
                    result.Add(JsonUtil<T>.Deserialize(item));
                }
            }
            return result;
        }

        public static List<T> List_GetAllList<T>(string path, string key)
        {
            var list = GetRedisClient(path).ListRange(key, 0, long.MaxValue);
            List<T> result = new List<T>();
            if (list != null && list.Length > 0)
            {
                foreach (var item in list)
                {
                    result.Add(JsonUtil<T>.Deserialize(item));
                }
            }
            return result;
        }

        public static List<T> List_GetList<T>(string path, string key, int pageIndex, int pageSize)
        {
            int start = pageSize * (pageIndex - 1);
            return List_GetRange<T>(path, key, start, pageSize);
        }

        #endregion

        #region -- Set --

        public static void Set_Add<T>(string path, string key, T t)
        {
            GetRedisClient(path).SetAdd(key, JsonUtil<T>.Serialize(t));
        }
        public static bool Set_Contains<T>(string path, string key, T t)
        {
            return GetRedisClient(path).SetContains(key, JsonUtil<T>.Serialize(t));
        }
        public static bool Set_Remove<T>(string path, string key, T t)
        {
            return GetRedisClient(path).SetRemove(key, JsonUtil<T>.Serialize(t));
        }

        public static List<T> Set_Get<T>(string path, string key)
        {
            var list = GetRedisClient(path).SetMembers(key);
            List<T> result = new List<T>();
            if (list != null && list.Length > 0)
            {
                foreach (var item in list)
                {
                    result.Add(JsonUtil<T>.Deserialize(item));
                }
            }
            return result;
        }

        public static long Set_Count(string path, string key)
        {
            return GetRedisClient(path).SetLength(key);
        }
        #endregion

        #region -- Hash --
        /// <summary> 
        /// 判断某个数据是否已经被缓存 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Exist(string path, string key, string dataKey)
        {
            return GetRedisClient(path).HashExists(key, dataKey);
        }

        /// <summary> 
        /// 批量判断hash中是否有对应的key 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="hashID"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static Dictionary<string, bool> Hash_Exist_Batch(string path, string hashID, IEnumerable<string> dataKey)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            var batch = GetRedisClient(path).CreateBatch();
            Dictionary<string, Task<bool>> tasks = new Dictionary<string, Task<bool>>();
            foreach (string key in dataKey)
            {
                var task = batch.HashExistsAsync(hashID, key);
                tasks[key] = task;
            }
            batch.Execute();
            batch.WaitAll(tasks.Values.ToArray());
            foreach (string key in tasks.Keys)
            {
                result.Add(key, tasks[key].Result);
            }
            return result;
        }

        /// <summary> 
        /// 存储数据到hash表 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Set<T>(string path, string key, string dataKey, T t)
        {
            string value = JsonUtil<T>.Serialize(t);
            return GetRedisClient(path).HashSet(key, dataKey, value);
        }

        /// <summary>
        /// 存储数据到hash表 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Hash_Set(string path, string key, string dataKey, string value)
        {
            return GetRedisClient(path).HashSet(key, dataKey, value);
        }

        /// <summary> 
        /// 移除hash中的某值 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Remove(string path, string key, string dataKey)
        {
            return GetRedisClient(path).HashDelete(key, dataKey);
        }

        /// <summary> 
        /// 从hash表获取数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static T Hash_Get<T>(string path, string key, string dataKey)
        {
            RedisValue value = GetRedisClient(path).HashGet(key, dataKey);
            T result = default(T);
            if (value.HasValue)
            {
                result = JsonUtil<T>.Deserialize(value); ;
            }
            return result;
        }

        /// <summary> 
        /// 从hash表获取数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static List<T> Hash_GetValues<T>(string path, string key, string[] dataKey)
        {
            RedisValue[] keys = new RedisValue[dataKey.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = dataKey[i];
            }
            RedisValue[] list = GetRedisClient(path).HashGet(key, keys);
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    if (!item.IsNullOrEmpty && !item.IsNull)
                    {
                        var value = JsonUtil<T>.Deserialize(item);
                        result.Add(value);
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary> 
        /// 从hash表获取数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static List<string> Hash_GetValues(string path, string key, string[] dataKey)
        {
            RedisValue[] keys = new RedisValue[dataKey.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = dataKey[i];
            }
            RedisValue[] list = GetRedisClient(path).HashGet(key, keys);
            if (list != null && list.Length > 0)
            {
                List<string> result = new List<string>();
                foreach (var item in list)
                {
                    if (!item.HasValue)
                    {
                        result.Add("");
                    }
                    else
                    {
                        result.Add(item.ToString());
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary> 
        /// 获取整个hash的数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static List<T> Hash_GetAllValues<T>(string path, string key)
        {
            HashEntry[] list = GetRedisClient(path).HashGetAll(key);
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var value = JsonUtil<T>.Deserialize(item.Value);
                    result.Add(value);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 获取hash所有键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> Hash_GetAllKeys(string path, string key)
        {
            RedisValue[] keys = GetRedisClient(path).HashKeys(key);
            List<string> result = new List<string>();
            if (keys != null && keys.Length > 0)
            {
                foreach (RedisValue value in keys)
                {
                    result.Add(value.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// 获取hash的键数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Hash_GetCount(string path, string key)
        {
            return GetRedisClient(path).HashLength(key);
        }

        /// <summary> 
        /// 设置缓存过期 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static void Hash_SetExpire(string path, string key, DateTime datetime)
        {
            GetRedisClient(path).KeyExpire(key, datetime);
        }

        /// <summary>
        /// hash 增加频次值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="value"></param>
        public static void Hash_Inc(string path, string key, string dataKey, long value)
        {
            GetRedisClient(path).HashIncrement(key, dataKey, value);
        }

        /// <summary> 
        /// 获取Hash 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static Dictionary<string, T> Hash_GetAllEntries<T>(string path, string key)
        {
            HashEntry[] list = GetRedisClient(path).HashGetAll(key);
            if (list != null && list.Length > 0)
            {
                Dictionary<string, T> result = new Dictionary<string, T>();
                foreach (var item in list)
                {
                    var value = JsonUtil<T>.Deserialize(item.Value);
                    result.Add(item.Name, value);
                }
                return result;
            }
            return null;
        }

        /// <summary> 
        /// 获取Hash 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static Dictionary<string, string> Hash_GetAllEntries(string path, string key)
        {
            HashEntry[] list = GetRedisClient(path).HashGetAll(key);
            if (list != null && list.Length > 0)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (var item in list)
                {
                    result.Add(item.Name, item.Value);
                }
                return result;
            }
            return new Dictionary<string, string>();
        }

        /// <summary> 
        /// 批量获取Hash 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static Dictionary<string, Dictionary<string, string>> Hash_GetAllEntries_Batch(string path, IEnumerable<string> keys)
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            if (keys != null)
            {
                var db = GetRedisClient(path);
                foreach (string key in keys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        HashEntry[] entries = db.HashGetAll(key);
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        foreach (HashEntry entry in entries)
                        {
                            dic.Add(entry.Name, entry.Value);
                        }
                        result.Add(key, dic);
                    }
                }
            }
            return result;
        }

        /// <summary> 
        /// 批量获取Hash 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static Dictionary<string, string> Hash_Get_Batch(string path, IEnumerable<string> hashIDs, string key)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Dictionary<string, Task<RedisValue>> tasks = new Dictionary<string, Task<RedisValue>>();
            var batch = GetRedisClient(path).CreateBatch();
            foreach (string hashID in hashIDs)
            {
                if (!string.IsNullOrEmpty(hashID))
                {
                    var task = batch.HashGetAsync(hashID, key);
                    tasks[hashID] = task;
                }
            }
            batch.Execute();
            batch.WaitAll(tasks.Values.ToArray());

            foreach (string hashID in tasks.Keys)
            {
                RedisValue value = tasks[hashID].Result;
                result[hashID] = value;
            }
            return result;
        }

        /// <summary>
        /// 批量设置hash值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hashID"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        public static bool Hash_Set_Batch(string path, string hashID, Dictionary<string, string> pair)
        {
            bool result = false;
            if (pair != null && pair.Count > 0)
            {
                var batch = GetRedisClient(path).CreateBatch();
                foreach (string key in pair.Keys)
                {
                    batch.HashSetAsync(hashID, key, pair[key]);
                }
                batch.Execute();
                result = true;
            }
            return result;
        }

        #endregion

        #region -- SortedSet --

        /// <summary> 
        ///  添加数据到 SortedSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <param name="score"></param> 
        public static double SortedSet_Inc<T>(string path, string key, T t, double score)
        {
            string value = JsonUtil<T>.Serialize(t);
            return GetRedisClient(path).SortedSetIncrement(key, value, score);
        }


        /// <summary> 
        ///  添加数据到 SortedSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <param name="score"></param> 
        public static bool SortedSet_Add<T>(string path, string key, T t, double score)
        {
            string value = JsonUtil<T>.Serialize(t);
            return GetRedisClient(path).SortedSetAdd(key, value, score);
        }

        /// <summary> 
        /// 移除数据从SortedSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <returns></returns> 
        public static bool SortedSet_Remove<T>(string path, string key, T t)
        {
            string value = JsonUtil<T>.Serialize(t);
            return GetRedisClient(path).SortedSetRemove(key, value);
        }


        /// <summary> 
        /// 移除数据从SortedSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <returns></returns> 
        public static long SortedSet_RemoveRangeByScore(string path, string key, double start, double top)
        {
            return GetRedisClient(path).SortedSetRemoveRangeByScore(key, start, top);
        }

        /// <summary> 
        /// 移除数据从SortedSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <returns></returns> 
        public static long SortedSet_RemoveRangeByRank(string path, string key, long start, long top)
        {
            return GetRedisClient(path).SortedSetRemoveRangeByRank(key, start, top);
        }

        /// <summary>
        /// 根据积分获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static T SortedSet_GetItemByScore<T>(string path, string key, double score)
        {
            RedisValue[] list = GetRedisClient(path).SortedSetRangeByScore(key, score, score);
            T result = default(T);
            if (list != null && list.Length > 0)
            {
                result = JsonUtil<T>.Deserialize(list[0]);
            }
            return result;
        }

        /// <summary>
        /// 获取指定分值区间的列表值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetRangeItemsByScore<T>(string path, string key, double minScore, double maxScore, int order = 1)
        {
            RedisValue[] list = GetRedisClient(path).SortedSetRangeByScore(key, minScore, maxScore, Exclude.None, (order == 1 ? Order.Descending : Order.Ascending));
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item);
                    result.Add(data);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 获取指定分值区间的列表值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetRangeItemsByScore<T>(string path, string key, double minScore, double maxScore, int page, int pageSize, int order = 1)
        {
            RedisValue[] list = GetRedisClient(path).SortedSetRangeByScore(key, minScore, maxScore, Exclude.None, (order == 1 ? Order.Descending : Order.Ascending), (page - 1) * pageSize, pageSize);
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item);
                    result.Add(data);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 获取分数
        /// double.MinValue：不存在对应的值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double SortedSet_GetItemScore<T>(string path, string key, T data)
        {
            double? value = GetRedisClient(path).SortedSetScore(key, JsonUtil<T>.Serialize(data));
            //存在
            if (value != null)
            {
                return value.Value;
            }
            else
            {
                return double.MinValue;
            }
        }

        /// <summary>
        /// 获取指定分值区间的列表值,包括分数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        /// <returns></returns>
        public static Dictionary<T, double> SortedSet_GetRangeWithScoreItemsByScore<T>(string path, string key, double minScore, double maxScore, int order = 1, long skip = 0, long take = -1)
        {
            var list = GetRedisClient(path).SortedSetRangeByScoreWithScores(key, minScore, maxScore, Exclude.None, (order == 1 ? Order.Descending : Order.Ascending), skip, take);
            if (list != null && list.Length > 0)
            {
                Dictionary<T, double> result = new Dictionary<T, double>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item.Element.ToString());
                    result.Add(data, item.Score);
                }
                return result;
            }
            return null;
        }

        /// <summary> 
        /// 获取SortedSet的长度 
        /// </summary> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static long SortedSet_Count(string path, string key, double min = -1, double max = -1)
        {
            if (min == -1 && max == -1)
            {
                return GetRedisClient(path).SortedSetLength(key);
            }
            else
            {
                return GetRedisClient(path).SortedSetLength(key, min, max);
            }
        }

        /// <summary> 
        /// 获取SortedSet的分页数据 降序
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="pageIndex"></param> 
        /// <param name="pageSize"></param> 
        /// <returns></returns> 
        public static List<T> SortedSet_GetListAsc<T>(string path, string key, int pageIndex, int pageSize)
        {
            //GetRedisClient(path).SortedSetRangeByScoreWithScores
            var list = GetRedisClient(path).SortedSetRangeByRank(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1, Order.Ascending);
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item);
                    result.Add(data);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static Dictionary<T, double> SortedSet_GetListWithScoreAsc<T>(string path, string key, int pageIndex, int pageSize)
        {
            var list = GetRedisClient(path).SortedSetRangeByRankWithScores(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1, Order.Ascending);
            if (list != null && list.Length > 0)
            {
                Dictionary<T, double> result = new Dictionary<T, double>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item.Element.ToString());
                    result[data] = item.Score;
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetListDesc<T>(string path, string key, int pageIndex, int pageSize)
        {
            var list = GetRedisClient(path).SortedSetRangeByRank(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1, Order.Descending);
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item);
                    result.Add(data);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static Dictionary<T, double> SortedSet_GetListWithScoreDesc<T>(string path, string key, int pageIndex, int pageSize)
        {
            var list = GetRedisClient(path).SortedSetRangeByRankWithScores(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1, Order.Descending);
            if (list != null && list.Length > 0)
            {
                Dictionary<T, double> result = new Dictionary<T, double>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item.Element.ToString());
                    result[data] = item.Score;
                }
                return result;
            }
            return null;
        }

        /// <summary> 
        /// 获取SortedSet的分页数据 降序
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="pageIndex"></param> 
        /// <param name="pageSize"></param> 
        /// <returns></returns> 
        public static List<T> SortedSet_GetListAscByRank<T>(string path, string key, long rank, int pageSize)
        {
            var list = GetRedisClient(path).SortedSetRangeByRank(key, rank, rank + pageSize - 1, Order.Ascending);
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item);
                    result.Add(data);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetListDescByRank<T>(string path, string key, long rank, int pageSize)
        {
            var list = GetRedisClient(path).SortedSetRangeByRank(key, rank, rank + pageSize - 1, Order.Descending);
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item);
                    result.Add(data);
                }
                return result;
            }
            return null;
        }

        /// <summary> 
        /// 获取SortedSet的全部数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="pageIndex"></param> 
        /// <param name="pageSize"></param> 
        /// <returns></returns> 
        public static List<T> SortedSet_GetListALLAsc<T>(string path, string key)
        {
            var list = GetRedisClient(path).SortedSetRangeByRank(key, 0, int.MaxValue, Order.Ascending);
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item);
                    result.Add(data);
                }
                return result;
            }
            return null;
        }

        /// <summary> 
        /// 获取SortedSet的全部数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="pageIndex"></param> 
        /// <param name="pageSize"></param> 
        /// <returns></returns> 
        public static List<T> SortedSet_GetListALLDesc<T>(string path, string key)
        {
            var list = GetRedisClient(path).SortedSetRangeByRank(key, 0, int.MaxValue, Order.Descending);
            if (list != null && list.Length > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item);
                    result.Add(data);
                }
                return result;
            }
            return null;
        }

        /// <summary> 
        /// 获取SortedSet的全部数据,并且包含分数值
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="pageIndex"></param> 
        /// <param name="pageSize"></param> 
        /// <returns></returns> 
        public static Dictionary<T, double> SortedSet_GetAllWithScore<T>(string path, string key, int order = 0)
        {
            var list = GetRedisClient(path).SortedSetRangeByRankWithScores(key, 0, int.MaxValue, (order == 0 ? Order.Ascending : Order.Descending));
            if (list != null && list.Length > 0)
            {
                Dictionary<T, double> result = new Dictionary<T, double>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item.Element.ToString());
                    if (!result.ContainsKey(data))
                    {
                        result.Add(data, item.Score); ;
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// sortedset 中是否含有指定的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SortedSet_Contains<T>(string path, string key, T value)
        {
            return GetRedisClient(path).SortedSetRank(key, JsonUtil<T>.Serialize(value)) != null;
        }

        /// <summary>
        /// sortedset 中是否含有指定的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long SortedSet_GetRank<T>(string path, string key, T value)
        {
            long? result = GetRedisClient(path).SortedSetRank(key, JsonUtil<T>.Serialize(value));
            if (result != null)
            {
                return result.Value;
            }
            return -1;
        }

        /// <summary>
        /// sortedset 中是否含有指定的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long SortedSet_GetRankDesc<T>(string path, string key, T value)
        {
            long? result = GetRedisClient(path).SortedSetRank(key, JsonUtil<T>.Serialize(value), Order.Descending);
            if (result != null)
            {
                return result.Value;
            }
            return -1;
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<T> SortedSetScan<T>(string path, string key, string value, int count, long cursor = 0)
        {
            var list = GetRedisClient(path).SortedSetScan(key, value, count, cursor, 0, CommandFlags.None).Take(30);
            if (list != null && list.Count() > 0)
            {
                List<T> result = new List<T>();
                foreach (var item in list)
                {
                    var data = JsonUtil<T>.Deserialize(item.Element.ToString());
                    if (!result.Contains(data))
                    {
                        result.Add(data);
                    }
                }
                return result;
            }
            return null;
        }
        #endregion

        #region -- msg --

        /// <summary> 
        ///  发布消息
        /// </summary> 
        /// <param name="path"></param> 
        /// <param name="channel"></param> 
        /// <param name="value"></param> 
        public static void Publish(string path, string channel, string value)
        {
            GetRedisClient(path).Publish(channel, value);
        }

        /// <summary> 
        ///  监听消息
        /// </summary> 
        /// <param name="path"></param> 
        /// <param name="channel"></param> 
        /// <param name="value"></param> 
        public static void Subscribe(string path, string channel, Action<string, string> action)
        {
            var db = GetRedisClient(path);
            db.Multiplexer.PreserveAsyncOrder = false;
            var sub = db.Multiplexer.GetSubscriber();
            sub.Subscribe(channel, (p, q) => action.Invoke(p, q));
            Thread.CurrentThread.Join();
        }

        #endregion
    }
}
