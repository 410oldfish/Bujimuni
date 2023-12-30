using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Lighten
{
    public class ConvenientText : MonoBehaviour
    {
        [System.Serializable]
        public struct Data
        {
            public string key;
            [TextArea] public string value;
        }

        [TableList] public List<Data> datas = new List<Data>();

        protected Dictionary<string, string> m_cacheDatas;

        public string GetText(string key)
        {
            if (datas.Count < 1)
                return key;
            if (m_cacheDatas == null)
            {
                m_cacheDatas = new Dictionary<string, string>();
                InitializeCacheData(m_cacheDatas);
            }
            if (!m_cacheDatas.ContainsKey(key))
                return key;
            return m_cacheDatas[key];
        }

        protected void InitializeCacheData(Dictionary<string, string> cacheData)
        {
            cacheData.Clear();
            foreach (var data in datas)
            {
                if (cacheData.ContainsKey(data.key))
                {
                    Debug.LogError($"不能重复定义key {data.key}");
                    continue;
                }
                cacheData.Add(data.key, data.value);
            }
        }
    }
}