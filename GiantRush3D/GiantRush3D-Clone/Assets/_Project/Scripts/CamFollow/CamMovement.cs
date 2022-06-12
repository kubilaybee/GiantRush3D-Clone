using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamMovement : MonoBehaviour
{
    public static CamMovement Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Transform Target; //TARGET

    public Vector3 CharacterFollowOffSet; //Karakter Offset'i
    Vector3 DefaultCharacterFollowOffset;
    public Vector3 CameraAngle;
    Vector3 DefaultCharacterAngle;

    public Transform LookTarget;


    private void Start()
    {
        DefaultCharacterFollowOffset = CharacterFollowOffSet;
        DefaultCharacterAngle = CameraAngle;
    }


    private void LateUpdate()
    {
        if (LookTarget != null)
        {
            CharacterFollowOffSet = Vector3.zero;
            transform.LookAt(LookTarget);
            transform.position = Vector3.Lerp(transform.position, Target.position + CharacterFollowOffSet, Time.deltaTime * 5f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Target.position + DefaultCharacterFollowOffset, Time.deltaTime * 5f);
        }
        //}
    }

    public void defaultCamRotation()
    {
        transform.DORotate(DefaultCharacterAngle,3f);
    }

    public void ChangeTarget(Transform target, Transform lookTarget)
    {
        Target = target;
        LookTarget = lookTarget;
    }

    public void camShaker(float shakingTime,float shakePower)
    {
        transform.DOShakePosition(shakingTime, shakePower);
        transform.DOShakeRotation(shakingTime, shakePower);
    }
}
