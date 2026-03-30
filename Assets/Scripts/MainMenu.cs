using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject panelSettings;

    private void Start()
    {
        panelSettings.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene("ShipScene");
    }
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Mars()
    {
        SceneManager.LoadScene("Mars");
    }
}
