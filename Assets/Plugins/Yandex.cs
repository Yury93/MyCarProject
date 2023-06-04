using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Yandex : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI userName;
    //[SerializeField] RawImage userPhoto;
    public Button helpButton;
    public Button AdvButton;
    [DllImport("__Internal")]
    private static extern void Hello();
    [DllImport("__Internal")]
    private static extern void GiveMeUserInfo();


    [DllImport("__Internal")]
    private static extern void AdvByRewards();

    [DllImport("__Internal")]
    private static extern void ShowAdv();
    //public Action OnCloseAdv;

    //[DllImport("__Internal")]
    //private static extern void RateGame();
    public static Yandex instance;
    public Action<bool> OnShowAdvReward;
    public void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.3f);
        HelloButton();
    }

    //нажимаю на кнопку
    public void ShowAdvButton()
    {
        AdvByRewards();
        
    }
    //добавил награду
    public void AddReward()
    {
        helpButton.gameObject.SetActive(true);
        AdvButton.gameObject.SetActive(false);

    }
    //видео с рекламой закрывается
    public void CloseAdvReward()
    {
        OnShowAdvReward?.Invoke(false);
        
    }
    public void StartVideoAdvReward()
    {
        OnShowAdvReward?.Invoke(true);
        
    }
    //public void SetName(string name)
    //{
    //    if (name == "") name = "Player 4321";
    //    userName.text = name;
    //    Debug.Log("получено имя пользователя");
    //}
    //public void SetPhoto(string url)
    //{
    //    StartCoroutine(DownloadImage(url));
    //    Debug.Log("получено фото пользователя");
    //}

    //IEnumerator DownloadImage(string mediaUrl)
    //{
    //    UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
    //    yield return request.SendWebRequest();
    //    if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
    //        Debug.Log(request.error);
    //    else
    //        userPhoto.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

    //    yield return new WaitForEndOfFrame();

    //}

    //public void CloseAdvBetweenScenes()
    //{
    //    OnCloseAdv?.Invoke();
    //}
    public  void ShowAdvBetweenScenes()
    {
        ShowAdv();
    }

    // Update is called once per frame
    public void HelloButton()
    {
        //Hello();//приветствие
        //GiveMeUserInfo();//авторизация
    }



}
