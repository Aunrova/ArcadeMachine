using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform inside;
    public Transform outside;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled && collision.gameObject.CompareTag("Obstacle"))
        {
            // Ghost'ın mevcut hareket yönünü tersine çevir
            ghost.movement.SetDirection(-ghost.movement.direction, true); // forced parametresi eklendi
        }
    }

    private IEnumerator ExitTransition()
    {
        // Movement script'ine erişim için doğru referans
        Movement movement = ghost.movement;
        movement.SetDirection(Vector2.up, true);
        movement.rb.isKinematic = true;
        movement.enabled = false;

        Vector3 position = transform.position;
        float duration = 0.5f;
        float elapsed = 0f;

        // İçeriye doğru hareket
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Dışarıya doğru hareket
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Rastgele yön seç (sol veya sağ)
        Vector2 newDirection = new Vector2(Random.value < 0.5f ? -1f : 1f, 0f);
        movement.SetDirection(newDirection, true);
        movement.rb.isKinematic = false;
        movement.enabled = true;
    }
}