/*
 * 该脚本为自动生成,请不要手动修改!!!!!!
 */

using UnityEngine;
using Lighten;

public partial class AutoBindSample2PrefabController : XEntityController
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
			
			m_A1GameObject = null;
			m_A1Transform = null;
			m_A1SpriteRenderer = null;
			
			m_B1GameObject = null;
			m_B1Transform = null;
			m_B1SpriteRenderer = null;
			
			m_inst01Controller = null;
			m_inst02Controller = null;
			#endregion


            this.Transform = null;
            this.Transform2D = null;
            this.GameObject = null;
        }

		#region 自动生成变量定义
		
		private GameObject m_A1GameObject;
		public GameObject A1GameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_A1GameObject == null)
				{
					m_A1GameObject = Transform.Q("w_A1").gameObject;
				}
				return m_A1GameObject;
			}
		}
		
		private UnityEngine.Transform m_A1Transform;
		public UnityEngine.Transform A1Transform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_A1Transform == null)
				{
					m_A1Transform = Transform.Q<UnityEngine.Transform>("w_A1");
				}
				return m_A1Transform;
			}
		}
		
		private UnityEngine.SpriteRenderer m_A1SpriteRenderer;
		public UnityEngine.SpriteRenderer A1SpriteRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_A1SpriteRenderer == null)
				{
					m_A1SpriteRenderer = Transform.Q<UnityEngine.SpriteRenderer>("w_A1");
				}
				return m_A1SpriteRenderer;
			}
		}
		
		
		private GameObject m_B1GameObject;
		public GameObject B1GameObject
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_B1GameObject == null)
				{
					m_B1GameObject = Transform.Q("w_B1").gameObject;
				}
				return m_B1GameObject;
			}
		}
		
		private UnityEngine.Transform m_B1Transform;
		public UnityEngine.Transform B1Transform
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_B1Transform == null)
				{
					m_B1Transform = Transform.Q<UnityEngine.Transform>("w_B1");
				}
				return m_B1Transform;
			}
		}
		
		private UnityEngine.SpriteRenderer m_B1SpriteRenderer;
		public UnityEngine.SpriteRenderer B1SpriteRenderer
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_B1SpriteRenderer == null)
				{
					m_B1SpriteRenderer = Transform.Q<UnityEngine.SpriteRenderer>("w_B1");
				}
				return m_B1SpriteRenderer;
			}
		}
		
		
		private SamplePrefab01Controller m_inst01Controller;
		public SamplePrefab01Controller inst01Controller
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_inst01Controller == null)
				{
					m_inst01Controller = Transform.Q<SamplePrefab01Controller>("w_inst01");
				}
				return m_inst01Controller;
			}
		}
		
		private SamplePrefab01Controller m_inst02Controller;
		public SamplePrefab01Controller inst02Controller
		{
			get
			{
				if (Transform == null)
				{
					Debug.LogError("Transform is null");
					return null;
				}
				if (m_inst02Controller == null)
				{
					m_inst02Controller = Transform.Q<SamplePrefab01Controller>("w_inst02");
				}
				return m_inst02Controller;
			}
		}
		
		#endregion

    }
    
    public ViewComponent View { get; private set; }

    protected override void OnInitComponents()
    {
        this.View = this.Entity.AddComponent<ViewComponent, Transform>(transform);
    }
    
}
