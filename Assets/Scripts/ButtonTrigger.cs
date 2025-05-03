using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Capsule1;
    [SerializeField] private GameObject Capsule2;
    [SerializeField] private GameObject Capsule3;
    [SerializeField] private GameObject Capsule4;
    [SerializeField] private GameObject Capsule5;
    [SerializeField] private GameObject Capsule6;
    [SerializeField] private GameObject Capsule7;
    [SerializeField] private GameObject Capsule8;
    [SerializeField] private GameObject Capsule9;

    private Renderer capsule1Renderer;
    private Renderer capsule2Renderer;
    private Renderer capsule3Renderer;
    private Renderer capsule4Renderer;
    private Renderer capsule5Renderer;
    private Renderer capsule6Renderer;
    private Renderer capsule7Renderer;
    private Renderer capsule8Renderer;
    private Renderer capsule9Renderer;

    private Color originalColor;

    private void Start()
    {
        capsule1Renderer = Capsule1.GetComponent<Renderer>();
        capsule2Renderer = Capsule2.GetComponent<Renderer>();
        capsule3Renderer = Capsule3.GetComponent<Renderer>();
        capsule4Renderer = Capsule4.GetComponent<Renderer>();
        capsule5Renderer = Capsule5.GetComponent<Renderer>();
        capsule6Renderer = Capsule6.GetComponent<Renderer>();
        capsule7Renderer = Capsule7.GetComponent<Renderer>();
        capsule8Renderer = Capsule8.GetComponent<Renderer>();
        capsule9Renderer = Capsule9.GetComponent<Renderer>();

        originalColor = capsule1Renderer.material.color;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            ChangeCapsuleColor(Capsule1, Color.blue);
        }
        else
        {
            ResetCapsuleColor(Capsule1);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            ChangeCapsuleColor(Capsule2, Color.blue);
        }
        else
        {
            ResetCapsuleColor(Capsule2);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            ChangeCapsuleColor(Capsule3, Color.blue);
        }
        else
        {
            ResetCapsuleColor(Capsule3);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            ChangeCapsuleColor(Capsule4, Color.blue);
        }
        else
        {
            ResetCapsuleColor(Capsule4);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ChangeCapsuleColor(Capsule5, Color.yellow);
        }
        else
        {
            ResetCapsuleColor(Capsule5);
        }

        if (Input.GetKey(KeyCode.E))
        {
            ChangeCapsuleColor(Capsule6, Color.yellow);
        }
        else
        {
            ResetCapsuleColor(Capsule6);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            ChangeCapsuleColor(Capsule7, Color.yellow);
        }
        else
        {
            ResetCapsuleColor(Capsule7);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            ChangeCapsuleColor(Capsule8, Color.yellow);
        }
        else
        {
            ResetCapsuleColor(Capsule8);
        }

        if (Input.GetKey(KeyCode.X))
        {
            ChangeCapsuleColor(Capsule9, Color.yellow);
        }
        else
        {
            ResetCapsuleColor(Capsule9);
        }
    }

    private void ChangeCapsuleColor(GameObject capsule, Color newColor)
    {
        Renderer capsuleRenderer = capsule.GetComponent<Renderer>();
        capsuleRenderer.material.SetColor("_Color", newColor);
    }

    private void ResetCapsuleColor(GameObject capsule)
    {
        Renderer capsuleRenderer = capsule.GetComponent<Renderer>();
        capsuleRenderer.material.SetColor("_Color", originalColor);
    }
}
