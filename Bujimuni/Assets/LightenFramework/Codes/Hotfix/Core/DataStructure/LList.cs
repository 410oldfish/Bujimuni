using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public class LList<T> : List<T>
    {
        private struct ChangeData
        {
            public T data;
            public bool addorremove;
        }
        private List<ChangeData> m_changeList = new List<ChangeData>();
        
        public void DelayAdd(T data)
        {
            m_changeList.Add(new ChangeData(){data = data, addorremove = true});
        }
        public void DelayRemove(T data)
        {
            m_changeList.Add(new ChangeData(){data = data, addorremove = false});
        }

        public void HandleChange()
        {
            if (m_changeList.Count < 1)
                return;
            foreach (var changeData in m_changeList)
            {
                if (changeData.addorremove)
                {
                    Add(changeData.data);
                }
                else
                {
                    Remove(changeData.data);
                }
            }
            m_changeList.Clear();
        }
    }
}
