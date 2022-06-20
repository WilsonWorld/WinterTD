using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLoop : MonoBehaviour
{
    public float RotationSpeed = 360.0f;
    public bool IsRotatingX = false, IsRotatingY = false;

    // Update is called once per frame
    void Update()
    {
        if (IsRotatingX)
            transform.Rotate(0.0f, RotationSpeed * Time.deltaTime, 0.0f, Space.Self);

        if (IsRotatingY)
            transform.Rotate(0.0f, RotationSpeed * Time.deltaTime, 0.0f, Space.World);
    }
}
