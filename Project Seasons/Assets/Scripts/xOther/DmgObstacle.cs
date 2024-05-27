using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgObstacle : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    float forceUp;
    [SerializeField, Tooltip("")]
    float forceSide;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            SpriteRenderer sr = collision.gameObject.transform.Find("SpriteHolder").GetComponent<SpriteRenderer>();

            if (sr.flipX)
            {
                forceSide = Mathf.Abs(forceSide);
            }
            else
            {
                forceSide = Mathf.Abs(forceSide) * -1f;
            }

            Vector2 pushForce = new (forceSide, forceUp);

            rb.velocity = Vector2.zero;
            rb.AddForce(pushForce, ForceMode2D.Impulse);
        }
    }
}
