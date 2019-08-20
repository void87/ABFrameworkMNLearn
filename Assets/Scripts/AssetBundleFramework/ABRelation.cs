using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFramework {

    /// <summary>
    /// 1. 存储指定AB包的所有依赖关系包
    /// 2. 存储指定AB包的所有被引用关系包
    /// </summary>
    public class ABRelation {

        // 当前AB包名
        private string abName;
        // 当前AB包的所有依赖包
        private List<string> dependencyABList;
        // 当前AB包的所有被引用包
        private List<string> referenceABList;


        public ABRelation(string abName) {
            if (!string.IsNullOrEmpty(abName)) {
                this.abName = abName;
            }

            dependencyABList = new List<string>();
            referenceABList = new List<string>();


        }

        /* 依赖关系 */

        /// <summary>
        /// 增加依赖关系
        /// </summary>
        public void AddDependency(string abName) {
            if (!dependencyABList.Contains(abName)) {
                dependencyABList.Add(abName);
            }
        }

        /// <summary>
        /// 移除依赖关系
        /// </summary>
        /// <returns>
        /// true    此AB没有依赖项
        /// false   此AB还有依赖项
        /// </returns>
        public bool RemoveDependency(string abName) {
            if (dependencyABList.Contains(abName)) {
                dependencyABList.Remove(abName);
            }

            if (dependencyABList.Count > 0) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// 获取所有依赖关系
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDependency() {
            return dependencyABList;
        }


        /* 被引用关系 */


        /// <summary>
        /// 增加被引用关系
        /// </summary>
        public void AddReference(string abName) {
            if (!referenceABList.Contains(abName)) {
                referenceABList.Add(abName);
            }
        }

        /// <summary>
        /// 移除被引用关系
        /// </summary>
        /// <returns>
        /// true    此AB没有被引用项
        /// false   此AB还有被引用项
        /// </returns>
        public bool RemoveReference(string abName) {
            if (referenceABList.Contains(abName)) {
                referenceABList.Remove(abName);
            }

            if (dependencyABList.Count > 0) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// 获取所有被引用关系
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllReference() {
            return referenceABList;
        }
    }
}


