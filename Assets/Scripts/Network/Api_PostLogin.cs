using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_PostLogin
{
    public class Data { }

    public class Result
    {
        public bool is_success;
        public Data data;
        public string code;
        public string message;
    }

    public class LoginRequest
    {
        public string username;
        public string password;
    }

    public static IEnumerator Send(
        string username,
        string password,
        Action<string, string> onComplete
    )
    {
        var payload = new LoginRequest
        {
            username = username,
            password = password
        };
        string json = JsonConvert.SerializeObject(payload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using var webRequest = new UnityWebRequest($"{Constants.Url}/api/auth/login", "POST")
        {
            uploadHandler   = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };

        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonText = webRequest.downloadHandler.text;
            var result = JsonConvert.DeserializeObject<Result>(jsonText);
            onComplete?.Invoke(result.code, result.message);
        }
        else
        {
            onComplete?.Invoke(null, $"Request Error: {webRequest.error}");
        }
    }
}