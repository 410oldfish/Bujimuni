/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class DlgSampleOfCircleKiller : UIWindow<DlgSampleOfCircleKiller.CPara>
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
			
			m_ScoreGameObject = null;
			m_ScoreRectTransform = null;
			m_ScoreCanvasRenderer = null;
			m_ScoreTextMeshProUGUI = null;
			
			m_BackGameObject = null;
			m_BackRectTransform = null;
			m_BackCanvasRenderer = null;
			m_BackImage = null;
			m_BackButton = null;
			
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
		
		
		private GameObject m_ScoreGameObject;
		public GameObject ScoreGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScoreGameObject == null)
				{
					m_ScoreGameObject = Transform.Q("w_Score").gameObject;
				}
				return m_ScoreGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_ScoreRectTransform;
		public UnityEngine.RectTransform ScoreRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScoreRectTransform == null)
				{
					m_ScoreRectTransform = Transform.Q<UnityEngine.RectTransform>("w_Score");
				}
				return m_ScoreRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_ScoreCanvasRenderer;
		public UnityEngine.CanvasRenderer ScoreCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScoreCanvasRenderer == null)
				{
					m_ScoreCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_Score");
				}
				return m_ScoreCanvasRenderer;
			}
		}
		
		private TMPro.TextMeshProUGUI m_ScoreTextMeshProUGUI;
		public TMPro.TextMeshProUGUI ScoreTextMeshProUGUI
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_ScoreTextMeshProUGUI == null)
				{
					m_ScoreTextMeshProUGUI = Transform.Q<TMPro.TextMeshProUGUI>("w_Score");
				}
				return m_ScoreTextMeshProUGUI;
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
		
		
		#endregion

    }
    
    public ViewComponent View { get; private set; }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        this.View = this.AddComponent<ViewComponent, Transform>(this.GameObject.transform);
    }
}
