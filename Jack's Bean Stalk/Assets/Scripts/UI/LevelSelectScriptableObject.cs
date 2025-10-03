using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Manager", menuName = "ScriptableObjects/Level Manager", order = 1)]
public class LevelSelectScriptableObject : ScriptableObject
{
    public bool[] levelsAvailable;
}
