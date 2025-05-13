using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_GetJson
{
    public class Result
    {
        public class Data
        {
            public int id;
            public string name;
            public string[] tags;
        }
    
        public Data data;
        public string error;
        public string message;
        public bool status;
    }
    
    public static IEnumerator Send()
    {
        var webRequest = UnityWebRequest.Get($"{Constants.Url}/get-json");
        Debug.Log(webRequest.uri.ToString());
        
        webRequest.SetRequestHeader("Content-Type", "text/plain");
        
        yield return webRequest.SendWebRequest();
        
        Debug.Log(webRequest.downloadHandler.text);

        string jsonText = webRequest.downloadHandler.text;
        Result result = JsonConvert.DeserializeObject<Result>(jsonText);
        Debug.Log($"{result.data.id} / {result.data.name} / {result.data.tags}");
    }
}