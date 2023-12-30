using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    [DisallowMultipleComponent]
    public class UnRegisterTriggerOnDestroy : MonoBehaviour, IUnRegisterableList
    {
        public List<IUnRegisterable> UnRegisterables { get; private set; } = new List<IUnRegisterable>();

        public void Add(IUnRegisterable value)
        {
            this.UnRegisterables.Add(value);
        }

        private void OnDestroy()
        {
            this.UnRegisterAll();
        }
    }
}
