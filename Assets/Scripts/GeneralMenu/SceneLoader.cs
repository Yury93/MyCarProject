using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Serializable]
    public class ButtonLoader
    {
    
        public string sceneName;
        public Button button;
        public void Init() {
            button.onClick.AddListener(() =>
            {
                carSelection.isMain = true;
                Loading.sceneName = sceneName;
                SceneManager.LoadSceneAsync("Loading");
                });
        }  
    }
    [SerializeField] private List<ButtonLoader> buttonLoaders;
    [SerializeField] private Button continueButton;
    private void Awake()    
    {
        buttonLoaders.ForEach(button => button.Init());

        carSelection.maxOpenedIndexCar =  Social1.PlayerPrefs.GetInt("MAX_OPENNED_CAR");


        if (Social1.PlayerPrefs.GetInt("MAX_OPENNED_CAR") > 0 || SeasonRacing.CurrentSeason > 0)
        {
            continueButton.interactable = true;
            continueButton.onClick.AddListener(() => { Continue(); });
        }
    }
    public static void LoadScene(string sceneName)
    {
        Loading.sceneName = sceneName;
        SceneManager.LoadScene("Loading");
    }
    public void Continue()
    {
        if(Social1.PlayerPrefs.GetInt("MAX_OPENNED_CAR") > 0 && SeasonRacing.CurrentSeason == 0)
        {
            carSelection.isMain = false;
            Loading.sceneName = "CarSelection";
        }
        if(SeasonRacing.CurrentSeason > 0)
        {
            Loading.sceneName = "SelectSeasons";

        }
        SceneManager.LoadScene("Loading");
    }
}
