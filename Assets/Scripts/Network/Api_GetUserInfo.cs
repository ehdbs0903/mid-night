using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Api_GetUserInfo
{
    public class UserInfoResponse
    {
        public int AccountId;
        public string Nickname;
        public string Email;
        public string Name;
        
    }

    public static IEnumerator Send(string nickname, string password, string email, string name)
    {
        string url = $"{Constants.Url}/api/account/userinfo";

        using var webRequest = UnityWebRequest.Get(url);
        webRequest.SetRequestHeader("Accept", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            yield break;
        }

        string json = webRequest.downloadHandler.text;
        var userInfo = JsonConvert.DeserializeObject<UserInfoResponse>(json);
    }
}
