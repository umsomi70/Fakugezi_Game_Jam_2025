using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CottonScript : MonoBehaviour
{

    [SerializeField] private float timeToMaxHeight;
    [SerializeField] private GameObject objectInPlant;
    public Transform finalDestination;
    public Vector3 finalDestHeight = new Vector3(0, 2f, 0);


    private void Start()
    {
        finalDestination.position = transform.position + finalDestHeight;
    }
    private void Update()
    {
        if(objectInPlant.transform.position.x == transform.position.x && objectInPlant.transform.position.z == transform.position.z && objectInPlant.transform.position.y <= finalDestination.position.y && objectInPlant != null)
        {
            objectInPlant.transform.position = Vector3.MoveTowards(objectInPlant.transform.position, finalDestination.position, timeToMaxHeight * Time.deltaTime);
        }
        else 
        {
            objectInPlant.GetComponent<Rigidbody>().useGravity = true;
            objectInPlant = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "watermelon" || other.tag == "egg")
        {
            objectInPlant = other.gameObject;
            objectInPlant.GetComponent<Rigidbody>().useGravity = false;
        }
    }
}
