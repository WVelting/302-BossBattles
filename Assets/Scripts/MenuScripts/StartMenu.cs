using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void OnStart()
    {
        SceneManager.LoadScene("BossBattleProject");
    }
    public void OnCredits()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
    public void OnMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
