using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    [SerializeField] private string obstacleTag = "Obstacle";

    public Rigidbody2D rb { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rb.isKinematic = false;
        enabled = true;
    }



    private void Update()
    {
        if (nextDirection != Vector2.zero && !Occupied(nextDirection))
        {
            direction = nextDirection;
            nextDirection = Vector2.zero;
        }
    }


    private void FixedUpdate()
    {
        Vector2 newPosition = rb.position + speed * speedMultiplier * Time.fixedDeltaTime * direction;
        rb.MovePosition(newPosition);
    }

    public void SetDirection(Vector2 newDirection, bool forced = false)
    {
        if (newDirection == direction) return;

        if (forced || !Occupied(newDirection))
        {
            direction = newDirection;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = newDirection;
        }
    }

    private void TryApplyNextDirection()
    {
        if (!Occupied(nextDirection))
        {
            ApplyDirection(nextDirection);
            nextDirection = Vector2.zero;
        }
    }

    private void ApplyDirection(Vector2 newDirection)
    {
        direction = newDirection;
        nextDirection = Vector2.zero;
    }

    private void QueueDirection(Vector2 newDirection)
    {
        nextDirection = newDirection;
    }

    private bool Occupied(Vector2 checkDirection)
    {
        if (checkDirection == Vector2.zero) return true;

        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            Vector2.one * 0.75f,
            0f,
            checkDirection,
            1.5f
        );

        Debug.DrawRay(transform.position, checkDirection * 1.5f,
                     hit.collider != null ? Color.red : Color.green, 0.1f);

        return hit.collider != null && hit.collider.CompareTag(obstacleTag);
    }
}