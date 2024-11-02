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

        // Normalize the vector for diagonal movement
        if (inputVector.magnitude > 1)
        {
            inputVector = inputVector.normalized;
        }

        rb.linearVelocity = new Vector2(
            inputVector.x * playerStats.horizontalSpeed,
            inputVector.y * playerStats.verticalSpeed
        );

        animator.SetFloat("Tilt", inputVector.y);
    }
}
