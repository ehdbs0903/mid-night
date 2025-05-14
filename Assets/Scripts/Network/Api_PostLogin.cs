using System;
using System.Collections;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Api_PostLogin
{
    public class Result
    {
        public string code;
        public string message;
    }
    
    public static IEnumerator Send(
        string nickname,
        string password,
        Action<string, string> onComplete
    )
    {
        WWWForm formData = new WWWForm();
        formData.AddField("nickname", nickname);
        formData.AddField("password", password);

        using var webRequest = UnityWebRequest.Post($"{Constants.Url}/api/auth/login", formData);
        yield return webRequest.SendWebRequest();

        string jsonText = webRequest.downloadHandler.text;

        Result result = JsonConvert.DeserializeObject<Result>(jsonText);
        
        onComplete?.Invoke(result.code, result.message);
    }
}