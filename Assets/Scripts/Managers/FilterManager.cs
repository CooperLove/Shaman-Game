using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterManager : MonoBehaviour
{
    private FilterManager instance;
    FilterManager () {
        if (instance == null)
            instance = this;
    }

    [Header("Filtros atuais")]
    [SerializeField] private Button currentGearFilter = null;
    [SerializeField] private Button currentRarityFilter = null;
    [SerializeField] private Button currentQuestFilter = null;

    [Header("Highlight colors")]
    [SerializeField] private Color highlightedColor = Color.white;
    [SerializeField] private Color unHighlightedColor = Color.black;
    
    public void SetRarityFilter (Button filter) => currentRarityFilter = filter;
    public void SetGearFilter (Button filter) => currentGearFilter = filter;
    public void SetQuestFilter (Button filter) => currentQuestFilter = filter;

    public void HighlightFilter (Button filter) {
        if (filter == null)
            return;

        if (filter.tag.Equals("FilterRarity")){
            //Debug.Log($"Highlighting Rarity {filter.name} {filter.tag}");
            UnhighlightFilter(currentRarityFilter);
            currentRarityFilter = filter;
            ColorBlock cb = currentRarityFilter.colors;
            cb.normalColor = highlightedColor;
            currentRarityFilter.colors = cb;
        }else if (filter.tag.Equals("FilterGear")) {
            //Debug.Log($"Highlighting Gear {filter.name} {filter.tag}");
            UnhighlightFilter(currentGearFilter);
            currentGearFilter = filter;
            ColorBlock cb = currentGearFilter.colors;
            cb.normalColor = highlightedColor;
            currentGearFilter.colors = cb;
        }else if (filter.tag.Equals("FilterQuest")) {
            //Debug.Log($"Highlighting Quest {filter.name} {filter.tag}");
            UnhighlightFilter(currentQuestFilter);
            currentQuestFilter = filter;
            ColorBlock cb = currentQuestFilter.colors;
            cb.normalColor = highlightedColor;
            currentQuestFilter.colors = cb;
        }        
    }

    private void UnhighlightFilter (Button filter) {
        if (filter == null)
            return;

        ColorBlock cb = filter.colors;
        cb.normalColor = unHighlightedColor;
        filter.colors = cb;
    }
}
