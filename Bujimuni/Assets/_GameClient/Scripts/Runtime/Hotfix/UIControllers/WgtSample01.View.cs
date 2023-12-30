/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class WgtSample01 : UIWidget<WgtSample01.CPara>
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
			m_SelfUIScriptToolMono = null;
			m_SelfCanvasRenderer = null;
			m_SelfImage = null;
			m_SelfUIWidgetMono = null;
			
			m_NameGameObject = null;
			m_NameRectTransform = null;
			m_NameCanvasRenderer = null;
			m_NameTextMeshProUGUI = null;
			
			#endregion


            this.Transform = null;
            this.Transform2D = null;
            this.GameObject = null;
        }

		#region 自动生成变量定义
		private Lighten.UIScriptToolMono m_SelfUIScriptToolMono;
		public Lighten.UIScriptToolMono SelfUIScriptToolMono
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_SelfUIScriptToolMono == null)
				{
					m_SelfUIScriptToolMono = Transform.GetComponent<Lighten.UIScriptToolMono>();
				}
				return m_SelfUIScriptToolMono;
			}
		}
		
		private UnityEngine.CanvasRenderer m_SelfCanvasRenderer;
		public UnityEngine.CanvasRenderer SelfCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_SelfCanvasRenderer == null)
				{
					m_SelfCanvasRenderer = Transform.GetComponent<UnityEngine.CanvasRenderer>();
				}
				return m_SelfCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_SelfImage;
		public UnityEngine.UI.Image SelfImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_SelfImage == null)
				{
					m_SelfImage = Transform.GetComponent<UnityEngine.UI.Image>();
				}
				return m_SelfImage;
			}
		}
		
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
		
		
		private GameObject m_NameGameObject;
		public GameObject NameGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_NameGameObject == null)
				{
					m_NameGameObject = Transform.Q("w_Name").gameObject;
				}
				return m_NameGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_NameRectTransform;
		public UnityEngine.RectTransform NameRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_NameRectTransform == null)
				{
					m_NameRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Name");
				}
				return m_NameRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_NameCanvasRenderer;
		public UnityEngine.CanvasRenderer NameCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_NameCanvasRenderer == null)
				{
					m_NameCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Name");
				}
				return m_NameCanvasRenderer;
			}
		}
		
		private TMPro.TextMeshProUGUI m_NameTextMeshProUGUI;
		public TMPro.TextMeshProUGUI NameTextMeshProUGUI
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_NameTextMeshProUGUI == null)
				{
					m_NameTextMeshProUGUI = Transform.Q<TMPro.TextMeshProUGUI>("w_Name");
				}
				return m_NameTextMeshProUGUI;
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
