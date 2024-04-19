using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager instance;

    public GameObject pause;
    public GameObject resume;
    public GameObject quit;

    bool isPaused = true;


    public void GoToMenu()
    {
        SceneManager.LoadScene("Start");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        if (isPaused == true)
        {
            pause.SetActive(false);
            UnityEngine.Time.timeScale = 0;
            resume.SetActive(true);
            quit.SetActive(true);
        }
        else
        {
            pause.SetActive(true);
            UnityEngine.Time.timeScale = 1;
            pause.SetActive(true);
            resume.SetActive(false);
            quit.SetActive(false);
        }

        isPaused = !isPaused;
    }

}
