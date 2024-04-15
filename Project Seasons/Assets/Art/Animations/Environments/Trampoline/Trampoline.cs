using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    Animator anim;

    [SerializeField, Tooltip("")]
    float jumpForce = 250.0f;

    [SerializeField, Tooltip("")]
    float jumpDelay = 0.2f;

    bool isTriggerd;
    bool wasCanceled;

    private void OnEnable()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isTriggerd) return;

            isTriggerd = false;
            wasCanceled = false;

            anim.Play("Trampoline");

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 force = new(0f, jumpForce);

            StartCoroutine(JumpExecute(rb, force));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wasCanceled = true;
        }
    }

    IEnumerator JumpExecute(Rigidbody2D rb, Vector2 force)
    {
        isTriggerd = true;
        
        yield return new WaitForSeconds(jumpDelay);

        if (!wasCanceled)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        isTriggerd = false;
        wasCanceled = false;
    }
}
