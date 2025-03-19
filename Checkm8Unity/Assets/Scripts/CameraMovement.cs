using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraScrollSpeed;
    [SerializeField] private bool isMoving = false;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (isMoving)
        {
            transform.Translate(0f, cameraScrollSpeed, 0f);
        }
        else
        {
            // transform.position = startPosition;
        }
    }
}
