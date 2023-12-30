using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lighten;

public class ScriptObjectPoolSample : MonoBehaviour
{
    public class A
    {
        public int value;
    }
    public class B
    {
        public int value;
    }

    // Start is called before the first frame update
    void Start()
    {
        var l = new List<A>();
        for (int i = 0; i < 10; ++i)
        {
            var a = Utility.ScriptPool.Spawn<A>();
            l.Add(a);
        }
        foreach (var a in l)
        {
            Utility.ScriptPool.Recycle(a);
        }
        for (int i = 0; i < 10; ++i)
        {
            var b = Utility.ScriptPool.Spawn<B>();
            Utility.ScriptPool.Recycle(b);
        }
        Debug.Log(Utility.ScriptPool.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
