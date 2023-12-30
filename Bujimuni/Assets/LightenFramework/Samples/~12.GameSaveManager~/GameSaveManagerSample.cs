using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Lighten;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSaveManagerSample : MonoBehaviour
{
    [GameSave]
    public class PlayerSaveData : GameSave
    {
        public const string NAME = "PlayerSaveData";
        public class Data
        {
            public string name;
            public int age;
        }

        public Data data { get; private set; } = new Data();

        public override byte[] Generate()
        {
            data.name = "Mike";
            data.age = Random.Range(18, 99);
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }

        public override void Load(byte[] datas)
        {
            data = JsonConvert.DeserializeObject<Data>(Encoding.UTF8.GetString(datas));
        }
    }

    [GameSave(false)]
    public class DeviceSaveData : GameSave
    {
        public const string NAME = "DeviceSaveData";
        public class Data
        {
            public bool allowedMusic;
            public bool allowedSound;
        }

        public Data data { get; private set; } = new Data();

        public override byte[] Generate()
        {
            data.allowedMusic = Random.Range(0, 100) > 50;
            data.allowedSound = Random.Range(0, 100) > 50;
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }

        public override void Load(byte[] datas)
        {
            data = JsonConvert.DeserializeObject<Data>(Encoding.UTF8.GetString(datas));
        }
    }

    // public class SaveWriter : IGameSaveWriter
    // {
    //     public async UniTask<bool> Save(string saveDataName, string content)
    //     {
    //         var filePath = $"{Application.dataPath}/../Document/{saveDataName}.txt";
    //         DirectoryExtension.GenerateDirectory(filePath);
    //         Debug.Log($"写入存档 {filePath}");
    //         await File.WriteAllTextAsync(filePath, content);
    //         return true;
    //     }
    //
    //     public async UniTask<string> Load(string saveDataName)
    //     {
    //         var filePath = $"{Application.dataPath}/../Document/{saveDataName}.txt";
    //         DirectoryExtension.GenerateDirectory(filePath);
    //         Debug.Log($"读取存档 {filePath}");
    //         return await File.ReadAllTextAsync(filePath);
    //     }
    // }

    // Start is called before the first frame update
    async void Start()
    {
        // await LightenEntry.InitializeModules();
        //
        // //LightenEntry.GameSaveManager.SetParentPath($"{Application.dataPath}/../Documents");
        // LightenEntry.GameSaveManager.SetAccount("GGYY");
        // LightenEntry.GameSaveManager.SetWriter(new SaveWriter());
        //
        // await LightenEntry.GameSaveManager.Load<PlayerSaveData>(PlayerSaveData.NAME);
        // var playerSaveData = LightenEntry.GameSaveManager.GetGameSave<PlayerSaveData>();
        // Debug.Log(JsonConvert.SerializeObject(playerSaveData.data));
        // await LightenEntry.GameSaveManager.Save<PlayerSaveData>(PlayerSaveData.NAME);
        //
        // await LightenEntry.GameSaveManager.Load<PlayerSaveData>(DeviceSaveData.NAME);
        // var deviceSaveData = LightenEntry.GameSaveManager.GetGameSave<DeviceSaveData>();
        // Debug.Log(JsonConvert.SerializeObject(deviceSaveData.data));
        // await LightenEntry.GameSaveManager.Save<PlayerSaveData>(DeviceSaveData.NAME);
        await UniTask.CompletedTask;
    }

    // Update is called once per frame
    void Update()
    {
    }
}