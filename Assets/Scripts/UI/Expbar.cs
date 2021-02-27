using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Expbar : MonoBehaviour
{
    public float xp, totalXP;
    public RectTransform rectTransform, canvasRect, currentXP_Rect, cMenu;
    public Slider slider;
    public TMP_Text xpText;
    public Image xpImage;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // if (xpImage != null)
        //     return; 
        // Vector2 sizeRect = this.tag != "CharacterMenu" ? new Vector2(canvasRect.sizeDelta.x, 10) : new Vector2(cMenu.sizeDelta.x
        // , cMenu.sizeDelta.y);
        // rectTransform.sizeDelta = sizeRect;
        // currentXP_Rect.sizeDelta = sizeRect;
    }
    private void Update() {
        UpdateEXP_Bar();
    }

    // Update is called once per frame
    public void UpdateEXP_Bar (){
        //if (this.tag != "CharacterMenu") Player.Instance.playerInfo.Experience = (int) slider.value;
        //Debug.Log("XP XP "+this.tag);
        if (xpText != null && this.tag == "CharacterMenu"){
            xpText.text = (int) Player.Instance.playerInfo.Experience+"/"+ (int) Player.Instance.playerInfo.NextLevelEXP;
            
        }
        xp = Player.Instance.playerInfo.Experience;
        totalXP = Player.Instance.playerInfo.NextLevelEXP;
        float exp = xp/totalXP;
        if (exp > 1)
            exp = 1;

        if (xpImage != null){
            xpImage.fillAmount = exp;
            return;
        }
        currentXP_Rect.localScale = new Vector3 (exp, 1, 1);
    }
    
}
