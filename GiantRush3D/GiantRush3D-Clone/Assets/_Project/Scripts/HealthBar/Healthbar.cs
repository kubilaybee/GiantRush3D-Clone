using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Healthbar : MonoBehaviour
{
    public Image healthbar;

    public void UpdateHealth(float fraction)
    {
        
        //healthbar.fillAmount = fraction;
        healthbar.DOFillAmount(fraction, 1f);
    }

    public void FullHealth()
    {
        healthbar.fillAmount = 1;
    }
}
