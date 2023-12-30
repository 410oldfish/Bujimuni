/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class UIWindowTemplate /*ScriptName*/ : UIWindow<UIWindowTemplate /*ScriptName*/.CPara>
{
    public class ViewComponent : XEntity, IAwake<Transform>, IDestroy
    {
        public Transform Transform { get; private set; }
        public RectTransform Transform2D { get; private set; }
        public GameObject GameObject { get; private set; }

        public void Awake(Transform transform)
        {
            this.Transform = transform;
            this.Transform2D = transform as RectTransform;
            this.GameObject = transform.gameObject;
        }

        public void Destroy()
        {
/*FIELDS_REMOVE*/

            this.Transform = null;
            this.Transform2D = null;
            this.GameObject = null;
        }

/*FIELDS_DEFINE*/
    }
    
    public ViewComponent View { get; private set; }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        this.View = this.AddComponent<ViewComponent, Transform>(this.GameObject.transform);
    }
}
