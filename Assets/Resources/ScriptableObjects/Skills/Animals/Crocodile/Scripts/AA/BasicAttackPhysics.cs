using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicAttackPhysics : AutoAttack
{
    public GameObject[] AAs;
    public Transform pos;
    public float velocity;
    private int index;
    [SerializeField] private Vector2 aaForce = new Vector2();
    private Rigidbody2D _rb;
    public ForceMode2D forceMode;
    private float resetAAtimer = 0f;
    private float resetAA = 2f;

    private Player player;
    private PlayerInfo pi;

    public Vector2 AaForce { get => aaForce; set => aaForce = value; }

    
    // Start is called before the first frame update
    void Start()
    {
        //projectile = GameObject.Find("Crocodile Basic Attack");
        _rb = GetComponent<Rigidbody2D>();
        pos = Player.Instance.transform;
        velocity = 170;

        player = Player.Instance;
        pi = Player.Instance.playerInfo;

        aaForce = player.AaForce;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.J))
            player.playerInfo.AutoAttack.OnUse();

        if (resetAAtimer >= resetAA){
            index = 0;
        }else {
            resetAAtimer += Time.deltaTime;
        }
    }

    // Update is called once per frame
    public override void Use()
    {
        if (AAs == null)
            return;
        
        GameObject g = Instantiate (AAs[index%AAs.Length], new Vector3(pos.position.x, pos.position.y, AAs[index%AAs.Length].transform.position.z), AAs[index%AAs.Length].transform.rotation);
        // g.TryGetComponent<Projectile_Shoot_Test>(out Projectile_Shoot_Test shoot);
        // if (shoot != null) {
        //     shoot.shouldDestroy = true;
        //     shoot.destroyTimer = 2f;
        //     shoot.velocity = velocity;
        // }
        if (index != 5){
            if (index >= 3)
                g.transform.SetParent(Player.Instance.transform);
            _rb.AddForce(new Vector2(player.IsFacingRight ? AaForce.x : -AaForce.x, AaForce.y), forceMode);
            StartCoroutine(Dash());
        }

        g.transform.localScale = player.IsFacingRight ? g.transform.localScale : new Vector3(-g.transform.localScale.x,g.transform.localScale.y,g.transform.localScale.z);

        if (AAs[index%AAs.Length].name.Equals("Crocodile - AA 04") && !player.IsFacingRight)
            g.transform.GetChild(0).localScale = new Vector3(-g.transform.GetChild(0).localScale.x, g.transform.GetChild(0).localScale.y, g.transform.GetChild(0).localScale.z);
        
        index++;
        if (index >= AAs.Length)
            index = 0;
        g.SetActive(true);
        resetAAtimer = 0f;
    }

    private IEnumerator Dash (){
        player.IgnoreCommands = true;
        yield return new WaitForSeconds(0.3f);
        player.IgnoreCommands = false;
    }
}
