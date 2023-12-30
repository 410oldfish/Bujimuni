using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Lighten
{
    public static class UIHelper
    {
        #region UI按钮事件

        private static bool IsClicked = false;
        public static void AddListenerAsync(this Button self, Func<UniTask> action)
        {
            async UniTask ClickActionAsync()
            {
                IsClicked = true;
                await action();
                IsClicked = false;
            }
            
            self.onClick.RemoveAllListeners();
            self.onClick.AddListener(() =>
            {
                if (IsClicked)
                {
                    return;
                }
                ClickActionAsync().Forget();
            });
            BindUIClickSound(self.gameObject);
        }

        public static void AddListener(this Toggle toggle, UnityAction<bool> selectEventHandler)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(selectEventHandler);
            BindUIClickSound(toggle.gameObject);
        }

        public static void AddListener(this Button button, UnityAction clickEventHandler)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(clickEventHandler);
            BindUIClickSound(button.gameObject);
        }

        public static void AddListenerWithId(this Button button, Action<int> clickEventHandler, int id)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { clickEventHandler(id); });
            BindUIClickSound(button.gameObject);
        }

        public static void AddListenerWithId(this Button button, Action<long> clickEventHandler, long id)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { clickEventHandler(id); });
            BindUIClickSound(button.gameObject);
        }

        public static void AddListenerWithParam<T>(this Button button, Action<T> clickEventHandler, T param)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { clickEventHandler(param); });
            BindUIClickSound(button.gameObject);
        }

        public static void AddListenerWithParam<T, A>(this Button button, Action<T, A> clickEventHandler, T param1, A param2)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { clickEventHandler(param1, param2); });
            BindUIClickSound(button.gameObject);
        }

        public static void AddListener(this ToggleGroup toggleGroup, UnityAction<int> selectEventHandler)
        {
            var togglesList = toggleGroup.GetComponentsInChildren<Toggle>();
            for (int i = 0; i < togglesList.Length; i++)
            {
                int index = i;
                togglesList[i].AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        selectEventHandler(index);
                    }
                });
                BindUIClickSound(toggleGroup.gameObject);
            }
        }
        
        private static void BindUIClickSound(GameObject root)
        {
            // var audioName = "UI_Click";
            // var tuiClickSound = root.SafeAddComponent<TUIClickSound>();
            // if (!string.IsNullOrEmpty(tuiClickSound.audioName))
            // {
            //     audioName = tuiClickSound.audioName;
            // }
            // tuiClickSound.onPointerDown.RemoveAllListeners();
            // tuiClickSound.onPointerDown.AddListener(_ =>
            // {
            //     AudioMgr.Instance.PlayUISound(audioName);
            // });
        }

        #endregion

        #region 动作监听事件

        // public static void AddListener(this AnimationEventListener self, Action<string> action)
        // {
        //     self.onTrigger.RemoveAllListeners();
        //     self.onTrigger.AddListener((eventName) =>
        //     {
        //         action?.Invoke(eventName);
        //     });
        // }
        
        #endregion

        public static string GetPrefixName(string name)
        {
            if (name.Length <= 3)
            {
                return string.Empty;
            }
            return name.Substring(0, 3);
        }
        
        public static EUIEntityType GetEntityType(string name)
        {
            if (name.Length <= 3)
            {
                return EUIEntityType.None;
            }
            var typeName = name.Substring(0, 3);
            switch (typeName)
            {
                case UIPrefix.Window:
                    return EUIEntityType.Window;
                case UIPrefix.Widget:
                    return EUIEntityType.Widget;
            }
            return EUIEntityType.None;
        }
        
        #region 获取UI对象名称

        public static string GetWindowName(string name)
        {
            return name.Replace(UIPrefix.Window, string.Empty);
        }
        public static string GetWidgetName(string name)
        {
            return name.Replace(UIPrefix.Widget, string.Empty);
        }
        
        #endregion
        
        //创建组件
        public static T CreateWidgetByNode<T>(XEntity parent, GameObject gameObject) where T : UIWidget
        {
            var widgetMono = gameObject.GetComponent<UIWidgetMono>();
            if (widgetMono == null)
            {
                Debug.LogError($"没有找到UIWidgetMono {gameObject.name}");
                return null;
            }
            var widgetType = Game.Architecture.GetManager<IAssemblyMgr>().GetType(widgetMono.WidgetName);
            if (widgetType == null)
            {
                Debug.LogError($"没有找到组件类型 {widgetMono.WidgetName}");
                return null;
            }
            var uiWidget = parent.AddChild(widgetType) as T;
            uiWidget.InitWidget(widgetMono.InitalState, widgetMono.DefaultParams);
            uiWidget.Bind(gameObject);
            return uiWidget;
        }
    }
}