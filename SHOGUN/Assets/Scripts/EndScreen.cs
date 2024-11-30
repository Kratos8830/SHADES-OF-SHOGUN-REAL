using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndScreen : MonoBehaviour
{
    public BoxCollider2D end;
    public GameObject endMenu;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        endMenu.SetActive(true);
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
