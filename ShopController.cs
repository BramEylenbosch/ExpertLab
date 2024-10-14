using UnityEngine;
using UnityEngine.UI; // Import the UI namespace for Button

public class ShopController : MonoBehaviour
{
    public GameObject shopCanvas; // Reference to the shop canvas
    public Button openShopButton; // Reference to the button that opens the shop

    void Start()
    {
        // Ensure the button is interactable when the game starts
        if (openShopButton != null)
        {
            openShopButton.interactable = true;
        }
    }

    // Method to toggle the shop canvas visibility
    public void ToggleShop()
    {
        if (shopCanvas != null)
        {
            bool isActive = shopCanvas.activeSelf;
            shopCanvas.SetActive(!isActive); // Toggle the shop canvas

            // Make sure the button remains interactable after toggling
            if (openShopButton != null)
            {
                openShopButton.interactable = true;
            }
        }
    }
}
