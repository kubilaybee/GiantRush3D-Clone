using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region CamShakeDatas
    [Header("CamShakerDatas")]
    public float shakeTime;
    public float shakePower;
    #endregion

    #region cam
    [Header("CamDatas")]
    public Camera cam;
    #endregion

    #region mainCharacter
    public GameObject mainCharPrefab;
    public GameObject mainChar;
    #endregion

    #region GameStatus
    public enum gameStates { None, Start, GamePlay, LevelSucces, LevelFail ,BossFight};
    [Header("CurrentGameState")]
    public gameStates currentGameState;
    #endregion

    #region currentLevelIndex
    public gameLevel scriptableCurrentLevelIndex;
    #endregion

    #region GameEconomy
    [Header("Game Economy")]
    public gameEco gameEconomy;
    public int diamondIncrease;
    public int godModeIncrease;
    public int stackIncrease;
    public int godModeClosed;
    public int godModeActivatedValue;
    // current level economy
    //[Header("Current Level Economy")]
    //public int currentDiamondCounter;
    //public int currentStackCounter;
    //public int currentGodMode;
    #endregion

    #region Particles
    [Header("Particles")]
    public GameObject deathPref;
    public float deathPrefTime;
    public GameObject punchPref;
    public float punchPrefTime;
    // increasePrefs
    public GameObject redBuffPref;
    public float redBuffPrefTime;
    public GameObject greenBuffPref;
    public float greenBuffPrefTime;
    public GameObject blueBuffPref;
    public float blueBuffPrefTime;
    // decreasePrefs
    public GameObject redNOBuffPref;
    public float redNOBuffPrefTime;
    public GameObject greenNOBuffPref;
    public float greenNOBuffPrefTime;
    public GameObject blueNOBuffPref;
    public float blueNOBuffPrefTime;
    #endregion

    // createParticle
    public void createParticle(GameObject particlePref,Vector3 spawnPos,float destroyTime)
    {
        GameObject tempParticle = Instantiate(particlePref);
        tempParticle.transform.position = spawnPos;
        Destroy(tempParticle, destroyTime);
    }

    private void Awake()
    {
        Instance = this;
        // create mainCharacter
        mainChar = Instantiate(mainCharPrefab);
        // camFollow /PLAYER
        cam.GetComponent<CamMovement>().Target = mainChar.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        changeGameState(gameStates.Start);
        // create the level
        LevelManager.Instance.createLevel(scriptableCurrentLevelIndex.savedLevel);
        //Debug.Log("Levels:" + LevelManager.Instance.levels.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.currentUI.GetComponent<UIGamePlay>())
        {
            UIManager.Instance.currentUI.GetComponent<UIGamePlay>().changeText(UIManager.Instance.currentUI.GetComponent<UIGamePlay>().diamondText, gameEconomy.currentDiamondCounter + " Diamond");
            UIManager.Instance.currentUI.GetComponent<UIGamePlay>().changeText(UIManager.Instance.currentUI.GetComponent<UIGamePlay>().stackText, gameEconomy.currentStackCounter + " Stack");
        }
        if (UIManager.Instance.currentUI.GetComponent<UIBossFight>())
        {
            UIManager.Instance.currentUI.GetComponent<UIBossFight>().changeText(UIManager.Instance.currentUI.GetComponent<UIBossFight>().diamondText, gameEconomy.currentDiamondCounter + " Diamond");
            UIManager.Instance.currentUI.GetComponent<UIBossFight>().changeText(UIManager.Instance.currentUI.GetComponent<UIBossFight>().stackText, gameEconomy.currentStackCounter + " Stack");
        }


        #region gameStateTEST
        if (Input.GetKeyDown("1"))
        {
            changeGameState(gameStates.Start);
        }
        if (Input.GetKeyDown("2"))
        {
            changeGameState(gameStates.GamePlay);
        }
        if (Input.GetKeyDown("3"))
        {
            changeGameState(gameStates.LevelSucces);
        }
        if (Input.GetKeyDown("4"))
        {
            changeGameState(gameStates.LevelFail);
        }
        if (Input.GetKeyDown("5"))
        {
            changeGameState(gameStates.BossFight);
        }
        #endregion
    }

    public void changeGameState(gameStates nextGameState)
    {
        currentGameState = nextGameState;

        switch (nextGameState)
        {
            case gameStates.None:
                break;
            case gameStates.Start:
                UIManager.Instance.createUIElement(UIManager.UIElementsID.UIStart);
                resetCurrentEconomy();
                break;
            case gameStates.GamePlay:
                UIManager.Instance.createUIElement(UIManager.UIElementsID.UIGamePlay);
                break;
            case gameStates.LevelSucces:
                UIManager.Instance.createUIElement(UIManager.UIElementsID.UILevelSuccess);
                // success level add score
                gameEconomy.diamondCounter += gameEconomy.currentDiamondCounter + (gameEconomy.currentStackCounter * 5);
                UIManager.Instance.currentUI.GetComponent<UILevelSuccess>().changeText(UIManager.Instance.currentUI.GetComponent<UILevelSuccess>().diamondText, gameEconomy.currentDiamondCounter+" Diamond");
                UIManager.Instance.currentUI.GetComponent<UILevelSuccess>().changeText(UIManager.Instance.currentUI.GetComponent<UILevelSuccess>().stackText, gameEconomy.currentStackCounter+" Stack");
                break;
            case gameStates.LevelFail:
                cam.GetComponent<CamMovement>().camShaker(shakeTime, shakePower);
                UIManager.Instance.createUIElement(UIManager.UIElementsID.UILevelFail);
                break;
            case gameStates.BossFight:
                UIManager.Instance.createUIElement(UIManager.UIElementsID.UIBossFight);
                mainChar.transform.DOMoveX(0, 1f);
                break;
            default:
                break;
        }
    }

    public IEnumerator godModeSlayer()
    {
        // change shader
        mainChar.GetComponent<MainChar>().shaderRainbow();
        // change UI text
        UIManager.Instance.currentUI.GetComponent<UIGamePlay>().godModeText.color = Color.magenta;

        while (gameEconomy.currentGodMode>1)
        {
            if (!mainChar.GetComponent<MainChar>().godMode)
            {
                mainChar.GetComponent<MainChar>().shaderStandard();
            }
            gameEconomy.currentGodMode -= godModeClosed;
            //Debug.Log("GodMode.." + gameEconomy.currentGodMode);
            yield return new WaitForSeconds(1);
        }
        //Debug.Log("TESTTTTTTTTTTTTTTTTT");
        mainChar.GetComponent<MainChar>().godMode = false;

        mainChar.GetComponent<MainChar>().getRandomColor();
        // change shader
        mainChar.GetComponent<MainChar>().shaderStandard();

        if (UIManager.Instance.currentUI.GetComponent<UIGamePlay>())
        {
            UIManager.Instance.currentUI.GetComponent<UIGamePlay>().godModeText.color = Color.black;
        }
    }

    // stack economy
    public void increaseCurrentStack()
    {
        gameEconomy.currentStackCounter += stackIncrease;
    }
    public void decreaseCurrentStack()
    {
        if (gameEconomy.currentStackCounter!=0)
        {
            gameEconomy.currentStackCounter -= stackIncrease;
        }
    }
    // godMode economy
    public void increaseCurrentGodMode()
    {
        gameEconomy.currentGodMode += godModeIncrease;
    }
    public void decreaseCurrentGodMode()
    {
        gameEconomy.currentGodMode -= godModeIncrease;
    }
    // diamond economy
    public void increaseCurrentDiamond()
    {
        gameEconomy.currentDiamondCounter += diamondIncrease;
    }

    void resetCurrentEconomy()
    {
        // resetCurrentEconomy
        gameEconomy.currentDiamondCounter = 0;
        gameEconomy.currentGodMode = 0;
        gameEconomy.currentStackCounter = 0;
    }

    public void restartLevel()
    {
        restartPlayer();
        LevelManager.Instance.createLevel(scriptableCurrentLevelIndex.savedLevel);
    }

    public void createNextLevel()
    {
        //Debug.Log(currentLevelIndex);
        restartPlayer();
        LevelManager.Instance.createLevel(scriptableCurrentLevelIndex.savedLevel);
    }

    public void restartPlayer()
    {
        // camFollow /PLAYER
        cam.GetComponent<CamMovement>().Target = mainChar.transform;
        cam.GetComponent<CamMovement>().defaultCamRotation();
        // main char datas
        mainChar.GetComponent<MainChar>().stayAnim(true);
        mainChar.GetComponent<MainChar>().health = 100;
        mainChar.GetComponent<MainChar>().shaderRainbow();
        mainChar.transform.localScale = new Vector3(1, 1, 1);
        mainChar.transform.position = new Vector3(mainCharPrefab.transform.position.x,mainCharPrefab.transform.position.y,mainCharPrefab.transform.position.z);
        mainChar.GetComponent<MainChar>().fightReady(false);
        mainChar.GetComponent<MainChar>().firstPunch(false);
        mainChar.GetComponent<MainChar>().secondPunch(false);
        mainChar.GetComponent<MainChar>().fightAreaEnter = false;
        mainChar.GetComponent<MainChar>().godMode = false;
        mainChar.GetComponent<MainChar>().healthbar.FullHealth();
        mainChar.GetComponent<MainChar>().healthbarCanvas.enabled = false;
    }
}
