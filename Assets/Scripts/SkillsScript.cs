using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillsScript : ScriptableObject
{
    public string name;

    public abstract void Execute(GameObject emitter);
    /*[Header("Atributes")]
    [SerializeField] string name;
    [SerializeField] string type;
    [SerializeField] string element;
    [SerializeField] int range;
    [SerializeField] int PA;
    [SerializeField] int requiredLevel;
    [SerializeField] int Level;
    [SerializeField] float minDamage;
    [SerializeField] float maxDamage;*/
    
}
