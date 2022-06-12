using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class MainChar : MonoBehaviour
{
    #region HealthbarDatas
    public Canvas healthbarCanvas;
    public Healthbar healthbar;
    public int maxHealth;
    #endregion

    #region RagDollDatas
    //[Header("Ragdoll Datas")]
    //public Collider[] ragdollCol;
    //public Rigidbody[] ragdollRb;
    #endregion

    #region AnimDatas
    [Header("Animation Datas")]
    public Animator anim;
    #endregion

    #region MyObjDatas
    [Header("MyObjectsDatas")]
    public Material getMaterial;
    [Header("Shaders")]
    public Shader currentShader;
    public Shader standartShader;
    public Shader rainbowShader;
    #endregion

    #region MainCharDatas
    public gameCharecters mainCharDatas;
    public bool godMode;
    public Color currentColor;
    [Header("ColorList")]
    public CharColor[] charColors;
    #endregion

    #region MovementDatas
    [Header("Movement Datas")]
    public float characterSpeed;
    public float touchRotationSpeed;
    public float xMinLimit;
    public float xMaxLimit;
    private Vector3 lastMousePos;
    private Vector3 firstMousePos;
    private Vector3 deltaMousePos;
    private float velocityX;
    #endregion

    #region FightDatas
    public bool fightAreaEnter;
    public GameObject tempBoss;
    [Header("FighterDatas")]
    public int health;
    public int punchPower;
    public int punchCounter;
    public bool fighterIDLE;
    #endregion

    // FIX**
    #region GameEconomy
    [Header("GameEconomy")]
    public int colorCount;
    public int godModeCount;
    #endregion


    #region ScaleDatas
    [Header("ScaleDatas")]
    public float scaleTime = 1f;
    #endregion

    [Serializable]
    public class CharColor
    {
        public string colorName;
        public Color colorType;
    }

    public void takeDamage(int damage)
    {
        healthbar.UpdateHealth((float)health / (float)maxHealth);
    }

    private void Start()
    {
        healthbarCanvas.enabled = false;
        // shaders
        standartShader = Shader.Find("Standard");
        rainbowShader = Shader.Find("_Shaders/Rainbow");
        shaderRainbow();

        anim = GetComponent<Animator>();
        getRandomColor();

        // healthbar func
        maxHealth = health;

        // ragdoll datas
        //ragdollCol = GetComponentsInChildren<Collider>();
        //ragdollRb = GetComponentsInChildren<Rigidbody>();
    }

    //void changeRagDoll(bool state)
    //{
    //    foreach (Collider colliders in ragdollCol)
    //    {
    //        colliders.enabled = state;
    //    }
    //    foreach (Rigidbody rbs in ragdollRb)
    //    {
    //        rbs.isKinematic = !state;
    //    }
    //}

    //void ragDollFix(bool ragDollFixData)
    //{
    //    // ragdollfixdata default true
    //    GetComponent<BoxCollider>().enabled = ragDollFixData;
    //    changeRagDoll(!ragDollFixData);
    //}

    public void shaderStandard()
    {
        currentShader = standartShader;
        getMaterial.shader = standartShader;
    }
    public void shaderRainbow()
    {
        currentShader = rainbowShader;
        getMaterial.shader = rainbowShader;
    }

    private void Update()
    {
        //if (godMode)
        //{
        //    getMaterial.color = charColors[3].colorType;
        //    mainCharDatas.color = charColors[3].colorType;
        //}

        #region characterMovements
        if (GameManager.Instance.currentGameState==GameManager.gameStates.GamePlay)
        {
            if (!godMode)
            {
                shaderStandard();
            }
            // anim
            stayAnim(false);
            deathAnim(false);
            runAnim(true);

            if (Input.GetMouseButtonDown(0))
            {
                // movement
                lastMousePos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                // firstMousePos
                firstMousePos = lastMousePos;
                // finalMousePos
                lastMousePos = Input.mousePosition;
                // find deltaMousePos
                deltaMousePos = (lastMousePos - firstMousePos);
                // delay
                velocityX = Mathf.Lerp(velocityX, deltaMousePos.x, 0.2f);
            }
            if (Input.GetMouseButtonUp(0))
            {
                // reset all movement datas
                firstMousePos = Vector3.zero;
                lastMousePos = Vector3.zero;
                deltaMousePos = Vector3.zero;
                velocityX = 0;
                // anim
                //changeCharAnim();
            }

            // movement 
            transform.Translate(Vector3.forward * Time.deltaTime * characterSpeed + new Vector3(velocityX * touchRotationSpeed / 300f, 0, 0));
            // x Limit
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMinLimit, xMaxLimit), transform.position.y, transform.position.z);
            
            #endregion
        }
        // fail anim
        if (GameManager.Instance.currentGameState == GameManager.gameStates.LevelFail)
        {
            // anim wait
            deathAnim(true);
        }
        // success anim
        if (GameManager.Instance.currentGameState == GameManager.gameStates.LevelSucces)
        {
            // anim
            runAnim(false);
            deathAnim(false);
            firstPunch(false);
            secondPunch(false);

        }
        // start anim
        if (GameManager.Instance.currentGameState == GameManager.gameStates.Start)
        {
            // anim
            stayAnim(true);
            deathAnim(false);
            runAnim(false);

            // change color
            //getMaterial.color = mainCharDatas.color;
            getRandomColor();
        }
        // change anim

        if (GameManager.Instance.currentGameState == GameManager.gameStates.BossFight)
        {
            if (!fightAreaEnter)
            {
                velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime*5f);
                // movement 
                transform.Translate(Vector3.forward * Time.deltaTime * characterSpeed + new Vector3(velocityX * touchRotationSpeed / 300f, 0, 0));
                Debug.Log(velocityX + "  => VELOCITY");
            }
            else
            {
                if (!fighterIDLE)
                {
                    fighterIDLE = true;
                    fightReady(true);
                    stayAnim(false);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(fightSenario());
                    /*
                    
                    GameManager.Instance.changeGameState(GameManager.gameStates.LevelSucces);
                    fightAreaEnter = false;
                    */
                }
            }
        }
    }
    #region FightDatas
    void resetPunchCounter()
    {
        punchCounter = 0;
    }

    void changePunchCounter()
    {
        if (punchCounter%2==0)
        {
            // secondPunch
            punchCounter = 1;
        }
        else
        {
            // firstPunch
            punchCounter = 0;
        }
    }
    // fight senario
    IEnumerator fightSenario()
    {
        Debug.Log("testt");
        // first punch
        if (punchCounter%2==0)
        {
            firstPunch(true);
        }
        else
        {
            secondPunch(true);
        }

        yield return null;
    }

    // fight anim datas
    public void fightReady(bool fightReadyState)
    {
        anim.SetBool("PunchReady", fightReadyState);
    }

    public void firstPunch(bool firstPunchState)
    {
        anim.SetBool("PunchFirst", firstPunchState);
    }
    public void leftPunchHitted()
    {
        Debug.Log("SOL-Player");
        tempBoss.GetComponent<Boss>().health -= punchPower;
        tempBoss.GetComponent<Boss>().takeDamage(punchPower);
        // createPunchParticle
        punchParticle(transform.position + new Vector3(0, 5, 3.5f));
        // after hitted
        StartCoroutine(afterLeft());
    }
    IEnumerator afterLeft()
    {
        if (tempBoss.GetComponent<Boss>().health <0)
        {
            Debug.Log("ONE RING!!");
            GameManager.Instance.changeGameState(GameManager.gameStates.LevelSucces);
            fightAreaEnter = false;
            yield break;
        }
        firstPunch(false);
        changePunchCounter();
    }

    public void secondPunch(bool secondPunchState)
    {
        anim.SetBool("PunchSecond", secondPunchState);
    }
    public void rightPunchHitted()
    {
        Debug.Log("SAG-Player");
        tempBoss.GetComponent<Boss>().health -= punchPower;
        tempBoss.GetComponent<Boss>().takeDamage(punchPower);
        // createPunchParticle
        punchParticle(transform.position + new Vector3(0, 5, 3.5f));
        // after hitted
        StartCoroutine(afterRight());
    }
    IEnumerator afterRight()
    {
        if (tempBoss.GetComponent<Boss>().health < 0)
        {
            Debug.Log("ONE RING!!"); 
            GameManager.Instance.changeGameState(GameManager.gameStates.LevelSucces);
            fightAreaEnter = false;
            yield break;
        }
        secondPunch(false);
        changePunchCounter();
    }
