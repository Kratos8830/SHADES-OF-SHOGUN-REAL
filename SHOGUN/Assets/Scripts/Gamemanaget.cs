using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Gamemanaget : MonoBehaviour
{

    public void NextStage()
    {
        SceneManager.LoadScene("Stage 2");
    }
   public void RestartLevel()
   { 
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
