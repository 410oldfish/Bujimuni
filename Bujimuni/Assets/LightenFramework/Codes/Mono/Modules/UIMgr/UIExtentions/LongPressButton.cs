using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Lighten
{
    [AddComponentMenu("UI/LongPressButton", 30)]
    public class LongPressButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        #region 和Button一样的代码
        
        [Serializable]
        public class ButtonClickedEvent : UnityEvent {}
        
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        protected LongPressButton()
        {}
        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            //TODO:这里做了修改,如果已经触发过长按了,那么就不触发click了 add by ggyy 2023.5.5
            if (m_longPressTriggered)
                return;
            Press();
        }
        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();

            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
        
        #endregion
        
        #region 长按开始事件
        [Serializable]
        public class ButtonLongPressTriggerBeginEvent : UnityEvent
        {
        }

        [FormerlySerializedAs("onLongPress")] [SerializeField]
        private ButtonLongPressTriggerBeginEvent m_onLongPressBegin = new ButtonLongPressTriggerBeginEvent();

        public ButtonLongPressTriggerBeginEvent onLongPressBegin
        {
            get { return m_onLongPressBegin; }
            set { m_onLongPressBegin = value; }
        }

        #endregion
        
        #region 长按事件
        [Serializable]
        public class ButtonLongPressTriggerEvent : UnityEvent<float>
        {
        }

        [FormerlySerializedAs("onLongPress")] [SerializeField]
        private ButtonLongPressTriggerEvent m_onLongPress = new ButtonLongPressTriggerEvent();

        public ButtonLongPressTriggerEvent onLongPress
        {
            get { return m_onLongPress; }
            set { m_onLongPress = value; }
        }
        #endregion
        
        #region 长按结束事件
        [Serializable]
        public class ButtonLongPressTriggerEndEvent : UnityEvent
        {
        }

        [FormerlySerializedAs("onLongPress")] [SerializeField]
        private ButtonLongPressTriggerEndEvent m_onLongPressEnd = new ButtonLongPressTriggerEndEvent();

        public ButtonLongPressTriggerEndEvent onLongPressEnd
        {
            get { return m_onLongPressEnd; }
            set { m_onLongPressEnd = value; }
        }
        #endregion
        
        public float longPressTriggerTime = 0.5f;
        public float longPressIntervalTime = 0.1f;

        private bool m_pressed;
        private bool m_longPressTriggered;
        private float m_longPressTime;
        private float m_lastTime;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            m_pressed = true;
            m_longPressTriggered = false;
            m_lastTime = Time.realtimeSinceStartup;
            //Debug.Log("press down");
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            m_pressed = false;
            if (m_longPressTriggered)
            {
                m_onLongPressEnd.Invoke();
            }
            //Debug.Log("press up");
        }
        
        private void Update()
        {
            if (!m_pressed)
                return;
            var currentTime = Time.unscaledTime;
            if (!m_longPressTriggered)
            {
                if (currentTime - m_lastTime >= longPressTriggerTime)
                {
                    m_longPressTriggered = true;
                    m_lastTime = currentTime;
                    m_longPressTime = 0;
                    m_onLongPressBegin.Invoke();
                    m_onLongPress.Invoke(m_longPressTime);
                }
            }
            if (m_longPressTriggered)
            {
                float passedTime = currentTime - m_lastTime;
                if (passedTime >= longPressIntervalTime)
                {
                    m_lastTime = currentTime;
                    m_longPressTime += passedTime;
                    m_onLongPress.Invoke(m_longPressTime);
                }
            }
        }
    }
}