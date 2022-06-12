using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : MonoBehaviour
{

    public Text diamondsTxt;
    public Text diamondValueText;
    public Text stackValueText;

    private void Start()
    {
        changeDiamondsTxt();
        changeStackValueTxt();
        changeDiaValueTxt();
    }

    public void startToGamePlay()
    {
        GameManager.Instance.changeGameState(GameManager.gameStates.GamePlay);
    }

    public void increaseDiamondValue()
    {
        if (GameManager.Instance.gameEconomy.diamondCounter> GameManager.Instance.diamondIncrease)
        {
            GameManager.Instance.diamondIncrease += 10;
            // decrease all diamonds
            GameManager.Instance.gameEconomy.diamondCounter -= GameManager.Instance.diamondIncrease;
            changeDiamondsTxt();
            changeDiaValueTxt();
        }
    }

    public void increaseStackValue()
    {
        if (GameManager.Instance.gameEconomy.diamondCounter> GameManager.Instance.stackIncrease)
        {
            GameManager.Instance.stackIncrease += 10;
            // decrease all diamonds
            GameManager.Instance.gameEconomy.diamondCounter -= GameManager.Instance.stackIncrease;
            changeDiamondsTxt();
            changeStackValueTxt();
        }
    }

    void changeStackValueTxt()
    {
        stackValueText.text = GameManager.Instance.stackIncrease + " Diamond";
    }

    void changeDiaValueTxt()
    {
        diamondValueText.text = GameManager.Instance.diamondIncrease + " Diamond";
    }

    void changeDiamondsTxt()
    {
        diamondsTxt.text = GameManager.Instance.gameEconomy.diamondCounter+" Diamonds";
    }
}
