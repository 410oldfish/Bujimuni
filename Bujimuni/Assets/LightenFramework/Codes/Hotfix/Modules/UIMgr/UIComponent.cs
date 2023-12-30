using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighten
{
    public class UIComponent : XEntity, IDestroy
    {
        private Dictionary<long, bool> m_windowInstanceIds = new Dictionary<long, bool>();
        
        public void Destroy()
        {
            if (this.m_windowInstanceIds.Count > 0)
            {
                var uiMgr = Game.Architecture.GetManager<IUIMgr>(false);
                if (uiMgr != null)
                {
                    foreach (var instanceId in this.m_windowInstanceIds.Keys)
                    {
                        uiMgr.CloseWindowInstance(instanceId);
                    }
                }
                m_windowInstanceIds.Clear();
            }
        }
        
        public async UniTask OpenWindowAsync<TWindow>(IUIShowPara showPara = null,
            ShowWindowOption showOption = default,
            CancellationToken cancellationToken = default) where TWindow : UIWindow
        {
            var instanceId = await Game.Architecture.GetManager<IUIMgr>()
                .OpenWindowAsync<TWindow>(showPara, showOption, cancellationToken);
            m_windowInstanceIds[instanceId] = true;
        }
        
        public async UniTask OpenWindowInstanceAsync<TWindow>(IUIShowPara showPara = null,
            ShowWindowOption showOption = default,
            CancellationToken cancellationToken = default) where TWindow : UIWindow
        {
            var instanceId = await Game.Architecture.GetManager<IUIMgr>()
                .OpenWindowInstanceAsync<TWindow>(showPara, showOption, cancellationToken);
            m_windowInstanceIds[instanceId] = true;
        }

        public void CloseWindow<TWindow>() where TWindow : UIWindow
        {
            var uiMgr = Game.Architecture.GetManager<IUIMgr>();
            var instanceId = uiMgr.GetWindowInstanceId<TWindow>();
            CloseWindow(instanceId);
        }
        
        public void CloseWindow(long instanceId)
        {
            if (!m_windowInstanceIds.ContainsKey(instanceId))
                return;
            Game.Architecture.GetManager<IUIMgr>().CloseWindowInstance(instanceId);
            m_windowInstanceIds.Remove(instanceId);
        }
        
    }
}
