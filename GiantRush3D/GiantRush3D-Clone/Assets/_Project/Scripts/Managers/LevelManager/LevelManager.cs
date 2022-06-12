using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Serializable]
    public class StageDatas
    {
        public string stageName;
        public StageTypes stageType;
        public GameObject stagePrefab;
    }
    public List<StageDatas> stageDatas;

    private int distance = 40;

    public enum StageTypes { None,EmptyStage,CollectableStage,ObstacleStage,FinalStage, DiamondStage }

    public List<Level> levels = new List<Level>();

    public GameObject parentObj;

    [Serializable]
    public class Stage
    {
        public string stageName;
        public StageTypes stageType;
    }

    [Serializable]
    public class Level
    {
        public string levelName;
        public List<Stage> stages;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void createLevel(int curLevelIndex)
    {
        int tempIndex= curLevelIndex % levels.Count;    // infinity level system
        //Debug.Log("LevelMng=>" + curLevelIndex);
        //Debug.Log("LevelsName==>" + levels[curLevelIndex].levelName);
        // clear currentLevel
        foreach (Transform child in parentObj.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < levels[tempIndex].stages.Count; i++)
        {
            for (int j = 0; j < stageDatas.Count; j++)
            {
                if (levels[tempIndex].stages[i].stageType == stageDatas[j].stageType)
                {
                    Debug.Log("StageName==>" + levels[tempIndex].stages[i].stageName);
                    Vector3 spawnPos = new Vector3(0, 0, transform.position.z + (i * distance));
                    GameObject tempStage = Instantiate(stageDatas[j].stagePrefab, spawnPos, Quaternion.identity);
                    tempStage.transform.SetParent(parentObj.transform);
                }
                

            }
          
        }
    }
}
