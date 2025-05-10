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

    // Yeni: Son girdi yönünü sakla
    private Vector2 inputDirection;
    private bool hasInputDirection;

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
        inputDirection = Vector2.zero;
        hasInputDirection = false;
        transform.position = startingPosition;
        rb.isKinematic = false;
        enabled = true;
    }

    public void SetInputDirection(Vector2 newInputDirection)
    {
        if (newInputDirection != Vector2.zero)
        {
            inputDirection = newInputDirection;
            hasInputDirection = true;
            TryChangeDirection();
        }
    }

    private void Update()
    {
        if (hasInputDirection)
        {
            TryChangeDirection();
        }
        else if (nextDirection != Vector2.zero)
        {
            TryApplyNextDirection();
        }
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = rb.position + speed * speedMultiplier * Time.fixedDeltaTime * direction;
        rb.MovePosition(newPosition);
    }

    private void TryChangeDirection()
    {
        // Girdi yönü mevcut yönle aynı değilse ve engel yoksa
        if (inputDirection != direction && !Occupied(inputDirection))
        {
            direction = inputDirection;
            nextDirection = Vector2.zero;
            hasInputDirection = false;
        }
        // Girdi yönünde engel varsa, bu yönü hatırla
        else if (inputDirection != direction)
        {
            nextDirection = inputDirection;
        }
    }

    private void TryApplyNextDirection()
    {
        if (!Occupied(nextDirection))
        {
            direction = nextDirection;
            nextDirection = Vector2.zero;
        }
    }

    public bool Occupied(Vector2 checkDirection)
    {
        if (checkDirection == Vector2.zero) return true;

        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            Vector2.one * 0.75f,
            0f,
            checkDirection,
            1.5f
        );

        return hit.collider != null && hit.collider.CompareTag(obstacleTag);
    }
}