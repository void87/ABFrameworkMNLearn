using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFramework {

    /// <summary>
    /// 框架主流程(4): 所有场景的AssetBundle管理
    /// 
    /// 1. 读取Manifest
    /// 2. 以场景为单位,管理整个项目中所有的AssetBundle包
    /// </summary>
    public class AssetBundleMgr : MonoBehaviour {

        private static AssetBundleMgr instance;

        private Dictionary<string, MultiABMgr> multiABMgrDict = new Dictionary<string, MultiABMgr>();

        private AssetBundleManifest manifest = null;


        public static AssetBundleMgr Instance() {
            if (instance == null) {
                instance = new GameObject("AssetBundleMgr").AddComponent<AssetBundleMgr>();
            }
            return instance;
        }

        private AssetBundleMgr() {

        }

        private void Awake() {
            StartCoroutine(ABManifestLoader.Instance.LoadManifest());
        }

        public IEnumerator LoadAssetBundle(string sceneName, string abName, ABLoadComplete abLoadComplete) {
            // 参数检查
            if (string.IsNullOrEmpty(sceneName) || string.IsNullOrEmpty(abName)) {
                Debug.LogError(GetType() + "/LoadAssetBundle()/sceneName or abName == null,请检查!");
                yield return null;
            }

            // 等待Manifest文件加载完成
            while (!ABManifestLoader.Instance.IsLoaded) {
                yield return null;
            }
            manifest = ABManifestLoader.Instance.GetABManifest();
            if (manifest == null) {
                Debug.LogError(GetType() + "/LoadAssetBundle()/manifest == null,请检查!");
                yield return null;
            }

            // 加入当前场景
            if (!multiABMgrDict.ContainsKey(sceneName)) {
                MultiABMgr multiABMgr = new MultiABMgr(sceneName, abName, abLoadComplete);
                multiABMgrDict.Add(sceneName, multiABMgr);
            }

            // 调用下一层(多包管理)
            MultiABMgr tempMultiABMgr = multiABMgrDict[sceneName];

            if (tempMultiABMgr == null) {
                Debug.LogError(GetType() + "/LoadAssetBundle()/tempMultiABMgr == null,请检查!");
            }

            yield return tempMultiABMgr.LoadAssetBundle(abName);
        }

        /// <summary>
        /// 加载(AB包中)资源
        /// </summary>
        public UnityEngine.Object LoadAsset(string sceneName, string abName, string assetName, bool isCache) {
            if (multiABMgrDict.ContainsKey(sceneName)) {
                MultiABMgr multiABMgr = multiABMgrDict[sceneName];
                return multiABMgr.LoadAsset(abName, assetName, isCache);
            }

            Debug.LogError(GetType() + "/LoadAsset()/找不到场景名称,无法加载资源,请检查! sceneName: " + sceneName);

            return null;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void DisposeAllAsset(string sceneName) {
            if (multiABMgrDict.ContainsKey(sceneName)) {
                MultiABMgr multiABMgr = multiABMgrDict[sceneName];
                multiABMgr.DisposeAllAsset();
            } else {
                Debug.LogError(GetType() + "/LoadAsset()/找不到场景名称,无法释放资源,请检查! sceneName: " + sceneName);
            }
        }

    }
}


