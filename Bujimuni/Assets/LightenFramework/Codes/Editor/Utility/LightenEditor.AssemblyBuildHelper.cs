using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

namespace Lighten.Editor
{
    public static partial class LightenEditor
    {
        public static class AssemblyBuildHelper
        {
            //编译程序集
            public static void BuildAssembly(string outputPath, string assemblyName,
                List<string> codePaths, 
                IEnumerable<string> additionalReferences,
                CodeOptimization codeOptimization,
                AssemblyBuilderFlags flags, IEnumerable<string> excludeReferences)
            {
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                string dllPath = Path.Combine(outputPath, $"{assemblyName}.dll");
                string pdbPath = Path.Combine(outputPath, $"{assemblyName}.pdb");
                if (File.Exists(dllPath))
                {
                    File.Delete(dllPath);
                }
                if (File.Exists(pdbPath))
                {
                    File.Delete(pdbPath);
                }

                var scripts = CollectScripts(codePaths);
                
                //
                var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
                
                var assemblyBuilder = new AssemblyBuilder(dllPath, scripts.ToArray());
                assemblyBuilder.compilerOptions.AllowUnsafeCode = true;
                assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
                assemblyBuilder.compilerOptions.ApiCompatibilityLevel =
                    PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
                assemblyBuilder.additionalReferences = additionalReferences.ToArray();
                assemblyBuilder.flags = flags;
                assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;
                assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;
                assemblyBuilder.buildTargetGroup = buildTargetGroup;
                assemblyBuilder.excludeReferences = excludeReferences.ToArray();
                assemblyBuilder.buildStarted += (assemblyPath) =>
                {
                    Debug.Log($"Build Start: {assemblyPath}");
                };
                assemblyBuilder.buildFinished += (assemblyPath, compilerMessages) =>
                {
                    int errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                    int warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);

                    Debug.LogFormat("Warnings: {0} - Errors: {1}", warningCount, errorCount);

                    if (warningCount > 0)
                    {
                        Debug.LogFormat("有{0}个Warning!!!", warningCount);
                        for (int i = 0; i < compilerMessages.Length; i++)
                        {
                            if (compilerMessages[i].type == CompilerMessageType.Warning)
                            {
                                string filename = Path.GetFullPath(compilerMessages[i].file);
                                Debug.LogWarning(
                                    $"{compilerMessages[i].message} (at <a href=\"file:///{filename}/\" line=\"{compilerMessages[i].line}\">{Path.GetFileName(filename)}</a>)");
                            }
                        }
                    }

                    if (errorCount > 0)
                    {
                        for (int i = 0; i < compilerMessages.Length; i++)
                        {
                            if (compilerMessages[i].type == CompilerMessageType.Error)
                            {
                                string filename = Path.GetFullPath(compilerMessages[i].file);
                                Debug.LogError(
                                    $"{compilerMessages[i].message} (at <a href=\"file:///{filename}/\" line=\"{compilerMessages[i].line}\">{Path.GetFileName(filename)}</a>)");
                            }
                        }
                    }
                };
                
                //DebugAssemblyBuilderInfo(assemblyBuilder);
                
                //开始构建
                if (!assemblyBuilder.Build())
                {
                    Debug.LogErrorFormat("build fail：" + assemblyBuilder.assemblyPath);
                    return;
                }
                Debug.LogFormat("build success：" + assemblyBuilder.assemblyPath);
                while (EditorApplication.isCompiling)
                {
                    // 主线程sleep并不影响编译线程
                    Thread.Sleep(1);
                }
            }
            
            //获取路径下所有脚本文件
            public static List<string> CollectScripts(List<string> codePaths)
            {
                List<string> scripts = new List<string>();
                foreach (var codePath in codePaths)
                {
                    scripts.AddRange(Directory.GetFiles(codePath, "*.cs", SearchOption.AllDirectories));
                }
                return scripts;
            }

            private static void DebugAssemblyBuilderInfo(AssemblyBuilder assemblyBuilder)
            {
                Debug.Log("=== defaultDefine ===" + assemblyBuilder.defaultDefines.Length);
                foreach (var defaultDefine in assemblyBuilder.defaultDefines)
                {
                    Debug.Log(defaultDefine);
                }
                if (assemblyBuilder.additionalDefines != null)
                {
                    Debug.Log("=== additionalDefines ===" + assemblyBuilder.additionalDefines.Length);
                    foreach (var additionalDefine in assemblyBuilder.additionalDefines)
                    {
                        Debug.Log(additionalDefine);
                    }
                }
                Debug.Log("=== defaultReferences ===" + assemblyBuilder.defaultReferences.Length);
                foreach (var defaultReference in assemblyBuilder.defaultReferences)
                {
                    Debug.Log(defaultReference);
                }

                Debug.Log("=== additionalReferences ===" + assemblyBuilder.additionalReferences.Length);
                foreach (var additionalReference in assemblyBuilder.additionalReferences)
                {
                    Debug.Log(additionalReference);
                }
                
                Debug.Log("=== excludeReferences ===" + assemblyBuilder.excludeReferences.Length);
                foreach (var excludeReference in assemblyBuilder.excludeReferences)
                {
                    Debug.Log(excludeReference);
                }
            }
        }
    }
}
