using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameChar", order = 1)]
public class gameCharecters : ScriptableObject
{
    public string name;
    public Color color;
    public float value;
}
