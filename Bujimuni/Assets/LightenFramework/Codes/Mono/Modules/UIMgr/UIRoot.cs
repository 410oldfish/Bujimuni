using UnityEngine;

namespace Lighten
{
    public class UIRoot : MonoBehaviour
    {
        public static UIRoot Instance = null;
        
        public Camera UICamera;
        public Canvas UICanvas;
        public Canvas UICanvasOfMask;
        public Canvas UICanvasOfBlank;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
    }
}
