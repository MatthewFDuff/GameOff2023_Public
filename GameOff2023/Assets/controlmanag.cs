using UnityEngine;

public class ItemSwitcher : MonoBehaviour
{
    // Define the items to be switched
    public GameObject[] items;

    // Update is called once per frame
    void Update()
    {
        // Check for keyboard input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchItem(2);
        }
    }

    // Switches the active item based on the provided index
    void SwitchItem(int index)
    {
        // Disable all items
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }

        // Enable the selected item
        if (index >= 0 && index < items.Length)
        {
            items[index].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Invalid index provided for switching items.");
        }
    }
}
