using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFramework {

    /// <summary>
    /// 主流程(3): (一个场景中)多个AssetBundle管理
    /// 
    /// 1. 获取AB包的依赖与引用
    /// 2. 管理AB包之间的自动连锁(递归)加载机制
    /// </summary>
    public class MultiABMgr {

        private SingleABLoader singleABLoader;

        // 缓存AB包,防止重复加载
        private Dictionary<string, SingleABLoader> singleABLoaderDict;

        // 当前场景(调试使用)
        private string currentSceneName;

        private string currentABName;

        private Dictionary<string, ABRelation> abRelationDict;

        private ABLoadComplete loadCompleteHandler;

        public MultiABMgr(string sceneName, string abName, ABLoadComplete abLoadComplete) {
            currentSceneName = sceneName;
            currentABName = abName;
            singleABLoaderDict = new Dictionary<string, SingleABLoader>();
            abRelationDict = new Dictionary<string, ABRelation>();

            loadCompleteHandler = abLoadComplete;
        }

        /// <summary>
        /// 完成指定AB包的调用
        /// </summary>
        /// <param name="abName"></param>
        private void LoadABCompleted(string abName) {
            //Debug.Log(GetType() + "/当前完成ABName:" + abName);
            if (abName.Equals(currentABName)) {
                if (loadCompleteHandler != null) {
                    loadCompleteHandler(abName);
                }
            }
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        public IEnumerator LoadAssetBundle(string abName) {
            // AB包关系的建立
            if (!abRelationDict.ContainsKey(abName)) {
                ABRelation abRelation = new ABRelation(abName);
                abRelationDict.Add(abName, abRelation);
            }

            ABRelation tempABRelation = abRelationDict[abName];

            // 得到指定AB包所有的依赖关系(查询manifest文件)
            string[] abDependencyArr = ABManifestLoader.Instance.GetAllDependency(abName);

            foreach (var item in abDependencyArr) {

                Debug.Log("abName: " + abName + ", depenABName: " + item);

                // 添加依赖项
                tempABRelation.AddDependency(item);
                // 添加被引用项
                yield return LoadReference(item, abName);
            }

            //Debug.Log(tempABRelation.GetAllDependency());
            //Debug.Log(tempABRelation.GetAllReference());


            // 真正加载AB包
            if (singleABLoaderDict.ContainsKey(abName)) {
                yield return singleABLoaderDict[abName].LoadAssetBundle();
            } else {
                singleABLoader = new SingleABLoader(abName, LoadABCompleted);
                singleABLoaderDict.Add(abName, singleABLoader);

                yield return singleABLoader.LoadAssetBundle();
            }
        }

        /// <summary>
        /// 加载被引用AB包
        /// </summary>
        private IEnumerator LoadReference(string abName, string refABName) {
            
            if (abRelationDict.ContainsKey(abName)) {
                ABRelation tempABRelation = abRelationDict[abName];
                // 添加AB包被引用关系(被依赖)
                tempABRelation.AddReference(refABName);
            } else {
                ABRelation tempABRelation = new ABRelation(abName);
                tempABRelation.AddReference(refABName);
                abRelationDict.Add(abName, tempABRelation);

                // 开始加载依赖的包
                yield return LoadAssetBundle(abName);
            }
        }

        /// <summary>
        /// 加载(AB包中)资源
        /// </summary>
        public UnityEngine.Object LoadAsset(string abName, string assetName, bool isCache) {
            foreach (var item in singleABLoaderDict.Keys) {
                if (abName == item) {
                    return singleABLoaderDict[item].LoadAsset(assetName, isCache);
                }
            }

            Debug.LogError(GetType() + "/LoadAsset()/找不到AssetBundle包,无法加载资源,请检查! abName = " + abName + ", assetName = " + assetName);

            return null;
        }

        /// <summary>
        /// 释放本场景中所有的资源
        /// </summary>
        public void DisposeAllAsset() {
            try {
                foreach (var item in singleABLoaderDict.Values) {
                    item.DisposeAll();
                }
            } finally {
                singleABLoaderDict.Clear();
                singleABLoaderDict = null;

                abRelationDict.Clear();
                abRelationDict = null;

                currentABName = null;
                currentSceneName = null;
                loadCompleteHandler = null;

                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
        }
    }
}

