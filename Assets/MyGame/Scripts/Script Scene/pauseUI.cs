using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanva;
    private static bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isPaused)
            {
                PauseGame();
            }
            else {
                ResumeGame();
            }
        }
    }

    public void PauseGame() {
        // game is paused and time stops
        pauseCanva.gameObject.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame() {
        // time restarts and unpause
        pauseCanva.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool getPauseState() {
        return isPaused;
    }
}
