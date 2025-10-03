using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ItemInStomach : MonoBehaviour
{
    [SerializeField] private GameObject itemPos;
    [SerializeField] private float rotSpeed;
    [SerializeField] private Vector3 rotation;

    private void Update()
    {
        GameObject itemInStomach = PlayerInventory.Instance.objectInStomach; 

        if (PlayerInventory.Instance.objectInStomach != null)
        {
            itemPos.SetActive(true);

            if (itemInStomach.tag == "seed")
            {
                itemPos.GetComponent<MeshFilter>().mesh = itemInStomach.GetComponent<SeedLogic>().chosenGUI.GetComponentInChildren<MeshFilter>().mesh;
                itemPos.GetComponent<MeshRenderer>().material = itemInStomach.GetComponent<SeedLogic>().chosenGUI.GetComponentInChildren<MeshRenderer>().material;
            }
            else if (itemInStomach.tag == "water")
            {
                itemPos.GetComponent<MeshFilter>().mesh = itemInStomach.GetComponent<MeshFilter>().sharedMesh; //make it the same mesh
                itemPos.GetComponent<MeshRenderer>().material = itemInStomach.GetComponent<MeshRenderer>().sharedMaterial;
            }
            else if (itemInStomach.tag == "egg")
            {
                itemPos.GetComponent<MeshFilter>().mesh = itemInStomach.transform.GetChild(0).GetComponentInChildren<MeshFilter>().mesh;
                itemPos.GetComponent<MeshRenderer>().material = itemInStomach.transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material;
            }

        }
        else if (PlayerInventory.Instance.objectInStomach == null)
        {
            itemPos.SetActive(false);
        }
    }

}
