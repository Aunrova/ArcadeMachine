using System.Collections;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
        farthestRow = spawnPosition.y;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Move(Vector3.up/3f);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left/2.55f);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right/2.55f);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down/3f);
        }

        if(transform.position.x < leftBoundX)
        {
            Death();
        }
        else if (transform.position.x > rightBoundX)
        {
            Death();
        }

    }

    private void Move(Vector3 direction)
{
    Vector3 destination = transform.position + direction;

    Collider2D hit = Physics2D.OverlapBox(destination, Vector2.one * 0.1f, 0f);

    if (hit != null)
    {
        if (hit.CompareTag("Barrier"))
        {
            return;
        }

        if (hit.CompareTag("Platform"))
        {
            transform.SetParent(hit.transform);
        }
        else
        {
            transform.SetParent(null);
        }

        if (hit.CompareTag("Obstacle"))
        {
            transform.position = destination;
            return;
        }
    }
    else
    {
        transform.SetParent(null);
    }

    if(destination.y > farthestRow)
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

    public void Death(){
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;
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
        if(collision.CompareTag("Obstacle"))
        {
            Debug.Log("Hit an obstacle!");
            Death();
        }
    }
}
