using System.Linq;
using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled || ghost.frightened.enabled) return;

        Node node = other.GetComponent<Node>();
        if (node != null && node.availableDirections.Count > 0)
        {
            var validDirections = node.availableDirections
                .Where(dir => dir != -ghost.movement.direction)
                .ToList();

            if (validDirections.Count == 0) return;

            Vector2 direction = validDirections.OrderBy(dir =>
                Vector2.Distance(
                    transform.position + (Vector3)dir,
                    ghost.target.position
                )).First();

            ghost.movement.SetDirection(direction);
        }
    }
}
