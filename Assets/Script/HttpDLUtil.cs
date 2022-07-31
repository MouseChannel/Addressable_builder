using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class HttpDLUtil{

    public static float progress { get; private set; }
    public static int count = 0;
    public static string patchURL = "https://a20.gph.netease.com/";
    public static string patchListURL = "http://listsvr.x.netease.com:6677/patch_list/a20/test_patch_list";

     
    public static bool HttpDownLoad(string url, string savePath, string fileName, string patchMD5, Action<float> progressAction)
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        progress = 0.0f;
        bool isDone = false;

        string filePath = savePath + fileName;

        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
        long fileLength = fs.Length;
        long totalLength = GetLength(url);

        while (fileLength < totalLength)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            fs.Seek(fileLength, SeekOrigin.Begin);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            
            request.AddRange((int)fileLength);
            Stream stream = request.GetResponse().GetResponseStream();

            byte[] buffer = new byte[1024];
            int length = stream.Read(buffer, 0, buffer.Length);
            while (length > 0)
            {
                fs.Write(buffer, 0, length);
                fileLength += length;
                progress = (float)fileLength / (float)totalLength;
                progressAction?.Invoke(progress);
                length = stream.Read(buffer, 0, buffer.Length);
            }
            stream.Close();
            stream.Dispose();
        }

        UnityEngine.Debug.Log("the progress" + progress);

        progress = 1;

        fs.Close();
        fs.Dispose();

        if (progress == 1)
        {
           isDone = true;
        }

        return isDone;
    }

    static long GetLength(string url)
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
        requet.Method = "HEAD";
        HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
        return response.ContentLength;
    }

    public static string HttpContent(string url)
    {
        string content = null;
        try
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                content = sr.ReadToEnd();
            }
            response.Dispose();
        }
        catch (Exception)
        {

        }

        return content;
    }

    public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }
}
