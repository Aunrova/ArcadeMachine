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
            Move(Vector3.left, true);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right, true);
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

        float moveDistance = 1f * (2f / 3f);

        if (isHorizontal)
        {
            moveDistance *= 1.5f;
        }

        Vector3 destination = transform.position + direction.normalized * moveDistance;

        Collider2D hitObject = Physics2D.OverlapPoint(destination);

        if (hitObject != null && hitObject.CompareTag("Obstacle"))
        {
            Death();
            return;
        }

        if (hitObject != null && hitObject.CompareTag("Platform"))
        {
            transform.SetParent(hitObject.transform);
        }
        else
        {
            transform.SetParent(null);
        }

        StopAllCoroutines();
        StartCoroutine(Leap(destination));
    }

    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 startPosition = transform.position;

        float elapsed = 0f;
        float duration = 0.125f;

        spriteRenderer.sprite = leapSprite;
        cooldown = true;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
        spriteRenderer.sprite = idleSprite;
        cooldown = false;
    }

    public void Respawn()
    {
        StopAllCoroutines();

        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
        farthestRow = spawnPosition.y;

        spriteRenderer.sprite = idleSprite;

        gameObject.SetActive(true);
        enabled = true;
        cooldown = false;
    }

    public void Death()
    {
        StopAllCoroutines();

        enabled = false;

        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;

        GameManager.Instance.Died();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool hitObstacle = other.CompareTag("Obstacle");
        bool onPlatform = transform.parent != null;

        if (enabled && hitObstacle && !onPlatform)
        {
            Death();
        }
    }
}
