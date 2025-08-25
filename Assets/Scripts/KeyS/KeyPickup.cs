using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip keyPickupSound; // Drag and drop the sound clip in the Inspector
    private AudioSource audioSource; // Reference to the AudioSource

    void Start()
    {
        // Ensure the AudioSource component is attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // If no AudioSource is attached, log an error
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on Key GameObject");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player picked up the key!");

            if (audioSource != null)
            {
                // Play sound through AudioSource
                audioSource.PlayOneShot(keyPickupSound);
            }

            // Unlock all doors
            foreach (Door door in Door.AllDoors)
            {
                door.isLocked = false;
                door.ApplyLockState();
            }

            // Show a green message when all doors are unlocked
            UIManager.Instance.ShowMessageWithBlink("<color=green>You found the key! All doors are now unlocked.</color>", Color.green, 1f, 5f);

            Destroy(gameObject); // Destroy the key after pickup
        }
    }
}
