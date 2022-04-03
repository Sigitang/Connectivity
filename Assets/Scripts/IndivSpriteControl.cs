using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndivSpriteControl : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("bump");
        if (collision.gameObject.tag == "IndivSprite")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
