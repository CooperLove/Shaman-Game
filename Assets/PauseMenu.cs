using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool isGamePaused;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        // if (Input.GetKeyDown(KeyCode.R)){
        //     SceneLevelManager.Instance.LoadScene(2);
        // }
    }

}
