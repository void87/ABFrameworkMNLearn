using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFramework {

    public class Test_SingleABLoader : MonoBehaviour {

        private SingleABLoader loader = null;

        private string dependABName1 = "scene_1/textures.ab";
        private string dependABName2 = "scene_1/materials.ab";
        private string abName1 = "scene_1/prefabs.ab";
        

        private string assetName1 = "TestCubePrefab.prefab";

        #region 简单(无依赖包)预设的加载

        //void Start() {
        //    loader = new SingleABLoader(abName1, LoadComplete);
        //    StartCoroutine(loader.LoadAssetBundle());
        //}

        //private void LoadComplete(string abName) {

        //    var prefab = loader.LoadAsset(assetName1, false);
        //    GameObject.Instantiate(prefab);
        //}

        #endregion

        void Start() {
            SingleABLoader dependABLoader = new SingleABLoader(dependABName1, LoadDependAB1Complete);
            StartCoroutine(dependABLoader.LoadAssetBundle());
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                Debug.Log("释放资源");
                //loader.Dispose();
                loader.DisposeAll();
            }    
        }

        private void LoadDependAB1Complete(string abName) {
            Debug.Log(abName + "加载完成");

            SingleABLoader dependABLoader = new SingleABLoader(dependABName2, LoadDependAB2Complete);
            StartCoroutine(dependABLoader.LoadAssetBundle());
        }

        private void LoadDependAB2Complete(string abName) {
            Debug.Log(abName + "加载完成");

            loader = new SingleABLoader(abName1, LoadAB1Complete);
            StartCoroutine(loader.LoadAssetBundle());
        }

        private void LoadAB1Complete(string abName) {
            Debug.Log(abName + "加载完成");

            var prefab = loader.LoadAsset(assetName1, false);
            GameObject.Instantiate(prefab);

            string[] arr = loader.RetrieveAllAssetName();

            foreach (var item in arr) {
                Debug.Log(item);
            }
        }

    }
}


