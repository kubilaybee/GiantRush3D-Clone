using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectableStageDatas : MonoBehaviour
{
    public List<GameObject> Collectables = new List<GameObject>();
    public float startPosY;

    private void Start()
    {
        for (int i = 0; i < Collectables.Count; i++)
        {
            float currentYPos = Collectables[i].transform.position.y; // first y pos
            Collectables[i].transform.position = Collectables[i].transform.position + startPosY * Vector3.up;
            Collectables[i].transform.DOMoveY(currentYPos, 2f);
        }
    }
}
