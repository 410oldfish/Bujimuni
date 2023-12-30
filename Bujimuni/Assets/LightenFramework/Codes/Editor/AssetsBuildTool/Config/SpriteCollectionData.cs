using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SpriteCollectionData : ScriptableObject
{
    [Serializable]
    public class Data
    {
        public GameObject prefab;
        public string folderPath;
        public List<Sprite> sprites = new List<Sprite>();
    }
    public List<Data> datas = new List<Data>();
    
}
