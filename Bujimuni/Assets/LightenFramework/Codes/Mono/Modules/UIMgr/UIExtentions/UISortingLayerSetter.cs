using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Lighten
{
    public class UISortingLayerSetter: MonoBehaviour
    {
        [OnValueChanged("OnLocalSortingOrderChanged")]
        [LabelText("SortingOrder偏移")]
        public int localSortingOrder = 0;
#if UNITY_EDITOR

        private Canvas m_cacheCanvas;
                
        void OnLocalSortingOrderChanged()
        {
            if (m_cacheCanvas == null)
            {
                m_cacheCanvas = FindRootCanvas(this.transform);
            }
            UpdateSortingOrder(m_cacheCanvas.sortingLayerID, m_cacheCanvas.sortingOrder);
        }

        private Canvas FindRootCanvas(Transform node)
        {
            while (node != null)
            {
                if (node.name.StartsWith("Dlg"))
                {
                    var canvas = node.GetComponent<Canvas>();
                    if (canvas != null)
                    {
                        return canvas;
                    }
                }
                node = node.parent;
            }
            return null;
        }
#endif
        
        
        private enum EComponentType
        {
            None = 0,
            Canvas,
            SortingGroup,
            SpriteRender,
            ParticleSystemRenderer,
        }

        private bool m_initialized;
        private EComponentType m_componentType;
        private SortingGroup m_sortingGroup;
        private Canvas m_canvas;
        private SpriteRenderer m_spriteRenderer;
        private ParticleSystemRenderer m_particleSystemRenderer;

        public void Initialize()
        {
            if (m_initialized)
                return;
            this.m_canvas = this.GetComponent<Canvas>();
            if (this.m_canvas != null)
            {
                m_componentType = EComponentType.Canvas;
                return;
            }

            this.m_sortingGroup = this.GetComponent<SortingGroup>();
            if (this.m_sortingGroup != null)
            {
                this.m_componentType = EComponentType.SortingGroup;
                return;
            }

            m_spriteRenderer = this.GetComponent<SpriteRenderer>();
            if (this.m_spriteRenderer != null)
            {
                m_componentType = EComponentType.SpriteRender;
                return;
            }

            this.m_particleSystemRenderer = this.GetComponent<ParticleSystemRenderer>();
            if (this.m_particleSystemRenderer != null)
            {
                this.m_componentType = EComponentType.ParticleSystemRenderer;
                return;
            }

            m_componentType = EComponentType.None;
        }

        public void UpdateSortingOrder(int sortingLayerId, int sortingOrder)
        {
            if (!m_initialized)
            {
                this.Initialize();
            }

            switch (m_componentType)
            {
                case EComponentType.Canvas:
                    m_canvas.overrideSorting = true;
                    m_canvas.sortingLayerID = sortingLayerId;
                    m_canvas.sortingOrder = sortingOrder + localSortingOrder;
                    break;
                case EComponentType.SortingGroup:
                    m_sortingGroup.sortingLayerID = sortingLayerId;
                    m_sortingGroup.sortingOrder = sortingOrder + localSortingOrder;
                    break;
                case EComponentType.SpriteRender:
                    m_spriteRenderer.sortingLayerID = sortingLayerId;
                    m_spriteRenderer.sortingOrder = sortingOrder + localSortingOrder;
                    break;
                case EComponentType.ParticleSystemRenderer:
                    m_particleSystemRenderer.sortingLayerID = sortingLayerId;
                    m_particleSystemRenderer.sortingOrder = sortingOrder + localSortingOrder;
                    break;
            }
        }
    }
}