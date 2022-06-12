using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossFight : MonoBehaviour
{
    public Text fightTheKingText;
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
    public void changeText(Text keyText, string value)
    {
        keyText.text = value;
    }
}
