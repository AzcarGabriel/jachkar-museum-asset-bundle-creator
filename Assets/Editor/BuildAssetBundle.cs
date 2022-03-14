//BuildAssets.cs
using System.Collections;
using System.Collections.Generic;

public class BuildAssets : UnityEngine.MonoBehaviour
{
    static void BuildAssetBundle()
    {
        int i = 0;
        string log = "BuildAssetBundleLog.txt";
        string[] assetN;
        int N_Files;
        UnityEditor.AssetBundleBuild[] AssetMap = new UnityEditor.AssetBundleBuild[2];
        AssetMap[0].assetBundleName = "bundle";

        // Adding to path /Models
        string path = UnityEngine.Application.dataPath + "/Models";

        // log
        System.IO.File.AppendAllText(log, System.DateTime.Now.ToString() + "\n\n");
        System.IO.File.AppendAllText(log, path + "\n");


        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
        System.IO.FileInfo[] files = dir.GetFiles();

        // Number of files in "/Models" folder
        N_Files = files.Length;

        // log
        System.IO.File.AppendAllText(log, "Num assets: " + N_Files + " \n");

        assetN = new string[N_Files];
        foreach (System.IO.FileInfo file in files)
        {
            if (file.Exists)
            {
                if (!file.Extension.Equals(".meta"))
                {
                    assetN[i] = "Assets/Models/" + file.Name;
                    System.IO.File.AppendAllText(log, assetN[i] + " \n");
                    i += 1;
                }
            }
        }
        AssetMap[0].assetNames = assetN;

        UnityEditor.BuildPipeline.BuildAssetBundles("Assets/AssetBundles", AssetMap, UnityEditor.BuildAssetBundleOptions.None, UnityEditor.BuildTarget.WebGL);

        // log
        System.IO.File.AppendAllText(log, "\t----X----\n");
    }
}