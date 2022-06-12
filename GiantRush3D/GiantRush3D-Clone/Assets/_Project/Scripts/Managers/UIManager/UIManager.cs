using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject currentUI;

    public string CanvasElementsResourcePathName;

    #region UIElementData
    public enum UIElementsID { None,UIStart,UIGamePlay,UILevelSuccess,UILevelFail,UIBossFight};

    [Serializable]
    public class UIElements
    {
        public string UIElementResourceName;
        public UIElementsID uIElementID;
    }
    #endregion

    public List<UIElements> uIElements = new List<UIElements>();

    public Transform UIPanels;

    private void Awake()
    {
        Instance = this;
    }

    public void createUIElement(UIElementsID uIElement)
    {
        // destroy last UIElement
        foreach (Transform item in UIPanels)
        {
            // destroy last panel
            Destroy(item.gameObject);
        }
        foreach (UIElements nextUI in uIElements)
        {
            if(nextUI.uIElementID == uIElement)
            {
                // **
                //Debug.Log("Current UI => " + nextUI.UIElementResourceName);
                string tempPath = CanvasElementsResourcePathName + "/" + nextUI.UIElementResourceName;
                var tempPanel = Instantiate(Resources.Load(tempPath), UIPanels) as GameObject;
                currentUI = tempPanel;
            }
        }
    }
}
