using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_GetUserInfo
{
    public class Result
    {
        public string account_id;
        
        public class Stages
        {
            public string stage_id;
            public bool is_cleared;
            public string name;
            public string created_at;
            public string updated_at;
        }
    }

    public static IEnumerator Send(string message)
    {
        var webRequest = UnityWebRequest.Get($"{Constants.Url}/get-echo-text?message={message}");
        
        webRequest.SetRequestHeader("Content-Type", "text/plain");
        
        yield return webRequest.SendWebRequest();
        
        string jsonText = webRequest.downloadHandler.text;
        Result result = JsonConvert.DeserializeObject<Result>(jsonText);
    }
}