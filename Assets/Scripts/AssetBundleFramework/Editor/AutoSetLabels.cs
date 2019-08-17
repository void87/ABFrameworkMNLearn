using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ABFramework {

    /// <summary>
    /// 1. 定义需要打包资源的文件夹根目录
    /// 2. 遍历每个"场景"文件夹(目录)
    ///     a. 遍历本场景目录下所有的目录或者文件
    ///        如果是目录,则继续"递归"访问里面的文件,直到定位到文件
    ///     b. 找到文件,则使用AssetImporter类,标记"包名"与"后缀名"
    /// </summary>
    public class AutoSetLabels {

        /// <summary>
        /// 设置AB包名称
        /// </summary>
        [MenuItem("AssetBundleTools/Set AB Label")]
        public static void SetABLabel() {

            AssetDatabase.RemoveUnusedAssetBundleNames();

            string strLabelRoot = string.Empty;
            strLabelRoot = Application.dataPath + "/" + "AB_Res";

            DirectoryInfo[] dirScenesArray = null;  // 根目录下的所有一级子目录

            DirectoryInfo dirTempInfo = new DirectoryInfo(strLabelRoot);
            dirScenesArray = dirTempInfo.GetDirectories();

            foreach (var item in dirScenesArray) {
                string tempSceneDir = strLabelRoot + "/" + item.Name;  // 全路径

                DirectoryInfo tempSceneDirInfo = new DirectoryInfo(tempSceneDir);

                int tempIndex = tempSceneDir.LastIndexOf("/");
                string tempSceneName = tempSceneDir.Substring(tempIndex + 1);  // 场景名称

                JudgeDirOrFileRecursive(item, tempSceneName);

                //string sceneName = tmpSceneDir
            }


            AssetDatabase.Refresh();

            Debug.Log("AssetBundle 本次操作设置标记完成!");
        }

        private static void JudgeDirOrFileRecursive(FileSystemInfo fileSystemInfo, string sceneName) {
            if (!fileSystemInfo.Exists) {
                Debug.LogError("文件或目录名称: " + fileSystemInfo + "不存在,检查");
                return;
            }

            DirectoryInfo dirInfo = fileSystemInfo as DirectoryInfo;    // 文件信息转换为目录信息
            FileSystemInfo[] fileSystemInfoArr = dirInfo.GetFileSystemInfos();

            foreach (var item in fileSystemInfoArr) {
                FileInfo fileInfo = item as FileInfo;

                if (fileInfo != null) {
                    SetFileABLabel(fileInfo, sceneName);
                } else {
                    JudgeDirOrFileRecursive(item, sceneName);
                }
            }
        }

        /// <summary>
        /// 对指定的文件设置AB包名
        /// </summary>
        private static void SetFileABLabel(FileInfo fileInfo, string sceneName) {
            string abName = string.Empty;
            string assetFilePath = string.Empty;    // 文件路径(相对路径)

            if (fileInfo.Extension == ".meta") {
                return;
            }

            // 获取资源文件的相对路径
            int tempIndex = fileInfo.FullName.IndexOf("Assets");
            assetFilePath = fileInfo.FullName.Substring(tempIndex);

            AssetImporter tempImporter = AssetImporter.GetAtPath(assetFilePath);
            tempImporter.assetBundleName = abName;

            if (fileInfo.Extension == ".unity") {
                tempImporter.assetBundleVariant = "u3d";
            } else {
                tempImporter.assetBundleVariant = "ab";
            }

            //if (fileInfo.Extension == ".unity") {
            //    // 定义AB包的扩展名
                
            //}

            //Debug.Log(assetFilePath);

            //AssetImporter tempImporter = AssetImporter.GetAtPath(fileInfo.FullName);
            //Debug.Log(tempImporter);
            //Debug.Log(tempImporter.)
        }
    }
}
