using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 3f;

    private void FixedUpdate()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
