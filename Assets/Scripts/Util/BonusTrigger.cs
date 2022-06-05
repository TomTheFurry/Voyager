using UnityEngine;

public class BonusTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BonusTracker tracker = FindObjectOfType<BonusTracker>();
        if (tracker == null) return;
        if (other.tag == "Player") { 
            tracker.AquireBonus();
            Destroy(gameObject);
        }
    }
}
