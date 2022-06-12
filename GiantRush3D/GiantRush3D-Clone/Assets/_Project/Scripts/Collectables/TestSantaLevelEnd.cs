using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSantaLevelEnd : MonoBehaviour
{
    public static TestSantaLevelEnd Instance;

    private void Awake()
    {
        Instance = this;
    }
    public Transform CameraPoint;
    public Transform CameraLookPoint;

}
