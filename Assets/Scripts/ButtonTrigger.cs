using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> capsules;
    [SerializeField] List<SpriteRenderer> capsuleRenderers;
    [SerializeField] List<bool> isPressed;
    private Color originalColor;
    private void Start()
    {
        for (int i = 0; i < capsuleRenderers.Count; i++)
        {
            capsuleRenderers[i] = capsules[i].GetComponent<SpriteRenderer>();
        }
        originalColor = capsuleRenderers[1].color;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            ChangeCapsuleColor(capsules[0], Color.blue);
            isPressed[0] = true;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            ChangeCapsuleColor(capsules[1], Color.blue);
            isPressed[1] = true;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            ChangeCapsuleColor(capsules[2], Color.blue);
            isPressed[2] = true;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            ChangeCapsuleColor(capsules[3], Color.blue);
            isPressed[3] = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            ChangeCapsuleColor(capsules[4], Color.yellow);
            isPressed[4] = true;
        }
        if (Input.GetKey(KeyCode.E))
        {
            ChangeCapsuleColor(capsules[5], Color.yellow);
            isPressed[5] = true;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            ChangeCapsuleColor(capsules[6], Color.yellow);
            isPressed[6] = true;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            ChangeCapsuleColor(capsules[7], Color.yellow);
            isPressed[7] = true;
        }
        if (Input.GetKey(KeyCode.X))
        {
            ChangeCapsuleColor(capsules[8], Color.yellow);
            isPressed[8] = true;
        }
        for (int i = 0; i < capsules.Count; i++)
        {
            if (isPressed[i])
            {
                StartCoroutine(ResetColorAfterDelay(capsules[i], 0.2f));
                isPressed[i] = false;
            }
        }
    }
    private void ChangeCapsuleColor(GameObject capsule, Color newColor)
    {
        SpriteRenderer capsuleRenderer = capsule.GetComponent<SpriteRenderer>();
        capsuleRenderer.color = newColor;
    }

    private IEnumerator ResetColorAfterDelay(GameObject capsule, float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetCapsuleColor(capsule);
    }

    private void ResetCapsuleColor(GameObject capsule)
    {
        SpriteRenderer capsuleRenderer = capsule.GetComponent<SpriteRenderer>();
        capsuleRenderer.color = originalColor;
    }
    
}
