#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;

public class Helper : EditorWindow
{

    [MenuItem("addressable/helper")]
    private static void ShowWindow()
    {
        var window = GetWindow<Helper>();
        window.titleContent = new GUIContent("helper");
        window.Show();
    }
    private Object dir;

    private void OnGUI()
    {
        // dir = EditorGUILayout.ObjectField("DIR", dir, typeof(Object), true) as Object;
        // if(dir == null){
        //     return;
        // }
        using (new EditorGUILayout.HorizontalScope())
        {

            if (GUILayout.Button("Set", GUILayout.Width(100)))
            {
                var temp = new TempObject();
                AssetDatabase.CreateAsset(temp, "Assets/temp.asset");
                for (int i = 0; i < 30; i++)
                {
                    Texture2D a = new Texture2D(2000+i, 2000+i, TextureFormat.RGBA32, 10, false);


                    AssetDatabase.AddObjectToAsset(a, temp);
                }



                AssetDatabase.SaveAssets();

            }
        }
        // Addressables.UpdateCatalogs

    }
}
#endif