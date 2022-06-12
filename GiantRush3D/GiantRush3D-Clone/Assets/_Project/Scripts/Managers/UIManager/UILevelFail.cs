using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelFail : MonoBehaviour
{
    public void restart()
    {
        GameManager.Instance.restartLevel();
        GameManager.Instance.changeGameState(GameManager.gameStates.Start);
    }
}
