using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetImage : MonoBehaviour {
    private YandexSDK yandexSdk;
    private Image profileImage;
    private void Start() {
        profileImage = GetComponent<Image>();
        yandexSdk = YandexSDK.instance;
        yandexSdk.onUserDataReceived += LoadImage;
    }

    private void LoadImage()
    {
        StartCoroutine(LoadTexture(yandexSdk.user.avatarUrlMedium, SetTexture));
    }
    
    private void SetTexture(Texture2D texture)
    {
        var rect = new Rect(0, 0, texture.width, texture.height);
        var pivot = Vector2.one * 0.5f;
        var sprite = Sprite.Create(texture, rect, pivot);

        profileImage.sprite = sprite;
    }

    private IEnumerator LoadTexture(string uri, Action<Texture2D> response)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            response?.Invoke(DownloadHandlerTexture.GetContent(www));
        }

        www.Dispose();
    }
}
