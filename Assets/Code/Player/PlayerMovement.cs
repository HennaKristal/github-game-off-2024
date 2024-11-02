using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private InputController inputController;
    [SerializeField] private PlayerStats playerStats;
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        rb.linearVelocity = new Vector2(
            inputController.Move.x * playerStats.horizontalSpeed,
            inputController.Move.y * playerStats.verticalSpeed
        );
    }
}
