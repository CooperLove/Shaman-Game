using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Debuff
{
    private static GameObject stunIcon = Resources.Load("Prefabs/Combat/Crowd Control/Stun Icon") as GameObject;
    
    public static IEnumerator Stun (Character character, float duration)
    {
        if (!character)
            yield break;
        
        character.IsStuned = true;
        // Debug.Log($"Applying {duration}s stun on "+character.name);
        character.IgnoreCommands = true;

        var pos = new Vector3(0, 0.25f, 0);
        var icon = Object.Instantiate(stunIcon, pos, stunIcon.transform.rotation);
        var iconTransform = icon.transform;
        iconTransform.SetParent(character.transform);
        iconTransform.localPosition = pos;
        icon.GetComponent<AutoDestroy>().destroyTime = duration;
        
        if (character.CcDurationBar)
            character.CcDurationBar.transform.localScale = Vector3.one;
        
        float OldMin = 0, oldMax = duration, newRange, newMin = 0, newMax = 1, newValue;
        var oldRange = (oldMax - OldMin);
        if (oldRange == 0)
            newValue = newMin;
        newRange = (newMax - newMin);

        var dur = 0.0f;
        while (dur < duration)
        {
            dur += Time.deltaTime;
            newValue = ((( (duration - dur) - OldMin) * newRange) / oldRange) + newMin;
            newValue = Mathf.Clamp01(newValue);
            if (character.CcDurationBar)
            {
                var transform = character.CcDurationBar.transform;
                var localScale = transform.localScale;
                localScale = new Vector3(newValue, localScale.y, localScale.z);
                transform.localScale = localScale;
            }

            //Debug.Log(NewValue);
            yield return null;
        }
        character.IgnoreCommands = false;
        character.IsStuned = false;
    }
    /*Aplica redução de velocidade de movimento em percentage% por duration segundos*/
    public static IEnumerator Slow (Character character, float percentage, float duration){
        if (character == null)
            yield break;


        var slow = character.Velocity * (percentage/100f);
        character.Velocity -= slow;
        Debug.Log($"Apllying {percentage}% slow for {duration} on "+character.name+$" MS {character.Velocity+slow} => {character.Velocity}");

        Debuff.InstantiateDebuffIcon(character, duration, "Slow");

        yield return new WaitForSeconds(duration);
        
        character.Velocity += slow;
    }
    /*Enraiza o personagem por duration segundos*/
    public static IEnumerator Root (Character character, float duration){
        if (character == null)
            yield break;
            
        Debug.Log("Apllying root on "+character.name);
        float vel = character.Velocity;
        character.Velocity = 0;
        //yield return new WaitForSeconds(duration);
        if (character.CcDurationBar != null)
            character.CcDurationBar.transform.localScale = Vector3.one;

        Debuff.InstantiateDebuffIcon(character, duration, "Root");

        float OldMin = 0, oldMax = duration, newRange, newMin = 0, newMax = 1, newValue;
        var oldRange = (oldMax - OldMin);
        if (oldRange == 0)
            newValue = newMin;
        newRange = (newMax - newMin);

        var dur = 0.0f;
        while (dur < duration)
        {
            dur += Time.deltaTime;
            newValue = ((( (duration - dur) - OldMin) * newRange) / oldRange) + newMin;
            newValue = Mathf.Clamp01(newValue);
            if (character.CcDurationBar)
            {
                var transform = character.CcDurationBar.transform;
                var localScale = transform.localScale;
                localScale = new Vector3(newValue, localScale.y, localScale.z);
                transform.localScale = localScale;
            }

            //Debug.Log(NewValue);
            yield return null;
        }
        
        character.Velocity = vel;
    }
    public static IEnumerator ArmorReductionPercentage (Character character, float duration, float percentage){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static IEnumerator ArmorReductionFlat (Character character, float duration, float value){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static IEnumerator MrReductionPercentage (Character character, float duration, float percentage){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static IEnumerator MrReductionFlat (Character character, float duration, float value){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static IEnumerator AdReduction (Character character, float duration, float percentage){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static IEnumerator ApReductin (Character character, float duration, float percentage){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static IEnumerator Silence (Character character, float duration){
        
        yield return new WaitForSeconds(duration);
        
    }

    public static IEnumerator KnockUp (Character character, Vector3 knockUpHeight, float knockUpDuration, Vector3 force)
    {
        var rb = character.Rb;
        if (character is Player)
            GameStatus.IgnoreCommands(true, character);
        else
            character.IgnoreCommands = true;

        var transform = character.transform;
        var position = transform.position;
        var startingPoint = position;
        var endPoint = position + knockUpHeight;

        var timer = 0f;
        var velocity = 1f / knockUpDuration;
        
        while (timer < 1f)
        {
            timer += velocity * Time.deltaTime;
            var posY = Mathf.Lerp(startingPoint.y, endPoint.y, timer);
            var curPos = transform.position;
            var pos = new Vector3(curPos.x, posY, curPos.z);
            transform.position = pos;
            yield return null;
        }
        
        transform.position = new Vector3(transform.position.x, endPoint.y, 0);
        
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity += new Vector2(0, force.y * 0.75f);
        rb.AddForce(force, ForceMode2D.Impulse);
        
        if (character is Enemy e )
        {
            //Debug.Log($"{e.name} {rb.velocity}");
            //rb.velocity = new Vector2();
            e.onAir = true;
        }

        yield return new WaitUntil(() => character.OnGround);
        //Debug.Log($"{character.name} on ground");

        if (character is Player)
            GameStatus.IgnoreCommands(false, character);
        else
            character.IgnoreCommands = false;
    }
    
    public static IEnumerator Jump (Character character, Vector3 knockUpHeight, float knockUpDuration, Vector3 force)
    {
        var rb = character.Rb;
        
        if (character is Player)
            GameStatus.IgnoreCommands(true, character);
        else
            character.IgnoreCommands = true;
        
        var transform = character.Transform;
        var position = transform.position;
        var startingPoint = position;
        var endPoint = position + knockUpHeight;

        var timer = 0f;
        var velocity = 1f / knockUpDuration;
        
        while (timer < 1f)
        {
            timer += velocity * Time.deltaTime;
            var posY = Mathf.Lerp(startingPoint.y, endPoint.y, timer);
            var curPos = transform.position;
            var pos = new Vector3(curPos.x, posY, curPos.z);
            transform.position = pos;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, endPoint.y, 0);
        
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity += new Vector2(0, force.y * 0.75f);
        rb.AddForce(force, ForceMode2D.Impulse);
        
        //Debug.Log($"Chegou");
        if (character is Player)
            GameStatus.IgnoreCommands(false, character);
        else
            character.IgnoreCommands = false;
        
        // rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // yield return new WaitForSeconds(0.4f);
        // rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public static IEnumerator HpReductionPercentage (Character character, float duration, float percentage){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static IEnumerator HpReductionFlat (Character character, float duration, float value){
        
        yield return new WaitForSeconds(duration);
        
    }
    public static IEnumerator Dot (Character character, float duration, int ticks, int damage){
        
        yield return new WaitForSeconds(duration);
        
    }

    public static IEnumerator Update_CC_Bar (float duration){
        var dur = 0.0f;
        while (dur < duration)
        {
            dur += Time.deltaTime;
            Debug.Log(dur);
            yield return null;
        }
    }

    /*Instancia um objeto contendo a imagem que indica qual o tipo de debuff foi usado e coloca ele na barra de debuffs*/
    private static void InstantiateDebuffIcon (Character character, float duration, string debuffName)
    {
        if (!(character is Player))
            return;
        var ccImage = new GameObject {name = "ccImage"};
        ccImage.AddComponent(typeof(Image));
        ccImage.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Debuffs/{debuffName}");
        ccImage.transform.SetParent(Player.Instance.buffDebuffBar);
        ccImage.transform.localScale = Vector3.one;
        Object.Destroy(ccImage, duration);
    }
}
