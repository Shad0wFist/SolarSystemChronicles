using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Travel : MonoBehaviour
{
    public void GoToMars()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Mars");
    }
    public void GoToVenus()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Venus");
    }
}
