using System;


namespace OAuth.Service.Interfaces
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheManager
    {
        void Set(string key, object data);

        void Set(string key, object data, DateTime absoluteExpiration, TimeSpan slidingExpiration);

        object Get(string key);

        T Get<T>(string key);

        bool IsSet(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void Clear();

    }
}
