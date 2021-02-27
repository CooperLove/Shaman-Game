using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class OpenLearnedSkillsMenu : MonoBehaviour, IPointerClickHandler
{
    [FormerlySerializedAs("skillbar")] 
    public Transform skillBar;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) 
            return;
        Debug.Log("Opening Learned Skills Menu");
            
        if (PlayerLearnedSkills.Instance.gameObject)
            PlayerLearnedSkills.Instance.Open();
        
        if (!gameObject.CompareTag("CharacterMenu"))
            PlayerLearnedSkills.Instance.SwitchSkills.spell01 = gameObject;
    }
}
