using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShoppingManager : MonoBehaviour
{
    [SerializeField] private Button[] button  = null;
    [SerializeField] private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (button.Length > 0){
            if (Input.GetKeyDown(KeyCode.W) && currentIndex > 0){
                Debug.Log("Up "+currentIndex);
                button[--currentIndex].Select();
            }else if (Input.GetKeyDown(KeyCode.S) && currentIndex < button.Length-1){
                Debug.Log("Down "+currentIndex);
                button[currentIndex].Select();
                currentIndex++;
            }
        }
    }
}
