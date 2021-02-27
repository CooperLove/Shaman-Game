using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MetroidvaniaSkills
{
    private static Player player = Player.Instance;
    private static PlayerInfo playerInfo = Player.Instance.playerInfo;

    public static void OnLearnDoubleJump () => player.gameObject.GetComponent<JumpHandler>().JumpCount = 2;
    public static void OnLearnTripleJump () => player.gameObject.GetComponent<JumpHandler>().JumpCount = 3;
    public static void OnLearnChargeJump () => player.gameObject.GetComponent<JumpHandler>().canChargeJump = true;
    public static void OnLearnDash () => player.gameObject.AddComponent<Test_Dash>();
    public static void OnLearnDestructiveDash (){}
    public static void OnLearnShamanMode () => player.gameObject.AddComponent<ShamanMode>();
    public static void OnLearnPerfectBlock () => player.gameObject.AddComponent<BlockAttack>();
    public static void OnLearnWallSliding () => player.gameObject.AddComponent<WallSliding>();
    public static void OnLearnWallJump () => player.gameObject.AddComponent<WallJump>();
    public static void OnLearnRoll () => player.gameObject.AddComponent<Roll>();
}
