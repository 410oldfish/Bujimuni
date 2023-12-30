using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIDragHandler: MonoBehaviour,
        IInitializePotentialDragHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler
{
    public class UIDragEvent01: UnityEvent<Vector2>
    {
    }

    public class UIDragEvent02: UnityEvent<Vector2, Vector2>
    {
    }

    public UIDragEvent01 onBeginDrag = new UIDragEvent01();
    public UIDragEvent02 onDrag = new UIDragEvent02();
    public UIDragEvent01 onEndDrag = new UIDragEvent01();

    public UnityEvent<PointerEventData> onClick = new UnityEvent<PointerEventData>();
    
    public UnityEvent<bool> onPress = new UnityEvent<bool>();
    public UnityEvent<float> onLongPress = new UnityEvent<float>();

    [Flags]
    public enum EDragDirection
    {
        Free = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
    }

    public EDragDirection DragDirection;
    public float DragDirectionThreshold = 0.9f;
    public float LongPressThreshold = 1.0f;

    private bool m_IsSelfHandle = false;
    private bool m_IsDraging;

    private bool m_pressed;
    private float m_pressDownTime;

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        //继续往上抛出事件
        ExecuteEvents.ExecuteHierarchy(this.transform.parent.gameObject, eventData, ExecuteEvents.initializePotentialDrag);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.m_IsSelfHandle = this.IsSelfHandle(eventData);
        //Debug.Log($"OnBeginDrag self:{this.m_IsSelfHandle}");
        if (this.m_IsSelfHandle)
        {
            onBeginDrag?.Invoke(eventData.position);
        }
        else
        {
            //继续往上抛出事件
            ExecuteEvents.ExecuteHierarchy(this.transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
        }

        m_IsDraging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        if (this.m_IsSelfHandle)
        {
            onDrag?.Invoke(eventData.position, eventData.delta);
        }
        else
        {
            //继续往上抛出事件
            ExecuteEvents.ExecuteHierarchy(this.transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        if (this.m_IsSelfHandle)
        {
            onEndDrag?.Invoke(eventData.position);
        }
        else
        {
            //继续往上抛出事件
            ExecuteEvents.ExecuteHierarchy(this.transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
        }

        m_IsDraging = false;
    }

    private bool IsSelfHandle(PointerEventData eventData)
    {
        if (this.DragDirection == EDragDirection.Free)
        {
            return true;
        }

        if ((this.DragDirection & EDragDirection.Up) != 0 &&
            Vector2.Dot(eventData.delta.normalized, new Vector2(0, 1)) > this.DragDirectionThreshold)
        {
            return true;
        }

        if ((this.DragDirection & EDragDirection.Down) != 0 &&
            Vector2.Dot(eventData.delta.normalized, new Vector2(0, -1)) > this.DragDirectionThreshold)
        {
            return true;
        }

        if ((this.DragDirection & EDragDirection.Left) != 0 &&
            Vector2.Dot(eventData.delta.normalized, new Vector2(-1, 0)) > this.DragDirectionThreshold)
        {
            return true;
        }

        if ((this.DragDirection & EDragDirection.Right) != 0 &&
            Vector2.Dot(eventData.delta.normalized, new Vector2(1, 0)) > this.DragDirectionThreshold)
        {
            return true;
        }

        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_IsDraging)
        {
            return;
        }

        onClick?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.m_pressed = true;
        this.m_pressDownTime = Time.realtimeSinceStartup;
        this.onPress?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.m_pressed = false;
        this.onPress?.Invoke(false);
    }
    
    private void Update()
    {
        if (m_pressed)
        {
            var pressTime = Time.realtimeSinceStartup - this.m_pressDownTime;
            if (pressTime > LongPressThreshold)
            {
                this.onLongPress.Invoke(pressTime - LongPressThreshold);
            }
        }
    }
}