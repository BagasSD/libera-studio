using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCollider = new List<Collider2D>();
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedCollider.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCollider.RemoveAll(item => item == collision);
    }
}
