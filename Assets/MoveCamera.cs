using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    private void Start()
    {
        //transform.position = cameraPosition.position;
    }
    private void Update()
    {
        this.transform.position = cameraPosition.position;
    }
}
