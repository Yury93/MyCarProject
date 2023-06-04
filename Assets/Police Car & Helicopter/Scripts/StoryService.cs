using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            SceneLoader.LoadScene(sceneName);
        }
    }

    [SerializeField] private StoryText startStory, storyTwo, storyThree,storyFour;

    private void Awake()
    {
        StartCoroutine(startStory.CorDelay());

     
            EndPoliceScene endPoliceScene = new EndPoliceScene();

            EndPoliceScene.instance.onEndPoliceScene += OnEndMessage;
        
    }

    private void OnEndMessage(EndType type)
    {
        if(type == EndType.Finish)
        {
            Debug.Log(" Игрок доехал, можно накинуть подарочек в виде очков");
        }
        if (type == EndType.offTrack)
        {
            Debug.Log(" Игрок Сошёл с трассы");
        }
        if (type == EndType.detainedByPolice)
        {
            Debug.Log(" Игрок был задержан полицией");
        }
    }
}
