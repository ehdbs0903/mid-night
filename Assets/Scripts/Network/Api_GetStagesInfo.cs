using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_UpdateStage
{
    private class PatchRequest
    {
        public bool IsCleared;
    }

    public class StageInfo
    {
        public int StageId;
        public bool IsCleared;
        public CropRankInfo CropRank;
    }

    public class CropRankInfo
    {
        public int CropId;
        public string Rank;
        public string Type;
    }

    public static IEnumerator Send(int stageId, bool isCleared)
    {
        string url = $"{Constants.Url}/api/stages/{stageId}";

        var patchData = new PatchRequest { IsCleared = isCleared };
        string jsonPayload = JsonConvert.SerializeObject(patchData);

        using var webRequest = new UnityWebRequest(url, "PATCH")
        {
            uploadHandler   = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonPayload)),
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

        var updatedStage = JsonConvert.DeserializeObject<StageInfo>(jsonResponse);
    }
}
