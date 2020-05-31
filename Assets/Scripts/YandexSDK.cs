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
    private static extern void AuthenticateUser();
    [DllImport("__Internal")]
    private static extern void InitPurchases();
    [DllImport("__Internal")]
    private static extern void Purchase(string id);

    public UserData user;
    public event Action onUserDataReceived;

    public event Action onInterstitialShown;
    public event Action<string> onInterstitialFailed;
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
    /// <summary>
    /// Покупка успешно совершена
    /// </summary>
    public event Action<string> onPurchaseSuccess;
    /// <summary>
    /// Покупка не удалась: в консоли разработчика не добавлен товар с таким id,
    /// пользователь не авторизовался, передумал и закрыл окно оплаты,
    /// истекло отведенное на покупку время, не хватило денег и т. д.
    /// </summary>
    public event Action<string> onPurchaseFailed;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Authenticate() {
        AuthenticateUser();
    }

    /// <summary>
    /// Не вызывайте рекламу чаще, чем раз в три минуты
    /// </summary>
    public void ShowInterstitial() {
        ShowFullscreenAd();
    }

    public void ShowRewarded(string placement) {
        ShowRewardedAd(placement);
    }

    public void RequestUserData() {
        GetUserData();
    }

    public void InitializePurchases() {
        InitPurchases();
    }

    public void ProcessPurchase(string id) {
        Purchase(id);
    }
    
    public void StoreUserData(string data) {
        user = JsonUtility.FromJson<UserData>(data);
        onUserDataReceived();
    }

    public void OnInterstitialShown() {
        onInterstitialShown();
    }

    public void OnInterstitialError(string error) {
        onInterstitialFailed(error);
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

    public void OnPurchaseSuccess(string id) {
        onPurchaseSuccess(id);
    }

    public void OnPurchaseFailed(string error) {
        onPurchaseFailed(error);
    }
}

public struct UserData {
    public string id;
    public string name;
    public string avatarUrlSmall;
    public string avatarUrlMedium;
    public string avatarUrlLarge;
}
