using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLevelEndPoint : MonoBehaviour
{
    public static CamLevelEndPoint Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Transform cameraPoint;
    public Transform cameraLookPoint;
}
