using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTools {

    /* 路径常量 */
    public const string AB_RESOURCES = "AB_Res";

    /// <summary>
    /// 获取AB资源的输入目录
    /// </summary>
    public static string GetABResourcesPath() {
        return Application.dataPath + "/" + AB_RESOURCES;
    }

    /// <summary>
    /// 获取AB资源的输出目录
    ///     1. 平台路径
    ///     2. 平台的名称
    /// </summary>
    public static string GetABOutputPath() {
        return GetPlatformPath() + "/" + GetPlatformName();
    }

    /// <summary>
    /// 获取平台路径
    /// </summary>
    private static string GetPlatformPath() {
        string platformPath = string.Empty;

        switch (Application.platform) {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                platformPath = Application.streamingAssetsPath;
                break;
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
                platformPath = Application.persistentDataPath;
                break;
        }
        return platformPath;
    }

    /// <summary>
    /// 获取平台名称
    /// </summary>
    private static string GetPlatformName() {
        string platformName = string.Empty;

        switch (Application.platform) {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                platformName = "Windows";
                break;
            case RuntimePlatform.IPhonePlayer:
                platformName = "iPhone";
                break;
            case RuntimePlatform.Android:
                platformName = "Android";
                break;
        }
        return platformName;
    }

    /// <summary>
    /// 获取WWW下载(AB包)路径
    /// </summary>
    public static string GetWWWPath() {
        string WWWPath = string.Empty;

        switch (Application.platform) {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                WWWPath = "file://" + GetABOutputPath();
                break;
            case RuntimePlatform.Android:
                WWWPath = "jar:file://" + GetABOutputPath();
                break;
        }

        return WWWPath;
    }
}
