using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnObjectOnClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] prefab = null;
    public Transform pos;
    private Camera mainCamera;
    int index = 0;
    private void Awake() {
        mainCamera = Camera.main;
        //prefab = Resources.Load("Prefabs/Combat/Rotating Chain") as GameObject;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.T))
            InstatiateOnPress();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (prefab != null && eventData.button == PointerEventData.InputButton.Left){
            Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            GameObject g = Instantiate(prefab[index % prefab.Length], new Vector3(pos.x, pos.y, prefab[index%prefab.Length].transform.position.z), prefab[index%prefab.Length].transform.rotation);
            g.SetActive(true);
            index++;
        }
    }

    private void InstatiateOnPress (){
        if (prefab != null){
            var indice = index % prefab.Length;
            var pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var g = Instantiate(prefab[indice], new Vector3(pos.x, pos.y, prefab[indice].transform.position.z), prefab[indice].transform.rotation);
            g.SetActive(true);
            index++;
        }
    }
}
