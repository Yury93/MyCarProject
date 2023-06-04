using System;
using System.Collections;
using System.Collections.Generic;
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
    public List<BodyCar> bodyCars;
    public Transform[] cars = new Transform[3];                         // List of car prefabs
    public Text carName;
    public string nameScene;// Name of the current car
    public static Color carColor ;

    int activeCarIndex = 0;

    public const string COLOR_R = "COLOR_R";
    public const string COLOR_G = "COLOR_G";
    public const string COLOR_B = "COLOR_B";
    public const string COLOR_A = "COLOR_A";

    void Start()
    {
        setCarName();

        carColor = GetColor();
        bodyCars.ForEach(b=>b.body.ForEach(b=>b.material.color = carColor));

    }
    public Color GetColor()
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
        cars[0].Rotate(0.0f, Time.deltaTime * 10, 0.0f, Space.Self);
        cars[1].Rotate(0.0f, Time.deltaTime * 10, 0.0f, Space.Self);
        cars[2].Rotate(0.0f, Time.deltaTime * 10, 0.0f, Space.Self);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    // Shows the next car to the user
    public void NextCar()
    {
        if (activeCarIndex < cars.Length-1)
        {
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
            cars[activeCarIndex].gameObject.SetActive(false);
            activeCarIndex--;
            cars[activeCarIndex].gameObject.SetActive(true);

        }

        setCarName();
    }

    // Sets the car name based on the active index
    void setCarName()
    {
        switch (activeCarIndex)
        {
            case 0:
                carName.text = "Car 1";
                break;
            case 1:
                carName.text = "Car 2";
                break;
            case 2:
                carName.text = "Car 3";
                break;
            case 3:
                carName.text = "Car 4";
                break;
        }
        
    }

    // Fills the PersistentData class with the selected car name and loads the main scene
    public void startGame()
    {
        PersistentData.selectedCarIndex = activeCarIndex;
        PersistentData.Level = 2;
        Loading.sceneName = "GeneralMenu";
        SceneManager.LoadScene(nameScene);
    }

    // Quit the game
    public void Quit()
    {
        Application.Quit();
    }
}
