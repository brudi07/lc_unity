using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Class for the Items using Pointer and Drag EventHandlers, attached to the ItemPanel prefab object
public class ItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    // Public variables
    public Item item;
    public string type;
    public GameObject tooltipPrefab;
    public GameObject floatingTextPrefab;
    // Drag static variables
    private static Item startItem;
    private static GameObject startObject;
    private static Vector3 startPosition;
    private static Transform startParent;
    // Private variables
    private Player player;
    private List<string> weaponTypes = new List<string> { "Weapon1Item", "Weapon2Item", "Weapon3Item" };
    private List<string> armorTypes = new List<string> { "HeadItem", "ChestItem", "LegsItem", "GlovesItem", "BootsItem" };
    private List<string> equipTypes = new List<string> { "Weapon1Item", "Weapon2Item", "Weapon3Item", "HeadItem", "ChestItem", "LegsItem", "GlovesItem", "BootsItem" };

    void Start()
    {
        // Set player
        player = GameManager.gm.player;
    }

    // Event for item begin drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        // Capture the start object and it's item
        startItem = gameObject.GetComponent<ItemPanel>().item;
        if (startItem != null)
        {
            startObject = gameObject;
            // Capture for reset purposes
            startPosition = transform.position;
            startParent = transform.parent;
            // Set parent to the panel so the item is in front of the other items
            transform.SetParent(GameObject.Find("ItemsPanel").transform);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        // If not an item, cancel drag
        else
        {
            eventData.pointerDrag = null;
        }

        // Highlighting equip slots
        Color orange = new Color(1.0F, 0.64F, 0.0F);
        if (startItem is Weapon)
        {
            Debug.Log("isWeapon");
            GameObject.Find("Weapon1Panel").GetComponent<Outline>().effectColor = orange;
            GameObject.Find("Weapon2Panel").GetComponent<Outline>().effectColor = orange;
            GameObject.Find("Weapon3Panel").GetComponent<Outline>().effectColor = orange;
        }
        if (startItem is Armor)
        {
            Debug.Log("isArmor");
            Armor item = (Armor)startItem;
            if (item.armorType == Armor.ArmorType.Head)
                GameObject.Find("HeadPanel").GetComponent<Outline>().effectColor = orange;
            if (item.armorType == Armor.ArmorType.Chest)
                GameObject.Find("ChestPanel").GetComponent<Outline>().effectColor = orange;
            if (item.armorType == Armor.ArmorType.Legs)
                GameObject.Find("LegsPanel").GetComponent<Outline>().effectColor = orange;
            if (item.armorType == Armor.ArmorType.Boots)
                GameObject.Find("BootsPanel").GetComponent<Outline>().effectColor = orange;
            if (item.armorType == Armor.ArmorType.Gloves)
                GameObject.Find("GlovesPanel").GetComponent<Outline>().effectColor = orange;
        }
    }

    // Event for item drag
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On drag");
        // Move item GameObject position with the drag
        transform.position = Input.mousePosition;
    }

    // Event for item end drag
    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset the item GameObject position and parent
        transform.SetParent(startParent);
        transform.position = startPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        // Update outlines
        GameObject.Find("Weapon1Panel").GetComponent<Outline>().effectColor = player.weapon1.GetRarityColor();
        GameObject.Find("Weapon2Panel").GetComponent<Outline>().effectColor = player.weapon2.GetRarityColor();
        GameObject.Find("Weapon3Panel").GetComponent<Outline>().effectColor = player.weapon3.GetRarityColor();
        if (player.head != null)
            GameObject.Find("HeadPanel").GetComponent<Outline>().effectColor = player.head.GetRarityColor();
        if (player.chest != null)
            GameObject.Find("ChestPanel").GetComponent<Outline>().effectColor = player.chest.GetRarityColor();
        if (player.legs != null)
            GameObject.Find("LegsPanel").GetComponent<Outline>().effectColor = player.legs.GetRarityColor();
        if (player.boots != null)
            GameObject.Find("BootsPanel").GetComponent<Outline>().effectColor = player.boots.GetRarityColor();
        if (player.gloves != null)
            GameObject.Find("GlovesPanel").GetComponent<Outline>().effectColor = player.gloves.GetRarityColor();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("On drop");
        Debug.Log("StartObject: " + startObject.GetComponent<ItemPanel>().type);
        Debug.Log("gameObject: " + gameObject.GetComponent<ItemPanel>().type);
        // Store to inventory
        if (startObject.GetComponent<ItemPanel>().type == "Store" && gameObject.GetComponent<ItemPanel>().type == "Inventory")
        {
            if (player.gold > startItem.buyValue)
            {
                Debug.Log("Dragged from Store");
                player.inventory.Add(startItem);
                RemoveItem(startObject);
                player.gold -= startItem.buyValue;
                SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/pickupitemsfx"));
                // Inventory (Set item border color to match rarity)
                // Update inventory
                for (int i = 0; i < 15; i++)
                {
                    GameObject.Find("Item" + i).GetComponent<ItemPanel>().item = null;
                    GameObject.Find("Item" + i).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("None");
                    GameObject.Find("Item" + i).GetComponentInParent<Outline>().effectColor = Color.black;
                }
                for (int i = 0; i < player.inventory.Count; i++)
                {
                    SetItem(GameObject.Find("Item" + i), player.inventory[i]);
                }
            }
            else
            {
                SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/failsfx"));
                ShowFloatingText("Not enough gold");
            }
        }
        // Store to non inventory
        else if (startObject.GetComponent<ItemPanel>().type == "Store" && gameObject.GetComponent<ItemPanel>().type != "Inventory")
        {
            Debug.Log("Store to non inventory");
            ShowFloatingText("Drag to inventory");
        }
        // Item dragged to store
        else if (gameObject.GetComponent<ItemPanel>().type == "Store")
        {
            Debug.Log("Dragged to Store");
            // Do nothing
        }
        // Item dragged to store
        else if (startObject.GetComponent<ItemPanel>().type == "Store" && gameObject.GetComponent<ItemPanel>().type == "Sell")
        {
            Debug.Log("Dragged to sell from store");
            // Do nothing
        }
        // Trying to sell from non inventory
        else if (gameObject.GetComponent<ItemPanel>().type == "Sell" && startObject.GetComponent<ItemPanel>().type != "Inventory")
        {
            Debug.Log("Cannot sell equipment");
            ShowFloatingText("Unequip first");
        }
        // Sell item to store from inventory
        else if (gameObject.GetComponent<ItemPanel>().type == "Sell" && startObject.GetComponent<ItemPanel>().type == "Inventory")
        {
            Debug.Log("Sell item to store");
            player.inventory.Remove(startItem);
            RemoveItem(startObject);
            player.gold += startItem.sellValue;
            GameObject.Find("Gold").GetComponentInChildren<Text>().text = "Gold: " + player.gold.ToString();
            SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/coinsfx"));
            // Update inventory
            for (int i = 0; i < 15; i++)
            {
                GameObject.Find("Item" + i).GetComponent<ItemPanel>().item = null;
                GameObject.Find("Item" + i).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("None");
                GameObject.Find("Item" + i).GetComponentInParent<Outline>().effectColor = Color.black;
            }
            for (int i = 0; i < player.inventory.Count; i++)
            {
                SetItem(GameObject.Find("Item" + i), player.inventory[i]);
            }
        }
        // Traying to stash from non inventory
        else if (gameObject.GetComponent<ItemPanel>().type == "Stash" && startObject.GetComponent<ItemPanel>().type != "Inventory")
        {
            Debug.Log("Cannot stash equipment");
            ShowFloatingText("Unequip first");
        }
        // Sell item to store from inventory
        else if (gameObject.GetComponent<ItemPanel>().type == "Stash" && startObject.GetComponent<ItemPanel>().type == "Inventory")
        {
            player.inventory.Remove(startItem);
            GameManager.gm.stash.Add(startItem);
            // Update inventory
            for (int i = 0; i < 15; i++)
            {
                GameObject.Find("Item" + i).GetComponent<ItemPanel>().item = null;
                GameObject.Find("Item" + i).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("None");
                GameObject.Find("Item" + i).GetComponentInParent<Outline>().effectColor = Color.black;
            }
            for (int i = 0; i < player.inventory.Count; i++)
            {
                SetItem(GameObject.Find("Item" + i), player.inventory[i]);
            }
        }
        // Trying to remove from stash to non inventory
        else if (startObject.GetComponent<ItemPanel>().type == "Stash" && gameObject.GetComponent<ItemPanel>().type != "Inventory")
        {
            Debug.Log("Cannot equip from stash");
            ShowFloatingText("Add to inventory first");
        }
        // Remove from stash to inventory
        else if (startObject.GetComponent<ItemPanel>().type == "Stash" && gameObject.GetComponent<ItemPanel>().type == "Inventory")
        {
            player.inventory.Add(startItem);
            GameManager.gm.stash.Remove(startItem);
            // Update inventory
            for (int i = 0; i < 15; i++)
            {
                GameObject.Find("Item" + i).GetComponent<ItemPanel>().item = null;
                GameObject.Find("Item" + i).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("None");
                GameObject.Find("Item" + i).GetComponentInParent<Outline>().effectColor = Color.black;
            }
            for (int i = 0; i < player.inventory.Count; i++)
            {
                SetItem(GameObject.Find("Item" + i), player.inventory[i]);
            }
        }
        // Equip
        else if (!equipTypes.Contains(startObject.name) && equipTypes.Contains(gameObject.name))
        {
            Debug.Log("Equip");
            Equip();
        }
        else if (weaponTypes.Contains(startObject.name) && weaponTypes.Contains(gameObject.name))
        {
            Debug.Log("Equip - swap weapon");
            Equip();
        }
        // Unequip Trash
        else if (equipTypes.Contains(startObject.name) && gameObject.GetComponent<ItemPanel>().type == "Trash")
        {
            Debug.Log("Unequip Trash");
            player.inventory.Add(startItem);
            RemoveItem(startObject);
            player.Unequip(startItem);
            player.inventory.Remove(startItem);
            UpdateEquipment();
            SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/dropsfx"));
        }
        // Inventory Trash
        else if (startObject.GetComponent<ItemPanel>().type == "Inventory" && gameObject.GetComponent<ItemPanel>().type == "Trash")
        {
            Debug.Log("Inventory Trash");
            RemoveItem(startObject);
            player.inventory.Remove(startItem);
            UpdateEquipment();
            SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/dropsfx"));
        }
        // Unequip
        else if (equipTypes.Contains(startObject.name) && !equipTypes.Contains(gameObject.name))
        {
            Debug.Log("Unequip");
            Unequip();
        }
        // Sort Inventory
        else if (startObject.GetComponent<ItemPanel>().type == "Inventory" && gameObject.GetComponent<ItemPanel>().type == "Inventory")
        {
            Debug.Log("Sort Inventory");
            SortInventory();
        }
        // Not accounted for
        else
        {
            Debug.Log("Drag and Drop action not accounted for");
        }

    }

    // Equip item
    void Equip()
    {
        // Required level and attributes check
        if (player.level >= startItem.requiredLevel &&
            player.strength >= startItem.requiredStr &&
            player.dexterity >= startItem.requiredDex &&
            player.intelligence >= startItem.requiredInt)
        {
            // Check if weapon
            if (weaponTypes.Contains(gameObject.name) && startItem is Weapon)
            {
                if (weaponTypes.Contains(startObject.name) && weaponTypes.Contains(gameObject.name))
                {
                    Debug.Log("Swap Equipped Weapons");
                    startObject.transform.SetParent(startParent);
                    // Set the items
                    SetItem(startObject, item);
                    SetItem(gameObject, startItem);
                }
                else if (startItem != null && item != null)
                {
                    Debug.Log("Weapon Swap");
                    startObject.transform.SetParent(startParent);
                    // Update inventory
                    player.inventory.Remove(startItem);
                    player.inventory.Add(item);
                    // Unequip swapped item
                    player.Unequip(item);
                    // Set the items
                    SetItem(startObject, item);
                    SetItem(gameObject, startItem);
                }
                else if (startItem != null && item == null)
                {
                    Debug.Log("Weapon No Swap");
                    // Update invetory
                    player.inventory.Remove(startItem);
                    // Update ui object
                    RemoveItem(startObject);
                    // Set the item
                    SetItem(gameObject, startItem);
                }
            }

            // Check if armor
            if (armorTypes.Contains(gameObject.name) && startItem is Armor)
            {
                Armor armor = (Armor)startItem;
                if (gameObject.name.Contains(armor.armorType.ToString()))
                {
                    if (startItem != null && item != null)
                    {
                        Debug.Log("Armor Swap");
                        startObject.transform.SetParent(startParent);
                        // Update inventory
                        player.inventory.Remove(startItem);
                        player.inventory.Add(item);
                        // Unequip swapped item
                        player.Unequip(item);
                        // Set the items
                        SetItem(startObject, item);
                        SetItem(gameObject, startItem);
                    }

                    if (startItem != null && item == null)
                    {
                        Debug.Log("Armor No Swap");
                        // Update invetory
                        player.inventory.Remove(startItem);
                        // Update ui object
                        RemoveItem(startObject);
                        // Set the item
                        SetItem(gameObject, startItem);
                    }
                }
            }
        }
        // Player does not meet requirements to equip
        else
        {
            SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/failsfx"));
            ShowFloatingText("Requirements not met");
        }

    }

    // Unequip item
    void Unequip()
    {
        // Inventory slots
        if (equipTypes.Contains(startObject.name) && !equipTypes.Contains(gameObject.name))
        {
            // Swap Items
            if (startItem != null && item != null)
            {
                Debug.Log("Swap");
                player.inventory.Add(startItem);
                player.inventory.Remove(item);
                startObject.transform.SetParent(startParent);
                if (startItem is Armor)
                    player.Unequip(startItem);
                SetItem(startObject, item);
                SetItem(gameObject, startItem);
            }
            // No Swap
            if (startItem != null && item == null)
            {
                Debug.Log("No Swap");
                player.inventory.Add(startItem);
                RemoveItem(startObject);
                if (startItem is Armor)
                    player.Unequip(startItem);
                // Set Item
                SetItem(gameObject, startItem);
            }
        }
    }

    // Move inventory item positions
    void SortInventory()
    {
        // Inventory slots
        if (!equipTypes.Contains(gameObject.name))
        {
            // Swap Items
            if (startItem != null && item != null)
            {
                Debug.Log("Swap");
                startObject.transform.SetParent(startParent);
                SetItem(startObject, item);
                SetItem(gameObject, startItem);
            }
            // No Swap
            if (startItem != null && item == null)
            {
                Debug.Log("No Swap");
                RemoveItem(startObject);
                // Set Item
                SetItem(gameObject, startItem);
            }
        }
    }

    // Set the game objects based on the item
    private void SetItem(GameObject gameObject, Item item)
    {
        Debug.Log("Set Item: " + item.name);
        // Set gameObject item to item
        gameObject.GetComponent<ItemPanel>().item = item;
        Debug.Log("Item ilvl: " + item.ilvl.ToString());
        gameObject.GetComponentInChildren<Text>().text = item.ilvl.ToString();
        // Sprite
        gameObject.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(item.spritePath);
        // Rarities
        gameObject.GetComponentInParent<Outline>().effectColor = item.GetRarityColor();

        // Equip item
        if (gameObject.GetComponent<ItemPanel>().type == "Head")
        {
            player.head = (Armor)player.Equip(item);
        }
        else if (gameObject.GetComponent<ItemPanel>().type == "Chest")
        {
            player.chest = (Armor)player.Equip(item);
        }
        else if (gameObject.GetComponent<ItemPanel>().type == "Gloves")
        {
            player.gloves = (Armor)player.Equip(item);
        }
        else if (gameObject.GetComponent<ItemPanel>().type == "Boots")
        {
            player.boots = (Armor)player.Equip(item);
        }
        else if (gameObject.GetComponent<ItemPanel>().type == "Legs")
        {
            player.legs = (Armor)player.Equip(item);
        }
        // Do not equip weapon, this is handled in player class
        else if (gameObject.GetComponent<ItemPanel>().type == "Weapon1")
        {
            player.weapon1 = (Weapon)item;
        }
        else if (gameObject.GetComponent<ItemPanel>().type == "Weapon2")
        {
            player.weapon1 = (Weapon)item;
        }
        else if (gameObject.GetComponent<ItemPanel>().type == "Weapon3")
        {
            player.weapon1 = (Weapon)item;
        }

        UpdateEquipment();
    }

    // Update equipped equipment and weapon buttons
    private void UpdateEquipment()
    {
        // Set Equipped Armor
        if ((Armor)GameObject.Find("HeadItem").GetComponent<ItemPanel>().item != null)
            player.head = (Armor)(GameObject.Find("HeadItem").GetComponent<ItemPanel>().item);
        else
            player.head = null;
        if ((Armor)GameObject.Find("ChestItem").GetComponent<ItemPanel>().item != null)
            player.chest = (Armor)(GameObject.Find("ChestItem").GetComponent<ItemPanel>().item);
        else
            player.chest = null;
        if ((Armor)GameObject.Find("GlovesItem").GetComponent<ItemPanel>().item != null)
            player.gloves = (Armor)(GameObject.Find("GlovesItem").GetComponent<ItemPanel>().item);
        else
            player.gloves = null;
        if ((Armor)GameObject.Find("BootsItem").GetComponent<ItemPanel>().item != null)
            player.boots = (Armor)(GameObject.Find("BootsItem").GetComponent<ItemPanel>().item);
        else
            player.boots = null;
        if ((Armor)GameObject.Find("LegsItem").GetComponent<ItemPanel>().item != null)
            player.legs = (Armor)(GameObject.Find("LegsItem").GetComponent<ItemPanel>().item);
        else
            player.legs = null;
        // Set Equipped Weapons
        if ((Weapon)GameObject.Find("Weapon1Item").GetComponent<ItemPanel>().item != null)
            player.weapon1 = (Weapon)(GameObject.Find("Weapon1Item").GetComponent<ItemPanel>().item);
        else
            player.weapon1 = new Weapon();
        if ((Weapon)GameObject.Find("Weapon2Item").GetComponent<ItemPanel>().item != null)
            player.weapon2 = (Weapon)(GameObject.Find("Weapon2Item").GetComponent<ItemPanel>().item);
        else
            player.weapon2 = new Weapon();
        if ((Weapon)GameObject.Find("Weapon3Item").GetComponent<ItemPanel>().item != null)
            player.weapon3 = (Weapon)(GameObject.Find("Weapon3Item").GetComponent<ItemPanel>().item);
        else
            player.weapon3 = new Weapon();

        // Update Weapon Buttons
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            GameObject.Find("Slot1Button").GetComponentInChildren<Text>().text = player.weapon1.name;
            GameObject.Find("Slot1Button").GetComponentInChildren<Text>().color = player.weapon1.GetRarityColor();
            GameObject.Find("Slot2Button").GetComponentInChildren<Text>().text = player.weapon2.name;
            GameObject.Find("Slot2Button").GetComponentInChildren<Text>().color = player.weapon2.GetRarityColor();
            GameObject.Find("Slot3Button").GetComponentInChildren<Text>().text = player.weapon3.name;
            GameObject.Find("Slot3Button").GetComponentInChildren<Text>().color = player.weapon3.GetRarityColor();
            if (GameObject.Find("Slot1Button").GetComponentInChildren<Outline>().effectColor == Color.white)
                player.equippedWeapon = player.weapon1;
            else if (GameObject.Find("Slot2Button").GetComponentInChildren<Outline>().effectColor == Color.white)
                player.equippedWeapon = player.weapon2;
            else if (GameObject.Find("Slot3Button").GetComponentInChildren<Outline>().effectColor == Color.white)
                player.equippedWeapon = player.weapon3;
        }

        // Update Character sheet
        GameObject.Find("Strength").GetComponentInChildren<Text>().text = "Strength: " + player.strength + " (" + player.GetBaseStat(player.strength, player.modifiedStrength) + ")";
        GameObject.Find("Dexterity").GetComponentInChildren<Text>().text = "Dexterity: " + player.dexterity + " (" + player.GetBaseStat(player.dexterity, player.modifiedDexterity) + ")";
        GameObject.Find("Intelligence").GetComponentInChildren<Text>().text = "Intelligence: " + player.intelligence + " (" + player.GetBaseStat(player.intelligence, player.modifiedIntelligence) + ")";
        GameObject.Find("ArmorPen").GetComponentInChildren<Text>().text = "Armor Pen: " + player.armorPen;
        GameObject.Find("MagicPen").GetComponentInChildren<Text>().text = "Magic Pen: " + player.magicPen;
        GameObject.Find("CritChance").GetComponentInChildren<Text>().text = "Crit Chance: " + player.critChance + "%";
        GameObject.Find("CritDamage").GetComponentInChildren<Text>().text = "Crit Damage: " + player.critDamage+ "%";
    }

    // Remove item from ui component and set to default
    private void RemoveItem(GameObject gameObject)
    {
        gameObject.GetComponent<ItemPanel>().item = null;
        gameObject.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("None");
        gameObject.transform.SetParent(startParent);
        gameObject.GetComponentInParent<Outline>().effectColor = Color.black;
    }

    // Pointer properties
    private GameObject tooltip;
    private Vector3 offset = new Vector3(0, 0, 0);

    // Event for mouseover on item panel
    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform rt = (RectTransform)tooltipPrefab.transform;
        offset.x = -1 * rt.rect.width / 2 - (Screen.width * 0.05F);
        offset.y = -1 * rt.rect.height / 2 - (Screen.height * 0.1F);
        // Create offsets so the tooltip doesn't go off the screen
        if (Input.mousePosition.y < Screen.height / 2)
            offset.y = 1 * rt.rect.height / 2 + (Screen.height * 0.1F);

        // Set the tooltip values from the item
        if (item != null)
        {
            // Instantiate the tooltip
            tooltip = Instantiate(tooltipPrefab, Input.mousePosition + offset, Quaternion.identity, transform);
            // Set parent to InventoryPanel so it isn't placed behind item panels
            tooltip.transform.SetParent(GameObject.Find("InventoryPanel").transform);
            //tooltip.transform.localScale = new Vector3(1.5F, 1.5F, 0);
            // Rarities
            GameObject.Find("TooltipName").GetComponent<Text>().color = item.GetRarityColor();
            // Set the tooltipo name and tooltip text
            GameObject.Find("TooltipName").GetComponentInChildren<Text>().text = item.name;
            GameObject.Find("Tooltip").GetComponentInChildren<Text>().text = item.GetToolTip();
        }

    }

    // Event for mouseexit that destroys the tooltip
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse exit");
        // Destroy tooltip
        if (tooltip)
            Destroy(tooltip, 0f);
    }

    // Update the position of the tooltip while we hover
    void Update()
    {
        // Check if tooltip exists
        if (tooltip)
        {
            // Create offsets so the tooltip doesn't go off the screen
            // Create offsets so the tooltip doesn't go off the screen
            RectTransform rt = (RectTransform)tooltip.transform;
            if (Input.mousePosition.y < Screen.height / 2)
                offset.y = 1 * rt.rect.height / 2 + (Screen.height * 0.1F);
            else
                offset.y = -1 * rt.rect.height / 2 - (Screen.height * 0.1F);

            // Update the tooltip position as the mouse moves inside
            tooltip.transform.position = Input.mousePosition + offset;
            // Rarity
            GameObject.Find("TooltipName").GetComponent<Text>().color = item.GetRarityColor();
            // Set the tooltipo name and tooltip text
            GameObject.Find("TooltipName").GetComponentInChildren<Text>().text = item.name;
            GameObject.Find("Tooltip").GetComponentInChildren<Text>().text = item.GetToolTip();
        }
    }

    private void ShowFloatingText(string text)
    {
        GameObject floatingText = Instantiate(floatingTextPrefab, Input.mousePosition + new Vector3(-150, 0, 0), Quaternion.identity, transform);
        floatingText.transform.SetParent(GameObject.Find("ItemsPanel").transform);
        floatingText.transform.localScale = new Vector3(2, 2, 0);
        floatingText.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        floatingText.GetComponentInChildren<Text>().fontSize = 20;
        floatingText.GetComponentInChildren<Text>().text = text;
    }
}
