#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;

public class AddressableCreater : EditorWindow
{

    [MenuItem("addressable/AddressableCreater")]
    private static void ShowWindow()
    {
        var window = GetWindow<AddressableCreater>();
        window.titleContent = new GUIContent("AddressableCreater");
        window.Show();
    }
    private Object dir;
    
    private void OnGUI()
    {
        dir = EditorGUILayout.ObjectField("DIR", dir, typeof(Object), true) as Object;
        if(dir == null){
            return;
        }
        // Addressables.UpdateCatalogs

    }
}
#endif