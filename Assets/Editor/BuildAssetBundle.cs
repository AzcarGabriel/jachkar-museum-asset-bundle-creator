// BuildAssets.cs
using System;
using System.IO;

public class BuildAssets : UnityEngine.MonoBehaviour
{
    static void BuildStoneAssetBundle()
    {
        Console.WriteLine("PROCESS BUILD ASSET BUNDLE BEGINS -------------------------------------------------------");

        int i = 0;
        string log = "BuildStoneAssetBundleDetailsLog.txt";
        string[] assetN;
        int N_Files;
        UnityEditor.AssetBundleBuild[] AssetMap = new UnityEditor.AssetBundleBuild[2];
        AssetMap[0].assetBundleName = "bundle";

        // Adding to path /Models
        string path = UnityEngine.Application.dataPath + "/Models";

        // log
        if (!File.Exists(log))
        {
            File.Create(log);
        }

        File.AppendAllText(log, System.DateTime.Now.ToString() + "\n\n");
        File.AppendAllText(log, path + "\n");

        DirectoryInfo dir = new System.IO.DirectoryInfo(path);
        FileInfo[] files = dir.GetFiles();

        // Number of files in "/Models" folder
        N_Files = files.Length;

        // log
        System.IO.File.AppendAllText(log, "Num assets: " + N_Files + " \n");

        assetN = new string[N_Files];
        foreach (FileInfo file in files)
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
        File.AppendAllText(log, "\t----X----\n");
    }

    static void BuildThumbsAssetBundle()
    {
        Console.WriteLine("PROCESS THUMBS ASSET BUNDLE BEGINS -------------------------------------------------------");

        int i = 0;
        string log = "BuildThumbsAssetBundleDetailsLog.txt";
        string[] assetN;
        int N_Files;
        UnityEditor.AssetBundleBuild[] AssetMap = new UnityEditor.AssetBundleBuild[2];
        AssetMap[0].assetBundleName = "stones_thumbs";

        // Adding to path /Models
        string path = UnityEngine.Application.dataPath + "/ProcessedThumbs";

        // log
        //if (!File.Exists(log))
        //{
         //   File.Create(log);
        //}

        //File.AppendAllText(log, DateTime.Now.ToString() + "\n\n");
        //File.AppendAllText(log, path + "\n");

        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] files = dir.GetFiles();

        // Number of files in "/ProcessedThumbs" folder
        N_Files = files.Length;

        // log
        //File.AppendAllText(log, "Num assets: " + N_Files + " \n");

        assetN = new string[N_Files];
        foreach (FileInfo file in files)
        {
            if (file.Exists)
            {
                if (!file.Extension.Equals(".meta"))
                {
                    assetN[i] = "Assets/ProcessedThumbs/" + file.Name;
                    //File.AppendAllText(log, assetN[i] + " \n");
                    i += 1;
                }
            }
        }
        AssetMap[0].assetNames = assetN;

        UnityEditor.BuildPipeline.BuildAssetBundles("Assets/AssetBundles", AssetMap, UnityEditor.BuildAssetBundleOptions.None, UnityEditor.BuildTarget.WebGL);

        // log
        //File.AppendAllText(log, "\t----X----\n");
    }
}