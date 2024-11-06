using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private InputController inputController;
    [SerializeField] private PlayerStats playerStats;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
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

        float xSpeed = playerStats.horizontalIdleSpeed;

        if (inputVector.x > 0)
        {
            xSpeed = playerStats.horizontalSpeed;
        }
        else if (inputVector.x < 0)
        {
            xSpeed = -playerStats.horizontalReverseSpeed;
        }

        float ySpeed = inputVector.y * playerStats.verticalSpeed;

        rb.linearVelocity = new Vector2(xSpeed, ySpeed);

        animator.SetFloat("Tilt", inputVector.y);
    }
}
