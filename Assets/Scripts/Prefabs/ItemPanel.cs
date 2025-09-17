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

    // Cached references to avoid repeated GameObject.Find calls
    private static Transform itemsPanelTransform;
    private static Transform inventoryPanelTransform;
    private static Player player;

    // Cached UI references for this specific panel
    private Text itemLevelText;
    private Image itemImage;
    private Outline itemOutline;
    private CanvasGroup canvasGroup;

    // Cached UI GameObjects (static to avoid repeated lookups)
    private static GameObject goldTextObject;
    private static Dictionary<string, GameObject> uiElementCache = new Dictionary<string, GameObject>();
    private static Dictionary<string, Outline> outlineCache = new Dictionary<string, Outline>();
    private static Dictionary<string, AudioClip> audioClipCache = new Dictionary<string, AudioClip>();
    private static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    // Constants
    private static readonly List<string> weaponTypes = new List<string> { "Weapon1Item", "Weapon2Item", "Weapon3Item" };
    private static readonly List<string> armorTypes = new List<string> { "HeadItem", "ChestItem", "LegsItem", "GlovesItem", "BootsItem" };
    private static readonly List<string> equipTypes = new List<string> { "Weapon1Item", "Weapon2Item", "Weapon3Item", "HeadItem", "ChestItem", "LegsItem", "GlovesItem", "BootsItem" };
    private static readonly Color orangeHighlight = new Color(1.0F, 0.64F, 0.0F);

    // Tooltip variables
    private GameObject tooltip;
    private Vector3 tooltipOffset = Vector3.zero;
    private static readonly Vector3 floatingTextOffset = new Vector3(-150, 0, 0);

    void Awake()
    {
        // Cache component references once at startup
        CacheComponents();
    }

    void Start()
    {
        // Initialize static references if they haven't been set
        InitializeStaticReferences();
    }

    private void CacheComponents()
    {
        itemLevelText = GetComponentInChildren<Text>();
        itemImage = GetComponentInChildren<Image>();
        itemOutline = GetComponentInParent<Outline>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Only log warnings if components are expected but missing
        if (itemLevelText == null) Debug.LogWarning($"No Text component found in children of {gameObject.name}");
        if (itemImage == null) Debug.LogWarning($"No Image component found in children of {gameObject.name}");
        if (canvasGroup == null && type != "Store" && type != "Sell" && type != "Stash")
            Debug.LogWarning($"No CanvasGroup component found on {gameObject.name}");
    }

    private static void InitializeStaticReferences()
    {
        // Initialize player reference
        if (player == null && GameManager.gm != null)
        {
            player = GameManager.gm.player;
        }

        // Cache frequently accessed transforms
        if (itemsPanelTransform == null)
        {
            GameObject itemsPanel = GameObject.Find("ItemsPanel");
            if (itemsPanel != null) itemsPanelTransform = itemsPanel.transform;
        }

        if (inventoryPanelTransform == null)
        {
            GameObject inventoryPanel = GameObject.Find("InventoryPanel");
            if (inventoryPanel != null) inventoryPanelTransform = inventoryPanel.transform;
        }

        // Cache gold text object
        if (goldTextObject == null)
        {
            goldTextObject = GameObject.Find("Gold");
        }

        // Cache common UI elements
        CacheUIElements();
        CacheOutlineComponents();
    }

    private static void CacheUIElements()
    {
        string[] commonElements = { "Strength", "Dexterity", "Intelligence", "ArmorPen", "MagicPen", "CritChance", "CritDamage" };

        foreach (string elementName in commonElements)
        {
            if (!uiElementCache.ContainsKey(elementName))
            {
                GameObject element = GameObject.Find(elementName);
                if (element != null) uiElementCache[elementName] = element;
            }
        }
    }

    private static void CacheOutlineComponents()
    {
        string[] panelNames = { "Weapon1Panel", "Weapon2Panel", "Weapon3Panel", "HeadPanel", "ChestPanel", "LegsPanel", "BootsPanel", "GlovesPanel" };

        foreach (string panelName in panelNames)
        {
            if (!outlineCache.ContainsKey(panelName))
            {
                GameObject panel = GameObject.Find(panelName);
                if (panel != null)
                {
                    Outline outline = panel.GetComponent<Outline>();
                    if (outline != null) outlineCache[panelName] = outline;
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");

        startItem = item;
        if (startItem != null)
        {
            startObject = gameObject;
            startPosition = transform.position;
            startParent = transform.parent;

            // Use cached reference instead of GameObject.Find
            if (itemsPanelTransform != null)
            {
                transform.SetParent(itemsPanelTransform);
            }

            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = false;
            }

            HighlightCompatibleSlots();
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (startParent != null)
        {
            transform.SetParent(startParent);
        }
        transform.position = startPosition;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        ResetSlotOutlines();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (startObject == null || startItem == null)
        {
            Debug.LogError("Start object or start item is null in OnDrop");
            return;
        }

        ItemPanel startPanel = startObject.GetComponent<ItemPanel>();

        if (startPanel == null)
        {
            Debug.LogError("Start object does not have ItemPanel component");
            return;
        }

        string startType = startPanel.type ?? "Unknown";
        string targetType = type ?? "Unknown";

        Debug.Log($"StartObject: {startType}, TargetObject: {targetType}");

        try
        {
            ProcessDropAction(startType, targetType);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in OnDrop: {e.Message}\n{e.StackTrace}");
            ShowFloatingText("Error occurred");
        }
    }

    private void ProcessDropAction(string startType, string targetType)
    {
        // Store to inventory
        if (startType == "Store" && targetType == "Inventory")
        {
            HandleStorePurchase();
        }
        // Store to non inventory
        else if (startType == "Store" && targetType != "Inventory")
        {
            ShowFloatingText("Drag to inventory");
        }
        // Item dragged to store
        else if (targetType == "Store")
        {
            // Do nothing
        }
        // Store item dragged to sell
        else if (startType == "Store" && targetType == "Sell")
        {
            // Do nothing
        }
        // Trying to sell from non inventory
        else if (targetType == "Sell" && startType != "Inventory")
        {
            ShowFloatingText("Unequip first");
        }
        // Sell item to store from inventory
        else if (targetType == "Sell" && startType == "Inventory")
        {
            HandleItemSale();
        }
        // Trying to stash from non inventory
        else if (targetType == "Stash" && startType != "Inventory")
        {
            ShowFloatingText("Unequip first");
        }
        // Stash item from inventory
        else if (targetType == "Stash" && startType == "Inventory")
        {
            HandleStashDeposit();
        }
        // Trying to remove from stash to non inventory
        else if (startType == "Stash" && targetType != "Inventory")
        {
            ShowFloatingText("Add to inventory first");
        }
        // Remove from stash to inventory
        else if (startType == "Stash" && targetType == "Inventory")
        {
            HandleStashWithdrawal();
        }
        // Equip
        else if (!equipTypes.Contains(startObject.name) && equipTypes.Contains(gameObject.name))
        {
            Equip();
        }
        else if (weaponTypes.Contains(startObject.name) && weaponTypes.Contains(gameObject.name))
        {
            Equip();
        }
        // Unequip Trash
        else if (equipTypes.Contains(startObject.name) && targetType == "Trash")
        {
            HandleEquipmentTrash();
        }
        // Inventory Trash
        else if (startType == "Inventory" && targetType == "Trash")
        {
            HandleInventoryTrash();
        }
        // Unequip
        else if (equipTypes.Contains(startObject.name) && !equipTypes.Contains(gameObject.name))
        {
            Unequip();
        }
        // Sort Inventory
        else if (startType == "Inventory" && targetType == "Inventory")
        {
            SortInventory();
        }
        else
        {
            Debug.Log("Drag and Drop action not accounted for");
        }
    }

    private void HandleStorePurchase()
    {
        if (player == null || startItem == null) return;

        if (player.gold >= startItem.buyValue)
        {
            Debug.Log("Dragged from Store");
            player.inventory.Add(startItem);
            RemoveItem(startObject);
            player.gold -= startItem.buyValue;

            PlayCachedSound("pickupitemsfx");
            UpdateInventoryDisplay();
        }
        else
        {
            PlayCachedSound("failsfx");
            ShowFloatingText("Not enough gold");
        }
    }

    private void HandleItemSale()
    {
        if (player == null || startItem == null) return;

        Debug.Log("Sell item to store");
        player.inventory.Remove(startItem);
        RemoveItem(startObject);
        player.gold += startItem.sellValue;

        UpdateGoldDisplay();
        PlayCachedSound("coinsfx");
        UpdateInventoryDisplay();
    }

    private void HandleStashDeposit()
    {
        if (player == null || startItem == null || GameManager.gm == null) return;

        player.inventory.Remove(startItem);
        GameManager.gm.stash.Add(startItem);
        UpdateInventoryDisplay();
    }

    private void HandleStashWithdrawal()
    {
        if (player == null || startItem == null || GameManager.gm == null) return;

        player.inventory.Add(startItem);
        GameManager.gm.stash.Remove(startItem);
        UpdateInventoryDisplay();
    }

    private void HandleEquipmentTrash()
    {
        if (player == null || startItem == null) return;

        Debug.Log("Unequip Trash");
        player.inventory.Add(startItem);
        RemoveItem(startObject);
        player.Unequip(startItem);
        player.inventory.Remove(startItem);
        UpdateEquipment();
        PlayCachedSound("dropsfx");
    }

    private void HandleInventoryTrash()
    {
        if (player == null || startItem == null) return;

        Debug.Log("Inventory Trash");
        RemoveItem(startObject);
        player.inventory.Remove(startItem);
        UpdateEquipment();
        PlayCachedSound("dropsfx");
    }

    private static void PlayCachedSound(string soundName)
    {
        if (SoundManager.sm == null) return;

        if (!audioClipCache.TryGetValue(soundName, out AudioClip clip))
        {
            clip = Resources.Load<AudioClip>($"Sfx/{soundName}");
            if (clip != null)
            {
                audioClipCache[soundName] = clip;
            }
        }

        if (clip != null)
        {
            SoundManager.sm.PlaySoundFX(clip);
        }
    }

    private static void UpdateGoldDisplay()
    {
        if (goldTextObject != null && player != null)
        {
            Text goldText = goldTextObject.GetComponentInChildren<Text>();
            if (goldText != null)
            {
                goldText.text = "Gold: " + player.gold.ToString();
            }
        }
    }

    private void HighlightCompatibleSlots()
    {
        if (startItem == null) return;

        if (startItem is Weapon)
        {
            Debug.Log("isWeapon");
            SetOutlineColor("Weapon1Panel", orangeHighlight);
            SetOutlineColor("Weapon2Panel", orangeHighlight);
            SetOutlineColor("Weapon3Panel", orangeHighlight);
        }

        if (startItem is Armor armor)
        {
            Debug.Log("isArmor");
            switch (armor.armorType)
            {
                case Armor.ArmorType.Head:
                    SetOutlineColor("HeadPanel", orangeHighlight);
                    break;
                case Armor.ArmorType.Chest:
                    SetOutlineColor("ChestPanel", orangeHighlight);
                    break;
                case Armor.ArmorType.Legs:
                    SetOutlineColor("LegsPanel", orangeHighlight);
                    break;
                case Armor.ArmorType.Boots:
                    SetOutlineColor("BootsPanel", orangeHighlight);
                    break;
                case Armor.ArmorType.Gloves:
                    SetOutlineColor("GlovesPanel", orangeHighlight);
                    break;
            }
        }
    }

    private void ResetSlotOutlines()
    {
        if (player == null) return;

        // Reset weapon outlines using cached references
        SetOutlineColor("Weapon1Panel", player.weapon1?.GetRarityColor() ?? Color.black);
        SetOutlineColor("Weapon2Panel", player.weapon2?.GetRarityColor() ?? Color.black);
        SetOutlineColor("Weapon3Panel", player.weapon3?.GetRarityColor() ?? Color.black);

        // Reset armor outlines using cached references
        SetOutlineColor("HeadPanel", player.head?.GetRarityColor() ?? Color.black);
        SetOutlineColor("ChestPanel", player.chest?.GetRarityColor() ?? Color.black);
        SetOutlineColor("LegsPanel", player.legs?.GetRarityColor() ?? Color.black);
        SetOutlineColor("BootsPanel", player.boots?.GetRarityColor() ?? Color.black);
        SetOutlineColor("GlovesPanel", player.gloves?.GetRarityColor() ?? Color.black);
    }

    private static void SetOutlineColor(string panelName, Color color)
    {
        if (outlineCache.TryGetValue(panelName, out Outline outline))
        {
            // Check if the cached outline is still valid
            if (outline != null && outline.gameObject != null)
            {
                outline.effectColor = color;
            }
            else
            {
                // Remove invalid cached reference and try to find it again
                outlineCache.Remove(panelName);
                GameObject panel = GameObject.Find(panelName);
                if (panel != null)
                {
                    Outline newOutline = panel.GetComponent<Outline>();
                    if (newOutline != null)
                    {
                        outlineCache[panelName] = newOutline;
                        newOutline.effectColor = color;
                    }
                }
            }
        }
        else
        {
            // Not cached, find and cache it
            GameObject panel = GameObject.Find(panelName);
            if (panel != null)
            {
                Outline newOutline = panel.GetComponent<Outline>();
                if (newOutline != null)
                {
                    outlineCache[panelName] = newOutline;
                    newOutline.effectColor = color;
                }
            }
        }
    }

    void Equip()
    {
        if (player == null || startItem == null)
        {
            Debug.LogError("Player or startItem is null in Equip()");
            return;
        }

        // Required level and attributes check
        if (!CanEquipItem(startItem))
        {
            PlayCachedSound("failsfx");
            ShowFloatingText("Requirements not met");
            return;
        }

        // Check if weapon
        if (weaponTypes.Contains(gameObject.name) && startItem is Weapon)
        {
            EquipWeapon();
        }
        // Check if armor
        else if (armorTypes.Contains(gameObject.name) && startItem is Armor armor)
        {
            EquipArmor(armor);
        }
    }

    private bool CanEquipItem(Item item)
    {
        if (player == null || item == null) return false;

        return player.level >= item.requiredLevel &&
               player.strength >= item.requiredStr ||
               player.dexterity >= item.requiredDex ||
               player.intelligence >= item.requiredInt;
    }

    private void EquipWeapon()
    {
        if (weaponTypes.Contains(startObject.name) && weaponTypes.Contains(gameObject.name))
        {
            Debug.Log("Swap Equipped Weapons");
            SwapEquippedItems();
        }
        else if (startItem != null && item != null)
        {
            Debug.Log("Weapon Swap");
            SwapWithInventoryItem();
        }
        else if (startItem != null && item == null)
        {
            Debug.Log("Weapon No Swap");
            EquipFromInventory();
        }
    }

    private void EquipArmor(Armor armor)
    {
        if (!gameObject.name.Contains(armor.armorType.ToString())) return;

        if (startItem != null && item != null)
        {
            Debug.Log("Armor Swap");
            SwapWithInventoryItem();
        }
        else if (startItem != null && item == null)
        {
            Debug.Log("Armor No Swap");
            EquipFromInventory();
        }
    }

    private void SwapEquippedItems()
    {
        if (startObject == null) return;

        startObject.transform.SetParent(startParent);
        SetItem(startObject, item);
        SetItem(gameObject, startItem);
    }

    private void SwapWithInventoryItem()
    {
        if (startObject == null || player == null) return;

        startObject.transform.SetParent(startParent);
        player.inventory.Remove(startItem);
        player.inventory.Add(item);
        player.Unequip(item);
        SetItem(startObject, item);
        SetItem(gameObject, startItem);
    }

    private void EquipFromInventory()
    {
        if (startObject == null || player == null) return;

        player.inventory.Remove(startItem);
        RemoveItem(startObject);
        SetItem(gameObject, startItem);
    }

    void Unequip()
    {
        if (!equipTypes.Contains(startObject.name) || equipTypes.Contains(gameObject.name)) return;

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
        else if (startItem != null && item == null)
        {
            Debug.Log("No Swap");
            player.inventory.Add(startItem);
            RemoveItem(startObject);
            if (startItem is Armor)
                player.Unequip(startItem);
            SetItem(gameObject, startItem);
        }
    }

    void SortInventory()
    {
        if (equipTypes.Contains(gameObject.name)) return;

        // Swap Items
        if (startItem != null && item != null)
        {
            Debug.Log("Swap");
            startObject.transform.SetParent(startParent);
            SetItem(startObject, item);
            SetItem(gameObject, startItem);
        }
        // No Swap
        else if (startItem != null && item == null)
        {
            Debug.Log("No Swap");
            RemoveItem(startObject);
            SetItem(gameObject, startItem);
        }
    }

    private void SetItem(GameObject targetObject, Item targetItem)
    {
        if (targetObject == null)
        {
            Debug.LogError("Target GameObject is null in SetItem");
            return;
        }

        if (targetItem == null)
        {
            Debug.LogError("Target Item is null in SetItem");
            return;
        }

        Debug.Log("Set Item: " + targetItem.name);

        ItemPanel itemPanel = targetObject.GetComponent<ItemPanel>();
        if (itemPanel == null)
        {
            Debug.LogError($"No ItemPanel component found on {targetObject.name}");
            return;
        }

        // Set the item
        itemPanel.item = targetItem;
        Debug.Log("Item ilvl: " + targetItem.ilvl.ToString());

        // Set item level text using cached component
        if (itemPanel.itemLevelText != null)
        {
            itemPanel.itemLevelText.text = targetItem.ilvl.ToString();
        }

        // Set sprite using cached component and sprite cache
        if (itemPanel.itemImage != null && !string.IsNullOrEmpty(targetItem.spritePath))
        {
            Sprite sprite = GetCachedSprite(targetItem.spritePath);
            if (sprite != null)
            {
                itemPanel.itemImage.sprite = sprite;
            }
        }

        // Set outline color using cached component
        if (itemPanel.itemOutline != null)
        {
            itemPanel.itemOutline.effectColor = targetItem.GetRarityColor();
        }

        // Equip the item based on slot type
        if (player != null)
        {
            string slotType = itemPanel.type;
            switch (slotType)
            {
                case "Head":
                    player.head = (Armor)player.Equip(targetItem);
                    break;
                case "Chest":
                    player.chest = (Armor)player.Equip(targetItem);
                    break;
                case "Gloves":
                    player.gloves = (Armor)player.Equip(targetItem);
                    break;
                case "Boots":
                    player.boots = (Armor)player.Equip(targetItem);
                    break;
                case "Legs":
                    player.legs = (Armor)player.Equip(targetItem);
                    break;
                case "Weapon1":
                    player.weapon1 = (Weapon)targetItem;
                    break;
                case "Weapon2":
                    player.weapon2 = (Weapon)targetItem;
                    break;
                case "Weapon3":
                    player.weapon3 = (Weapon)targetItem;
                    break;
            }
        }

        UpdateEquipment();
    }

    private static Sprite GetCachedSprite(string spritePath)
    {
        if (!spriteCache.TryGetValue(spritePath, out Sprite sprite))
        {
            sprite = Resources.Load<Sprite>(spritePath);
            if (sprite != null)
            {
                spriteCache[spritePath] = sprite;
            }
        }
        return sprite;
    }

    private void UpdateEquipment()
    {
        if (player == null) return;

        // Set Equipped Armor
        UpdateArmorSlot("HeadItem", ref player.head);
        UpdateArmorSlot("ChestItem", ref player.chest);
        UpdateArmorSlot("GlovesItem", ref player.gloves);
        UpdateArmorSlot("BootsItem", ref player.boots);
        UpdateArmorSlot("LegsItem", ref player.legs);

        // Set Equipped Weapons
        UpdateWeaponSlot("Weapon1Item", ref player.weapon1);
        UpdateWeaponSlot("Weapon2Item", ref player.weapon2);
        UpdateWeaponSlot("Weapon3Item", ref player.weapon3);

        UpdateWeaponButtons();
        UpdateCharacterSheet();
    }

    private void UpdateArmorSlot(string slotName, ref Armor armorSlot)
    {
        GameObject slotObject = GameObject.Find(slotName);
        if (slotObject != null)
        {
            ItemPanel slotPanel = slotObject.GetComponent<ItemPanel>();
            if (slotPanel?.item is Armor armor)
            {
                armorSlot = armor;
            }
            else
            {
                armorSlot = null;
            }
        }
    }

    private void UpdateWeaponSlot(string slotName, ref Weapon weaponSlot)
    {
        GameObject slotObject = GameObject.Find(slotName);
        if (slotObject != null)
        {
            ItemPanel slotPanel = slotObject.GetComponent<ItemPanel>();
            if (slotPanel?.item is Weapon weapon)
            {
                weaponSlot = weapon;
            }
            else
            {
                weaponSlot = new Weapon();
            }
        }
    }

    private void UpdateWeaponButtons()
    {
        if (SceneManager.GetActiveScene().name != "Battle" || player == null) return;

        UpdateWeaponButton("Slot1Button", player.weapon1);
        UpdateWeaponButton("Slot2Button", player.weapon2);
        UpdateWeaponButton("Slot3Button", player.weapon3);

        // Update equipped weapon
        GameObject slot1 = GameObject.Find("Slot1Button");
        GameObject slot2 = GameObject.Find("Slot2Button");
        GameObject slot3 = GameObject.Find("Slot3Button");

        if (slot1?.GetComponentInChildren<Outline>()?.effectColor == Color.white)
            player.equippedWeapon = player.weapon1;
        else if (slot2?.GetComponentInChildren<Outline>()?.effectColor == Color.white)
            player.equippedWeapon = player.weapon2;
        else if (slot3?.GetComponentInChildren<Outline>()?.effectColor == Color.white)
            player.equippedWeapon = player.weapon3;
    }

    private void UpdateWeaponButton(string buttonName, Weapon weapon)
    {
        GameObject button = GameObject.Find(buttonName);
        if (button != null && weapon != null)
        {
            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = weapon.name;
                buttonText.color = weapon.GetRarityColor();
            }
        }
    }

    private void UpdateCharacterSheet()
    {
        if (player == null) return;

        UpdateStatText("Strength", "Strength: " + player.strength + " (" + player.GetBaseStat(player.strength, player.modifiedStrength) + ")");
        UpdateStatText("Dexterity", "Dexterity: " + player.dexterity + " (" + player.GetBaseStat(player.dexterity, player.modifiedDexterity) + ")");
        UpdateStatText("Intelligence", "Intelligence: " + player.intelligence + " (" + player.GetBaseStat(player.intelligence, player.modifiedIntelligence) + ")");
        UpdateStatText("ArmorPen", "Armor Pen: " + player.armorPen);
        UpdateStatText("MagicPen", "Magic Pen: " + player.magicPen);
        UpdateStatText("CritChance", "Crit Chance: " + player.critChance + "%");
        UpdateStatText("CritDamage", "Crit Damage: " + player.critDamage + "%");
    }

    private void UpdateStatText(string startName, string statValue)
    {
        if (uiElementCache.TryGetValue(startName, out GameObject cachedStatObject))
        {
            // Check if cached object is still valid
            if (cachedStatObject != null)
            {
                Text statText = cachedStatObject.GetComponentInChildren<Text>();
                if (statText != null)
                {
                    statText.text = statValue;
                    return;
                }
            }

            // Remove invalid cached reference
            uiElementCache.Remove(startName);
        }

        // Find the object fresh
        GameObject foundStatObject = GameObject.Find(startName);
        if (foundStatObject != null)
        {
            Text statText = foundStatObject.GetComponentInChildren<Text>();
            if (statText != null)
            {
                statText.text = statValue;
                // Cache it for future use
                uiElementCache[startName] = foundStatObject;
            }
        }
    }

    private void UpdateInventoryDisplay()
    {
        if (player == null) return;

        // Clear all inventory slots
        for (int i = 0; i < 15; i++)
        {
            GameObject slot = GameObject.Find("Item" + i);
            if (slot != null)
            {
                RemoveItemVisual(slot);
            }
        }

        // Populate inventory slots with current items
        for (int i = 0; i < player.inventory.Count && i < 15; i++)
        {
            GameObject slot = GameObject.Find("Item" + i);
            if (slot != null && player.inventory[i] != null)
            {
                SetItem(slot, player.inventory[i]);
            }
        }

        // Clear any invalid cached references after UI updates
        CleanupInvalidCachedReferences();
    }

    // Method to clean up invalid cached references
    private static void CleanupInvalidCachedReferences()
    {
        // Clean up outline cache
        List<string> invalidOutlineKeys = new List<string>();
        foreach (var kvp in outlineCache)
        {
            if (kvp.Value == null || kvp.Value.gameObject == null)
            {
                invalidOutlineKeys.Add(kvp.Key);
            }
        }

        foreach (string key in invalidOutlineKeys)
        {
            outlineCache.Remove(key);
        }

        // Clean up UI element cache
        List<string> invalidUIKeys = new List<string>();
        foreach (var kvp in uiElementCache)
        {
            if (kvp.Value == null)
            {
                invalidUIKeys.Add(kvp.Key);
            }
        }

        foreach (string key in invalidUIKeys)
        {
            uiElementCache.Remove(key);
        }
    }

    private void RemoveItemVisual(GameObject targetObject)
    {
        if (targetObject == null) return;

        ItemPanel itemPanel = targetObject.GetComponent<ItemPanel>();
        if (itemPanel != null)
        {
            itemPanel.item = null;

            if (itemPanel.itemLevelText != null)
            {
                itemPanel.itemLevelText.text = "";
            }

            if (itemPanel.itemImage != null)
            {
                Sprite noneSprite = GetCachedSprite("None");
                if (noneSprite != null)
                {
                    itemPanel.itemImage.sprite = noneSprite;
                }
            }

            if (itemPanel.itemOutline != null)
            {
                itemPanel.itemOutline.effectColor = Color.black;
            }
        }

        if (startParent != null)
        {
            targetObject.transform.SetParent(startParent);
        }
    }

    private void RemoveItem(GameObject targetObject)
    {
        // This method is kept for compatibility with existing equip/unequip logic
        RemoveItemVisual(targetObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || tooltipPrefab == null) return;
        CreateTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyTooltip();
    }

    private void CreateTooltip()
    {
        if (tooltip != null) return;

        RectTransform rt = (RectTransform)tooltipPrefab.transform;
        CalculateTooltipOffset(rt);

        tooltip = Instantiate(tooltipPrefab, Input.mousePosition + tooltipOffset, Quaternion.identity);

        if (inventoryPanelTransform != null)
        {
            tooltip.transform.SetParent(inventoryPanelTransform, false);
        }

        UpdateTooltipContent();
    }

    private void CalculateTooltipOffset(RectTransform tooltipRect)
    {
        tooltipOffset.x = -tooltipRect.rect.width * 0.5f - (Screen.width * 0.05f);
        tooltipOffset.y = Input.mousePosition.y < Screen.height * 0.5f ?
            tooltipRect.rect.height * 0.5f + (Screen.height * 0.1f) :
            -tooltipRect.rect.height * 0.5f - (Screen.height * 0.1f);
    }

    private void UpdateTooltipContent()
    {
        if (tooltip == null || item == null) return;

        // Find tooltip name component in the instantiated tooltip
        Text tooltipNameText = tooltip.transform.Find("TooltipName")?.GetComponent<Text>();
        if (tooltipNameText == null)
        {
            // Fallback: search in children if direct path doesn't work
            tooltipNameText = tooltip.GetComponentInChildren<Text>();
            // If there are multiple Text components, we need to find the right one
            Text[] textComponents = tooltip.GetComponentsInChildren<Text>();
            if (textComponents.Length > 0)
            {
                // First text component is typically the name
                tooltipNameText = textComponents[0];
            }
        }

        if (tooltipNameText != null)
        {
            tooltipNameText.color = item.GetRarityColor();
            tooltipNameText.text = item.name;
        }

        // Find tooltip description component in the instantiated tooltip
        Text tooltipDescText = null;
        Transform tooltipDesc = tooltip.transform.Find("Tooltip");
        if (tooltipDesc != null)
        {
            tooltipDescText = tooltipDesc.GetComponentInChildren<Text>();
        }
        else
        {
            // Fallback: if there are multiple Text components, the second one is usually the description
            Text[] textComponents = tooltip.GetComponentsInChildren<Text>();
            if (textComponents.Length > 1)
            {
                tooltipDescText = textComponents[1];
            }
            else if (textComponents.Length == 1)
            {
                // If there's only one text component, it might be used for both name and description
                tooltipDescText = textComponents[0];
                // In this case, combine name and description
                tooltipDescText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(item.GetRarityColor())}>{item.name}</color>\n\n{item.GetToolTip()}";
                return;
            }
        }

        if (tooltipDescText != null)
        {
            tooltipDescText.text = item.GetToolTip();
        }
        else
        {
            Debug.LogWarning("Could not find tooltip description text component");
        }
    }

    private void DestroyTooltip()
    {
        if (tooltip != null)
        {
            Destroy(tooltip);
            tooltip = null;
        }
    }

    void Update()
    {
        UpdateTooltipPosition();
    }

    private void UpdateTooltipPosition()
    {
        if (tooltip == null || item == null) return;

        RectTransform rt = (RectTransform)tooltip.transform;
        CalculateTooltipOffset(rt);
        tooltip.transform.position = Input.mousePosition + tooltipOffset;
        UpdateTooltipContent();
    }

    private void ShowFloatingText(string text)
    {
        if (floatingTextPrefab == null)
        {
            Debug.LogWarning("FloatingTextPrefab is null");
            return;
        }

        GameObject floatingText = Instantiate(floatingTextPrefab, Input.mousePosition + floatingTextOffset, Quaternion.identity, transform);

        if (itemsPanelTransform != null)
        {
            floatingText.transform.SetParent(itemsPanelTransform);
        }
        else
        {
            Debug.LogWarning("ItemsPanel not found for floating text");
        }

        floatingText.transform.localScale = new Vector3(2, 2, 0);

        Text floatingTextComponent = floatingText.GetComponentInChildren<Text>();
        if (floatingTextComponent != null)
        {
            floatingTextComponent.fontStyle = FontStyle.Bold;
            floatingTextComponent.fontSize = 20;
            floatingTextComponent.text = text;
        }
        else
        {
            Debug.LogWarning("No Text component found in FloatingText prefab");
        }
    }
}