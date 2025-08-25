using UnityEngine;

public class CounterInteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        QueueManagement.Instance.HandleCounterInteraction();
    }
}
