using UnityEngine;

namespace Lighten
{
    public static class TransformExtension
    {
        //获取自身到子节点的路径
        public static string GetChildPath(this Transform transform, Transform child)
        {
            if (transform == child)
                return string.Empty;
            Transform node = child;
            var relativePath = string.Empty;
            while (node != child.root)
            {
                if (string.IsNullOrEmpty(relativePath))
                {
                    relativePath = node.name;
                }
                else
                {
                    relativePath = $"{node.name}/{relativePath}";
                }
                var parent = node.parent;
                if (parent == transform)
                {
                    return relativePath;
                }
                node = parent;
            }
            return string.Empty;
        }
        
        //根据名字查找子节点
        public static Transform Q(this Transform transform, string childName, bool isRoot = true)
        {
            if (transform == null)
            {
                return null;
            }

            var findedNode = transform.Find(childName);
            if (findedNode != null)
            {
                return findedNode;
            }

            foreach (Transform child in transform)
            {
                findedNode = child.Q(childName, false);
                if (findedNode != null)
                {
                    return findedNode;
                }
            }

            if (isRoot)
            {
                Debug.LogError($"node:{transform.name} not found child:{childName}");
            }

            return null;
        }
        
        //查找子节点(泛型)
        public static T Q<T>(this Transform transform, string childName) where T : Component
        {
            var findedNode = transform.Q(childName);
            if (findedNode == null)
            {
                return null;
            }
            return findedNode.GetComponent<T>();
        }
    }
}
