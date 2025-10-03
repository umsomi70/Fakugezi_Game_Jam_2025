using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeScript : MonoBehaviour
{
    [SerializeField] private LayerMask m_LayerMask;

    [SerializeField] private float timeToFinalDest;
    [SerializeField] private GameObject objectInPlant;

    [SerializeField] private Transform chosenFinalDest;
    [SerializeField] private Transform[] finalDestinations;
    [SerializeField] private Vector3[] _finalDestinations = new Vector3[4];
    [SerializeField] private float maxRadius;
    [SerializeField]
    private Vector3[] directions =
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    private void Start()
    {
        finalDestinations[0].position = new Vector3(0, 0, maxRadius) + transform.position;
        _finalDestinations[0] = finalDestinations[0].position;

        finalDestinations[1].position = new Vector3(0, 0, -maxRadius) + transform.position;
        _finalDestinations[1] = finalDestinations[1].position;

        finalDestinations[2].position = new Vector3(-maxRadius, 0, 0) + transform.position;
        _finalDestinations[2] = finalDestinations[2].position;

        finalDestinations[3].position = new Vector3(maxRadius, 0, 0) + transform.position;
        _finalDestinations[3] = finalDestinations[3].position;
    }
    private void Update()
    {
        MoveObject();


        if (objectInPlant != null && chosenFinalDest != null)
        {
            objectInPlant.layer = 0;

            if (objectInPlant == PlayerMovement.Instance.gameObject)
            {
                PlayerMovement.Instance.canMove = false;
            }


            if (objectInPlant.transform.position == chosenFinalDest.position)
            {
                objectInPlant.GetComponent<Rigidbody>().useGravity = true;
                switch (objectInPlant.tag)
                {
                    case "watermelon":
                        objectInPlant.layer = 8;
                        break;
                    case "seed":
                    case "egg":
                        objectInPlant.layer = 7;
                        break;
                    case "Player":
                        objectInPlant.layer = 0;
                        PlayerMovement.Instance.canMove = true;
                        break;
                    default:
                        break;
                }
                objectInPlant = null;
                chosenFinalDest = null;
            }
        }
    }

    private void MoveObject()
    {
        if (objectInPlant != null)
        {
            objectInPlant.GetComponent<Rigidbody>().useGravity = false;
            objectInPlant.transform.position = Vector3.MoveTowards(objectInPlant.transform.position, Direction().position, timeToFinalDest * Time.deltaTime);
            chosenFinalDest = Direction();
        }
    }

    private Transform Direction()
    {
        if (PlayerMovement.Instance.playerGUI.transform.rotation.eulerAngles == PlayerMovement.Instance.rotation[0])
        {
            DirectionCheck(0, finalDestinations[0]);
            return finalDestinations[0];
        }
        else if (PlayerMovement.Instance.playerGUI.transform.rotation.eulerAngles == PlayerMovement.Instance.rotation[1])
        {
            DirectionCheck(1, finalDestinations[1]);
            return finalDestinations[1];
        }
        else if (PlayerMovement.Instance.playerGUI.transform.rotation.eulerAngles == PlayerMovement.Instance.rotation[2])
        {
            DirectionCheck(2, finalDestinations[2]);
            return finalDestinations[2];
        }
        else if (PlayerMovement.Instance.playerGUI.transform.rotation.eulerAngles == PlayerMovement.Instance.rotation[3])
        {
            DirectionCheck(3, finalDestinations[3]);
            return finalDestinations[3];
        }
        else return null;
    }

    private void DirectionCheck(int _direction, Transform finalDest)
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, new Vector3(0.45f, 0.45f, 0.45f), directions[_direction], out hit, Quaternion.identity, maxRadius, m_LayerMask))
        {
            GameObject _other = hit.collider.gameObject;

            float xDifference = Mathf.Abs(transform.position.x - _other.transform.position.x) - 1;
            float zDifference = Mathf.Abs(transform.position.z - _other.transform.position.z) - 1;

            float xDif = (finalDest.position.x - transform.position.x > 0) ? xDifference : -xDifference;
            float zDif = (finalDest.position.z - transform.position.z > 0) ? zDifference : -zDifference;

            float difference = (xDifference > zDifference) ? xDifference : zDifference;

            if (difference == xDifference) // z the same
            {
                finalDest.position = new Vector3(transform.position.x + xDif, transform.position.y, finalDest.position.z);
            }
            else
            {
                finalDest.position = new Vector3(finalDest.position.x, transform.position.y, transform.position.z + zDif);
            }

            Debug.Log("Hit" + _other);
        }
        else if (!Physics.BoxCast(transform.position, new Vector3(0.45f, 0.45f, 0.45f), directions[_direction], out hit, Quaternion.identity, maxRadius, m_LayerMask))
        {
            finalDest.position = _finalDestinations[_direction];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "trigger") objectInPlant = other.gameObject;
    }
}
