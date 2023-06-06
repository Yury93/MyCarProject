using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class carSelection : MonoBehaviour
{
    [Serializable]
    public class  BodyCar
    {
        public List<MeshRenderer> body;
        //public List<MeshRenderer> wheels;
    }
    [SerializeField] private List<Button> buttonColors;
    [SerializeField] private Button buttonRight, buttonLeft,buttonSelectedCar;//надо скрывать кнопки если не открыт не один кар еще
    public List<BodyCar> bodyCars;
    public CarMenu[] cars = new CarMenu[3];                         // List of car prefabs
    //public Text carName;
    public Text messageMaxCar;
    public string nameScene;// Name of the current car
    public static Color carColor ;
    public static int maxOpenedIndexCar;

    int activeCarIndex = 0;

    public const string COLOR_R = "COLOR_R";
    public const string COLOR_G = "COLOR_G";
    public const string COLOR_B = "COLOR_B";
    public const string COLOR_A = "COLOR_A";
    public const string ACTIVE_CAR = "ACTIVE_CAR";
    public static string MAX_OPENNED_CAR = "MAX_OPENNED_CAR";
    public static bool isMain;
    

    void Start()
    {
    

        carColor = GetColor();
        bodyCars.ForEach(b=>b.body.ForEach(b=>b.material.color = carColor));
        buttonColors.ForEach(b => b.onClick.AddListener(()=>SetCarColor(b)));
        PersistentData.selectedCarIndex =  Social1.PlayerPrefs.GetInt(ACTIVE_CAR);
        activeCarIndex = Social1.PlayerPrefs.GetInt(ACTIVE_CAR);
        cars.ToList().ForEach(c=>c.gameObject.SetActive(false));
        cars[activeCarIndex].gameObject.SetActive(true);    
           if(maxOpenedIndexCar == 0)
        {
            buttonRight.gameObject.SetActive(false);
            buttonLeft.gameObject.SetActive(false);
            messageMaxCar.text = "Пока открыта только одна машина! Проходите игру, чтобы открыть еще машины!";
        }
        setCarName();
        
        if(isMain)
        {
            buttonSelectedCar.onClick.AddListener(startMain);
        }
        else
        {
            buttonSelectedCar.onClick.AddListener(startGame);
        }


        cars.ToList().ForEach(c => c.Init());
    }
    public static void OpenNewCar()
    {
        maxOpenedIndexCar += 1;
        Debug.Log("ACCESSED" + (maxOpenedIndexCar) + " КЛЮЧ КОТОРЫЙ зАПИСЫВАЕМ");
        Social1.PlayerPrefs.SetInt("ACCESSED" + (maxOpenedIndexCar).ToString(), maxOpenedIndexCar );
 
        Social1.PlayerPrefs.SetInt("MAX_OPENNED_CAR",maxOpenedIndexCar);
        Debug.Log(MAX_OPENNED_CAR + " " +  maxOpenedIndexCar);
    }

    private void SetCarColor(Button b)
    {
        carColor = b.image.color;
        bodyCars.ForEach(b => b.body.ForEach(b => b.material.color = carColor));
        SaveColor(carColor);
    }

    public static Color GetColor()
    {
        if (Social1.PlayerPrefs.HasKey(COLOR_R))
        {
            Color color = new Color();
            float r = Social1.PlayerPrefs.GetFloat(COLOR_R, color.r);
            float g = Social1.PlayerPrefs.GetFloat(COLOR_G, color.g);
            float b = Social1.PlayerPrefs.GetFloat(COLOR_B, color.b);
            float a = Social1.PlayerPrefs.GetFloat(COLOR_A, color.a);
            color.r = r; color.g = g; color.b = b; color.a = a;
            return color;
        }
        else
        {
            return Color.white;
        }
    }
    public void SaveColor(Color color)
    {
        Social1.PlayerPrefs.SetFloat(COLOR_R,color.r);
        Social1.PlayerPrefs.SetFloat(COLOR_G, color.g);
        Social1.PlayerPrefs.SetFloat(COLOR_B, color.b);
        Social1.PlayerPrefs.SetFloat(COLOR_A, color.a);
    }

    void Update()
    {
        // Rotates the cars slowly
        cars[0].transform.Rotate(0.0f, Time.deltaTime * 10, 0.0f, Space.Self);
        cars[1].transform.Rotate(0.0f, Time.deltaTime * 10, 0.0f, Space.Self);
        cars[2].transform.Rotate(0.0f, Time.deltaTime * 10, 0.0f, Space.Self);

        if (Input.GetKeyDown(KeyCode.Escape) && isMain)
        {
            Quit();
        }
    }

    // Shows the next car to the user
    public void NextCar()
    {
       
        if (activeCarIndex < cars.Length-1)
        {
            if (cars[activeCarIndex + 1].IsAccessed == false) return;
            cars[activeCarIndex].gameObject.SetActive(false);
            activeCarIndex++;
            cars[activeCarIndex].gameObject.SetActive(true);

            setCarName();
        }
    }

    // Shows the previous car to the user
    public void PreviousCar()
    {
       


        if (activeCarIndex > 0)
        {
            if (cars[activeCarIndex - 1].IsAccessed == false) return;
            cars[activeCarIndex].gameObject.SetActive(false);
            activeCarIndex--;
            cars[activeCarIndex].gameObject.SetActive(true);

        }

        setCarName();
    }

    // Sets the car name based on the active index
    void setCarName()
    {
       
        
    }

    public void startMain()
    {
        PersistentData.selectedCarIndex = activeCarIndex;
        Social1.PlayerPrefs.SetInt(ACTIVE_CAR, activeCarIndex);
        //PersistentData.Level = 2;
        Loading.sceneName = "GeneralMenu";
        SceneManager.LoadScene(nameScene);
    }
    public void startGame()
    {
        PersistentData.selectedCarIndex = activeCarIndex;
        Social1.PlayerPrefs.SetInt(ACTIVE_CAR, activeCarIndex);
        //PersistentData.Level = 2;
        Loading.sceneName = "Season"+ SeasonRacing.CurrentSeason;
        SceneManager.LoadScene(nameScene);
    }

    // Quit the game
    public void Quit()
    {
        Loading.sceneName = "GeneralMenu";
        SceneManager.LoadScene(nameScene);
    }
  
}
