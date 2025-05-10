using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private string obstacleTag = "Obstacle";
    public readonly List<Vector2> availableDirections = new();

    private void Start()
    {
        availableDirections.Clear();
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    private void CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f);

        if (hit.collider == null || !hit.collider.CompareTag("Obstacle"))
        {
            availableDirections.Add(direction);
            Debug.DrawRay(transform.position, direction, Color.green, 2f);
        }
    }
}