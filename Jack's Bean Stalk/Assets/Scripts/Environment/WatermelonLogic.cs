 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonLogic : MonoBehaviour
{
    public bool falling;

    public bool[] moveDirection = new bool[4]; // 0 = fwd, 1 = back, 2 = left, 3 = right
    private Vector3[] directions =
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };
    [SerializeField] private LayerMask[] layers = new LayerMask[2];

    [SerializeField] private CottonScript cs_CottonScript;
    public bool inCotton;

    private void Update()
    {
        DirectionCheck();

        if (GetComponent<Rigidbody>().velocity.y < 0) falling = true;
        else falling = false;

        InCottonCheck();
    }

    private void DirectionCheck()
    {
        for(int i = 0; i < moveDirection.Length; i++)
        {
            if (ObjectDetection(directions[i], layers[0])/* || ObjectDetection(directions[i], layers[1]) || ObjectDetection(directions[i], layers[2])*/)
            {
                moveDirection[i] = false;
            }
            else moveDirection[i] = true;
        }
    }

    private bool ObjectDetection(Vector3 direction, LayerMask layerMask)
    {
        Ray ray = new Ray(transform.position, direction); // starts from center of the grid block

        return Physics.BoxCast(transform.position, new Vector3(0.475f, 0.475f, 0.475f), direction, Quaternion.identity,  1f, layerMask);
    }

    private void InCottonCheck()
    {
        if (inCotton && transform.position.x == cs_CottonScript.finalDestination.position.x && transform.position.z == cs_CottonScript.finalDestination.position.z)
        {
            GetComponent<Rigidbody>().useGravity = false;
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (transform.position.y > cs_CottonScript.finalDestination.position.y && !ObjectDetection(Vector3.down, layers[0]) && cs_CottonScript != null)
        {
            GetComponent<Rigidbody>().useGravity = true;
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
