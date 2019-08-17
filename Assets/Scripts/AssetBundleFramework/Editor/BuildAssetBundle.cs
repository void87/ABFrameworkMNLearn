using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ABFramework {

    public class BuildAssetBundle { 

        [MenuItem("AssetBundleTools/BuildAllAssetBundles")]
        public static void BuildAllAB() {
            string abOutPathDir = string.Empty;

            abOutPathDir = PathTools.GetABOutputPath();

            if (!Directory.Exists(abOutPathDir)) {
                Directory.CreateDirectory(abOutPathDir);
            }

            BuildPipeline.BuildAssetBundles(abOutPathDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
    }
}


