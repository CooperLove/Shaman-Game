using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneClipManager : MonoBehaviour
{
    public SceneClip[] clips;
    public bool play;
    public Transform playerTransform;
    public AnimationsScript playerAnimator;
    public DialogueTrigger OldShaman;
    public float ms;
    public int curr;
    private static SceneClipManager instance;

    public static SceneClipManager Instance { get => instance; set => instance = value; }

    SceneClipManager (){
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = Player.Instance.transform;
        playerAnimator = Player.Instance.GetComponent<AnimationsScript>();
        curr = 0;
        play = false;
        GameStatus.isCutScenePlaying = false;
    }
    private void Update() {
        if (play && curr < clips.Length){
            PlaySceneClip();
        }else{
            //GameStatus.isCutScenePlaying = false;
        }
    }

    public void PlaySceneClip (){
        
        if (playerTransform.position.x < clips[curr].endPosition.x){
            GameStatus.isCutScenePlaying = true;
            Player.Instance.IgnoreCommands = true;
            //if (Player.Instance.transform.localScale.x < 0)
                //Player.Instance.GetComponent<MovementHandler>().FlipSprite(true);
            playerTransform.Translate(ms * Time.deltaTime, 0,0);
            playerAnimator.OnRunEnter(1);
        }
        else{
            GameStatus.isCutScenePlaying = false;
            playerAnimator.OnRunExit();
            //Player.Instance.ignoreCommands = false;
            play = false;
            SpeakWithOldShaman();
            curr++;

        }
    }

    private void SpeakWithOldShaman (){
        //OldShaman.speak= true;
    }
}
