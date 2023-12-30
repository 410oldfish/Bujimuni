using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    [DisallowMultipleComponent]
    public class DisposeOnDestroy : MonoBehaviour, IDisposableList
    {
        public List<IDisposable> Disposables { get; private set; } = new List<IDisposable>();

        public void Add(IDisposable value)
        {
            this.Disposables.Add(value);
        }
        
        private void OnDestroy()
        {
            this.DisposeAll();
        }
        
    }
}