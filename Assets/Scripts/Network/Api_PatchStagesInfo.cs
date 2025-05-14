using System;
using System.Collections;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class Api_PostStage
{
    public class RequestData
    {
        public int user_id;
        public int crop_rank;
        public string crop_type;
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
    

    public static IEnumerator Send(
        int stageId,
        RequestData payload,
        Action<Result> onComplete
    )
    {
        string url = $"{Constants.Url}/api/stages/{stageId}";
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
            onComplete?.Invoke(null);
            yield break;
        }

        string jsonResponse = webRequest.downloadHandler.text;
        if (string.IsNullOrEmpty(jsonResponse))
        {
            onComplete?.Invoke(null);
            yield break;
        }

        Result result = JsonConvert.DeserializeObject<Result>(jsonResponse);
        onComplete?.Invoke(result);
    }
}