using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss : MonoBehaviour
{

    #region HealthbarDatas
    [Header("HealthbarDatas")]
    public Canvas healthbarCanvas;
    public Healthbar healthbar;
    public int maxHealth;
    #endregion
    [Header("BossFightDatas")]
    public int health;
    public int punchPower;
    [Header("MyCollectableDatas")]
    public Material getMaterial;
    public gameCharecters collectableDatas;

    #region AnimatorDatas
    [Header("AnimatorDatas")]
    public Animator anim;
    public float animWait = 3f;
    #endregion

    public bool readyFight;
    public bool fightBegan;

    private void Awake()
    {
        getMaterial.color = collectableDatas.color;
    }

    public void takeDamage(int damage)
    {
        healthbar.UpdateHealth((float)health / (float)maxHealth);
    }

    private void Start()
    {
        // define the anim
        anim = GetComponent<Animator>();
        maxHealth = health;
        healthbarCanvas.enabled = false;
    }

    private void Update()
    {
        
        if (GameManager.Instance.currentGameState == GameManager.gameStates.BossFight && !readyFight)
        {
            readyFight = true;
            healthbarCanvas.enabled = true;
            fightReady(readyFight);
            // delay time
            StartCoroutine(fightSenario(3));
        }

        if (health<0)
        {
            StopAllCoroutines();
            anim.SetBool("Knockout", true);
        }
    }

    // fight anim datas
    void fightReady(bool fightReadyState)
    {
        anim.SetBool("PunchReady", fightReadyState);
    }

    IEnumerator fightSenario(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // first punch
        anim.SetBool("PunchFirst", true);

        yield return null;
    }
    // anim event
    public void leftPunchHitted()
    {
        if (health >= 0)
        {
            Debug.Log("SOL");
            GameManager.Instance.mainChar.GetComponent<MainChar>().health -= punchPower;
            GameManager.Instance.mainChar.GetComponent<MainChar>().takeDamage(punchPower);
            GameManager.Instance.mainChar.GetComponent<MainChar>().punchParticle(transform.position + new Vector3(0, 4, -3.5f));
            StartCoroutine(afterLeft());
        }
        else
        {
            anim.SetBool("Knockout", true);
        }
    }
    // anim event
    public void rightPunchHitted()
    {
        if (0<=health)
        {
            Debug.Log("SAÐ");
            GameManager.Instance.mainChar.GetComponent<MainChar>().health -= punchPower;
            GameManager.Instance.mainChar.GetComponent<MainChar>().takeDamage(punchPower);
            GameManager.Instance.mainChar.GetComponent<MainChar>().punchParticle(transform.position + new Vector3(0, 4, -3.5f));
            StartCoroutine(afterRight());
        }
        else
        {
            anim.SetBool("Knockout", true);
        }
    }

    public IEnumerator afterLeft()
    {
        if (health < 0)
        {
            anim.SetBool("Knockout", true);
            yield break;
        }
        if (GameManager.Instance.mainChar.GetComponent<MainChar>().health < 0)
        {
            Debug.Log("BOSS IS HERE");
            GameManager.Instance.changeGameState(GameManager.gameStates.LevelFail);
            anim.SetBool("PunchFirst", false); // back to fighter idle
            yield break;
        }

        if (anim.GetBool("PunchFirst"))
        {
            // turn to IDLE
            anim.SetBool("PunchFirst", false);
            // wait the IDLE
            yield return new WaitForSeconds(animWait);
        }
        // hit the other punch
        anim.SetBool("PunchSecond", true);
    }
    public IEnumerator afterRight()
    {
        if (health<0)
        {
            anim.SetBool("Knockout", true);
            yield break;
        }
        if(GameManager.Instance.mainChar.GetComponent<MainChar>().health < 0)
        {
            Debug.Log("BOSS IS HERE");
            GameManager.Instance.changeGameState(GameManager.gameStates.LevelFail);
            anim.SetBool("PunchSecond", false); // back to fighter idle
            yield break;
        }

        if (anim.GetBool("PunchSecond"))
        {
            // turn to IDLE
            anim.SetBool("PunchSecond", false);
            // wait the IDLE
            yield return new WaitForSeconds(animWait);
        }
        // hit the other punch
        anim.SetBool("PunchFirst", true);
    }
}
