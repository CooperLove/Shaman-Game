using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gold : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText  = null;
    public int gainRate;
    // Start is called before the first frame update
    void Start()
    {
        UpdateGoldText ();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGoldText ();
    }

    private void UpdateGoldText (){
        goldText.text = (int) Player.Instance.playerInfo.Gold+"";
        Player.Instance.playerInfo.Gold+= gainRate * Time.deltaTime;
    }
}
