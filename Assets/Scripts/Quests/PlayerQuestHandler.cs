using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerQuestHandler : QuestHandler, ISelectHandler, IDeselectHandler
{
    public override void OnSelect(BaseEventData eventData)
    {
        QuestDescriptionPanel.Instance.SetQuest(Quest);
        IngameQuestDescriptionPanel.Instance.SetQuest(Quest);
        QuestDescriptionPanel.Instance.UpdateUI();
        QuestDescriptionPanel.Instance.questTransform = transform;
        // Destroi o icone de quest nova
        if (transform.childCount > 2)
            Destroy(transform.GetChild(2).gameObject);
        QuestManager.Instance.UpdateQuestTabState(transform.GetSiblingIndex());
        tabQuests.UpdateIndicators(transform.GetSiblingIndex());
        QuestDescriptionPanel.Instance.Open();
    }

}
