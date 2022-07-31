using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;

#if UNITY_EDITOR
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
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

    AddressableAssetSettings dir;
    Object sss;
    AddressableAssetGroup group;

    private void OnGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            dir = EditorGUILayout.ObjectField("DIR", dir, typeof(AddressableAssetSettings), true) as AddressableAssetSettings;
            // temp = EditorGUILayout.ObjectField("temp", temp, typeof(GameObject), true) as GameObject;
            // if (dir == null) return;
        }
        using (new EditorGUILayout.HorizontalScope())
        {
            sss = EditorGUILayout.ObjectField("sss", sss, typeof(Object), true) as Object;
            // temp = EditorGUILayout.ObjectField("temp", temp, typeof(GameObject), true) as GameObject;
            // if (dir == null) return;
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            group = EditorGUILayout.ObjectField("group", group, typeof(AddressableAssetGroup), true) as AddressableAssetGroup;
            // temp = EditorGUILayout.ObjectField("temp", temp, typeof(GameObject), true) as GameObject;
            // if (dir == null) return;
        }


        if (GUILayout.Button("Set", GUILayout.Width(100)))
        {
            // MoChengBuildUtil.RemoveAllProfileValue();

            // AddressableAssetSettings.BuildPlayerContent();
            // Debug.LogError(AddressableAssetSettingsDefaultObject.Settings == dir);
            MoChengBuildUtil.InitProfileValue();
            MoChengBuildUtil.CreateGroup("mochengHello");
            MoChengBuildUtil.AddAsset("mochengHello", sss);
        }
    }
}
#endif