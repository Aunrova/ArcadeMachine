using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[0];  // Animasyon kareleri
    public float animationTime = 0.5f;  // Animasyon hızını belirler
    public bool loop = true;  // Animasyonun döngüde olup olmayacağını belirler

    private SpriteRenderer spriteRenderer;  // SpriteRenderer bileşeni
    private int animationFrame;  // Geçerli animasyon karesi

    private void Awake()
    {
        // SpriteRenderer bileşenini alıyoruz
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Animasyon başlatılıyor
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if (!spriteRenderer.enabled)
        {
            return; // Eğer spriteRenderer devre dışıysa animasyon durur
        }

        animationFrame++;  // Bir sonraki kareye geç

        // Eğer animasyon sona erdi ve loop aktifse başa dön
        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }

        // Eğer animasyon karenin aralığındaysa, sprite'ı değiştir
        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    public void Restart()
    {
        // Animasyonu sıfırlıyoruz
        animationFrame = -1;
        Advance();  // İlk kareyi başlat
    }
}
