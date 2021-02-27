using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class PrimarySkills : MonoBehaviour
{
    [SerializeField] private static PrimarySkills instance;
    PrimarySkills (){
        if (instance == null)
            instance = this;
    }
    public Image basicAttack;
    public Image lowAttack;
    public Image mediumAttack;
    public Image highAttack;
    public TMP_Text aaText;
    public TMP_Text lowText;
    public TMP_Text medText;
    public TMP_Text highText;


    private void Start() {
        //Debug.Log($"Image {basicAttack} {lowAttack} {mediumAttack} {highAttack}");
    }

    public static PrimarySkills Instance { get => instance; }

    public IEnumerator OnCooldownEnter (AnimalAttacks skill , string method, Image cooldownBG, TMP_Text cdText, float duration)
    {
        bool exp = (!method.Equals("CanUseLow") && !method.Equals("CanUseMedium") && !method.Equals("CanUseHigh") && !method.Equals("CanUseAA"));

       // Debug.Log($"Image {cooldownBG} {method} {!method.Equals("CanUseAA")} {!method.Equals("CanUseLow")} {!method.Equals("CanUseMedium")} {!method.Equals("CanUseHigh")} = {exp}");
        if (cooldownBG == null && exp )
            yield break;
            

        cooldownBG.fillAmount = 1;
        cooldownBG.gameObject.SetActive(true);
        cdText.gameObject.SetActive(true);

        float counter = 0;
        //Get the current life of the player
        float startLife = 1;
        //Calculate how much to lose
        float endLife = 0;
        //Stores the new player life
        float newFillAmout = 0;
        float lifeGain = 0;
        float cd = duration;
        while (counter < duration)
        {
            //Debug.Log($"{counter} => {cd} - {counter / duration} = {cd-(counter / duration)}");
            counter += Time.deltaTime;
            cdText.text = string.Format(CultureInfo.InvariantCulture,"{0:0.0}", duration - counter);
            newFillAmout = Mathf.Lerp(startLife, endLife, counter / duration);
            lifeGain = newFillAmout - lifeGain;
            cooldownBG.fillAmount += lifeGain;
            //Debug.Log("Current Life: " + newFillAmout+" healed: "+lifeGain);
            lifeGain = newFillAmout;
            yield return null;
        }

        cooldownBG.gameObject.SetActive(false);
        cdText.gameObject.SetActive(false);
        skill.SendMessage(method);
    }
}
