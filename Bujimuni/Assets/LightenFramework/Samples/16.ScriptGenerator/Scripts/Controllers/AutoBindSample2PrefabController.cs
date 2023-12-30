using System.Threading;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;

public partial class AutoBindSample2PrefabController : XEntityController
{
    public int a;

    private void Start()
    {
        Utility.Debug.Log("GGYY", $"{this.GetType().Name} = {a}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            View.inst01Controller.Show1();
            View.inst02Controller.Show2();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            View.inst01Controller.Show2();
            View.inst02Controller.Show1();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.PlayFlash(this.Entity.UniTaskCTS.Default).Forget();
            //this.PlayFlash().Forget();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Destroy(gameObject);
        }
    }

    private async UniTask PlayFlash(CancellationToken cancellationToken = default)
    {
        for (int i = 0; i < 10; ++i)
        {
            View.inst01Controller.Show1();
            View.inst02Controller.Show2();
            await UniTask.WaitForSeconds(0.1f, cancellationToken: cancellationToken);
            View.inst01Controller.Show2();
            View.inst02Controller.Show1();
            await UniTask.WaitForSeconds(0.1f, cancellationToken: cancellationToken);
        }
    }
}