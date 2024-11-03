using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
