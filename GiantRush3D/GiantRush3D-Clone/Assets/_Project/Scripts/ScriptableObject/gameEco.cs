using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameEco", order = 2)]
public class gameEco : ScriptableObject
{
    public int diamondCounter;
    // currentLevelDatas
    public int currentDiamondCounter;
    public int currentStackCounter;
    public int currentGodMode;
}
