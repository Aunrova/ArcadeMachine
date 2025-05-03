using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Frogger : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;

    private SpriteRenderer spriteRenderer;
    private Vector3 spawnPosition;
    private float farthestRow;
    private bool cooldown;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Move(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left, true); // Adjusted for left move
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right, true); // Adjusted for right move
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down);
        }
    }

    private void Move(Vector3 direction, bool isHorizontal = false)
    {
        if (cooldown) return;

        // Default move distance
        float moveDistance = 1f * (2f / 3f);

        // Adjust horizontal movement to 2x the distance
        if (isHorizontal)
        {
            moveDistance *= 1.5f; // 2x for horizontal (left and right)
        }

        Vector3 destination = transform.position + direction.normalized * moveDistance;

        // Collision check for platform, obstacle, and barrier are all handled with a single layer
        Collider2D hitObject = Physics2D.OverlapPoint(destination);  // Check all objects on the same layer

        // If hit an obstacle, handle death
        if (hitObject != null && hitObject.CompareTag("Obstacle"))
        {
            Death();
            return;
        }

        // If hit a platform, attach or detach the frogger
        if (hitObject != null && hitObject.CompareTag("Platform"))
        {
            transform.SetParent(hitObject.transform);
        }
        else
        {
            transform.SetParent(null);
        }

        // If there's no issue, move to the destination
        StopAllCoroutines();
        StartCoroutine(Leap(destination));
    }

    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 startPosition = transform.position;

        float elapsed = 0f;
        float duration = 0.125f; // Adjust jump time if needed

        // Set initial state
        spriteRenderer.sprite = leapSprite;
        cooldown = true;

        while (elapsed < duration)
        {
            // Move towards the destination over time
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final state
        transform.position = destination;
        spriteRenderer.sprite = idleSprite;
        cooldown = false;
    }

    public void Respawn()
    {
        // Stop animations
        StopAllCoroutines();

        // Reset transform to spawn
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
        farthestRow = spawnPosition.y;

        // Reset sprite
        spriteRenderer.sprite = idleSprite;

        // Enable control
        gameObject.SetActive(true);
        enabled = true;
        cooldown = false;
    }

    public void Death()
    {
        // Stop animations
        StopAllCoroutines();

        // Disable control
        enabled = false;

        // Display death sprite
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;

        // Update game state (call GameManager instance)
        GameManager.Instance.Died();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool hitObstacle = other.CompareTag("Obstacle");
        bool onPlatform = transform.parent != null;

        // If the frogger hits an obstacle and is not on a platform, it dies
        if (enabled && hitObstacle && !onPlatform)
        {
            Death();
        }
    }
}
