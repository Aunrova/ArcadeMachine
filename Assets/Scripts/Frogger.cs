using System.Collections;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]
public class Frogger : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;
    [SerializeField] private float leftBoundX;
    [SerializeField] private float rightBoundX;

    private SpriteRenderer spriteRenderer;
    private Vector3 spawnPosition;
    private float farthestRow;
    private PlayerInput playerInput;
    private Vector3 moveInput;
    private const float TARGET_WORLD_SCALE = 0.4f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
        farthestRow = spawnPosition.y;
        playerInput = new PlayerInput();
        playerInput.Player.Enable();

        playerInput.Player.Move.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            moveInput = new Vector3(input.x, input.y, 0f);
        };

        playerInput.Player.Move.canceled += ctx =>
        {
            moveInput = Vector3.zero;
        };
    }

    private void Update()
    {
        if (moveInput != Vector3.zero)
        {
            Vector3 direction = Vector3.zero;

            if (moveInput.y > 0.5f)
            {
                direction = Vector3.up / 2.99f;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (moveInput.y < -0.5f)
            {
                direction = Vector3.down / 2.99f;
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else if (moveInput.x < -0.5f)
            {
                direction = Vector3.left / 2.55f;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else if (moveInput.x > 0.5f)
            {
                direction = Vector3.right / 2.55f;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }

            if (direction != Vector3.zero)
            {
                moveInput = Vector3.zero;
                Move(direction);
            }
        }

        if (transform.position.x < leftBoundX || transform.position.x > rightBoundX)
        {
            Death();
        }
    }
    private void LateUpdate()
{
    if (transform.parent != null)
    {
        Vector3 parentScale = transform.parent.lossyScale;
        transform.localScale = new Vector3(
            TARGET_WORLD_SCALE / parentScale.x,
            TARGET_WORLD_SCALE / parentScale.y,
            TARGET_WORLD_SCALE / parentScale.z
        );
    }
    else
    {
        transform.localScale = Vector3.one * TARGET_WORLD_SCALE;
    }
}

    private void Move(Vector3 direction)
    {
        Vector3 destination = transform.position + direction;
        // Küçük bir alan içinde tüm collider’ları al
        Collider2D[] hits = Physics2D.OverlapBoxAll(destination, Vector2.one * 0.1f, 0f);

        if (hits.Any(h => h.CompareTag("Barrier")))
            return;

        var platform = hits.FirstOrDefault(h => h.CompareTag("Platform"));
        if (platform != null)
        {
            SetParentKeepWorldScale(transform, platform.transform);
        }
        else
        {
            transform.SetParent(null, true);

            if (hits.Any(h => h.CompareTag("Water")))
            {
                Death();
                return;
            }
        }

        if (hits.Any(h => h.CompareTag("Obstacle")))
        {
            StartCoroutine(Leap(destination));
            return;
        }

        if (destination.y > farthestRow)
        {
            farthestRow = destination.y;
            GameManager.instance.AdvancedRow();
        }

        StartCoroutine(Leap(destination));
    }


    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 startPosition = transform.position;

        float elapsed = 0f;
        float duration = 0.125f;

        spriteRenderer.sprite = leapSprite;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
        spriteRenderer.sprite = idleSprite;
    }

    public void Death()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;
        transform.SetParent(null);
        enabled = false;

        GameManager.instance.Died();
    }

    public void Respawn()
    {
        StopAllCoroutines();
        transform.position = spawnPosition;
        transform.rotation = Quaternion.identity;
        farthestRow = spawnPosition.y;
        spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
        enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Death();
        }
    }
    
    void SetParentKeepWorldScale(Transform child, Transform newParent)
{
    // 1) Şu anki dünya ölçeğini al
    Vector3 desiredWorldScale = child.lossyScale;

    // 2) Parent’ı değiştir (worldPositionStays = true)
    child.SetParent(newParent, true);

    // 3) Parent’ın dünya ölçeğini al (null ise 1,1,1)
    Vector3 parentWorldScale = child.parent != null 
        ? child.parent.lossyScale 
        : Vector3.one;

    // 4) LocalScale’ı ayarla: desiredWorld / parentWorld
    child.localScale = new Vector3(
        desiredWorldScale.x / parentWorldScale.x,
        desiredWorldScale.y / parentWorldScale.y,
        desiredWorldScale.z / parentWorldScale.z
    );
}
}
