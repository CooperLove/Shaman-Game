using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceImpactMechanic : SkillMechanic
{
    [SerializeField] private Transform fallTrail = null;
    [SerializeField] private float fallVelocity = 0f;
    [SerializeField] private Vector3 cameraShakeValues = Vector3.zero;
    private GameObject impactFX = null;
    private GameObject leftSpikes = null;
    private GameObject rightSpikes = null;
    private Player player;
    private Coroutine fallCoroutine = null;

    private LayerMask groundLayer = 0;

    // Start is called before the first frame update
    private void Awake() {
        player = Player.Instance;
        fallTrail = transform.GetChild(0);
        impactFX = transform.GetChild(1).gameObject;
        leftSpikes = transform.GetChild(2).gameObject;
        rightSpikes = transform.GetChild(3).gameObject;

        leftSpikes?.SetActive(false);
        rightSpikes?.SetActive(false);

        groundLayer = 1 << 10 | 1 << 14;
    }

    void Start()
    {
        StartCoroutine(OnFall());
    }

    // private void Update() {
    //     if (Input.GetKeyDown(KeyCode.V) && !player.OnGround && fallCoroutine == null){
    //         fallCoroutine = StartCoroutine (OnFall());
    //     }
    // }

    private IEnumerator OnFall (){
        GameStatus.IgnoreCommands(true, this);
        fallTrail.SetParent(Player.Instance.transform);
        fallTrail.localPosition = Vector3.zero;
        player.Rb.velocity = new Vector2(0, fallVelocity); 
        impactFX.gameObject.SetActive(false);
        fallTrail.gameObject.SetActive(true);

        leftSpikes?.SetActive(false);
        rightSpikes?.SetActive(false);

        yield return new WaitUntil (() => player.OnGround);

        var ray = Physics2D.Raycast(player.transform.position, Vector2.down, 25f, groundLayer);

        if (ray.collider)
            transform.position = ray.point;
        else
            transform.position = player.transform.position;

        StartCoroutine(SimpleCameraShake.instance.ShakeCamera(cameraShakeValues.x, cameraShakeValues.y, cameraShakeValues.z));

        GameStatus.IgnoreCommands(false, this);
        impactFX.gameObject.SetActive(true);
        fallTrail.gameObject.SetActive(false);

        leftSpikes?.SetActive(true);
        rightSpikes?.SetActive(true);

        fallCoroutine = null;
    }

    public override void ApplyDamage()
    {
        throw new System.NotImplementedException();
    }
}
