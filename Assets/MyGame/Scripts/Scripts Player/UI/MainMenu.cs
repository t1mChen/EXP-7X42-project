using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the game scene as needed
        SceneManager.LoadScene("Intro");
    }
    public void EnterSettings()
    {
        // Enter settings menu as needed
        SceneManager.LoadScene("SettingScene");
    }

    public void EnterCredits()
    {
        // Enter settings menu as needed
        SceneManager.LoadScene("Credits");
    }
}
