using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkibidy : MonoBehaviour
{
    [SerializeField] private GameObject garageButton;
    [SerializeField] private Button advSkibidiButton;
    private void Start()
    {
        advSkibidiButton.onClick.AddListener(ShowAdvReward);
        Yandex.instance.OnShowAdvReward += OnShowAdvReward;
        Yandex.instance.onAddReward += OnAddReward;
    }

    private void OnAddReward()
    {
        carSelection.SetCarIndex(3);
        garageButton.SetActive(false);
        advSkibidiButton.gameObject.SetActive(false);
    }

    private void OnShowAdvReward(bool obj)
    {
        SoundSystem.instance.OnShowFullScreenADV(obj);
       
    }

    private void ShowAdvReward()
    {
        Yandex.instance.ShowAdvButton();
    }
}
