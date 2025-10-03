using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggLogic : MonoBehaviour
{
    public bool inCotton;
    [SerializeField] private CottonScript cs_CottonScript;

    [SerializeField] private LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InCottonCheck();
    }

    private bool ObjectDetection(Vector3 direction, LayerMask layerMask)
    {
        Ray ray = new Ray(transform.position, direction); // starts from center of the grid block

        return Physics.BoxCast(transform.position, new Vector3(0.475f, 0.475f, 0.475f), direction, Quaternion.identity, 1f, layerMask);
    }

    private void InCottonCheck()
    {
        if (inCotton && transform.position.x == cs_CottonScript.finalDestination.position.x && transform.position.z == cs_CottonScript.finalDestination.position.z)
        {
            GetComponent<Rigidbody>().useGravity = false;
            cs_CottonScript.finalDestination.position = cs_CottonScript.transform.position + new Vector3(0, 1.75f, 0);
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = true;
            if (cs_CottonScript != null) cs_CottonScript.finalDestination.position = cs_CottonScript.transform.position + cs_CottonScript.finalDestHeight;
        }

        if (cs_CottonScript != null && transform.position.y > cs_CottonScript.finalDestination.position.y && !ObjectDetection(Vector3.down, layer))
        {
            GetComponent<Rigidbody>().useGravity = true;
            cs_CottonScript.finalDestination.position = cs_CottonScript.transform.position + cs_CottonScript.finalDestHeight;
            inCotton = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "cotton")
        {
            inCotton = true;
            cs_CottonScript = other.GetComponent<CottonScript>();
        }

    }

}
