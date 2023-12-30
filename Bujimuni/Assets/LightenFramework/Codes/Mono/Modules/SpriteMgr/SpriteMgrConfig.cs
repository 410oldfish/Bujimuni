using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public enum ESpriteType
    {
        Sprite = 0,
        SpriteSheet,
        SpriteAtlas,
    }
    public class SpriteMgrConfig : ScriptableObject
    {
        public const string DEFAULT_NAME = "SpriteMgrConfig";
        
        [Serializable]
        public class SpriteData
        {
            public string AssetName;
            public ESpriteType Type;
            public List<string> SpriteNames = new List<string>();
        }

        public List<SpriteData> Datas = new List<SpriteData>();
    }
}