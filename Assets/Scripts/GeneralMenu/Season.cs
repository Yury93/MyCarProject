using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Season : MonoBehaviour
{
    public int indexSeason;
    public Button button;
    public Text textPlace;
    public bool isOpen;
    public int GetSavedPlace
    {
        get { return Social1.PlayerPrefs.GetInt("Season" + indexSeason); }
    }
    public void Init()
    {
        button = GetComponent<Button>();
  
        button.onClick.AddListener(LoadScene);
    }

    public void RefreshState()
    {
        if (Social1.PlayerPrefs.HasKey("Season" + indexSeason))
        {
            textPlace.text = GetSavedPlace + " место";
        }

        textPlace.color = isOpen ? Color.green : Color.gray;
        button.interactable = isOpen;
    }

    private void LoadScene()
    {
        Loading.sceneName = "Season" + indexSeason;
        SceneManager.LoadScene("Loading");
    }
}
