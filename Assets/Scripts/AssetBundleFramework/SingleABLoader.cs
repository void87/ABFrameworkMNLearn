using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ABFramework {

    /// <summary>
    /// 框架主流程
    ///     2. WWW 加载AssetBundle
    /// 
    /// AB包内资源的加载
    /// 
    /// 功能:
    ///     1. 管理与加载指定AB的资源
    ///     2. 加载具有"缓存功能"的资源,带选用参数
    ///     3. 卸载,释放AB资源
    ///     4. 查看当前AB资源
    /// </summary>
    public class SingleABLoader : System.IDisposable {

        private AssetLoader assetLoader;

        private ABLoadComplete abLoadCompleteHandler;

        private string abName;

        private string abDownloadPath;


        public SingleABLoader(string abName, ABLoadComplete loadComplete) {
            assetLoader = null;

            abLoadCompleteHandler = loadComplete;

            this.abName = abName;
            abDownloadPath = PathTools.GetWWWPath() + "/" + abName;

        }

        public IEnumerator LoadAssetBundle() {
            using (WWW www = new WWW(abDownloadPath)) {
                yield return www;

                if (www.progress >= 1) {
                    AssetBundle ab = www.assetBundle;

                    if (ab != null) {
                        assetLoader = new AssetLoader(ab);

                        if (abLoadCompleteHandler != null) {
                            abLoadCompleteHandler(abName);
                        }

                    } else {
                        Debug.LogError(GetType() + "/LoadAssetBundle()/www下载出错,请检查! AssetBundle URL: " + abDownloadPath + " 错误信息: " + www.error);
                    }
                }
            }
        }

        /// <summary>
        /// 加载(AB包内)资源
        /// </summary>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache) {
            if (assetLoader != null) {
                return assetLoader.LoadAsset(assetName, isCache);
            }
            Debug.LogError(GetType() + "/LoadAsset/assetLoader==null!, 请检查!");
            return null;
        }

        /// <summary>
        /// 卸载(AB包内)资源
        /// </summary>
        public void UnLoadAsset(UnityEngine.Object asset) {
            if (assetLoader != null) {
                assetLoader.UnLoadAsset(asset);
            } else {
                Debug.LogError(GetType() + "/UnLoadAsset()/参数 assetLoader == null, 请检查!");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            if (assetLoader != null) {
                assetLoader.Dispose();
                assetLoader = null;
            } else {
                Debug.LogError(GetType() + "/Dispose()/参数 assetLoader == null, 请检查!");
            }
        }

        /// <summary>
        /// 释放当前AssetBundle资源包,且卸载所有资源
        /// </summary>
        public void DisposeAll() {
            if (assetLoader != null) {
                assetLoader.DisposeAll();
                assetLoader = null;
            } else {
                Debug.LogError(GetType() + "/DisposeAll()/参数 assetLoader == null, 请检查!");
            }
        }

        /// <summary>
        /// 查询当前AssetBundle包中所有的资源
        /// </summary>
        public string[] GetAllAssetName() {
            if (assetLoader != null) {
                return assetLoader.GetAllAssetName();
            }
            Debug.LogError(GetType() + "/RetrieveAllAssetName()/参数 assetLoader == null, 请检查!");

            return null;
        }
    }
}
