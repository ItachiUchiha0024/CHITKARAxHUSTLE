using UnityEngine;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    [Header("Door Info")]
    public string doorName = "Door";
    public bool canBeLocked = true;
    public float interactionDistance = 3f;

    [Header("Rotation")]
    public float openAngle = 90f;
    public float closeAngle = 0f;

    [Header("Audio")]
    public AudioClip openSound;
    public AudioClip lockedSound;

    [HideInInspector] public bool isLocked = false;
    private bool isOpen = false;

    private Transform doorTransform;
    private Quaternion targetRotation;
    private AudioSource audioSource;
    private GameObject player;
    private BoxCollider2D doorCollider;
    private SpriteRenderer spriteRenderer;

    public static List<Door> AllDoors = new List<Door>();

    void Awake()
    {
        doorTransform = transform;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");
        doorCollider = GetComponent<BoxCollider2D>();
        if (doorCollider == null)
        {
            Debug.LogError($"{doorName} is missing BoxCollider2D!");
            enabled = false;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"{doorName} is missing SpriteRenderer!");
        }

        AllDoors.Add(this);
    }

    void Start()
    {
        // Let DoorManager handle locking logic, we just apply the final state
        Invoke(nameof(ApplyLockState), 0.2f);
    }

    public void ApplyLockState()
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = true;
            doorCollider.isTrigger = !isLocked;

            if (spriteRenderer != null)
                spriteRenderer.enabled = true;

            isOpen = false;
            targetRotation = Quaternion.Euler(0, closeAngle, 0);
            doorTransform.localRotation = targetRotation;

            Debug.Log($"{Time.time}: {doorName} => {(isLocked ? "LOCKED" : "UNLOCKED")} (isTrigger = {!isLocked})");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with door!");

            if (isLocked)
            {
                Debug.Log("Door is locked. Please find the key.");
                PlaySound(lockedSound);
            }
            else
            {
                OpenDoor(other.gameObject.name);
            }
        }
    }

    void OpenDoor(string interactorName)
    {
        if (!isOpen)
        {
            targetRotation = Quaternion.Euler(0, openAngle, 0);
            doorTransform.localRotation = targetRotation;
            isOpen = true;

            // Hide door visually and disable collider
            if (spriteRenderer != null)
                spriteRenderer.enabled = false;

            if (doorCollider != null)
                doorCollider.enabled = false;

            PlaySound(openSound);
            Debug.Log($"{Time.time}: {doorName} is opening. Opened by: {interactorName}");
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void OnDestroy()
    {
        AllDoors.Remove(this);
    }
}
