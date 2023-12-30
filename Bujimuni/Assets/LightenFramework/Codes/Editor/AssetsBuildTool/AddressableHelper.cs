// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using UnityEditor;
// using UnityEditor.AddressableAssets;
// using UnityEditor.AddressableAssets.Build;
// using UnityEditor.AddressableAssets.Settings;
// using UnityEditor.AddressableAssets.Settings.GroupSchemas;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
// using Object = UnityEngine.Object;
//
// namespace Lighten.Editor
// {
//     public static class AddressableHelper
//     {
//         /*
//          * 1.根据AddressConfig统计资源
//          * 2.创建更新组
//          * 3.创建更新组资源
//          * 4.移除过期组资源
//          * 5.移除过期组
//          */
//
//         static List<string> m_retainGroupName = new List<string>()
//         {
//             "Default Local Group"
//         };
//
//         /// <summary>
//         /// 根据AddressConfig更新AddressableGroups
//         /// </summary>
//         /// <param name="addressableSetupSchema"></param>
//         // public static bool GenerateAddressableGroups(AddressableSetupSchema addressableSetupSchema)
//         // {
//         //     var success = true;
//         //     AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
//         //     try
//         //     {
//         //         var groupNames = new List<string>();
//         //         foreach (var addressData in addressableSetupSchema.addressDatas)
//         //         {
//         //             UpdateAddressableGroup(settings, addressData);
//         //             if (!groupNames.Contains(addressData.groupName))
//         //             {
//         //                 groupNames.Add(addressData.groupName);
//         //             }
//         //         }
//         //         //统计过期组
//         //         var removeGroupList = new List<AddressableAssetGroup>();
//         //         foreach (var group in settings.groups)
//         //         {
//         //             if (!groupNames.Contains(group.Name) && !m_retainGroupName.Contains(group.Name))
//         //             {
//         //                 removeGroupList.Add(group);
//         //             }
//         //         }
//         //         //移除过期组
//         //         if (removeGroupList.Count > 0)
//         //         {
//         //             foreach (var group in removeGroupList)
//         //             {
//         //                 settings.RemoveGroup(group);
//         //             }
//         //             removeGroupList.Clear();
//         //         }
//         //     }
//         //     catch (Exception e)
//         //     {
//         //         Debug.LogError(e.StackTrace);
//         //         success = false;
//         //     }
//         //     EditorUtility.ClearProgressBar();
//         //     return success;
//         // }
//
//         /// <summary>
//         /// 编译资源
//         /// </summary>
//         /// <returns></returns>
//         public static bool BuildAddressable()
//         {
//             var addressableAssetSettings = AddressableAssetSettingsDefaultObject.Settings;
//             //Debug.Log(settings.RemoteCatalogBuildPath.GetValue(settings));
//             //Debug.Log(settings.RemoteCatalogLoadPath.GetValue(settings));
//             var remoteBuildPath = addressableAssetSettings.RemoteCatalogBuildPath.GetValue(addressableAssetSettings);
//             var contentStateDataPath =
//                 $"Assets/AddressableAssetsData/{GetContentBinDirectoryName()}/addressables_content_state.bin";
//             AddressablesPlayerBuildResult result;
//             var cacheData = ContentUpdateScript.LoadContentState(contentStateDataPath);
//             if (cacheData != null)
//             {
//                 result = ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings,
//                     contentStateDataPath);
//                 var filePaths = result.FileRegistry.GetFilePaths();
//                 var retainFiles = new List<string>();
//                 foreach (var filePath in filePaths)
//                 {
//                     retainFiles.Add(filePath.Replace("\\", "/"));
//                     Debug.Log($"编译输出文件 {filePath}");
//                 }
//
//                 //result.OutputPath
//                 RemoveExpiredFiles(remoteBuildPath, retainFiles);
//             }
//             else
//             {
//                 AddressableAssetSettings.BuildPlayerContent(out result);
//             }
//
//             //TODO:result.OutputPath这个输出的是setting.json的路径
//             //清理编译路径的空文件夹
//             DirectoryExtension.DeleteEmptyDirectory(remoteBuildPath);
//
//             if (!string.IsNullOrEmpty(result.Error))
//             {
//                 Debug.LogError(result.Error);
//                 return false;
//             }
//
//             Debug.Log($"编译完成,耗时{result.Duration}s");
//             return true;
//         }
//
//         //创建文件清单+拷贝到StreamingAssets目录
//         public static bool ProcessForInPack()
//         {
//             var addressableAssetSettings = AddressableAssetSettingsDefaultObject.Settings;
//             var remoteBuildPath = addressableAssetSettings.RemoteCatalogBuildPath.GetValue(addressableAssetSettings);
//             //创建资源清单文件
//             if (!GenerateFileList(remoteBuildPath, $"{remoteBuildPath}/filelist.txt"))
//             {
//                 Debug.LogError($"清单文件创建失败 {remoteBuildPath}");
//                 return false;
//             }
//
//             //把编译出的文件拷贝到StreamingAssets目录
//             var streamingAssetsPath = $"{Application.streamingAssetsPath}/aa/inpack";
//             if (Directory.Exists(streamingAssetsPath))
//             {
//                 Directory.Delete(streamingAssetsPath, true);
//             }
//
//             DirectoryExtension.GenerateDirectory(streamingAssetsPath);
//
//             FileUtil.CopyFileOrDirectory(remoteBuildPath, streamingAssetsPath);
//             AssetDatabase.Refresh();
//             return true;
//         }
//
//         static void RemoveExpiredFiles(string root, IEnumerable<string> retainFiles)
//         {
//             var filePaths = Directory.GetFiles(root, "*", SearchOption.AllDirectories);
//             foreach (var filePath in filePaths)
//             {
//                 var finalFilePath = filePath.Replace("\\", "/");
//                 if (retainFiles.Contains(finalFilePath))
//                     continue;
//                 if (finalFilePath.EndsWith("link.xml"))
//                     continue;
//                 File.Delete(filePath);
//                 Debug.Log($"移除旧文件 {finalFilePath}");
//             }
//         }
//
//         //创建folder目录下的文件清单
//         static bool GenerateFileList(string folder, string outputFilePath)
//         {
//             var lines = new List<string>();
//             var filePaths = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
//             foreach (var filePath in filePaths)
//             {
//                 if (filePath.EndsWith(".meta"))
//                     continue;
//                 var data = filePath.Replace("\\", "/").Replace(folder + "/", string.Empty);
//                 lines.Add(data);
//             }
//
//             try
//             {
//                 File.WriteAllLines(outputFilePath, lines);
//             }
//             catch (Exception e)
//             {
//                 Debug.LogError(e.ToString());
//                 return false;
//             }
//
//             return true;
//         }
//
//         class AddressableEntryData
//         {
//             public string assetPath;
//             public string label;
//             public string address;
//         }
//
//         class AddressableGroupData
//         {
//             public BundledAssetGroupSchema.BundlePackingMode bundleMode;
//             public Dictionary<string, AddressableEntryData> entryDatas = new Dictionary<string, AddressableEntryData>();
//         }
//
//         // private static AddressableGroupData ConvertToAddressableGroupData(AddressableSetupSchema.AddressData addressData)
//         // {
//         //     var result = new AddressableGroupData();
//         //     switch (addressData.searchMode)
//         //     {
//         //         case AddressableSetupSchema.EnumSearchMode.File:
//         //             foreach (var folderData in addressData.folderDatas)
//         //             {
//         //                 var folderPath = AssetDatabase.GetAssetPath(folderData.folderObj);
//         //                 if (string.IsNullOrEmpty(folderPath) || !AssetDatabase.IsValidFolder(folderPath))
//         //                     continue;
//         //                 var filePaths = Directory.GetFiles(folderPath, addressData.searchPattern,
//         //                     folderData.searchOption);
//         //                 foreach (var assetPath in filePaths)
//         //                 {
//         //                     if (assetPath.EndsWith(".meta"))
//         //                         continue;
//         //                     var guid = AssetDatabase.AssetPathToGUID(assetPath);
//         //                     if (string.IsNullOrEmpty(guid))
//         //                     {
//         //                         Debug.LogError($"{assetPath} 没有guid");
//         //                         continue;
//         //                     }
//         //                     if (result.entryDatas.ContainsKey(guid))
//         //                         continue;
//         //                     var finalAssetPath = assetPath.Replace("\\", "/");
//         //                     if (IsIgnoreAsset(finalAssetPath))
//         //                         continue;
//         //                     var entryData = new AddressableEntryData();
//         //                     entryData.assetPath = finalAssetPath;
//         //                     if (addressData.addressable)
//         //                     {
//         //                         entryData.address = Path.GetFileNameWithoutExtension(finalAssetPath).ToLower();
//         //                     }
//         //                     else
//         //                     {
//         //                         entryData.address = finalAssetPath.Replace(folderPath + "/", string.Empty);
//         //                     }
//         //                     Debug.Log(guid + " " + finalAssetPath);
//         //                     result.entryDatas.Add(guid, entryData);
//         //                 }
//         //             }
//         //             break;
//         //         case AddressableSetupSchema.EnumSearchMode.Folder:
//         //             foreach (var folderData in addressData.folderDatas)
//         //             {
//         //                 var folderPath = AssetDatabase.GetAssetPath(folderData.folderObj);
//         //                 if (string.IsNullOrEmpty(folderPath) || !AssetDatabase.IsValidFolder(folderPath))
//         //                     continue;
//         //                 var directoryPaths = Directory.GetDirectories(folderPath, addressData.searchPattern,
//         //                     folderData.searchOption);
//         //                 foreach (var assetPath in directoryPaths)
//         //                 {
//         //                     var guid = AssetDatabase.AssetPathToGUID(assetPath);
//         //                     if (string.IsNullOrEmpty(guid))
//         //                     {
//         //                         Debug.LogError($"{assetPath} 没有guid");
//         //                         continue;
//         //                     }
//         //                     var finalAssetPath = assetPath.Replace("\\", "/");
//         //                     if (IsIgnoreAsset(finalAssetPath))
//         //                         continue;
//         //                     var entryData = new AddressableEntryData();
//         //                     entryData.assetPath = finalAssetPath;
//         //                     if (addressData.addressable)
//         //                     {
//         //                         entryData.address = Path.GetFileNameWithoutExtension(finalAssetPath).ToLower();
//         //                     }
//         //                     else
//         //                     {
//         //                         entryData.address = finalAssetPath.Replace(folderPath + "/", string.Empty);
//         //                     }
//         //                     result.entryDatas.Add(guid, entryData);
//         //                 }
//         //             }
//         //             break;
//         //         case AddressableSetupSchema.EnumSearchMode.Manual:
//         //         {
//         //             var assetPaths = ConvertAssetObjectsToAssetPaths(addressData.assets);
//         //             foreach (var assetPath in assetPaths)
//         //             {
//         //                 var guid = AssetDatabase.AssetPathToGUID(assetPath);
//         //                 if (string.IsNullOrEmpty(guid))
//         //                 {
//         //                     Debug.LogError($"{assetPath} 没有guid");
//         //                     continue;
//         //                 }
//         //                 var finalAssetPath = assetPath.Replace("\\", "/");
//         //                 if (IsIgnoreAsset(finalAssetPath))
//         //                     continue;
//         //                 var entryData = new AddressableEntryData();
//         //                 entryData.assetPath = finalAssetPath;
//         //                 if (addressData.addressable)
//         //                 {
//         //                     entryData.address = Path.GetFileNameWithoutExtension(finalAssetPath).ToLower();
//         //                 }
//         //                 else
//         //                 {
//         //                     entryData.address = finalAssetPath;
//         //                 }
//         //                 result.entryDatas.Add(guid, entryData);
//         //             }
//         //         }
//         //             break;
//         //         case AddressableSetupSchema.EnumSearchMode.TheOne:
//         //         {
//         //             foreach (var folderData in addressData.folderDatas)
//         //             {
//         //                 var assetPath = AssetDatabase.GetAssetPath(folderData.folderObj);
//         //                 var guid = AssetDatabase.AssetPathToGUID(assetPath);
//         //                 if (string.IsNullOrEmpty(guid))
//         //                 {
//         //                     Debug.LogError($"{assetPath} 没有guid");
//         //                     continue;
//         //                 }
//         //                 var finalAssetPath = assetPath.Replace("\\", "/");
//         //                 if (IsIgnoreAsset(finalAssetPath))
//         //                     continue;
//         //                 var entryData = new AddressableEntryData();
//         //                 entryData.assetPath = finalAssetPath;
//         //                 result.entryDatas.Add(guid, entryData);
//         //             }
//         //         }
//         //             break;
//         //     }
//         //     switch (addressData.packMode)
//         //     {
//         //         case AddressableSetupSchema.EnumPackMode.PackSeparately:
//         //             result.bundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
//         //             break;
//         //         case AddressableSetupSchema.EnumPackMode.PackTogether:
//         //             result.bundleMode = BundledAssetGroupSchema.BundlePackingMode.PackTogether;
//         //             break;
//         //         case AddressableSetupSchema.EnumPackMode.PackTogetherByLabel:
//         //             result.bundleMode = BundledAssetGroupSchema.BundlePackingMode.PackTogetherByLabel;
//         //             foreach (var entryData in result.entryDatas.Values)
//         //             {
//         //                 //如果是根据label打包,那么生成一下label
//         //                 entryData.label = CreateBuildLabel(entryData.assetPath);
//         //             }
//         //             break;
//         //     }
//         //     return result;
//         // }
//         //
//         // private static void UpdateAddressableGroup(AddressableAssetSettings settings,
//         //     AddressableSetupSchema.AddressData addressData)
//         // {
//         //     var addressableGroupData = ConvertToAddressableGroupData(addressData);
//         //
//         //     if (addressableGroupData.entryDatas.Count < 1)
//         //         return;
//         //
//         //     var group = GetOrCreateAddressableGroup(settings, addressData.groupName);
//         //
//         //     var groupSchema = group.GetSchema<BundledAssetGroupSchema>();
//         //     groupSchema.BundleMode = addressableGroupData.bundleMode;
//         //
//         //     int count = 0;
//         //     var removeEntryList = new List<AddressableAssetEntry>();
//         //     //收集过期资产
//         //     foreach (var entry in group.entries)
//         //     {
//         //         if (!addressableGroupData.entryDatas.ContainsKey(entry.guid))
//         //         {
//         //             removeEntryList.Add(entry);
//         //         }
//         //     }
//         //     //移除过期资产
//         //     if (removeEntryList.Count > 0)
//         //     {
//         //         count = 0;
//         //         foreach (var removeEntry in removeEntryList)
//         //         {
//         //             if (EditorUtility.DisplayCancelableProgressBar(addressData.groupName, $"移除{removeEntry.address}",
//         //                 (count++ + 1.0f) / removeEntryList.Count))
//         //                 break;
//         //             group.RemoveAssetEntry(removeEntry);
//         //         }
//         //         removeEntryList.Clear();
//         //     }
//         //     //更新资产
//         //     int maxCount = addressableGroupData.entryDatas.Count;
//         //     foreach (var pairs in addressableGroupData.entryDatas)
//         //     {
//         //         var guid = pairs.Key;
//         //         var data = pairs.Value;
//         //         if (EditorUtility.DisplayCancelableProgressBar(addressData.groupName, $"更新{data}",
//         //             (count++ + 1.0f) / maxCount))
//         //             break;
//         //         var entry = group.GetAssetEntry(guid);
//         //         if (entry == null)
//         //         {
//         //             entry = settings.CreateOrMoveEntry(guid, group, false, true);
//         //         }
//         //         if (data.assetPath.Length >= 256)
//         //         {
//         //             Debug.LogError($"{data} too long");
//         //         }
//         //         entry.SetAddress(data.address);
//         //         ClearEntryLabel(entry);
//         //         if (!string.IsNullOrEmpty(data.label))
//         //         {
//         //             entry.SetLabel(data.label, true, true);
//         //         }
//         //     }
//         // }
//
//         private static void ClearEntryLabel(AddressableAssetEntry entry)
//         {
//             var removeLabels = new List<string>();
//             foreach (var label in entry.labels)
//             {
//                 removeLabels.Add(label);
//             }
//
//             foreach (var label in removeLabels)
//             {
//                 entry.SetLabel(label, false);
//             }
//         }
//
//         private static AddressableAssetGroup GetOrCreateAddressableGroup(AddressableAssetSettings settings,
//             string groupName)
//         {
//             var group = GetAddressableGroup(settings, groupName);
//             if (group == null)
//                 group = CreateAddressableGroup(settings, groupName);
//             return group;
//         }
//
//         private static AddressableAssetGroup GetAddressableGroup(AddressableAssetSettings settings, string groupName)
//         {
//             foreach (var group in settings.groups)
//             {
//                 if (group.name == groupName)
//                     return group;
//             }
//
//             return null;
//         }
//
//         private static AddressableAssetGroup CreateAddressableGroup(AddressableAssetSettings settings, string groupName)
//         {
//             var group = settings.CreateGroup(groupName, false, false, false, null);
//             var contentUpdateGroupSchema = group.AddSchema<ContentUpdateGroupSchema>();
//             contentUpdateGroupSchema.StaticContent = false;
//             var bundleAssetGroupSchema = group.AddSchema<BundledAssetGroupSchema>();
//             bundleAssetGroupSchema.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackTogether;
//             bundleAssetGroupSchema.Compression = BundledAssetGroupSchema.BundleCompressionMode.LZ4;
//             bundleAssetGroupSchema.BuildPath.SetVariableByName(settings, "Remote.BuildPath");
//             bundleAssetGroupSchema.LoadPath.SetVariableByName(settings, "Remote.LoadPath");
//
//             return group;
//         }
//
//         //将路径列表装换成相对路径列表
//         private static IEnumerable<string> ConvertToRelativePaths(string rootPath, IEnumerable<string> inputPaths)
//         {
//             var result = new List<string>();
//             foreach (var inputPath in inputPaths)
//             {
//                 result.Add(inputPath.Replace("\\", "/").Replace(rootPath + "/", string.Empty));
//             }
//
//             return result;
//         }
//
//         //将资源列表转换成资源路径列表
//         private static IEnumerable<string> ConvertAssetObjectsToAssetPaths(IEnumerable<Object> assets)
//         {
//             if (assets.Count() < 1)
//                 return null;
//             var result = new List<string>();
//             foreach (var asset in assets)
//             {
//                 var assetPath = AssetDatabase.GetAssetPath(asset);
//                 if (string.IsNullOrEmpty(assetPath))
//                     continue;
//                 result.Add(assetPath);
//             }
//
//             return result;
//         }
//
//         //如果是根据label分包的话,以向上两层文件夹作为label
//         private static string CreateBuildLabel(string assetPath)
//         {
//             assetPath = Path.GetDirectoryName(assetPath);
//             var split = assetPath;
//             int index = -1;
//             index = split.LastIndexOf("\\");
//             if (index == -1)
//                 return string.Empty;
//             split = split.Substring(0, index);
//             index = split.LastIndexOf("\\");
//             if (index != -1)
//             {
//                 split = split.Substring(0, index + 1); //这里+1是为了带上/
//             }
//
//             var label = assetPath.Replace(split, string.Empty);
//             label = label.Replace("\\", "_");
//             return label.ToLower();
//         }
//
//         //是否包含无视符号
//         private static bool IsIgnoreAsset(string assetPath)
//         {
//             if (assetPath.EndsWith(".meta"))
//                 return true;
//             if (assetPath.StartsWith("~") || assetPath.IndexOf("/~") != -1)
//                 return true;
//             return false;
//         }
//
//         //
//         private static string GetContentBinDirectoryName()
//         {
//             switch (EditorUserBuildSettings.activeBuildTarget)
//             {
//                 case BuildTarget.StandaloneWindows:
//                 case BuildTarget.StandaloneWindows64:
//                     return "Windows";
//             }
//
//             return EditorUserBuildSettings.activeBuildTarget.ToString();
//         }
//     }
// }