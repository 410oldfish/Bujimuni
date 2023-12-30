/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class DlgSampleOfLoadSprite : UIWindow<DlgSampleOfLoadSprite.CPara>
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
			
			m_ImgGameObject = null;
			m_ImgRectTransform = null;
			m_ImgCanvasRenderer = null;
			m_ImgImage = null;
			
			m_InputNameGameObject = null;
			m_InputNameRectTransform = null;
			m_InputNameCanvasRenderer = null;
			m_InputNameImage = null;
			m_InputNameTMP_InputField = null;
			
			m_LoadGameObject = null;
			m_LoadRectTransform = null;
			m_LoadCanvasRenderer = null;
			m_LoadImage = null;
			m_LoadButton = null;
			
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
		
		
		private GameObject m_ImgGameObject;
		public GameObject ImgGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ImgGameObject == null)
				{
					m_ImgGameObject = Transform.Q("w_Img").gameObject;
				}
				return m_ImgGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_ImgRectTransform;
		public UnityEngine.RectTransform ImgRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ImgRectTransform == null)
				{
					m_ImgRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Img");
				}
				return m_ImgRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_ImgCanvasRenderer;
		public UnityEngine.CanvasRenderer ImgCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ImgCanvasRenderer == null)
				{
					m_ImgCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Img");
				}
				return m_ImgCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_ImgImage;
		public UnityEngine.UI.Image ImgImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ImgImage == null)
				{
					m_ImgImage = Transform.Q<UnityEngine.UI.Image>("w_Img");
				}
				return m_ImgImage;
			}
		}
		
		
		private GameObject m_InputNameGameObject;
		public GameObject InputNameGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_InputNameGameObject == null)
				{
					m_InputNameGameObject = Transform.Q("w_InputName").gameObject;
				}
				return m_InputNameGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_InputNameRectTransform;
		public UnityEngine.RectTransform InputNameRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_InputNameRectTransform == null)
				{
					m_InputNameRectTransform = Transform.Q<UnityEngine.RectTransform>("w_InputName");
				}
				return m_InputNameRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_InputNameCanvasRenderer;
		public UnityEngine.CanvasRenderer InputNameCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_InputNameCanvasRenderer == null)
				{
					m_InputNameCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_InputName");
				}
				return m_InputNameCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_InputNameImage;
		public UnityEngine.UI.Image InputNameImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_InputNameImage == null)
				{
					m_InputNameImage = Transform.Q<UnityEngine.UI.Image>("w_InputName");
				}
				return m_InputNameImage;
			}
		}
		
		private TMPro.TMP_InputField m_InputNameTMP_InputField;
		public TMPro.TMP_InputField InputNameTMP_InputField
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_InputNameTMP_InputField == null)
				{
					m_InputNameTMP_InputField = Transform.Q<TMPro.TMP_InputField>("w_InputName");
				}
				return m_InputNameTMP_InputField;
			}
		}
		
		
		private GameObject m_LoadGameObject;
		public GameObject LoadGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadGameObject == null)
				{
					m_LoadGameObject = Transform.Q("w_Load").gameObject;
				}
				return m_LoadGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_LoadRectTransform;
		public UnityEngine.RectTransform LoadRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadRectTransform == null)
				{
					m_LoadRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Load");
				}
				return m_LoadRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_LoadCanvasRenderer;
		public UnityEngine.CanvasRenderer LoadCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadCanvasRenderer == null)
				{
					m_LoadCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Load");
				}
				return m_LoadCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_LoadImage;
		public UnityEngine.UI.Image LoadImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadImage == null)
				{
					m_LoadImage = Transform.Q<UnityEngine.UI.Image>("w_Load");
				}
				return m_LoadImage;
			}
		}
		
		private UnityEngine.UI.Button m_LoadButton;
		public UnityEngine.UI.Button LoadButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadButton == null)
				{
					m_LoadButton = Transform.Q<UnityEngine.UI.Button>("w_Load");
				}
				return m_LoadButton;
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
