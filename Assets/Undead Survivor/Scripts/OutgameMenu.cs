using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutgameMenu : MonoBehaviour
{
     public void LoadIngame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }
}
