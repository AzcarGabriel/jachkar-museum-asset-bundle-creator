// BuildStonePrefabs.cs
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class BuildPrefabs : UnityEngine.MonoBehaviour
{
    static string STONE_FROM_FOLDER_PATH = "Assets/RawAssets/";
    static string STONE_TO_FOLDER_PATH = "Assets/"; // Deberia ser Stones o algo asi
    static string STONE_PROCESSED_FOLDER_PATH = "Assets/ProcessedAssets/";
    static string THUMBS_FROM_FOLDER_PATH = "Assets/RawThumbs/";
    static string THUMBS_PROCESSED_FOLDER_PATH = "Assets/ProcessedThumbs/";
    static string CONFIG_FILE = "Assets/config.json";

    public class Config
    {
        public int actualStoneNumber;
        public int actualThumbNumber;
    }

    /*
     * This function searchs in FROM_FOLDER_PATH files of type obj and create a prefab from them
     */
    static void ProcessStonePrefabs()
    {
        Console.WriteLine("PROCESS PREFABS BEGINS ------------------------------------------------------------------");
        string[] files = Directory.GetFiles(STONE_FROM_FOLDER_PATH);
        Config config = FileManager.Load<Config>(CONFIG_FILE);

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Contains(".obj") && !files[i].Contains(".meta"))
            {
                string[] filenameParts = files[i].Split('/');
                string filename = filenameParts[filenameParts.Length - 1].Replace(".obj", "");
                CreateStonePrefab("Stone" + config.actualStoneNumber.ToString(), filename, filename, "texture_1001");
                config.actualStoneNumber++;
                FileManager.Save<Config>(CONFIG_FILE, config);
            }
        }
    }

    static void CreateStonePrefab(string prefabName, string objName, string mtlName = null, string textureName = null)
    {
        Console.WriteLine("Creating {0}", objName);

        if (mtlName == null)
        {
            mtlName = objName;
        }

        if (textureName == null)
        {
            textureName = objName;
        }

        // Import asset
        var relativePath = STONE_FROM_FOLDER_PATH + objName + ".obj";
        GameObject createdObject = AssetDatabase.LoadAssetAtPath(relativePath, typeof(GameObject)) as GameObject;
        createdObject.AddComponent<ContureRendering>();
        Quaternion rt = Quaternion.Euler(-90, 0, 0);
        Vector3 sp = new Vector3(0.0f, 0.0f, 0.0f);
        var instantiatedObject = Instantiate(createdObject, sp, rt);

        // Set the path as within the Assets folder and name it as the GameObject's name with the .Prefab format
        string localPath = STONE_TO_FOLDER_PATH + prefabName + ".prefab";

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab.
        PrefabUtility.SaveAsPrefabAssetAndConnect(instantiatedObject, localPath, InteractionMode.UserAction);
        DestroyImmediate(instantiatedObject);

        // Move the files to a processed folder
        string[] filesNames = { objName + ".obj", mtlName + ".mtl", textureName + ".png" };
        for (int i = 0; i < filesNames.Length; i++)
        {
            MoveFileToProcessed(STONE_FROM_FOLDER_PATH, STONE_PROCESSED_FOLDER_PATH, filesNames[i]);
        }
    }

    static void ProcessThumbs()
    {
        Console.WriteLine("PROCESS THUMBS BEGINS ------------------------------------------------------------------");
        string[] files = Directory.GetFiles(THUMBS_FROM_FOLDER_PATH);
        Config config = FileManager.Load<Config>(CONFIG_FILE);

        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].Contains(".meta"))
            {
                string[] filenameParts = files[i].Split('/');
                string filename = filenameParts[filenameParts.Length - 1];
                MoveFileToProcessed(THUMBS_FROM_FOLDER_PATH, THUMBS_PROCESSED_FOLDER_PATH, filename, filename.Replace("Stone", ""));
                config.actualStoneNumber++;
                FileManager.Save<Config>(CONFIG_FILE, config);
            }
        }
    }

    static void MoveFileToProcessed(string fromFolder, string destinationFolder, string filename, string destinationName = null)
    {
        try
        {
            string path = fromFolder + filename;
            string metaPath = path + ".meta";
            string newPath = destinationFolder + filename;
            if (destinationName == null)
            {
                destinationName = filename;
            }

            if (!File.Exists(path))
            {
                // This statement ensures that the file is created, but the handle is not kept.  
                using FileStream fs = File.Create(path);
            }

            // Ensure that the target does not exist.  
            if (File.Exists(newPath))
            {
                Console.WriteLine("New path is taken.");
            }

            // Move the file.  
            File.Move(path, newPath);
            Console.WriteLine("{0} was moved to {1}.", path, newPath);

            if (File.Exists(metaPath))
            {
                File.Delete(metaPath);
                Console.WriteLine("{0} was deleted.", metaPath);
            }

            // See if the original exists now.  
            if (File.Exists(path))
            {
                Console.WriteLine("The original file still exists, which is unexpected.");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The process failed: {0}", e.ToString());
        }
    }
}