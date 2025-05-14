using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class Api_GetStagesInfo
{
    public class StageInfo
    {
        public int stage_id;
        public bool is_cleared;
        public CropRankInfo crop_rank;
    }

    public class CropRankInfo
    {
        public int crop_id;
        public string rank;
        public string type;
    }

    public static IEnumerator Send(int accountId)
    {
        string url = $"{Constants.Url}/api/account/{accountId}/stages";
        using var webRequest = UnityWebRequest.Get(url);
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

        StageInfo stage = JsonConvert.DeserializeObject<StageInfo>(jsonResponse);
    }
}
