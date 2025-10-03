using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedLogic : MonoBehaviour
{
    public SeedType seedType;
    public SeedState seedState;
    [SerializeField] private Transform plantParent;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask ground;

    [Header("Fruits")]
    [SerializeField] private GameObject watermelon;
    [SerializeField] private GameObject cotton;
    [SerializeField] private GameObject grape;
    [SerializeField] private GameObject[] GUI;
    public GameObject chosenGUI;

    private void Start()
    {
        plantParent = GameObject.Find("Plants").transform;

        switch (seedType)
        {
            case SeedType.Watermelon:
                GUI[0].SetActive(true);
                GUI[1].SetActive(false);
                GUI[2].SetActive(false);

                chosenGUI = GUI[0];
                break;
            case SeedType.Cotton:
                GUI[0].SetActive(false);
                GUI[1].SetActive(true);
                GUI[2].SetActive(false);

                chosenGUI = GUI[1];
                break;
            case SeedType.Grape:
                GUI[0].SetActive(false);
                GUI[1].SetActive(false);
                GUI[2].SetActive(true);

                chosenGUI = GUI[2];
                break;
        }
    }
    private void Update()
    {
        PlantedCheck();
        WateredCheck();
    }

    private void PlantedCheck()
    {
        RaycastHit hit;
        bool grounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.75f, ground);

        if (grounded && hit.collider.tag == "plantable")
        {
            seedState = SeedState.Planted;
        }
    }

    private void WateredCheck()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 0.5f, ground);

        if (hit.collider.gameObject != null)
        {
            if (hit.collider.gameObject.GetComponent<GroundScript>().watered == true && seedState == SeedState.Planted)
            {
                seedState = SeedState.Watered;
                FruitCheck();
                hit.collider.gameObject.GetComponent<GroundScript>().watered = false;
                Destroy(gameObject);
            }

        }


    }

    private void FruitCheck()
    {
        switch (seedType)
        {
            case SeedType.Watermelon:
                Instantiate(watermelon, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity, plantParent);
                break;
            case SeedType.Cotton:
                Instantiate(cotton, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity, plantParent);
                break;
            case SeedType.Grape:
                Instantiate(grape, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity, plantParent);
                break;

        }
    }
}

public enum SeedType
{
    Watermelon,
    Cotton,
    Grape
}

public enum SeedState
{
    Null,
    Planted,
    Watered
}