#endregion

    // scale functions
    void scaleMainChar(Vector3 newScale)
    {
        transform.DOScale(newScale, scaleTime);
    }

    public void getRandomColor()
    {
        Color tempColor = charColors[UnityEngine.Random.Range(0, 3)].colorType;
        getMaterial.color = tempColor;
        mainCharDatas.color = tempColor;
    }

    public void stayAnim(bool stayState)
    {
        anim.SetBool("stay", stayState);
    }
    void runAnim(bool runState)
    {
        anim.SetBool("run", runState);
    }

    void deathAnim(bool deathState)
    {
        anim.SetBool("death", deathState);
    }

    // collider other object scale up and scale down **scriptable color

    public void checkGodeMode()
    {
        if (GameManager.Instance.gameEconomy.currentGodMode >=GameManager.Instance.godModeActivatedValue)
        {
            godMode = true;
            StartCoroutine(GameManager.Instance.godModeSlayer());
        }
    }

    // collider funct
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FinalStageWay>())
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            GameManager.Instance.changeGameState(GameManager.gameStates.BossFight);
            // closed GodMode
            godMode = false;
        }

        if (other.gameObject.GetComponent<BossFightArea>())
        {
            // healthbar canvas
            healthbarCanvas.enabled = true;

            fightAreaEnter = true;
            // anim
            CamMovement.Instance.ChangeTarget(CamLevelEndPoint.Instance.cameraPoint, CamLevelEndPoint.Instance.cameraLookPoint);
            fightReady(true);
            runAnim(false);
            deathAnim(false);

            // fight anim
            tempBoss = other.gameObject.GetComponent<BossFightArea>().boss;

            Debug.Log("BOSSSSSSS!!!!!!");

        }

        if (other.gameObject.GetComponent<Obstacle>())
        {
            // death particle
            deathParticle(transform.position);
            // anim
            runAnim(false);
            deathAnim(true);

            GameManager.Instance.changeGameState(GameManager.gameStates.LevelFail);
            fightAreaEnter = false;
            godMode = false;
        }

        if (other.gameObject.GetComponent<Diamond>())
        {
            // changeable particle **
            GameManager.Instance.createParticle(GameManager.Instance.redBuffPref, other.gameObject.transform.position, 2f);
            // increase diamond economy
            GameManager.Instance.increaseCurrentDiamond();
            // increase godMode
            GameManager.Instance.increaseCurrentGodMode();
            // check godMode
            checkGodeMode();

            Destroy(other.gameObject);
        }


        if (!godMode)
        {
            #region Collectables
            if (other.gameObject.GetComponent<Collectable>())
            {
                // same color
                if (other.gameObject.GetComponent<Collectable>().collectableDatas.color == mainCharDatas.color)
                {
                    // sameCollision Destroy Particle
                    sameCollectable(other.gameObject.GetComponent<Collectable>().collectableDatas.color, other.gameObject.transform.position);
                    // increase godMode
                    GameManager.Instance.increaseCurrentGodMode();
                    // increase stack
                    GameManager.Instance.increaseCurrentStack();
                    // check godMode
                    checkGodeMode();

                    float tempScaleData = other.gameObject.GetComponent<Collectable>().collectableDatas.value;
                    Destroy(other.gameObject);
                    //transform.localScale += new Vector3(tempScaleData, tempScaleData, tempScaleData);
                    Vector3 tempScale = transform.localScale + new Vector3(tempScaleData, tempScaleData, tempScaleData);
                    scaleMainChar(tempScale);
                    //Vector3 tempScaleValue = transform.localScale + new Vector3(tempScaleData, tempScaleData, tempScaleData);
                    //transform.DOScale(tempScaleData, 1f);
                }
                // dif color
                if (other.gameObject.GetComponent<Collectable>().collectableDatas.color != mainCharDatas.color)
                {
                    // diffCollision Destroy Particle
                    diffCollectable(other.gameObject.GetComponent<Collectable>().collectableDatas.color, other.gameObject.transform.position);
                    // decrease godMode
                    GameManager.Instance.decreaseCurrentGodMode();
                    // decrease stack
                    GameManager.Instance.decreaseCurrentStack();

                    float tempScaleData = other.gameObject.GetComponent<Collectable>().collectableDatas.value;
                    Destroy(other.gameObject);
                    //transform.localScale -= new Vector3(tempScaleData, tempScaleData, tempScaleData);
                    Vector3 tempScale = transform.localScale - new Vector3(tempScaleData, tempScaleData, tempScaleData);
                    scaleMainChar(tempScale);
                    // CHECK THE FAIL
                    if (transform.localScale.x < 0.5)
                    {
                        // death particle
                        deathParticle(transform.position);
                        // anim
                        runAnim(false);
                        deathAnim(true);

                        GameManager.Instance.changeGameState(GameManager.gameStates.LevelFail);
                        fightAreaEnter = false;
                        godMode = false;
                    }
                }
            }
            #endregion
        }

        #region GODMODE
        if (godMode)
        {
            #region Collectables
            if (other.gameObject.GetComponent<Collectable>())
            {
                // sameCollision Destroy Particle
                sameCollectable(other.gameObject.GetComponent<Collectable>().collectableDatas.color, other.gameObject.transform.position);
                // increase stack
                GameManager.Instance.increaseCurrentStack();

                float tempScaleData = other.gameObject.GetComponent<Collectable>().collectableDatas.value;
                Destroy(other.gameObject);
                //transform.localScale += new Vector3(tempScaleData, tempScaleData, tempScaleData);
                Vector3 tempScale = transform.localScale + new Vector3(tempScaleData, tempScaleData, tempScaleData);
                scaleMainChar(tempScale);
            }
            #endregion
        }
        #endregion
    }

    public void diffCollectable(Color difColor,Vector3 spawnPos)
    {

        spawnPos.y += 0.1f;

        foreach (CharColor temp in charColors)
        {
            if (temp.colorType==difColor)
            {
                if (temp.colorName == "red")
                {
                    GameManager.Instance.createParticle(GameManager.Instance.redNOBuffPref, spawnPos, GameManager.Instance.redNOBuffPrefTime);
                }
                if (temp.colorName == "green")
                {
                    GameManager.Instance.createParticle(GameManager.Instance.greenNOBuffPref, spawnPos, GameManager.Instance.greenNOBuffPrefTime);
                }
                if (temp.colorName == "blue")
                {
                    GameManager.Instance.createParticle(GameManager.Instance.blueNOBuffPref, spawnPos, GameManager.Instance.blueNOBuffPrefTime);
                }
            }
        }
    }

    public void deathParticle(Vector3 spawnPos)
    {
        // death particle
        GameManager.Instance.createParticle(GameManager.Instance.deathPref, transform.position, GameManager.Instance.deathPrefTime);
    }

    public void punchParticle(Vector3 spawnPos)
    {
        GameManager.Instance.createParticle(GameManager.Instance.punchPref, spawnPos, GameManager.Instance.punchPrefTime);
    }

    public void sameCollectable(Color difColor,Vector3 spawnPos)
    {
        //float destroyTime = 2f;

        foreach (CharColor temp in charColors)
        {
            if (temp.colorType==difColor)
            {
                if (temp.colorName == "red")
                {
                    GameManager.Instance.createParticle(GameManager.Instance.redBuffPref, spawnPos, GameManager.Instance.redBuffPrefTime);
                }
                if (temp.colorName == "green")
                {
                    GameManager.Instance.createParticle(GameManager.Instance.greenBuffPref, spawnPos, GameManager.Instance.greenBuffPrefTime);
                }
                if (temp.colorName == "blue")
                {
                    GameManager.Instance.createParticle(GameManager.Instance.blueBuffPref, spawnPos, GameManager.Instance.blueBuffPrefTime);
                }
            }
        }
    }
}
