using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public class OSASimpleScrollViewComponent : XEntity, IAwake, IDestroy
    {
        public delegate void ItemCallback(OSASimpleScrollView.MyItemViewsHolder vh, UIEntity uiEnitity);

        public OSASimpleScrollView OSASimpleScrollView { get; set; }

        
        public Dictionary<Transform, UIEntity> ItemViewDict = new Dictionary<Transform, UIEntity>();
        
        public ItemCallback OnCreateItem { get; set; }
        public ItemCallback OnRemoveItem { get; set; }
        public ItemCallback OnUpdateItem { get; set; }
        
        public void Awake()
        {
        }

        public void Destroy()
        {
            if (this.ItemViewDict.Count > 0)
            {
                foreach (var itemView in this.ItemViewDict)
                {
                    itemView.Value.Dispose();
                }
                this.ItemViewDict.Clear();
            }
            if (this.OSASimpleScrollView != null)
            {
                this.OSASimpleScrollView.onCreateItem.RemoveAllListeners();
                this.OSASimpleScrollView.onRemoveItem.RemoveAllListeners();
                this.OSASimpleScrollView.onUpdateItem.RemoveAllListeners();
                this.OSASimpleScrollView = null;
            }
            this.OnCreateItem = null;
            this.OnRemoveItem = null;
            this.OnUpdateItem = null;
        }
        
        public void Init(OSASimpleScrollView scrollView)
        {
            this.OSASimpleScrollView = scrollView;
            this.ItemViewDict = new Dictionary<Transform, UIEntity>();
            scrollView.onCreateItem.AddListener(this.CallOnCreateItem);
            scrollView.onRemoveItem.AddListener(this.CallOnRemoveItem);
            scrollView.onUpdateItem.AddListener(this.CallOnUpdateItem);
            scrollView.Init();
        }
        public void SetOnCreate(ItemCallback OnCreateItem)
        {
            this.OnCreateItem = OnCreateItem;
        }
        public void SetOnRemove(ItemCallback OnRemoveItem)
        {
            this.OnRemoveItem = OnRemoveItem;
        }
        public void SetOnUpdate(ItemCallback OnUpdateItem)
        {
            this.OnUpdateItem = OnUpdateItem;
        }
        
        public void Show(int count)
        {
            this.OSASimpleScrollView.ResetItems(count);
        }

        public void Refresh()
        {
            this.OSASimpleScrollView.ForceUpdateVisibleItems();
        }
        private void CallOnCreateItem(OSASimpleScrollView.MyItemViewsHolder vh)
        {
            var itemView = this.GetItemView(vh.root);
            if (itemView == null)
            {
                itemView = UIHelper.CreateWidgetByNode<UIWidget>(this, vh.root.gameObject);
                this.ItemViewDict.Add(vh.root, itemView);
            }
            this.OnCreateItem?.Invoke(vh, itemView);
        }
        private void CallOnRemoveItem(OSASimpleScrollView.MyItemViewsHolder vh)
        {
            var itemView = this.GetItemView(vh.root);
            if (itemView == null)
                return;
            this.OnRemoveItem?.Invoke(vh, itemView);
        }
        private void CallOnUpdateItem(OSASimpleScrollView.MyItemViewsHolder vh)
        {
            var itemView = this.GetItemView(vh.root);
            if (itemView == null)
                return;
            this.OnUpdateItem?.Invoke(vh, itemView);
        }

        private UIEntity GetItemView(Transform root)
        {
            if (this.ItemViewDict.TryGetValue(root, out var itemView))
            {
                return itemView;
            }
            return null;
        }
    }
}
