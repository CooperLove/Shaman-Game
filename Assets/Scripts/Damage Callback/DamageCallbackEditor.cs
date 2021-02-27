using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DamageCallback))]
public class DamageCallbackEditor : Editor
{
    public override void OnInspectorGUI (){
        var callback = target as DamageCallback;

        /////////////////////////////////////////////////////////////////////////////////////
        GUILayout.Label("<b>Damage type</b>", new GUIStyle{richText = true});

        callback.isCollision = GUILayout.Toggle(callback.isCollision, "Uses OnTrigger or OnCollision callbacks ?");
        if (callback.isCollision){
            // callback.isTrigger = GUILayout.Toggle(callback.isTrigger, "Is Trigger ?");
            var destroyChildren = callback.destroyChildrenOnHit;
            var tag = callback.targetTag;
            callback.targetTag = EditorGUILayout.TextField("Target Tag", tag);
            callback.destroyOnHit = GUILayout.Toggle(destroyChildren ? false : callback.destroyOnHit, "Destroyed object on hit ?");
            callback.destroyChildrenOnHit = GUILayout.Toggle(destroyChildren, "Destroy any children on hit ?");

            if (callback.destroyChildrenOnHit){
                GUILayout.Label("\t<b>Children</b>", new GUIStyle{richText = true});
                callback.listSize = EditorGUILayout.IntField ("\tSize", callback.listSize);
                var listSize = callback.listSize; 
                int size = callback.children.Count;
                if (listSize > size){
                    for (int i = 0; i < listSize - size; i++)
                        callback.children.Add(null);
                }else if (listSize < size){
                    for (int i = size - 1; i >= listSize ; i--)
                        callback.children.RemoveAt(i);
                }

                size = callback.children.Count;
                for (int i = 0; i < size; i++)
                    callback.children[i] = EditorGUILayout.ObjectField($"\tChild {i}", callback.children[i], typeof(GameObject), allowSceneObjects: true) as GameObject;
                
            }
                
        }
        GUILayout.Space(5);
        
        /////////////////////////////////////////////////////////////////////////////////////
        GUILayout.Label("<b>AOE or Single Target ?</b>", new GUIStyle{richText = true});

        callback.isAOE = GUILayout.Toggle(callback.isAOE, "Is Area of effect ?");
        callback.damageDelay = EditorGUILayout.FloatField("First damage delay", callback.damageDelay);
        
        if (!callback.isAOE){
            callback.stopOnFirstEnemyHit = GUILayout.Toggle(callback.stopOnFirstEnemyHit, "Hit only the first enemy ?");
            callback.rayDistance = EditorGUILayout.FloatField("Ray Distance", callback.rayDistance);
        }
        
        if (callback.isAOE){
            callback.stopOnFirstEnemyHit = false;
            callback.aoeCenterOffset = EditorGUILayout.Vector2Field("AOE Center", callback.aoeCenterOffset);
            callback.aoeArea = EditorGUILayout.Vector2Field("AOE Area", callback.aoeArea);
        }
        GUILayout.Space(5);

        /////////////////////////////////////////////////////////////////////////////////////
        GUILayout.Label("<b>Does damage over time?</b>", new GUIStyle{richText = true});

        callback.isDOT = GUILayout.Toggle(callback.isDOT, "Is DOT ?");

        if (callback.isDOT){
            callback.numTicks = EditorGUILayout.IntField("Number of ticks", callback.numTicks);
            callback.tickInterval = EditorGUILayout.FloatField("Delay between ticks", callback.tickInterval);
        }
        GUILayout.Space(5);

        /////////////////////////////////////////////////////////////////////////////////////
        GUILayout.Label("<b>Debuffs</b>", new GUIStyle{richText = true});

        callback.stun = GUILayout.Toggle(callback.stun, "Stun target ?");
        if (callback.stun)
            callback.stunDuration = EditorGUILayout.FloatField("Stun Duration", callback.stunDuration);

        callback.slow = GUILayout.Toggle(callback.slow, "Slow target ?");
        if (callback.slow){
            callback.slowPercentage = EditorGUILayout.FloatField("Slow Percentage", callback.slowPercentage);
            callback.slowDuration = EditorGUILayout.FloatField("Slow Duration", callback.slowDuration);
        }

        callback.root = GUILayout.Toggle(callback.root, "Root target ?");
        if (callback.root)
            callback.rootDuration = EditorGUILayout.FloatField("Root Duration", callback.rootDuration);

        callback.knockUp = GUILayout.Toggle(callback.knockUp, "Knock up target ?");

    }
}
