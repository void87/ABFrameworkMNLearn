using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ABFramework {

    public class DeleteAssetBundle {

        [MenuItem("AssetBundleTools/DeleteAllAssetBundle")]
        public static void DelAssetBundle() {
            string deleteDir = string.Empty;

            deleteDir = PathTools.GetABOutputPath();

            if (!string.IsNullOrEmpty(deleteDir)) {
                Directory.Delete(deleteDir, true);
                File.Delete(deleteDir + " .meta");
                AssetDatabase.Refresh();
            }
        }
    }
}


