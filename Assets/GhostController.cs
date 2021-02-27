using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostController : MonoBehaviour
{
    public bool isOneWay = false;
    [SerializeField] private bool direction = false;
    public GameObject ghostPrefab;
    [Range(0, 1.0f)] public float delay;
    [Range(0, 1.0f)] public float destroyTime;
    [ColorUsage(true, true)] public Color color;
    private Player player;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSpriteRenderer;
    public float delta;
    
    public bool ghostActive;

    // Start is called before the first frame update
    private void Start()
    {
        //ghostActive = false;
        player = Player.Instance;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        ghostPrefab = Resources.Load("Prefabs/Combat/Ghost Sprite") as GameObject;
        
        if (isOneWay)
            direction = player.IsFacingRight;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!ghostActive)
            return;

        if (delta > 0){
            delta -= Time.deltaTime;
        }else{
            delta = delay;
            CreateGhost();
        }
    }

    private void CreateGhost (){
        var trans = transform;
        var ghost = Instantiate (ghostPrefab, trans.position, trans.rotation);
        ghost.transform.localScale = playerSpriteRenderer.transform.localScale;
        Destroy(ghost, destroyTime);

        spriteRenderer = ghost.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerSpriteRenderer.sprite;
        spriteRenderer.color = playerSpriteRenderer.color;
        ghost.GetComponent<SpriteRenderer>().material = playerSpriteRenderer.material;
        ghost.GetComponent<SpriteRenderer>().sortingLayerID = playerSpriteRenderer.sortingLayerID;
        spriteRenderer.flipX = isOneWay ? !direction : !player.IsFacingRight;
    }

}
