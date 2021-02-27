using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class AnimalPath : ScriptableObject
{
   public enum Animal
   {
       Crow, Bull, Eagle, Tiger, Crocodile
   }
   public Animal path;
   public abstract void Passive ();
   public abstract void OnLevelUp ();
}
