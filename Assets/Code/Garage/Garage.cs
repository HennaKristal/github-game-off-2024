using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Garage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GarageUI garageUI;

    [Header("Parts")]
    [SerializeField] private List<PlaneStats> planeParts;
    [SerializeField] private List<EngineStats> engineParts;
    [SerializeField] private List<GeneratorStats> generatorParts;
    [SerializeField] private List<CoolerStats> coolerParts;
    [SerializeField] private List<WeaponStats> weapons;

    [Header("UI Elements")]
    [SerializeField] private GameObject partSelectionWindow;
    [SerializeField] private GameObject partUIPrefab;
    [SerializeField] private TextMeshProUGUI partSelectionTitle;
    [SerializeField] private GameObject partParentContainer;
    [SerializeField] private Button exitPartSelectionButton;

    [Header("Slot Images")]
    [SerializeField] private Sprite normalSlotImage;
    [SerializeField] private Sprite activeSlotImage;




    private void ClosePartSelectionWindow()
    {
        partSelectionWindow.SetActive(false);

        // Clear any existing UI elements in the container to avoid duplicates
        foreach (Transform child in partParentContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }



    public void DisplayPlaneParts()
    {
        partSelectionWindow.SetActive(true);
        partSelectionTitle.text = "Plane Cores";

        foreach (PlaneStats planePart in planeParts)
        {
            // Instantiate a new UI element for the part and get the UI components from the prefab
            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            // Set the title text to the part's name
            titleText.text = planePart.name;

            // Set the price or equipped status
            if (planePart.isEquipped)
            {
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (planePart.isPurchasable)
            {
                priceText.text = $"${planePart.purchasePrice}";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayEngineParts()
    {
        partSelectionWindow.SetActive(true);
        partSelectionTitle.text = "Engines";

        foreach (EngineStats enginePart in engineParts)
        {
            // Instantiate a new UI element for the part and get the UI components from the prefab
            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            // Set the title text to the part's name
            titleText.text = enginePart.name;

            // Set the price or equipped status
            if (enginePart.isEquipped)
            {
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (enginePart.isPurchasable)
            {
                priceText.text = $"${enginePart.purchasePrice}";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayGeneratorParts()
    {
        partSelectionWindow.SetActive(true);
        partSelectionTitle.text = "Generators";

        foreach (GeneratorStats generatorPart in generatorParts)
        {
            // Instantiate a new UI element for the part and get the UI components from the prefab
            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            // Set the title text to the part's name
            titleText.text = generatorPart.name;

            // Set the price or equipped status
            if (generatorPart.isEquipped)
            {
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (generatorPart.isPurchasable)
            {
                priceText.text = $"${generatorPart.purchasePrice}";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }

    public void DisplayCoolerParts()
    {
        partSelectionWindow.SetActive(true);
        partSelectionTitle.text = "Coolers";

        foreach (CoolerStats coolerPart in coolerParts)
        {
            // Instantiate a new UI element for the part and get the UI components from the prefab
            GameObject partUI = Instantiate(partUIPrefab, partParentContainer.transform);
            TextMeshProUGUI titleText = partUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = partUI.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            // Set the title text to the part's name
            titleText.text = coolerPart.name;

            // Set the price or equipped status
            if (coolerPart.isEquipped)
            {
                priceText.text = "Equipped";
                priceText.color = Color.green;
            }
            else if (coolerPart.isPurchasable)
            {
                priceText.text = $"${coolerPart.purchasePrice}";
                priceText.color = Color.white;
            }
            else
            {
                priceText.text = "";
            }
        }
    }



    private void Start()
    {
        // TODO: on start load prefab or serialized data about every single scriptable object part and load their values
        // Also set this object to do not destory on load
    }

    public void AddItem(string category, string name)
    {
        // TODO find the correct part's scrptable object and set it's stats correctly
        //part.isOwned = true;
        //part.isPurchasable = true;
    }

    private void EquipPart(string category, string name)
    {
        // Get currently equipped item and unequip it or unequip just every item in this category
        // currentpart.isequipped = false;

        // TODO: equip the new item
        // part.isEquipped = true;
    }
}
