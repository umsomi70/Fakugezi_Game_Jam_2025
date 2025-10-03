using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDestinationLogic : MonoBehaviour
{
    [SerializeField] private LevelSelectScriptableObject sO;

    [SerializeField] private int level;

    [SerializeField] private GameObject egg;
    [SerializeField] private GameObject levelEndPanel;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if(PlayerInventory.Instance.objectInStomach == egg)
            {
                levelEndPanel.SetActive(true);
                Time.timeScale = 0f;
                PlayerMovement.Instance.canMove = false; 
                sO.levelsAvailable[level - 1] = true;
                sO.levelsAvailable[level] = true;
            }
        }
    }
}
