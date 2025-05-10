using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    private PlayerInput PlayerInput;

    /*private void Awake()
    {
        PlayerInput = new PlayerInput();
        PlayerInput.Player.Enable();
    }*/

    [SerializeField]
    private AnimatedSprite deathSequence;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Movement movement;

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        PlayerInput.Player.Enable();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        Vector2 inputVector = PlayerInput.Player.Move.ReadValue<Vector2>();

        /*if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            movement.SetDirection(Vector2.right);
        }*/

        if (inputVector != Vector2.zero)
        {
            movement.SetDirection(inputVector);

            float angle = Mathf.Atan2(inputVector.y, inputVector.x);
            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        deathSequence.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }

}
