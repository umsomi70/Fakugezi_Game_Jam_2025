using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private LevelSelectScriptableObject sO;
    [SerializeField] private Button[] levels;

    private void Start()
    {
        for (int i = 0; i < sO.levelsAvailable.Length; i++)
        {
            if (sO.levelsAvailable[i])
            {
                levels[i].interactable = true;
            }
            else levels[i].interactable = false;
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(1 + level);
    }
}
