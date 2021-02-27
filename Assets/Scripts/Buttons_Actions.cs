using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Buttons_Actions : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public void StartGame(){
        Debug.Log("Game Started!");
        mainMenu.SetActive(false);
        SceneLevelManager.Instance.LoadScene(1);
    }
    public void Settings(){
        Debug.Log("Settings!");
    }
    public void ExitGame(){
        Debug.Log("Exit!");
        Application.Quit();
    }
    public void GoBackToMainMenu (){
        SceneLevelManager.Instance.LoadScene(0);
    }
    public void CloseWindow (){
        transform.parent.gameObject.SetActive(false);
    }
    public void OpenWindow (){
        optionsMenu.SetActive(true);
    }
}
