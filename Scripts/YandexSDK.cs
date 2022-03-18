using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexSDK : MonoBehaviour {

    #region Parameters
    public static YandexSDK instance;
    public bool isInited = false;
    public string gameSave = null;
    public UserData user;
    public string envData;

    public Queue<int> rewardedAdPlacementsAsInt = new Queue<int>();
    public Queue<string> rewardedAdsPlacements = new Queue<string>();

    private bool rewardAdOpened = false;
    #endregion

    #region Internal
    [DllImport("__Internal")]
    private static extern void Internal_GetUserData();
    [DllImport("__Internal")]
    private static extern void Internal_SetToClipboard(string text);
    [DllImport("__Internal")]
    private static extern void Internal_ShowFullscreenAd();
    /// <summary>
    /// Returns an int value which is sent to index.html
    /// </summary>
    /// <param name="placement"></param>
    /// <returns></returns>
    [DllImport("__Internal")]
    private static extern int Internal_ShowRewardedAd(string placement);
    [DllImport("__Internal")]
    private static extern void Internal_GetEnvironment();
    [DllImport("__Internal")]
    private static extern void Internal_AuthenticateUser();
    [DllImport("__Internal")]
    private static extern void Internal_GetGameData();
    [DllImport("__Internal")]
    private static extern void Internal_SetGameData(string _data);
    [DllImport("__Internal")]
    private static extern void Internal_InitPurchases();
    [DllImport("__Internal")]
    private static extern void Internal_Purchase(string id);
    [DllImport("__Internal")]
    private static extern void Internal_OpenWindow(string link);
    #endregion

    #region Actions
    /// <summary>
    /// Библиотека была инициализирвоана
    /// </summary>
    public event Action action_initSdkCompleted;
    /// <summary>
    /// Получены данные игрока
    /// </summary>
    public event Action action_OnUserDataReceived;
    /// <summary>
    /// Получены данные игры
    /// </summary>
    public event Action action_OnGameDataReceived;
    /// <summary>
    /// Полученны данные окружения
    /// </summary>
    public event Action action_OnEnvironmentReceived;
    /// <summary>
    /// Пользователь открыл полноэкранную рекламму
    /// </summary>
    public event Action action_OnInterstitialShown;
    /// <summary>
    /// Ошибка при попытке посмотреть полноэкранную рекламу
    /// </summary>
    public event Action<string> action_OnInterstitialFailed;
    /// <summary>
    /// Пользователь открыл рекламу
    /// </summary>
    public event Action<int> action_OnRewardedAdOpened;
    /// <summary>
    /// Пользователь должен получить награду за просмотр рекламы
    /// </summary>
    public event Action<string> action_OnRewardedAdReward;
    /// <summary>
    /// Пользователь закрыл рекламу
    /// </summary>
    public event Action<int> action_OnRewardedAdClosed;
    /// <summary>
    /// Вызов/просмотр рекламы повлёк за собой ошибку
    /// </summary>
    public event Action<string> action_OnRewardedAdError;
    /// <summary>
    /// Покупка успешно совершена
    /// </summary>
    public event Action<string> action_OnPurchaseSuccess;
    /// <summary>
    /// Покупка не удалась: в консоли разработчика не добавлен товар с таким id,
    /// пользователь не авторизовался, передумал и закрыл окно оплаты,
    /// истекло отведенное на покупку время, не хватило денег и т. д.
    /// </summary>
    public event Action<string> action_OnPurchaseFailed;

    public event Action action_OnClose;
    #endregion

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    #region Methods
    public void GetEnvironment()
    {
        Internal_GetEnvironment();
    }

    public void CopyToClipboard(string text)
    {
        Internal_SetToClipboard(text);
    }

    /// <summary>
    /// Call this to ask user to authenticate
    /// </summary>
    public void Authenticate() {
        Internal_AuthenticateUser();
    }

    /// <summary>
    /// Call this to show interstitial ad. Don't call frequently. There is a 3 minute delay after each show.
    /// </summary>
    public void ShowInterstitial() {
        Internal_ShowFullscreenAd();
    }

    /// <summary>
    /// Call this to show rewarded ad
    /// </summary>
    /// <param name="placement"></param>
    public void ShowRewarded(string placement) {
        rewardAdOpened = false;
        rewardedAdPlacementsAsInt.Enqueue(Internal_ShowRewardedAd(placement));
        rewardedAdsPlacements.Enqueue(placement);
    }
    
    /// <summary>
    /// Call this to receive user data
    /// </summary>
    public void GetUserData() {
        Internal_GetUserData();
    }
    
    public void InitializePurchases() {
        Internal_InitPurchases();
    }

    public void ProcessPurchase(string id) {
        Internal_Purchase(id);
    }
    public void GetGameData()
    {
        Internal_GetGameData();
    }
	
    public void SetGameData(string save)
    {
        Internal_SetGameData(save);
    }
    #endregion

    #region Callbacks
    /// <summary>
    /// Callback from index.html
    /// </summary>
    public void Callback_InitYaSDK() {
        isInited = true;
        action_initSdkCompleted();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="data"></param>
    public void Callback_Environment(string data) {
        envData = JsonUtility.FromJson<string>(data);
        action_OnEnvironmentReceived();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="data"></param>
    public void Callback_StoreUserData(string data) {
        user = JsonUtility.FromJson<UserData>(data);
        action_OnUserDataReceived();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="data"></param>
    public void Callback_GameUserData(string data)
    {
        gameSave = JsonUtility.FromJson<string>(data);
        action_OnGameDataReceived();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    public void Callback_OnInterstitialShown() {
        action_OnInterstitialShown();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="error"></param>
    public void Callback_OnInterstitialError(string error) {
        action_OnInterstitialFailed(error);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="placement"></param>
    public void Callback_OnRewardedOpen(int placement) {
        action_OnRewardedAdOpened(placement);
        rewardAdOpened = true;
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="placement"></param>
    public void Callback_OnRewarded(int placement) {
        if (rewardAdOpened)
        {
            if (placement == rewardedAdPlacementsAsInt.Dequeue())
            {
                action_OnRewardedAdReward.Invoke(rewardedAdsPlacements.Dequeue());
                rewardAdOpened = false;
            }
        }
        else
        {
            action_OnRewardedAdError("error_no_open_rewarded");
            rewardedAdsPlacements.Clear();
            rewardedAdPlacementsAsInt.Clear();
        }
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="placement"></param>
    public void Callback_OnRewardedClose(int placement) {
        action_OnRewardedAdClosed(placement);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="placement"></param>
    public void Callback_OnRewardedError(string placement) {
        action_OnRewardedAdError(placement);
        rewardedAdsPlacements.Clear();
        rewardedAdPlacementsAsInt.Clear();
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="id"></param>
    public void Callback_OnPurchaseSuccess(string id) {
        action_OnPurchaseSuccess(id);
    }

    /// <summary>
    /// Callback from index.html
    /// </summary>
    /// <param name="error"></param>
    public void Callback_OnPurchaseFailed(string error) {
        action_OnPurchaseFailed(error);
    }
    
    /// <summary>
    /// Browser tab has been closed
    /// </summary>
    /// <param name="error"></param>
    public void Callback_OnClose() {
        action_OnClose.Invoke();
    }
    #endregion
}

public struct UserData {
    public string id;
    public string name;
    public string avatarUrlSmall;
    public string avatarUrlMedium;
    public string avatarUrlLarge;
}