using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene("MyGame");
    }

    public void MainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("StartScene");
    }

    public void Lose()
    {
        // Load the main menu scene
        SceneManager.LoadScene("LoseGame");
    }

    public void Win()
    {
        // Load the main menu scene
        SceneManager.LoadScene("WinGame");
    }
}
