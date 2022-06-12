using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("MyCollectableDatas")]
    public Material getMaterial;
    public gameCharecters collectableDatas;

    private void Awake()
    {
        getMaterial.color = collectableDatas.color;
    }
}
