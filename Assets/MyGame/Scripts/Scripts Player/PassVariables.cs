using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PassVariables : MonoBehaviour
{
    private static float sens = 50f;
    private static float volume = 0.5f;

    public Slider sensSlider;
    public Slider volumeSlider;
    public GameObject ad;

    void Start()
    {
        LoadValues();
    }

    public void setSens()
    {
        sens = sensSlider.value;
    }
    public static float getSens()
    {
        return sens;
    }

    public void setVolume()
    {
        volume = volumeSlider.value;
        ad.GetComponent<AudioSource>().volume = volume;
        ad.GetComponent<AudioSource>().Play();

    }
    public static float getVolume()
    {
        return volume;
    }

    public void BackToMain()
    {
        // Load the main scene
        SceneManager.LoadScene("StartScene");
    }

    public void LoadValues()
    {
        volumeSlider.value = volume;
        sensSlider.value = sens;
    }
}
