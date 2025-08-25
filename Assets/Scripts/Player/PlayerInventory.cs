using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 10; // Maximum number of items the inventory can hold.
    public List<string> items = new List<string>(); // List to store the names of the items.

    [SerializeField]
    private List<string> startingKeys = new List<string>(); // List of keys the player starts with


    void Start()
    {
        // Add starting keys to the inventory
        foreach (string keyName in startingKeys)
        {
            AddItemToInventory(keyName);
        }
    }

    void Update()
    {
        // No item pickup or UI logic here anymore.
    }

    private void AddItemToInventory(string itemName)
    {
        items.Add(itemName);
    }

    // Function to add an item to the inventory.
    public bool AddItem(string itemName) // Changed parameter type to string
    {
        if (items.Count >= inventorySize)
        {
            Debug.Log("Inventory is full!");
            return false; // Inventory is full.
        }

        // Add the item name to the inventory.
        items.Add(itemName);
        Debug.Log($"Added {itemName} to inventory.");
        return true; // Item added successfully.
    }

    // Function to remove an item from the inventory by name.
    public bool RemoveItem(string itemName)
    {
        if (items.Contains(itemName))
        {
            items.Remove(itemName);
            return true;
        }
        else
        {
            Debug.Log($"Item {itemName} not found in inventory.");
            return false;
        }
    }

    // Function to check if the inventory contains a specific item.
    public bool ContainsItem(string itemName)
    {
        return items.Contains(itemName);
    }

    // Function to use an item (e.g., a key).  Returns true if the item was used.
    public bool UseItem(string itemName)
    {
        if (ContainsItem(itemName))
        {
            items.Remove(itemName);
            return true;
        }
        else
        {
            Debug.Log($"Item {itemName} not found in inventory.");
            return false;
        }
    }

    // Called when the GameObject is destroyed.
    private void OnDestroy()
    {
        // Clear the lists to prevent memory leaks.
        items.Clear();
    }
}
