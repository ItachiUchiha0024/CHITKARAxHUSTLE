using UnityEngine;
using UnityEngine.UI;

public class MapTrigger : MonoBehaviour
{
    public Image imageToTrigger; // Assign the UI Image GameObject in the Inspector
    public string targetTag = "Player"; // Tag to identify the player

    void Start()
    {
        if (imageToTrigger == null)
        {
            Debug.LogError("Image To Trigger is not assigned in the Inspector on GameObject: " + gameObject.name);
        }
        else
        {
            Debug.Log("Image To Trigger is assigned to: " + imageToTrigger.gameObject.name + " on GameObject: " + gameObject.name);
            imageToTrigger.gameObject.SetActive(false); // Ensure it's hidden at start
        }

        Collider2D triggerCollider2D = GetComponent<Collider2D>();
        if (triggerCollider2D == null || !triggerCollider2D.isTrigger)
        {
            Debug.LogError("No 2D trigger collider found or 'Is Trigger' is not enabled on GameObject: " + gameObject.name);
        }
        else
        {
            Debug.Log("2D trigger collider found and 'Is Trigger' is enabled on GameObject: " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called by: " + other.gameObject.name + " entering trigger on: " + gameObject.name);

        if (other.CompareTag(targetTag))
        {
            Debug.Log(other.gameObject.name + " has the target tag: " + targetTag);
            if (imageToTrigger != null)
            {
                imageToTrigger.gameObject.SetActive(true);
                Debug.Log("Image GameObject enabled: " + imageToTrigger.gameObject.name);
            }
        }
        else
        {
            Debug.Log(other.gameObject.name + " does NOT have the target tag: " + targetTag);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D called by: " + other.gameObject.name + " exiting trigger on: " + gameObject.name);

        if (other.CompareTag(targetTag))
        {
            Debug.Log(other.gameObject.name + " (exiting) has the target tag: " + targetTag);
            if (imageToTrigger != null)
            {
                imageToTrigger.gameObject.SetActive(false);
                Debug.Log("Image GameObject disabled: " + imageToTrigger.gameObject.name);
            }
        }
        else
        {
            Debug.Log(other.gameObject.name + " (exiting) does NOT have the target tag: " + targetTag);
        }
    }
}
