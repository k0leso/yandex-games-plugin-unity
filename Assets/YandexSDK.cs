using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexSDK : MonoBehaviour {
    public static YandexSDK instance;
    [DllImport("__Internal")]
    private static extern void GetUserData();
    [DllImport("__Internal")]
    private static extern void ShowFullscreenAd();
    [DllImport("__Internal")]
    private static extern void ShowRewardedAd(string placement);
    [DllImport("__Internal")]
    private static extern void AuthUser();

    public UserData user;
    public event Action onUserDataReceived;
    
    /// <summary>
    /// Пользователь открыл рекламу
    /// </summary>
    public event Action<string> onRewardedAdOpened;
    /// <summary>
    /// Пользователь должен получить награду за просмотр рекламы
    /// </summary>
    public event Action<string> onRewardedAdReward;
    /// <summary>
    /// Пользователь закрыл рекламу
    /// </summary>
    public event Action<string> onRewardedAdClosed;
    /// <summary>
    /// Вызов/просмотр рекламы повлёк за собой ошибку
    /// </summary>
    public event Action<string> onRewardedAdError;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Authenticate() {
        AuthUser();
    }

    public void ShowInterstitial() {
        ShowFullscreenAd();
    }

    public void ShowRewarded(string placement) {
        ShowRewardedAd(placement);
    }

    public void RequestUserData() {
        GetUserData();
    }
    
    public void StoreUserData(string data) {
        user = JsonUtility.FromJson<UserData>(data);
        onUserDataReceived();
    }

    public void OnRewardedOpen(string placement) {
        onRewardedAdOpened(placement);
    }

    public void OnRewarded(string placement) {
        onRewardedAdReward(placement);
    }

    public void OnRewardedClose(string placement) {
        onRewardedAdClosed(placement);
    }

    public void OnRewardedError(string placement) {
        onRewardedAdError(placement);
    }
}

public struct UserData {
    public string id;
    public string name;
    public string avatarUrlSmall;
    public string avatarUrlMedium;
    public string avatarUrlLarge;
}
