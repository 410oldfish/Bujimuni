/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class DlgSampleOfScrollViewWithTitle : UIWindow<DlgSampleOfScrollViewWithTitle.CPara>
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
			#region 自动生成变量销毁
			m_SelfCanvas = null;
			m_SelfGraphicRaycaster = null;
			
			m_OSAMultiGroupGrid = null;
			#endregion


            this.Transform = null;
            this.Transform2D = null;
            this.GameObject = null;
        }

		#region 自动生成变量定义
		private UnityEngine.Canvas m_SelfCanvas;
		public UnityEngine.Canvas SelfCanvas
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_SelfCanvas == null)
				{
					m_SelfCanvas = Transform.GetComponent<UnityEngine.Canvas>();
				}
				return m_SelfCanvas;
			}
		}
		
		private UnityEngine.UI.GraphicRaycaster m_SelfGraphicRaycaster;
		public UnityEngine.UI.GraphicRaycaster SelfGraphicRaycaster
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_SelfGraphicRaycaster == null)
				{
					m_SelfGraphicRaycaster = Transform.GetComponent<UnityEngine.UI.GraphicRaycaster>();
				}
				return m_SelfGraphicRaycaster;
			}
		}
		
		
				private Lighten.OSAMultiGroupGridViewComponent m_OSAMultiGroupGrid;
				public Lighten.OSAMultiGroupGridViewComponent OSAMultiGroupGrid
				{
					get
					{
						if (m_OSAMultiGroupGrid == null)
						{
							m_OSAMultiGroupGrid = this.GetParent<UIWidget>().GetChild<Lighten.OSAMultiGroupGridViewComponent>("OSAMultiGroupGrid");
						}
						return m_OSAMultiGroupGrid;
					}
				}
		
		#endregion

    }
    
    public ViewComponent View { get; private set; }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        this.View = this.AddComponent<ViewComponent, Transform>(this.GameObject.transform);
    }
}
