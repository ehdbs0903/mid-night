using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_PostSignUp
{
    public class Result
    {
        public string code;
        public string message;
    }
    
    public static IEnumerator Send(
        string nickname, string password, string email, string name,
        Action<string> onComplete
    )
    {
        WWWForm formData = new WWWForm();
        formData.AddField("nickname", nickname);
        formData.AddField("password", password);
        formData.AddField("email", email);
        formData.AddField("name", name);

        using var webRequest = UnityWebRequest.Post($"{Constants.Url}/api/account/signup", formData);
        yield return webRequest.SendWebRequest();

        string jsonText = webRequest.downloadHandler.text;
        Result result = JsonConvert.DeserializeObject<Result>(jsonText);

        onComplete?.Invoke(result.code);
    }
}