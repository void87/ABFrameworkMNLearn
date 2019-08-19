using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFramework {

    /// <summary>
    /// 框架主流程
    ///     1. AB资源加载类
    /// 
    /// AB包内资源的加载
    /// 
    /// 功能:
    ///     1. 管理与加载指定AB的资源
    ///     2. 加载具有"缓存功能"的资源,带选用参数
    ///     3. 卸载,释放AB资源
    ///     4. 查看当前AB资源
    /// </summary>
    public class AssetLoader : System.IDisposable {

        private AssetBundle currentAB;

        private Hashtable ht;


        public AssetLoader(AssetBundle ab) {
            if (ab != null) {
                currentAB = ab;
                ht = new Hashtable();
            } else {
                Debug.LogError(GetType() + "/构造函数 AssetBundle()/ 参数 ab == null!,请检查");
            }
        }

        /// <summary>
        /// 加载当前包中指定的资源
        /// </summary>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache = false) {
            return LoadResource<UnityEngine.Object>(assetName, isCache);
        }

        /// <summary>
        /// 加载当前AB包的资源
        /// </summary>
        private T LoadResource<T>(string assetName, bool isCahce) where T: UnityEngine.Object {
            if (ht.ContainsKey(assetName)) {
                return ht[assetName] as T;
            }

            T tempRes = currentAB.LoadAsset<T>(assetName);

            if (tempRes != null && isCahce) {
                ht.Add(assetName, tempRes);
            } else if (tempRes == null) {
                Debug.LogError(GetType() + "/LoadResource<T>/参数 tempRes==null, 请检查");
            }

            return tempRes;
        }

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        public bool UnLoadAsset(UnityEngine.Object asset) {
            if (asset != null) {
                Resources.UnloadAsset(asset);
                return true;
            }

            Debug.LogError(GetType() + "/UnLoadAsset()/参数 asset == null, 请检查");
            return false;
        }

        /// <summary>
        /// 释放当前AssetBundle内存镜像资源
        /// </summary>
        public void Dispose() {
            currentAB.Unload(false);    
        }

        /// <summary>
        /// 释放当前AssetBundle内存镜像资源,且释放内存资源
        /// </summary>
        public void DisposeAll() {
            currentAB.Unload(true);
        }

        /// <summary>
        /// 查询当前AssetBundle中包含的所有资源名称
        /// </summary>
        /// <returns></returns>
        public string[] RetrieveAllAssetName() {
            return currentAB.GetAllAssetNames();
        }
    }
}


