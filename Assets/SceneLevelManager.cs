using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLevelManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;
    private static SceneLevelManager instance;

    public static SceneLevelManager Instance { get => instance; set => instance = value; }

    SceneLevelManager (){
        if (Instance == null)
            Instance = this;
    }
    private void Start() {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(loadingScreen);
    }
    public void LoadScene (int sceneIndex){
        loadingScreen?.SetActive(true);
        StartCoroutine (LoadAsync(sceneIndex));
    }
    private IEnumerator LoadAsync (int sceneIndex){
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone){
            progressBar.value = Mathf.Clamp01(operation.progress/0.9f);
            yield return null;
        }
        if (progressBar.value == 1)
            loadingScreen.SetActive(false);
    }
}
