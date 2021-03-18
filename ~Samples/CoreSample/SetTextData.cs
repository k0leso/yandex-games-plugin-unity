using UnityEngine;
using UnityEngine.UI;

public class SetTextData : MonoBehaviour {
    private YandexSDK yandexSdk;
    public Text idValue;
    public Text nameValue;

    private void Start() {
        yandexSdk = YandexSDK.instance;
        yandexSdk.onUserDataReceived += SetText;
    }

    void SetText() {
        idValue.text = yandexSdk.user.id;
        nameValue.text = yandexSdk.user.name;
    }
}