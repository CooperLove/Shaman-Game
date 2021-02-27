using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text dmgText;
    [SerializeField] private Vector2 minMaxForceX = new Vector2();
    [SerializeField] private Vector2 minMaxForceY = new Vector2();
    private Vector2 force = new Vector2();
    [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Force;
    private Rigidbody2D rb = null;
    private const float minFontSize = 4.5f;
    private const float maxFontSize = 10f;
    

    private void Start() {
        dmgText = GetComponentInChildren<TMP_Text>();
        rb = GetComponent<Rigidbody2D>();

        force = new Vector2(Random.Range(minMaxForceX.x, minMaxForceX.y), Random.Range(minMaxForceY.x, minMaxForceY.y));

        rb.AddForce(force, forceMode2D);
    }


    public void SetText (string text, Color color, float hp = 0, float maxHp = 1){
        dmgText.text = text;

        dmgText.fontSize = Mathf.Lerp (minFontSize, maxFontSize, (hp/maxHp) );
        dmgText.color = color;
    }

    public void SetText (string text, float hp = 0, float maxHp = 1){
        dmgText.text = text;

        dmgText.fontSize = Mathf.Lerp (minFontSize, maxFontSize, (hp/maxHp) );
    }


}
