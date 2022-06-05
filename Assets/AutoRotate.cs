using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float speed = 10f;
    public enum Direction {
        x,
        y,
        z
    };
    public Direction direction = Direction.x;

    void Update()
    {
        switch (direction)
        {
            case Direction.x:
                transform.Rotate(Vector3.right, speed * Time.deltaTime);
                break;
            case Direction.y:
                transform.Rotate(Vector3.up, speed * Time.deltaTime);
                break;
            case Direction.z:
                transform.Rotate(Vector3.forward, speed * Time.deltaTime);
                break;
        }
    }
}
