/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class DlgMessageBox : UIWindow<DlgMessageBox.CPara>
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
			m_SelfCanvas = null;
			m_SelfGraphicRaycaster = null;
			
			m_MessageGameObject = null;
			m_MessageRectTransform = null;
			m_MessageCanvasRenderer = null;
			m_MessageTextMeshProUGUI = null;
			
			m_ConfirmGameObject = null;
			m_ConfirmRectTransform = null;
			m_ConfirmCanvasRenderer = null;
			m_ConfirmImage = null;
			m_ConfirmButton = null;
			
			m_CancelGameObject = null;
			m_CancelRectTransform = null;
			m_CancelCanvasRenderer = null;
			m_CancelImage = null;
			m_CancelButton = null;
			
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
		
		private GameObject m_MessageGameObject;
		public GameObject MessageGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_MessageGameObject == null)
				{
					m_MessageGameObject = Transform.Q("w_Message").gameObject;
				}
				return m_MessageGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_MessageRectTransform;
		public UnityEngine.RectTransform MessageRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_MessageRectTransform == null)
				{
					m_MessageRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Message");
				}
				return m_MessageRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_MessageCanvasRenderer;
		public UnityEngine.CanvasRenderer MessageCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_MessageCanvasRenderer == null)
				{
					m_MessageCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Message");
				}
				return m_MessageCanvasRenderer;
			}
		}
		
		private TMPro.TextMeshProUGUI m_MessageTextMeshProUGUI;
		public TMPro.TextMeshProUGUI MessageTextMeshProUGUI
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_MessageTextMeshProUGUI == null)
				{
					m_MessageTextMeshProUGUI = Transform.Q<TMPro.TextMeshProUGUI>("w_Message");
				}
				return m_MessageTextMeshProUGUI;
			}
		}
		
		
		private GameObject m_ConfirmGameObject;
		public GameObject ConfirmGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ConfirmGameObject == null)
				{
					m_ConfirmGameObject = Transform.Q("w_Confirm").gameObject;
				}
				return m_ConfirmGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_ConfirmRectTransform;
		public UnityEngine.RectTransform ConfirmRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ConfirmRectTransform == null)
				{
					m_ConfirmRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Confirm");
				}
				return m_ConfirmRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_ConfirmCanvasRenderer;
		public UnityEngine.CanvasRenderer ConfirmCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ConfirmCanvasRenderer == null)
				{
					m_ConfirmCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Confirm");
				}
				return m_ConfirmCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_ConfirmImage;
		public UnityEngine.UI.Image ConfirmImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ConfirmImage == null)
				{
					m_ConfirmImage = Transform.Q<UnityEngine.UI.Image>("w_Confirm");
				}
				return m_ConfirmImage;
			}
		}
		
		private UnityEngine.UI.Button m_ConfirmButton;
		public UnityEngine.UI.Button ConfirmButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ConfirmButton == null)
				{
					m_ConfirmButton = Transform.Q<UnityEngine.UI.Button>("w_Confirm");
				}
				return m_ConfirmButton;
			}
		}
		
		
		private GameObject m_CancelGameObject;
		public GameObject CancelGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CancelGameObject == null)
				{
					m_CancelGameObject = Transform.Q("w_Cancel").gameObject;
				}
				return m_CancelGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_CancelRectTransform;
		public UnityEngine.RectTransform CancelRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CancelRectTransform == null)
				{
					m_CancelRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Cancel");
				}
				return m_CancelRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_CancelCanvasRenderer;
		public UnityEngine.CanvasRenderer CancelCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CancelCanvasRenderer == null)
				{
					m_CancelCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Cancel");
				}
				return m_CancelCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_CancelImage;
		public UnityEngine.UI.Image CancelImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CancelImage == null)
				{
					m_CancelImage = Transform.Q<UnityEngine.UI.Image>("w_Cancel");
				}
				return m_CancelImage;
			}
		}
		
		private UnityEngine.UI.Button m_CancelButton;
		public UnityEngine.UI.Button CancelButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CancelButton == null)
				{
					m_CancelButton = Transform.Q<UnityEngine.UI.Button>("w_Cancel");
				}
				return m_CancelButton;
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
