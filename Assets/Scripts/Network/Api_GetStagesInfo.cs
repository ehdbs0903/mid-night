using System.Collections;
using System.Text;
using Newtonsoft.Json;
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
        public int Rank;
        public string ImageUrl;
    }
    
    public static IEnumerator Send(int stageId, bool isCleared)
    {
        string url = $"{Constants.Url}/stages/{stageId}";

        var patchData = new PatchRequest { IsCleared = isCleared };
        string jsonPayload = JsonConvert.SerializeObject(patchData);

        var webRequest = new UnityWebRequest(url, "PATCH")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonPayload)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            yield break;
        }

        string jsonResponse = webRequest.downloadHandler.text;
        StageInfo updatedStage = JsonConvert.DeserializeObject<StageInfo>(jsonResponse);
    }
}
