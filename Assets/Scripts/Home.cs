using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Home : MonoBehaviour
{
    public GameObject frog;  // Frog objesini tanımlıyoruz

    private BoxCollider2D boxCollider;  // BoxCollider2D objesi

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();  // BoxCollider2D bileşenini alıyoruz
    }

    private void OnEnable()
    {
        // Home objesi aktif olduğunda, frog objesini aktif yapıyoruz
        frog.SetActive(true);
        boxCollider.enabled = false;  // Collider'ı devre dışı bırakıyoruz
    }

    private void OnDisable()
    {
        // Home objesi devre dışı kaldığında, frog objesini devre dışı bırakıyoruz
        frog.SetActive(false);
        boxCollider.enabled = true;  // Collider'ı tekrar aktif hale getiriyoruz
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer bu home objesine bir oyuncu (Player) çarptıysa
        if (other.gameObject.CompareTag("Player"))
        {
            // Home objesini tamamlanmış sayıyoruz
            GameManager.Instance.HomeOccupied();
        }
    }
}
