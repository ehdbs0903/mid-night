using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_PostLogin
{
    public class Result
    {
        // public class Data
        // {
        //     public int age;
        //     public string name;
        // }
        //
        // public Data data;
        public string error;
        public string message;
        // public bool status;
    }
    
    public static IEnumerator Send(string nickname, string password)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("nickname", nickname);
        formData.AddField("password", password);
        
        var webRequest = UnityWebRequest.Post($"{Constants.Url}/login", formData);
        yield return webRequest.SendWebRequest();
        
        Debug.Log(webRequest.downloadHandler.text);

        string jsonText = webRequest.downloadHandler.text;
        Result result = JsonConvert.DeserializeObject<Result>(jsonText);
        Debug.Log($"{result.error} / {result.message}");
        
        // WWWForm formData = new WWWForm();
        // formData.AddField("nickname", name);
        // formData.AddField("password",  age.ToString());
        //
        // var webRequest = UnityWebRequest.Post($"{Constants.Url}/post-form", formData);
        // yield return webRequest.SendWebRequest();
        //
        // Debug.Log(webRequest.downloadHandler.text);
        //
        // string jsonText = webRequest.downloadHandler.text;
        // Result result = JsonConvert.DeserializeObject<Result>(jsonText);
        // Debug.Log($"{result.data.age} / {result.data.name}");
    }
}