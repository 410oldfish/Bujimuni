using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lighten.Sample
{
    public class SimpleUI : XEntityMonoBehaviour<GameEntity>, IController
    {
        public TextMeshProUGUI NumberText;
        public Button IncreaseButton;
        public Button DecreaseButton;
        public Button Increase5Button;
        public Button StopButton;
        
        private void Start()
        {
            var model = this.GetModel<ArchhitectureSample.SimpleModel>();
            model.Count.Subscribe(v => this.NumberText.text = $"{v}");
            this.IncreaseButton.onClick.AddListener(() => { this.SendCommand<ArchhitectureSample.IncreaseCommand>(); });
            this.DecreaseButton.onClick.AddListener(() => { this.SendCommand<ArchhitectureSample.DecreaseCommand>(); });
            this.Increase5Button.onClick.AddListener(() =>
            {
                var cts = this.Entity.UniTaskCTS.Create("Counter");
                var command = this.CreateCommand<ArchhitectureSample.IncreaseValueAsyncCommand>();
                command.Count = 5;
                this.SendCommandAsync(command, cts.Token).Forget();
            });
            this.StopButton.onClick.AddListener(() =>
            {
                Debug.Log("按下停止");
                this.Entity.UniTaskCTS.Cancel("Counter");
            });
            this.AddListener<ArchhitectureSample.CountArrived100Event>(e => { Debug.Log($"数字到达100"); }).AddTo(this);
            this.AddListener<ArchhitectureSample.CountArrived100Event>(async (e, token) =>
            {
                Debug.Log($"数字到达100 异步");
                for (int i = 0; i < 3; ++i)
                {
                    await UniTask.Delay(100);
                    Debug.Log($"恭喜!");
                }
            }).AddTo(this);
            this.AddListener<ArchhitectureSample.ShowDebugTextEvent>(async (e, token) =>
            {
                Debug.Log(e.Text);
                await UniTask.Delay(1000, cancellationToken: token);
                Debug.Log($"{e.Text} 结束");
            }).AddTo(this);
        }

        public IArchitecture GetArchitecture()
        {
            return Game.Architecture;
        }
    }
}