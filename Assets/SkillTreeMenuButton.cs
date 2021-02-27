using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject skillTree = null;
    [SerializeField] private GameObject treeMenu = null;
    private void Start() {
        treeMenu = GameObject.Find("TreeMenu");
    }
    public void OpenSkillTree () => skillTree.SetActive(true);
    public void CloseSkillTree () => skillTree.SetActive(false);
    public void OpenTreeMenu () => treeMenu.SetActive(true);
    public void CloseTreeMenu () => treeMenu.SetActive(false);
    
}
