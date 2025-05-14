using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_PostSignUp
{
    public class Data { }

    public class Result
    {
        public bool is_success;
        public Data data;
        public string code;
        public string message;
    }

    public class SignUpRequest
    {
        public string nickname;
        public string password;
        public string passwordConfirm;
        public string email;
        public string name;
    }

    public static IEnumerator Send(
        string nickname,
        string password,
        string passwordConfirm,
        string email,
        string name,
        Action<string> onComplete
    )
    {
        var payload = new SignUpRequest
        {
            nickname        = nickname,
            password        = password,
            passwordConfirm = passwordConfirm,
            email           = email,
            name            = name
        };
        string json = JsonConvert.SerializeObject(payload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using var webRequest = new UnityWebRequest($"{Constants.Url}/api/account/signup", "POST")
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
            onComplete?.Invoke(result.code);
        }
        else
        {
            onComplete?.Invoke(null);
        }
    }
}
