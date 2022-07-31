using System.Security.AccessControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Profiling;

public class AddressableHelper : MonoBehaviour
{
    AsyncOperationHandle<GameObject> hh;
    [SerializeField]
    Image a;
    public TMP_Text text;
    bool hasStart = false;
    void Start()
    {
        Addressables.InitializeAsync();

        Debug.Log(Addressables.BuildPath);
        Debug.Log(Addressables.RuntimePath);
        Debug.Log(Application.dataPath);
        Debug.Log(Application.streamingAssetsPath);


        text.text = text.text + Addressables.BuildPath + "\n" +
         Addressables.RuntimePath + "\n" +
         Application.dataPath + "\n" +
         Application.streamingAssetsPath + "\n";



        // var t = Addressables.LoadAssetAsync<TempObject>("addressFolder/temp.asset");
        // t.Completed += LoadComplete;
        // StartCoroutine(Wait(t));

        // HttpDLUtil.HttpDownLoad("https://bz11-static.meta.youdao.com/mocheng/zip/res1.zip",Addressables.RuntimePath+ "/","mocheng.zip","",(a) =>{});
        Debug.Log(1230);
        // AddressablesPlayerBuildProcessor.BuildAddressablesOverride = ()=>{

        // };
        Addressables.InstantiateAsync ("Assets/addressFolder/Cube123.prefab");


        // var h = Addressables.GetDownloadSizeAsync("addressFolder/temp.asset");
        // h.Completed += Com;

    }

    private void QAZ(AsyncOperationHandle<GameObject> obj)
    {
        Debug.Log(obj.Status);
             Debug.Log(DateTime.Now.Millisecond);
               Debug.Log(DateTime.Now.Second);
    }

    IEnumerator Wait(AsyncOperationHandle<TempObject> t)
    {
        while (!t.IsDone)
        {

            yield return 0;
            Debug.Log(t.PercentComplete);
        }
    }

    private void LoadComplete(AsyncOperationHandle<TempObject> obj)
    {
        Debug.Log(obj.Status);
        Debug.Log(obj.Result.a.name);
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Sprite sprite = Sprite.Create(obj.Result.a, new Rect(0, 0, 640, 640), Vector2.zero);
            a.sprite = sprite;
        }

        // GUI.DrawTexture(new Rect(0,0,1000,1000),);

    }

    private void Com(AsyncOperationHandle<long> obj)
    {
        Debug.Log(obj.Status);
        Debug.Log(obj.Result);
    }
    private void CCom(AsyncOperationHandle obj)
    {
        Debug.Log(obj.Status);
        TempObject t = obj.Result as TempObject;
        if (t != null)
        {

            Debug.Log(t.a.name);
            Debug.Log(t.a.width);
            Debug.Log(t.b.name);
            Debug.Log(t.b.width);
        }
        Debug.Log(obj.Result);
    }

    private void ManuAddressable()
    {
        //  AssetDatabase.GetAssetPath()
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStart && !hh.IsDone)
        {
            Debug.Log(hh.PercentComplete);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
       
        // Debug.Log(t);
    }

    private void Spawn()
    {
        hh = Addressables.InstantiateAsync("Assets/addressFolder/Sprite1.prefab");
        Debug.Log(DateTime.Now.Millisecond);
        Debug.Log(DateTime.Now.Second);
        hh.Completed += QAZ;
        hasStart = true;
    }
}
