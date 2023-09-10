using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            y += 0.05f;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            y -= 0.05f;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            x += 0.05f;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x -= 0.05f;
        }

        transform.position = new Vector3(x, y, z);
    }
}
