using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryService : MonoBehaviour
{
    [Serializable]
    public class StoryText
    {
        public List<GameObject> gameobjectsDelays;
        public GameObject goText;
        public TextMeshProUGUI textMesh;
        public string text;
        public float delay;

      

        public IEnumerator CorDelay()
        {
            gameobjectsDelays.ForEach(go=>go.SetActive(false));
            goText.gameObject.SetActive(true);
            textMesh.text = text;
            yield return new WaitForSecondsRealtime( delay);
            gameobjectsDelays.ForEach(go => go.SetActive(true));
            goText.gameObject.SetActive(false);
        }
        public IEnumerator CorNextScene(string sceneName)
        {
            gameobjectsDelays.ForEach(go => go.SetActive(false));
            goText.gameObject.SetActive(true);
            textMesh.text = text;
            yield return new WaitForSecondsRealtime(delay);
            //Loading.sceneName = sceneName;
            SceneManager.LoadScene(sceneName);
        }
    }

    [SerializeField] private StoryText startStory, storyTwo, storyThree,storyFour;
   

    private void Awake()
    {
        StartCoroutine(startStory.CorDelay());

        if (GameManager.instance.policeScene)
        {

            EndPoliceScene endPoliceScene = new EndPoliceScene();

            EndPoliceScene.instance.onEndPoliceScene += OnEndMessage;
        }
        
    }

    private void OnEndMessage(EndType type)
    {
        carSelection.isMain = false;
        if (carSelection.maxOpenedIndexCar == 0)
        {
            carSelection.OpenNewCar();
        }
            Loading.sceneName = "CarSelection";
        
        if (type == EndType.Finish)
        {
            Debug.Log(" Игрок доехал, можно накинуть подарочек в виде очков");
         
            StartCoroutine( storyTwo.CorNextScene("Loading"));
            
        }
        if (type == EndType.offTrack)
        {
            Debug.Log(" Игрок Сошёл с трассы");
            StartCoroutine(storyThree.CorNextScene("Loading"));
        }
        if (type == EndType.detainedByPolice)
        {
            Debug.Log(" Игрок был задержан полицией");
            StartCoroutine(storyFour.CorNextScene("Loading"));
        }
    }
}
