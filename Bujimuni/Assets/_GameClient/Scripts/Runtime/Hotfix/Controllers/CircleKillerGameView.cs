/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class CircleKillerGameView : XEntity, IAwake<Transform>, IDestroy
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
			#region 自动生成变量销毁
			m_SelfScriptToolMono = null;
			
			#endregion


        this.Transform = null;
        this.Transform2D = null;
        this.GameObject = null;
    }

		#region 自动生成变量定义
		private Lighten.ScriptToolMono m_SelfScriptToolMono;
		public Lighten.ScriptToolMono SelfScriptToolMono
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_SelfScriptToolMono == null)
				{
					m_SelfScriptToolMono = Transform.GetComponent<Lighten.ScriptToolMono>();
				}
				return m_SelfScriptToolMono;
			}
		}
		
		
		#endregion

}

public partial class CircleKillerGameController : XEntityController
{
    public CircleKillerGameView View { get; private set; }

    protected override void OnInitComponents()
    {
        this.View = this.Entity.AddComponent<CircleKillerGameView, Transform>(transform);
    }
}