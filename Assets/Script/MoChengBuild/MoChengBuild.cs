using System.IO.Compression;
using UnityEngine;
using System.IO;



#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
#endif

using MoChengBuilder;
public class MoChengBuild : EditorWindow
{

    [MenuItem("MochengBuild/MoChengBuild")]
    private static void ShowWindow()
    {
        var window = GetWindow<MoChengBuild>();
        window.titleContent = new GUIContent("MoChengBuild");
        window.Show();
    }
    private GUIStyle bigLabel = new GUIStyle();
    private GUIStyle midLabel = new GUIStyle();
    private GUIStyle rightLabel = new GUIStyle();

    Object BundleDir;

    string pathText;
    string path;

    string buildPath = string.Empty;
    string loadPath = string.Empty;

    private void OnGUI()
    {
        bigLabel.fontSize = 15;
        midLabel.alignment = TextAnchor.UpperCenter;
        rightLabel.alignment = TextAnchor.UpperRight;

        // var sss = MoChengBuildUtil.profileSettings.GetVariableNames();
        // foreach(var i in sss){
        //      GUILayout.Label(i);
        // }


        GUILayout.Label($"当前path： ");

        foreach (var i in MoChengBuildUtil.GetAllPath())
        {
            bool remove = false;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(i.Key, bigLabel);
                if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                    remove = true;
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(i.Value);
            }
            if (remove)
            {
                MoChengBuildUtil.RemovePath(i.Value);
                break;
            }
        }
        GUILayout.Label("-------------------------------------------------------------------------");
        using (new EditorGUILayout.HorizontalScope())
        {
            using (new EditorGUILayout.VerticalScope())
            {
                GUILayout.Label($"add path ", midLabel);
                GUILayout.Label("___PathText");
                pathText = GUILayout.TextArea(pathText);
                GUILayout.Label("___Path");
                path = GUILayout.TextArea(path);

                if (GUILayout.Button("Add", GUILayout.MaxWidth(100)))
                {
                    if (pathText == "") return;
                    MoChengBuildUtil.AddPath(pathText, path);
                    path = pathText = "";

                }



            }

        }
        GUILayout.Label("-------------------------------------------------------------------------");


        using (new EditorGUILayout.HorizontalScope())
        {
            BundleDir = EditorGUILayout.ObjectField("BundleDir", BundleDir, typeof(Object), true) as Object;
            if (BundleDir == null) return;
        }

        EditorGUILayout.HelpBox("Select the new " + "logsFeatureName" + " Folder", MessageType.Info, true);
        using (new EditorGUILayout.HorizontalScope())
        {

            using (new EditorGUILayout.VerticalScope())
            {
                GUILayout.Label($"BuildPath: ", bigLabel);

                GUILayout.Label($"{buildPath}");
                foreach (var i in MoChengBuildUtil.GetAllPath())
                {
                    if (i.Value == buildPath) continue;
                    if (GUILayout.Button($"{i.Key}", GUILayout.Width(100)))
                    {
                        buildPath = i.Value;
                        break;
                    }
                }



            }

            using (new EditorGUILayout.VerticalScope(rightLabel))
            {
                GUILayout.Label($"LoadPath: ", bigLabel);

                GUILayout.Label($"{loadPath}");
                foreach (var i in MoChengBuildUtil.GetAllPath())
                {
                    if (i.Value == loadPath) continue;
                    if (GUILayout.Button($"{i.Key}", GUILayout.Width(100)))
                    {
                        loadPath = i.Value;
                        break;
                    }
                }

            }

        }
        GUILayout.Label("------------------------------------------------------------------------------------------");

        if (GUILayout.Button("Set", GUILayout.Width(100)))
        {



            // MoChengBuildUtil.InitProfileValue();
            string dirPath = AssetDatabase.GetAssetPath(BundleDir);

            DirectoryInfo direction = new DirectoryInfo(dirPath);
            #region  文件夹


            DirectoryInfo[] directions = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (var dir in directions)
            {
                Debug.Log(dir.FullName);
                string groupName = string.Empty;
                string assetName = string.Empty;

                if (Directory.Exists(dir.FullName))
                {
                    //是文件夹
                    groupName = Path.GetFileName(dir.FullName);
                    assetName = groupName;




                }
                // else if (File.Exists(dir.FullName))
                // {
                //     groupName = Path.GetFileName(dir.FullName).Split('.')[0];
                //     assetName = Path.GetFileName(dir.FullName);

                // }


                if (groupName != string.Empty && buildPath != string.Empty && loadPath != string.Empty)
                {
                    var newGroup = MoChengBuildUtil.CreateGroup(groupName);
                    MoChengBuildUtil.SetGroupData(newGroup, buildPath.GetText(), loadPath.GetText());
                    Debug.Log(AssetDatabase.GetAssetPath(BundleDir) + $"/{assetName}");
                    Object assetDir = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(BundleDir) + $"/{assetName}");
                    MoChengBuildUtil.AddAsset(newGroup.Name, assetDir);
                }
                else
                {

                }


            }
            #endregion

            #region  文件



            FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
            foreach (var dir in files)
            {
                Debug.Log(dir.FullName);
                string groupName = string.Empty;
                string assetName = string.Empty;

                if (File.Exists(dir.FullName))
                {
                    groupName = Path.GetFileName(dir.FullName).Split('.')[0];
                    assetName = Path.GetFileName(dir.FullName);

                }
                if (Path.GetExtension(assetName) == ".meta") continue;


                if (groupName != string.Empty && buildPath != string.Empty && loadPath != string.Empty)
                {
                    var newGroup = MoChengBuildUtil.CreateGroup(groupName);
                    MoChengBuildUtil.SetGroupData(newGroup, buildPath.GetText(), loadPath.GetText());
                    Debug.Log(AssetDatabase.GetAssetPath(BundleDir) + $"/{assetName}");
                    Object assetDir = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(BundleDir) + $"/{assetName}");
                    MoChengBuildUtil.AddAsset(newGroup.Name, assetDir);
                }
                else
                {

                }


            }
            #endregion


        }
        if (GUILayout.Button("Build", GUILayout.Width(100)))
        {


            AddressableAssetSettings.BuildPlayerContent();

        }




    }
}
 