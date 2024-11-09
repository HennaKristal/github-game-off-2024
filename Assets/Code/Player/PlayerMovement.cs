using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private PlayerStats playerStats;
    private CameraMovement cameraMovement;
    private InputController inputController;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        cameraMovement = GameObject.Find("Camera Follow Target").GetComponent<CameraMovement>();
        inputController = GameManager.Instance.GetComponent<InputController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 inputVector = new Vector2(inputController.Move.x, inputController.Move.y);

        if (inputVector.magnitude > 1)
        {
            inputVector = inputVector.normalized;
        }

        float xSpeed = inputVector.x * (playerStats.horizontalSpeed / 100) + cameraMovement.speed;
        float ySpeed = inputVector.y * (playerStats.verticalSpeed / 100);

        rb.linearVelocity = new Vector2(xSpeed, ySpeed);

        animator.SetFloat("Tilt", inputVector.y);
    }
}
