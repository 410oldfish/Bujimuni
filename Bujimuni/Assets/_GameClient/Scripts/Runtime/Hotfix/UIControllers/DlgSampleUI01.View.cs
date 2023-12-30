/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class DlgSampleUI01 : UIWindow<DlgSampleUI01.CPara>
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
			
			m_TitleGameObject = null;
			m_TitleRectTransform = null;
			m_TitleCanvasRenderer = null;
			m_TitleTextMeshProUGUI = null;
			
			m_OpenGameObject = null;
			m_OpenRectTransform = null;
			m_OpenCanvasRenderer = null;
			m_OpenImage = null;
			m_OpenButton = null;
			
			m_OpenInstanceGameObject = null;
			m_OpenInstanceRectTransform = null;
			m_OpenInstanceCanvasRenderer = null;
			m_OpenInstanceImage = null;
			m_OpenInstanceButton = null;
			
			m_CloseGameObject = null;
			m_CloseRectTransform = null;
			m_CloseCanvasRenderer = null;
			m_CloseImage = null;
			m_CloseButton = null;
			
			m_WgtW1 = null;
			m_WgtW2 = null;
			m_WgtW3 = null;
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
		
		
		private GameObject m_TitleGameObject;
		public GameObject TitleGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_TitleGameObject == null)
				{
					m_TitleGameObject = Transform.Q("w_Title").gameObject;
				}
				return m_TitleGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_TitleRectTransform;
		public UnityEngine.RectTransform TitleRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_TitleRectTransform == null)
				{
					m_TitleRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Title");
				}
				return m_TitleRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_TitleCanvasRenderer;
		public UnityEngine.CanvasRenderer TitleCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_TitleCanvasRenderer == null)
				{
					m_TitleCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Title");
				}
				return m_TitleCanvasRenderer;
			}
		}
		
		private TMPro.TextMeshProUGUI m_TitleTextMeshProUGUI;
		public TMPro.TextMeshProUGUI TitleTextMeshProUGUI
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_TitleTextMeshProUGUI == null)
				{
					m_TitleTextMeshProUGUI = Transform.Q<TMPro.TextMeshProUGUI>("w_Title");
				}
				return m_TitleTextMeshProUGUI;
			}
		}
		
		
		private GameObject m_OpenGameObject;
		public GameObject OpenGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenGameObject == null)
				{
					m_OpenGameObject = Transform.Q("w_Open").gameObject;
				}
				return m_OpenGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_OpenRectTransform;
		public UnityEngine.RectTransform OpenRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenRectTransform == null)
				{
					m_OpenRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Open");
				}
				return m_OpenRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_OpenCanvasRenderer;
		public UnityEngine.CanvasRenderer OpenCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenCanvasRenderer == null)
				{
					m_OpenCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Open");
				}
				return m_OpenCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_OpenImage;
		public UnityEngine.UI.Image OpenImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenImage == null)
				{
					m_OpenImage = Transform.Q<UnityEngine.UI.Image>("w_Open");
				}
				return m_OpenImage;
			}
		}
		
		private UnityEngine.UI.Button m_OpenButton;
		public UnityEngine.UI.Button OpenButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenButton == null)
				{
					m_OpenButton = Transform.Q<UnityEngine.UI.Button>("w_Open");
				}
				return m_OpenButton;
			}
		}
		
		
		private GameObject m_OpenInstanceGameObject;
		public GameObject OpenInstanceGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenInstanceGameObject == null)
				{
					m_OpenInstanceGameObject = Transform.Q("w_OpenInstance").gameObject;
				}
				return m_OpenInstanceGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_OpenInstanceRectTransform;
		public UnityEngine.RectTransform OpenInstanceRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenInstanceRectTransform == null)
				{
					m_OpenInstanceRectTransform = Transform.Q<UnityEngine.RectTransform>("w_OpenInstance");
				}
				return m_OpenInstanceRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_OpenInstanceCanvasRenderer;
		public UnityEngine.CanvasRenderer OpenInstanceCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenInstanceCanvasRenderer == null)
				{
					m_OpenInstanceCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_OpenInstance");
				}
				return m_OpenInstanceCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_OpenInstanceImage;
		public UnityEngine.UI.Image OpenInstanceImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenInstanceImage == null)
				{
					m_OpenInstanceImage = Transform.Q<UnityEngine.UI.Image>("w_OpenInstance");
				}
				return m_OpenInstanceImage;
			}
		}
		
		private UnityEngine.UI.Button m_OpenInstanceButton;
		public UnityEngine.UI.Button OpenInstanceButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_OpenInstanceButton == null)
				{
					m_OpenInstanceButton = Transform.Q<UnityEngine.UI.Button>("w_OpenInstance");
				}
				return m_OpenInstanceButton;
			}
		}
		
		
		private GameObject m_CloseGameObject;
		public GameObject CloseGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CloseGameObject == null)
				{
					m_CloseGameObject = Transform.Q("w_Close").gameObject;
				}
				return m_CloseGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_CloseRectTransform;
		public UnityEngine.RectTransform CloseRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CloseRectTransform == null)
				{
					m_CloseRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Close");
				}
				return m_CloseRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_CloseCanvasRenderer;
		public UnityEngine.CanvasRenderer CloseCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CloseCanvasRenderer == null)
				{
					m_CloseCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Close");
				}
				return m_CloseCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_CloseImage;
		public UnityEngine.UI.Image CloseImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CloseImage == null)
				{
					m_CloseImage = Transform.Q<UnityEngine.UI.Image>("w_Close");
				}
				return m_CloseImage;
			}
		}
		
		private UnityEngine.UI.Button m_CloseButton;
		public UnityEngine.UI.Button CloseButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CloseButton == null)
				{
					m_CloseButton = Transform.Q<UnityEngine.UI.Button>("w_Close");
				}
				return m_CloseButton;
			}
		}
		
		
		private WgtSample01 m_WgtW1;
		public WgtSample01 WgtW1
		{
			get
			{
				if (m_WgtW1 == null)
				{
					m_WgtW1 = this.GetParent<UIWindow>().GetWidget<WgtSample01>("WgtW1");
				}
				return m_WgtW1;
			}
		}
		
		private WgtSample01 m_WgtW2;
		public WgtSample01 WgtW2
		{
			get
			{
				if (m_WgtW2 == null)
				{
					m_WgtW2 = this.GetParent<UIWindow>().GetWidget<WgtSample01>("WgtW2");
				}
				return m_WgtW2;
			}
		}
		
		private WgtSample01 m_WgtW3;
		public WgtSample01 WgtW3
		{
			get
			{
				if (m_WgtW3 == null)
				{
					m_WgtW3 = this.GetParent<UIWindow>().GetWidget<WgtSample01>("WgtW3");
				}
				return m_WgtW3;
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
