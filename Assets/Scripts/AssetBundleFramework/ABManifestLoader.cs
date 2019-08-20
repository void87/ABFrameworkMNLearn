using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFramework {

    /// <summary>
    /// 读取AssetBundle依赖关系文件
    /// </summary>
    public class ABManifestLoader : System.IDisposable {

        private static ABManifestLoader instance;

        private AssetBundleManifest manifest;

        private string manifestPath;

        // 读取Manifest的AB
        private AssetBundle manifestAB;

        private bool isLoaded;


        public bool IsLoaded {
            get {
                return isLoaded;
            }
        }

        public static ABManifestLoader Instance {
            get {
                if (instance == null) {
                    instance = new ABManifestLoader();
                }
                return instance;
            }
        }


        private ABManifestLoader() {
            manifestPath = PathTools.GetWWWPath() + "/" + PathTools.GetPlatformName();

            manifest = null;
            manifestAB = null;
            isLoaded = false;
        }

        /// <summary>
        /// 加载Manifest清单文件
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadManifest() {
            using (WWW www = new WWW(manifestPath)) {
                yield return www;

                if (www.progress >= 1) {
                    AssetBundle ab = www.assetBundle;

                    if (ab != null) {
                        manifestAB = ab;
                        manifest = manifestAB.LoadAsset(ABDefine.ASSETBUNDLEMANIFEST) as AssetBundleManifest;    // "AssetBundleManifest"固定写法
                        isLoaded = true;
                    } else {
                        Debug.Log(GetType() + "/LoadManifest()/WWW下载出错,请检查! manifestPath = " + manifestPath + " 错误信息: " + www.error);
                    }
                }
            }
        }

        /// <summary>
        /// 获取Manifest
        /// </summary>
        public AssetBundleManifest GetABManifest() {
            if (isLoaded) {
                if (manifest != null) {
                    return manifest;
                } else {
                    Debug.Log(GetType() + "/GetABManifest()/manifest == null!请检查");
                }
            } else {
                Debug.Log(GetType() + "/GetABManifest()/isLoaded == false,Manifest没有加载完毕!请检查");
            }

            return null;
        }

        /// <summary>
        /// 获取Manifest中指定包名的依赖项
        /// </summary>
        /// <returns></returns>
        public string[] GetAllDependency(string abName) {
            if (manifest != null && !string.IsNullOrEmpty(abName)) {
                return manifest.GetAllDependencies(abName);
            }
            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            if (manifestAB != null) {
                manifestAB.Unload(true);
            }
        }
    }
}


