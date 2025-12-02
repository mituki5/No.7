using UnityEngine;

public class GoombaEnemy : MonoBehaviour
{
    public float speed = 1f;
    public Transform leftLimit;
    public Transform rightLimit;

    bool movingRight = true;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            if (transform.position.x > rightLimit.position.x)
                movingRight = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            if (transform.position.x < leftLimit.position.x)
                movingRight = true;
        }
    }
}
