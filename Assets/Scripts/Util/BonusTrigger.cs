using UnityEngine;

public class BonusTrigger : MonoBehaviour
{
    public BonusTracker tracker;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { 
            tracker.AquireBonus();
            Destroy(gameObject);
        }
    }
}
