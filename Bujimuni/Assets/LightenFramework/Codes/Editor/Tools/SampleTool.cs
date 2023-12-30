using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace Lighten.Editor
{
    public static class SampleTool
    {
        // [MenuItem("LightenFramework/框 架 工 具/同步示例代码", priority = 999)]
        // static void SynchorizeSamples()
        // {
        //     var srcPath = "Assets/Samples";
        //     var dstPath = $"{LightenEditorConst.LIGHTEN_PACKAGE_PATH}/Samples~";
        //
        //     var filePaths = Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories);
        //     foreach (var filePath in filePaths)
        //     {
        //         if (filePath.EndsWith(".meta"))
        //             continue;
        //         var outputPath = filePath.Replace(srcPath, dstPath);
        //         if (File.Exists(outputPath))
        //             File.Delete(outputPath);
        //         FileUtil.CopyFileOrDirectory(filePath, outputPath);
        //     }
        //     EditorUtility.DisplayDialog("提示", "同步完成!记得上传!", "OK");
        // }

        // [MenuItem("LightenFramework/框 架 工 具/同步框架代码", priority = 999)]
        // static void SynchorizeSamples()
        // {
        //     var filePaths = Directory.GetFiles(LightenEditorConst.LIGHTEN_PACKAGE_PATH, "*", SearchOption.AllDirectories);
        //     foreach (var filePath in filePaths)
        //     {
        //         if (filePath.EndsWith(".meta"))
        //             continue;
        //         var outputPath = filePath.Replace(LightenEditorConst.LIGHTEN_PACKAGE_PATH, LightenEditorConst.LIGHTEN_PROJECT_PATH);
        //         if (File.Exists(outputPath))
        //             File.Delete(outputPath);
        //         FileUtil.CopyFileOrDirectory(filePath, outputPath);
        //     }
        // }
    }
}