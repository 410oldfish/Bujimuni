using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public class OSASimpleGridViewComponent : XEntity, IAwake, IDestroy
    {
        public delegate void ItemCallback(OSASimpleGridView.MyItemViewsHolder vh, UIEntity uiEnitity);
        
        public OSASimpleGridView OSASimpleGridView { get; set; }

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
                this.ItemViewDict.Clear();
            }
            if (this.OSASimpleGridView != null)
            {
                this.OSASimpleGridView.onCreateItem.RemoveAllListeners();
                this.OSASimpleGridView.onRemoveItem.RemoveAllListeners();
                this.OSASimpleGridView.onUpdateItem.RemoveAllListeners();
                this.OSASimpleGridView = null;
            }
                
            this.OnCreateItem = null;
            this.OnRemoveItem = null;
            this.OnUpdateItem = null;
        }
        
        public void Init(OSASimpleGridView gridView)
        {
            this.OSASimpleGridView = gridView;
            this.ItemViewDict = new Dictionary<Transform, UIEntity>();
            gridView.onCreateItem.AddListener(this.CallOnCreateItem);
            gridView.onRemoveItem.AddListener(this.CallOnRemoveItem);
            gridView.onUpdateItem.AddListener(this.CallOnUpdateItem);
            gridView.Init();
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
            this.OSASimpleGridView.ResetItems(count);
        }
        public void Refresh()
        {
            this.OSASimpleGridView.ForceUpdateVisibleItems();
        }
        private void CallOnCreateItem(OSASimpleGridView.MyItemViewsHolder vh)
        {
            var itemView = this.GetItemView(vh.root);
            if (itemView == null)
            {
                itemView = UIHelper.CreateWidgetByNode<UIWidget>(this, vh.root.gameObject);
                this.ItemViewDict.Add(vh.root, itemView);
            }
            this.OnCreateItem?.Invoke(vh, itemView);
        }
        private void CallOnRemoveItem(OSASimpleGridView.MyItemViewsHolder vh)
        {
            var itemView = this.GetItemView(vh.root);
            if (itemView == null)
                return;
            this.OnRemoveItem?.Invoke(vh, itemView);
        }
        private void CallOnUpdateItem(OSASimpleGridView.MyItemViewsHolder vh)
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
