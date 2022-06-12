using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    public Text fightTheKingText;
    public Text stackText;
    public Text diamondText;
    public Text godModeText;
    public Text currentLevelText;
    // Start is called before the first frame update
    void Start()
    {
        currentLevelText.text = "LEVEL - "+(GameManager.Instance.scriptableCurrentLevelIndex.savedLevel + 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeText(Text keyText, string value)
    {
        keyText.text = value;
    }
}
