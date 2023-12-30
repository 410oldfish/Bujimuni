using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using  Lighten;

public class EventManagerSample : MonoBehaviour
{
    public class SampleEvent01 : GameEvent<SampleEvent01>
    {
        public int a;

        public static void Trigger(int a)
        {
            var evtData = LightenEntry.GameEventManager.AcquireEvent<SampleEvent01>();
            evtData.a = a;
            evtData.PostEvent();
        }
    }

    private async void Start()
    {
        await LightenEntry.InitializeModules();
        
        var eventManager = LightenEntry.GameEventManager;
        //eventManager.AddListener<SampleEvent01>(OnEvent_SampleEvent01);
        var eventService = eventManager.AcquireService();
        eventService.AddListener<SampleEvent01>(OnEvent_SampleEvent01);
        eventService.AddListener<SampleEvent01>(OnEvent_SampleEvent02);
        Debug.Log(eventManager.ToString());
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        SampleEvent01.Trigger(1);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        SampleEvent01.Trigger(2);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        SampleEvent01.Trigger(3);
        
        //LightenEntry.EventManager.RemoveListener<SampleEvent01>(OnEvent_SampleEvent01);

        eventService.RemoveAll();
        eventManager.RecycleService(eventService);
        Debug.Log(eventManager.ToString());
    }
    private void OnEvent_SampleEvent01(IGameEvent gameEvent)
    {
        Debug.Log($"01  {(gameEvent as SampleEvent01).a}");
    }

    private void OnEvent_SampleEvent02(IGameEvent gameEvent)
    {
        Debug.Log($"02  {(gameEvent as SampleEvent01).a}");
    }
}
