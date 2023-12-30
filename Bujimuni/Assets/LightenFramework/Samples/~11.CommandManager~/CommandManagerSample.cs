using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;

public class CommandManagerSample : MonoBehaviour
{
    // public class Command1 : Command<Command1>
    // {
    //     private string m_message;
    //     public override async UniTask Execute()
    //     {
    //         Debug.Log($"execute Command1 {m_message}");
    //         await UniTask.CompletedTask;
    //     }
    //
    //     public void Send(string message)
    //     {
    //         m_message = message;
    //         Send();
    //     }
    // }
    // public class Command2 : Command<Command2>
    // {
    //     public override async UniTask Execute()
    //     {
    //         Debug.Log("execute Command2");
    //         await UniTask.CompletedTask;
    //     }
    // }
    // // Start is called before the first frame update
    // async void Start()
    // {
    //     await LightenEntry.InitializeModules();
    //     Debug.Log(LightenEntry.CommandManager.ToString());
    //     var command1 = LightenEntry.CommandManager.AcquireCommand<Command1>();
    //     command1.Send("Hello World");
    //     var command2 = LightenEntry.CommandManager.AcquireCommand<Command2>();
    //     command2.Send();
    //     Debug.Log(LightenEntry.CommandManager.ToString());
    //     await UniTask.Delay(TimeSpan.FromSeconds(1f));
    //     Debug.Log(LightenEntry.CommandManager.ToString());
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
