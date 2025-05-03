using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public Vector2 direction = Vector2.right;  // Hareket yönü
    public float speed = 1f;  // Hareket hızı
    public int size = 1;  // Nesnenin boyutu (kenarlara çarpma tespiti için)

    private Vector3 leftEdge;  // Sol kenar
    private Vector3 rightEdge;  // Sağ kenar

    private void Start()
    {
        // Ekranın sol ve sağ kenarlarını dünya koordinatlarına çeviriyoruz
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    private void Update()
    {
        // Sağ kenara geçtiyse sol tarafa geç
        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x)
        {
            transform.position = new Vector3(leftEdge.x - size, transform.position.y, transform.position.z);
        }
        // Sol kenara geçtiyse sağ tarafa geç
        else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            transform.position = new Vector3(rightEdge.x + size, transform.position.y, transform.position.z);
        }
        // Hareket et
        else
        {
            transform.Translate(speed * Time.deltaTime * direction);
        }
    }
}
