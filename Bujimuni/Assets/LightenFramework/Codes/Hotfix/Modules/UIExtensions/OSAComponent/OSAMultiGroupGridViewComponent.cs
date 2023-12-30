using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public class OSAMultiGroupGridViewComponent : XEntity, IAwake, IDestroy
    {
        public delegate void ItemCallback(OSAMultiGroupGridView.MyItemViewsHolder vh, UIEntity uiEnitity);
        public delegate void HeaderCallback(int GroupId, UIEntity uiEnitity);
        
        public OSAMultiGroupGridView OSAMultiGroupGridView { get; set; }

        public Dictionary<Transform, UIEntity> ItemViewDict = new Dictionary<Transform, UIEntity>();
        
        public ItemCallback OnCreateItem { get; set; }
        public ItemCallback OnRemoveItem { get; set; }
        public ItemCallback OnUpdateItem { get; set; }

        public HeaderCallback OnUpdateHeader { get; set; }
        
        public void Awake()
        {
            
        }

        public void Destroy()
        {
            if (this.ItemViewDict.Count > 0)
            {
                this.ItemViewDict.Clear();
            }

            if (this.OSAMultiGroupGridView != null)
            {
                this.OSAMultiGroupGridView.onCreateItem.RemoveAllListeners();
                this.OSAMultiGroupGridView.onRemoveItem.RemoveAllListeners();
                this.OSAMultiGroupGridView.onUpdateItem.RemoveAllListeners();
                this.OSAMultiGroupGridView = null;
            }

            this.OnCreateItem = null;
            this.OnRemoveItem = null;
            this.OnUpdateItem = null;
        }
        
        public void Init(OSAMultiGroupGridView gridView)
        {
            this.OSAMultiGroupGridView = gridView;
            this.ItemViewDict = new Dictionary<Transform, UIEntity>();
            gridView.onCreateItem.AddListener(this.CallOnCreateItem);
            gridView.onRemoveItem.AddListener(this.CallOnRemoveItem);
            gridView.onUpdateItem.AddListener(this.CallOnUpdateItem);
            gridView.onUpdateGroupHeader.AddListener(this.CallOnUpdateHeader);
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

        public void SetOnUpdateHeader(HeaderCallback OnUpdateHeader)
        {
            this.OnUpdateHeader = OnUpdateHeader;
        }

        public void ClearGroups()
        {
            this.OSAMultiGroupGridView.ClearGroups();
        }

        public void AddGroup(int groupId, int count)
        {
            this.OSAMultiGroupGridView.AddGroup(groupId, count);
        }

        public void Show()
        {
            this.OSAMultiGroupGridView.ResetGroupItems();
        }

        public void Refresh()
        {
            this.OSAMultiGroupGridView.ForceUpdateVisibleItems();
        }

        private void CallOnCreateItem(OSAMultiGroupGridView.MyItemViewsHolder vh)
        {
            var itemView = this.GetItemView(vh.root);
            if (itemView == null)
            {
                itemView = UIHelper.CreateWidgetByNode<UIWidget>(this, vh.root.gameObject);
                this.ItemViewDict.Add(vh.root, itemView);
            }

            this.OnCreateItem?.Invoke(vh, itemView);
        }

        private void CallOnRemoveItem(OSAMultiGroupGridView.MyItemViewsHolder vh)
        {
            var itemView = this.GetItemView(vh.root);
            if (itemView == null)
                return;
            this.OnRemoveItem?.Invoke(vh, itemView);
        }

        private void CallOnUpdateItem(OSAMultiGroupGridView.MyItemViewsHolder vh)
        {
            var itemView = this.GetItemView(vh.root);
            if (itemView == null)
                return;
            this.OnUpdateItem?.Invoke(vh, itemView);
        }

        private void CallOnUpdateHeader(int groupId, GameObject headerObj)
        {
            var trans = headerObj.transform;
            var itemView = this.GetItemView(trans);
            if (itemView == null)
            {
                itemView = UIHelper.CreateWidgetByNode<UIWidget>(this, headerObj);
                this.ItemViewDict.Add(trans, itemView);
            }

            this.OnUpdateHeader?.Invoke(groupId, itemView);
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
