using System.IO;

namespace Lighten
{
    public static class DirectoryExtension
    {
        //移除空文件夹
        public static void DeleteEmptyDirectory(string directory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            DeleteEmptyDirectoryRcursion(directoryInfo);
        }

        //递归移除空文件夹
        private static void DeleteEmptyDirectoryRcursion(DirectoryInfo directoryInfo)
        {
            var children = directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (var child in children)
            {
                DeleteEmptyDirectoryRcursion(child);
            }
            //子节点检查完成后,再检测一遍自己是否空了
            var filePaths = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            if (filePaths.Length < 1)
            {
                directoryInfo.Delete();
            }
        }
        
        //检查可靠路径
        public static void GenerateDirectory(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (Directory.Exists(dir))
                return;
            Directory.CreateDirectory(dir);
        }
        
        //获取相同路径
        public static string GetSameParentDirectory(string path1, string path2, string symbol = "\\")
        {
            var result = string.Empty;
            var index1 = 0;
            var index2 = 0;
            var str1 = string.Empty;
            var str2 = string.Empty;
            for (int i = 0; i < 100; ++i)
            {
                index1 = path1.IndexOf(symbol);
                index2 = path2.IndexOf(symbol);
                str1 = index1 == -1 ? path1 : path1.Substring(0, index1);
                str2 = index2 == -1 ? path2 : path2.Substring(0, index2);
                if (str1 != str2)
                    break;
                if (string.IsNullOrEmpty(result))
                {
                    result += str1;
                }
                else
                {
                    result += symbol + str1;
                }
                if (index1 == -1 || index2 == -1)
                    break;
                path1 = path1.Substring(index1 + 1);
                path2 = path2.Substring(index2 + 1);
            }
            return result;
        }
    }
}
