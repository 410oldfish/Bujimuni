using System.IO;
using Cysharp.Threading.Tasks;
using Lighten;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;

public class GameDataManagerSample : MonoBehaviour
{
    [GameData]
    public class PlayerData : GameData
    {
        public string name;
        public int age;
        public string phone;
        [JsonIgnore]
        public ReactiveProperty<bool> changed = new ReactiveProperty<bool>();
    }

    async UniTask Start()
    {
        await LightenEntry.InitializeModules();
        var playerData = LightenEntry.GameDataManager.GetGameData<PlayerData>();
        playerData.name = "Mike";
        playerData.age = 18;
        playerData.phone = "13391109110";
        playerData.changed.Value = !playerData.changed.Value;
        
        Debug.Log(JsonConvert.SerializeObject(playerData, Formatting.Indented));
        
        await UniTask.CompletedTask;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
