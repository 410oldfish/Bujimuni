/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class DlgGameLogin : UIWindow<DlgGameLogin.CPara>
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
			
			m_EnterGameGameObject = null;
			m_EnterGameRectTransform = null;
			m_EnterGameCanvasRenderer = null;
			m_EnterGameImage = null;
			m_EnterGameButton = null;
			
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
		
		
		private GameObject m_EnterGameGameObject;
		public GameObject EnterGameGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_EnterGameGameObject == null)
				{
					m_EnterGameGameObject = Transform.Q("w_EnterGame").gameObject;
				}
				return m_EnterGameGameObject;
			}
		}
		
		private UnityEngine.RectTransform m_EnterGameRectTransform;
		public UnityEngine.RectTransform EnterGameRectTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_EnterGameRectTransform == null)
				{
					m_EnterGameRectTransform = Transform.Q<UnityEngine.RectTransform>("w_EnterGame");
				}
				return m_EnterGameRectTransform;
			}
		}
		
		private UnityEngine.CanvasRenderer m_EnterGameCanvasRenderer;
		public UnityEngine.CanvasRenderer EnterGameCanvasRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_EnterGameCanvasRenderer == null)
				{
					m_EnterGameCanvasRenderer = Transform.Q<UnityEngine.CanvasRenderer>("w_EnterGame");
				}
				return m_EnterGameCanvasRenderer;
			}
		}
		
		private UnityEngine.UI.Image m_EnterGameImage;
		public UnityEngine.UI.Image EnterGameImage
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_EnterGameImage == null)
				{
					m_EnterGameImage = Transform.Q<UnityEngine.UI.Image>("w_EnterGame");
				}
				return m_EnterGameImage;
			}
		}
		
		private UnityEngine.UI.Button m_EnterGameButton;
		public UnityEngine.UI.Button EnterGameButton
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_EnterGameButton == null)
				{
					m_EnterGameButton = Transform.Q<UnityEngine.UI.Button>("w_EnterGame");
				}
				return m_EnterGameButton;
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
