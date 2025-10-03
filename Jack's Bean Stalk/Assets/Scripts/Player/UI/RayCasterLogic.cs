using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCasterLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Material[] materials = new Material[2]; // 0 = clear, 1 = white
    private MeshRenderer meshRenderer;

    public GameObject objectInTrigger;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask water;

    [Header("Bool Checks")]
    [SerializeField] private bool mouseOn;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        MaterialCheck();
        EatCheck();
        PoopCheck();
    }

    private void EatCheck()
    {
        if (mouseOn && Input.GetMouseButtonDown(0) && PlayerInventory.Instance.objectInStomach == null)
        {
            if (objectInTrigger != null)
            {
                PlayerInventory.Instance.objectInStomach = objectInTrigger;
                objectInTrigger.SetActive(false);
                objectInTrigger = null;
            }
            else if (WaterCheck())
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, Vector3.down);
                Physics.Raycast(ray, out hit, 0.5f, water);

                GameObject _water = hit.collider.gameObject.GetComponent<WaterScript>().water;

                PlayerInventory.Instance.objectInStomach = _water;
            }
        }
    }


    private void PoopCheck()
    {
        if (mouseOn && Input.GetMouseButtonDown(1) && GroundCheck())
        {
            GameObject objectInStomach = PlayerInventory.Instance.objectInStomach;

            switch (objectInStomach.tag)
            {
                case "seed":
                    if (PlantableCheck())
                    {
                        objectInStomach.SetActive(true);

                        objectInStomach.transform.position = transform.position + new Vector3(0, 0.5f, 0);

                        PlayerInventory.Instance.objectInStomach = null;
                    }
                    break;
                case "water":
                    RaycastHit hit;
                    Ray ray = new Ray(transform.position, Vector3.down);
                    Physics.Raycast(ray, out hit, 0.5f, ground);

                    hit.collider.gameObject.GetComponent<GroundScript>().watered = true;

                    PlayerInventory.Instance.objectInStomach = null;
                    break;
                case "egg":
                    objectInStomach.SetActive(true);

                    objectInStomach.transform.position = transform.position + new Vector3(0, 0.5f, 0);

                    PlayerInventory.Instance.objectInStomach = null;
                    break;

            }


        }
    }

    private void MaterialCheck()
    {
        if (mouseOn && GroundCheck()) meshRenderer.material = materials[1];
        else if (mouseOn && WaterCheck()) meshRenderer.material = materials[1];
        else meshRenderer.material = materials[0];
    }
    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.35f, ground);
    }

    private bool WaterCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.5f, water);
    }

    private bool PlantableCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f, ground) && hit.collider.tag == "plantable")
        {
            return Physics.Raycast(transform.position, Vector3.down, 0.5f, ground);
        }
        else return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "seed":
            case "egg":
                objectInTrigger = other.gameObject;
                break;
            default:
                objectInTrigger = null;
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "seed":
            case "egg":
                objectInTrigger = other.gameObject;
                break;
            default:
                objectInTrigger = null;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "seed":
            case "egg":
                objectInTrigger = null;
                break;
        }

    }


    #region Pointer Events
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOn = false;
    }
    #endregion

}
