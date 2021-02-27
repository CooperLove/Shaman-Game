using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSaving : MonoBehaviour
{
    public void Save () => SavingSystem.Save();
    public void Load () => SavingSystem.Load();
}
