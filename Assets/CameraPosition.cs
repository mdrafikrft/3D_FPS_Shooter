using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] Transform CameraPositionInsidePlayer;
    private void Update()
    {
        transform.position = CameraPositionInsidePlayer.position;
    }
}
