using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFramework {

    public class Test_Framework : MonoBehaviour {

        private string sceneName = "scene_1";

        private string abName = "scene_1/prefabs.ab";

        private string assetName = "TestCubePrefab.prefab";


        private void Start() {
            Debug.Log("开始 ABFramework 测试");

            // 调用AB包(连锁智能调用AB包)
            StartCoroutine(AssetBundleMgr.Instance().LoadAssetBundle(sceneName, abName, LoadALLABComplete));
        }

        private void LoadALLABComplete(string abName) {
            Debug.Log("所有的AB包都已经加载完毕了");

            Object tempObj = AssetBundleMgr.Instance().LoadAsset(sceneName, abName, assetName, false);

            if (tempObj != null) {
                Instantiate(tempObj);
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                Debug.Log("测试销毁资源");
                AssetBundleMgr.Instance().DisposeAllAsset(sceneName);
            }
        }
    }
}


