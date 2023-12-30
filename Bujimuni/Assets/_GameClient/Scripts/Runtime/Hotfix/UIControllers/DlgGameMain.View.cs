/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class DlgGameMain : UIWindow<DlgGameMain.CPara>
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
			
			m_BackGameObject = null;
			m_BackRectTransform = null;
			m_BackCanvasRenderer = null;
			m_BackImage = null;
			m_BackButton = null;
			
			m_ScrollViewGameObject = null;
			m_ScrollViewRectTransform = null;
			m_ScrollViewCanvasRenderer = null;
			m_ScrollViewImage = null;
			m_ScrollViewButton = null;
			
			m_ScrollViewWithTitleGameObject = null;
			m_ScrollViewWithTitleRectTransform = null;
			m_ScrollViewWithTitleCanvasRenderer = null;
			m_ScrollViewWithTitleImage = null;
			m_ScrollViewWithTitleButton = null;
			
			m_LoadSpriteGameObject = null;
			m_LoadSpriteRectTransform = null;
			m_LoadSpriteCanvasRenderer = null;
			m_LoadSpriteImage = null;
			m_LoadSpriteButton = null;
			
			m_CircleKillerGameObject = null;
			m_CircleKillerRectTransform = null;
			m_CircleKillerCanvasRenderer = null;
			m_CircleKillerImage = null;
			m_CircleKillerButton = null;
			
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
		
		
		private GameObject m_BackGameObject;
		public GameObject BackGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_BackGameObject == null)
				{
					m_BackGameObject = Transform.Q("w_Back").gameObject;
				}
				return m_BackGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_BackRectTransform;
		public UnityEngine.RectTransform BackRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_BackRectTransform == null)
				{
					m_BackRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Back");
				}
				return m_BackRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_BackCanvasRenderer;
		public UnityEngine.CanvasRenderer BackCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_BackCanvasRenderer == null)
				{
					m_BackCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Back");
				}
				return m_BackCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_BackImage;
		public UnityEngine.UI.Image BackImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_BackImage == null)
				{
					m_BackImage = Transform.Q<UnityEngine.UI.Image>("w_Back");
				}
				return m_BackImage;
			}
		}
		
		private UnityEngine.UI.Button m_BackButton;
		public UnityEngine.UI.Button BackButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_BackButton == null)
				{
					m_BackButton = Transform.Q<UnityEngine.UI.Button>("w_Back");
				}
				return m_BackButton;
			}
		}
		
		
		private GameObject m_ScrollViewGameObject;
		public GameObject ScrollViewGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewGameObject == null)
				{
					m_ScrollViewGameObject = Transform.Q("w_ScrollView").gameObject;
				}
				return m_ScrollViewGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_ScrollViewRectTransform;
		public UnityEngine.RectTransform ScrollViewRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewRectTransform == null)
				{
					m_ScrollViewRectTransform = Transform.Q<UnityEngine.RectTransform>("w_ScrollView");
				}
				return m_ScrollViewRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_ScrollViewCanvasRenderer;
		public UnityEngine.CanvasRenderer ScrollViewCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewCanvasRenderer == null)
				{
					m_ScrollViewCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_ScrollView");
				}
				return m_ScrollViewCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_ScrollViewImage;
		public UnityEngine.UI.Image ScrollViewImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewImage == null)
				{
					m_ScrollViewImage = Transform.Q<UnityEngine.UI.Image>("w_ScrollView");
				}
				return m_ScrollViewImage;
			}
		}
		
		private UnityEngine.UI.Button m_ScrollViewButton;
		public UnityEngine.UI.Button ScrollViewButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewButton == null)
				{
					m_ScrollViewButton = Transform.Q<UnityEngine.UI.Button>("w_ScrollView");
				}
				return m_ScrollViewButton;
			}
		}
		
		
		private GameObject m_ScrollViewWithTitleGameObject;
		public GameObject ScrollViewWithTitleGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewWithTitleGameObject == null)
				{
					m_ScrollViewWithTitleGameObject = Transform.Q("w_ScrollViewWithTitle").gameObject;
				}
				return m_ScrollViewWithTitleGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_ScrollViewWithTitleRectTransform;
		public UnityEngine.RectTransform ScrollViewWithTitleRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewWithTitleRectTransform == null)
				{
					m_ScrollViewWithTitleRectTransform = Transform.Q<UnityEngine.RectTransform>("w_ScrollViewWithTitle");
				}
				return m_ScrollViewWithTitleRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_ScrollViewWithTitleCanvasRenderer;
		public UnityEngine.CanvasRenderer ScrollViewWithTitleCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewWithTitleCanvasRenderer == null)
				{
					m_ScrollViewWithTitleCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_ScrollViewWithTitle");
				}
				return m_ScrollViewWithTitleCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_ScrollViewWithTitleImage;
		public UnityEngine.UI.Image ScrollViewWithTitleImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewWithTitleImage == null)
				{
					m_ScrollViewWithTitleImage = Transform.Q<UnityEngine.UI.Image>("w_ScrollViewWithTitle");
				}
				return m_ScrollViewWithTitleImage;
			}
		}
		
		private UnityEngine.UI.Button m_ScrollViewWithTitleButton;
		public UnityEngine.UI.Button ScrollViewWithTitleButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScrollViewWithTitleButton == null)
				{
					m_ScrollViewWithTitleButton = Transform.Q<UnityEngine.UI.Button>("w_ScrollViewWithTitle");
				}
				return m_ScrollViewWithTitleButton;
			}
		}
		
		
		private GameObject m_LoadSpriteGameObject;
		public GameObject LoadSpriteGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadSpriteGameObject == null)
				{
					m_LoadSpriteGameObject = Transform.Q("w_LoadSprite").gameObject;
				}
				return m_LoadSpriteGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_LoadSpriteRectTransform;
		public UnityEngine.RectTransform LoadSpriteRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadSpriteRectTransform == null)
				{
					m_LoadSpriteRectTransform = Transform.Q<UnityEngine.RectTransform>("w_LoadSprite");
				}
				return m_LoadSpriteRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_LoadSpriteCanvasRenderer;
		public UnityEngine.CanvasRenderer LoadSpriteCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadSpriteCanvasRenderer == null)
				{
					m_LoadSpriteCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_LoadSprite");
				}
				return m_LoadSpriteCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_LoadSpriteImage;
		public UnityEngine.UI.Image LoadSpriteImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadSpriteImage == null)
				{
					m_LoadSpriteImage = Transform.Q<UnityEngine.UI.Image>("w_LoadSprite");
				}
				return m_LoadSpriteImage;
			}
		}
		
		private UnityEngine.UI.Button m_LoadSpriteButton;
		public UnityEngine.UI.Button LoadSpriteButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_LoadSpriteButton == null)
				{
					m_LoadSpriteButton = Transform.Q<UnityEngine.UI.Button>("w_LoadSprite");
				}
				return m_LoadSpriteButton;
			}
		}
		
		
		private GameObject m_CircleKillerGameObject;
		public GameObject CircleKillerGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CircleKillerGameObject == null)
				{
					m_CircleKillerGameObject = Transform.Q("w_CircleKiller").gameObject;
				}
				return m_CircleKillerGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_CircleKillerRectTransform;
		public UnityEngine.RectTransform CircleKillerRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CircleKillerRectTransform == null)
				{
					m_CircleKillerRectTransform = Transform.Q<UnityEngine.RectTransform>("w_CircleKiller");
				}
				return m_CircleKillerRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_CircleKillerCanvasRenderer;
		public UnityEngine.CanvasRenderer CircleKillerCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CircleKillerCanvasRenderer == null)
				{
					m_CircleKillerCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_CircleKiller");
				}
				return m_CircleKillerCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_CircleKillerImage;
		public UnityEngine.UI.Image CircleKillerImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CircleKillerImage == null)
				{
					m_CircleKillerImage = Transform.Q<UnityEngine.UI.Image>("w_CircleKiller");
				}
				return m_CircleKillerImage;
			}
		}
		
		private UnityEngine.UI.Button m_CircleKillerButton;
		public UnityEngine.UI.Button CircleKillerButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_CircleKillerButton == null)
				{
					m_CircleKillerButton = Transform.Q<UnityEngine.UI.Button>("w_CircleKiller");
				}
				return m_CircleKillerButton;
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
