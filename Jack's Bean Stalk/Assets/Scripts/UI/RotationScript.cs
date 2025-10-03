using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Vector3 rotationSpeed;

    private void Update()
    {
        transform.Rotate(rotationSpeed * time * Time.deltaTime);
    }
}
