using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public GameObject objectInStomach;

    private void Update()
    {
        if (objectInStomach != null)
        {
            objectInStomach.transform.position = transform.position + new Vector3(0, 0.25f, 0);
        }
    }

    private void Awake()
    {
        Instance = this;
    }


}
