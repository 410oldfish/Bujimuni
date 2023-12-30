using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    private const float ONE_OVER_1024 = 1.0f / 1024;
    
    public static string GetByteText(long size)
    {
        var kb = size * ONE_OVER_1024;
        if (kb < 1024)
        {
            return kb.ToString("f2") + "KB";
        }
        var mb = kb * ONE_OVER_1024;
        return mb.ToString("f2") + "MB";
    }
}
