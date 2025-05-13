using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Api_GetFile
{
    public static Sprite GetSprite(UnityWebRequest webRequest)
    {
        var texture = DownloadHandlerTexture.GetContent(webRequest);
        var sprite = Sprite.Create(texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        sprite.name = Path.GetFileName(webRequest.url);
        return sprite;
    }
    
    public static IEnumerator Send(string filename,
        Action<Sprite> OnCompleted)
    {
        var webRequest = UnityWebRequest.Get($"{Constants.Url}/get-file/{filename}");
        Debug.Log(webRequest.uri.ToString());
        
        webRequest.SetRequestHeader("Content-Type", "text/plain");
        
        yield return webRequest.SendWebRequest();
        
        Sprite sprite = GetSprite(webRequest);
        OnCompleted.Invoke(sprite);
    }
}