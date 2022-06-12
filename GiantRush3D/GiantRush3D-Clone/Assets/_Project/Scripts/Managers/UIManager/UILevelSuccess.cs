using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSuccess : MonoBehaviour
{
    public Text stackText;
    public Text diamondText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeText(Text keyText,string value)
    {
        keyText.text = value;
    }

    public void successToStart()
    {
        // increase the levelIndex
        GameManager.Instance.scriptableCurrentLevelIndex.savedLevel++;
        GameManager.Instance.createNextLevel();
        GameManager.Instance.changeGameState(GameManager.gameStates.Start);
    }
}
