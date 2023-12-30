/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class SamplePlayerView : XEntity, IAwake<Transform>, IDestroy
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
			m_SelfPlayerInput = null;
			m_SelfRigidbody2D = null;
			
			m_RenderGameObject = null;
			m_RenderTransform = null;
			m_RenderSpriteRenderer = null;
			m_RenderCircleCollider2D = null;
			
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
		
		private UnityEngine.InputSystem.PlayerInput m_SelfPlayerInput;
		public UnityEngine.InputSystem.PlayerInput SelfPlayerInput
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_SelfPlayerInput == null)
				{
					m_SelfPlayerInput = Transform.GetComponent<UnityEngine.InputSystem.PlayerInput>();
				}
				return m_SelfPlayerInput;
			}
		}
		
		private UnityEngine.Rigidbody2D m_SelfRigidbody2D;
		public UnityEngine.Rigidbody2D SelfRigidbody2D
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_SelfRigidbody2D == null)
				{
					m_SelfRigidbody2D = Transform.GetComponent<UnityEngine.Rigidbody2D>();
				}
				return m_SelfRigidbody2D;
			}
		}
		
		
		private GameObject m_RenderGameObject;
		public GameObject RenderGameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_RenderGameObject == null)
				{
					m_RenderGameObject = Transform.Q("w_Render").gameObject;
				}
				return m_RenderGameObject;
			}
		}
		
		private UnityEngine.Transform m_RenderTransform;
		public UnityEngine.Transform RenderTransform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_RenderTransform == null)
				{
					m_RenderTransform = Transform.Q<UnityEngine.Transform>("w_Render");
				}
				return m_RenderTransform;
			}
		}
		
		private UnityEngine.SpriteRenderer m_RenderSpriteRenderer;
		public UnityEngine.SpriteRenderer RenderSpriteRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_RenderSpriteRenderer == null)
				{
					m_RenderSpriteRenderer = Transform.Q<UnityEngine.SpriteRenderer>("w_Render");
				}
				return m_RenderSpriteRenderer;
			}
		}
		
		private UnityEngine.CircleCollider2D m_RenderCircleCollider2D;
		public UnityEngine.CircleCollider2D RenderCircleCollider2D
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_RenderCircleCollider2D == null)
				{
					m_RenderCircleCollider2D = Transform.Q<UnityEngine.CircleCollider2D>("w_Render");
				}
				return m_RenderCircleCollider2D;
			}
		}
		
		
		#endregion

}

public partial class SamplePlayerController : XEntityController
{
    public SamplePlayerView View { get; private set; }

    protected override void OnInitComponents()
    {
        this.View = this.Entity.AddComponent<SamplePlayerView, Transform>(transform);
    }
}