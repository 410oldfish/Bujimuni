/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class WgtSampleOfScrollViewItem : UIWidget<WgtSampleOfScrollViewItem.CPara>
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
			m_SelfUIWidgetMono = null;
			
			m_LabelGameObject = null;
			m_LabelRectTransform = null;
			m_LabelCanvasRenderer = null;
			m_LabelTextMeshProUGUI = null;
			
			#endregion


            this.Transform = null;
            this.Transform2D = null;
            this.GameObject = null;
        }

		#region 自动生成变量定义
		private Lighten.UIWidgetMono m_SelfUIWidgetMono;
		public Lighten.UIWidgetMono SelfUIWidgetMono
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_SelfUIWidgetMono == null)
				{
					m_SelfUIWidgetMono = Transform.GetComponent<Lighten.UIWidgetMono>();
				}
				return m_SelfUIWidgetMono;
			}
		}
		
		
		private GameObject m_LabelGameObject;
		public GameObject LabelGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LabelGameObject == null)
				{
					m_LabelGameObject = Transform.Q("w_Label").gameObject;
				}
				return m_LabelGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_LabelRectTransform;
		public UnityEngine.RectTransform LabelRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LabelRectTransform == null)
				{
					m_LabelRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Label");
				}
				return m_LabelRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_LabelCanvasRenderer;
		public UnityEngine.CanvasRenderer LabelCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LabelCanvasRenderer == null)
				{
					m_LabelCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Label");
				}
				return m_LabelCanvasRenderer;
			}
		}
		
		private TMPro.TextMeshProUGUI m_LabelTextMeshProUGUI;
		public TMPro.TextMeshProUGUI LabelTextMeshProUGUI
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LabelTextMeshProUGUI == null)
				{
					m_LabelTextMeshProUGUI = Transform.Q<TMPro.TextMeshProUGUI>("w_Label");
				}
				return m_LabelTextMeshProUGUI;
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
