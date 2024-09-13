using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 15f;
    private float horizontalInput;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        horizontalInput = moveSpeed * Time.deltaTime;
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }
}
