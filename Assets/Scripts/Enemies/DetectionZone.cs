using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCollider = new List<Collider2D>();
    public Collider2D roamingZone;
    public Collider2D battleZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedCollider.Add(collision);
        if (collision.CompareTag("Player"))
        {
                roamingZone.enabled = false;
                battleZone.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectedCollider.Any(item => item.CompareTag("Player")))
        {
                roamingZone.enabled = true;
                battleZone.enabled = false;
        }
        detectedCollider.RemoveAll(item => item == collision);
    }
}
