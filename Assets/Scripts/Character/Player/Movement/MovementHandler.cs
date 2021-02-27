using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    public float vertical, horizontal, moveSpeed, jumpForce, fallMultiplier, lowJumpMultiplier;
    private Rigidbody2D _rigidbody = null;
    private Transform _transform = null;
    public bool IsFacingRight = false, grabingObject = false;
    // Start is called before the first frame update
    void Start()
    {
       // _rigidbody      = GetComponent<Rigidbody2D>();
        _transform      = GetComponent<Transform>();
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        //_animScript = GetComponent<AnimationsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.IgnoreCommands)
            return;
        vertical   = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        Movement(vertical, horizontal);
        // FlipSprite();
        return;

        // Run();
        //     Jump();
        // if (Input.GetKeyDown(KeyCode.Space)){
        //     _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        //     _animScript.OnJumpEnter();
        // }
    }
    private void Movement (float v, float h){
        Vector3 pos = new Vector3 (h * moveSpeed, 0, 0) * Time.fixedDeltaTime;
        _transform.position = _transform.position + pos;
    }
    private void Jump (){
        if (_rigidbody.velocity.y < 0){
            _rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
            _rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }
    public void FlipSprite(){
        if (horizontal > 0 && _transform.localScale.x < 0 ){
            if (!grabingObject) IsFacingRight = true;
            _transform.localScale = new Vector3 (_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
        }else if (horizontal < 0 && _transform.localScale.x > 0 ){
            if (!grabingObject) IsFacingRight = false;
            _transform.localScale = new Vector3 (_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
        }
    }
    public void FlipSprite(bool flip){
        if (flip){
            _transform.localScale = new Vector3 (_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
        }else if (!flip){
            _transform.localScale = new Vector3 (_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
        }
    }

    private void Run (){
        if (horizontal > 0 || horizontal < 0){
            //_animScript.OnRunEnter(horizontal);
        }else {
            //_animScript.OnRunExit();
        }
    }
}
