using System.Collections;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class Api_PatchStage
{
    public class RequestData
    {
        public bool is_cleared;
    }
    
    public class Data
    {
    }
    
    public class Result
    {
        public bool is_success;
        [CanBeNull] public Data data;
        public string code;
        public string message;
    }
    

    public static IEnumerator Send(int stageId, bool isCleared)
    {
        string url = $"{Constants.Url}/api/stages/{stageId}";

        var payload = new RequestData { is_cleared = isCleared };
        string jsonPayload = JsonConvert.SerializeObject(payload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        using var webRequest = new UnityWebRequest(url, "PATCH")
        {
            uploadHandler   = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };

        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Accept", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            yield break;
        }

        string jsonResponse = webRequest.downloadHandler.text;
        if (string.IsNullOrEmpty(jsonResponse))
        {
            yield break;
        }

        Result result = JsonConvert.DeserializeObject<Result>(jsonResponse);
    }
}