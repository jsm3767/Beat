using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void NewGameButton(string newLevel)
    {
        SceneManager.LoadScene(newLevel);
    }
	
    public void ExitGameButton()
    {
        Application.Quit();
    }
}
