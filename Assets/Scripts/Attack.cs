using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int AttackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
        if(damageable != null )
        {
            damageable.Hit(AttackDamage, deliveredKnockBack);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
        if (damageable != null)
        {
            damageable.Hit(AttackDamage, deliveredKnockBack);
        }
    }
}

    