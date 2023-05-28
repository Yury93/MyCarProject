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
                Loading.sceneName = sceneName;
                SceneManager.LoadSceneAsync("Loading");
                });
        }  
    }
    [SerializeField] private List<ButtonLoader> buttonLoaders;
    private void Awake()
    {
        buttonLoaders.ForEach(button => button.Init());
    }
    public static void LoadScene(string sceneName)
    {
        Loading.sceneName = sceneName;
        SceneManager.LoadSceneAsync("Loading");
    }
}
