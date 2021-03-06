﻿using System;
using System.Collections.Generic;

[Serializable]
public class Armor : Item {

    // Enumerators
    public enum ArmorType
    {
        None,
        Head,
        Chest,
        Gloves,
        Legs,
        Boots,
        Amulet
    }

    public ArmorType armorType = ArmorType.None;

    // Constructor
    public Armor()
    {
        name = "Garbage";
        spritePath = "Icons/garbage";
    }

    //Tooltip override
    override
    public string GetToolTip()
    {
        string tooltip = "";
        tooltip += "Armor Type: " + armorType + Environment.NewLine;
        tooltip += "Str: " + strength + " Dex: " + dexterity + " Int: " + intelligence + Environment.NewLine;
        tooltip += "A Pen: " + armorPen + " M Pen: " + magicPen + Environment.NewLine;
        tooltip += "Crit: " + critChance + "% Crit Dmg: " + critDamage + "%" + Environment.NewLine;
        if (requiredLevel > 0)
            tooltip += "Required Level: " + requiredLevel + Environment.NewLine;
        if (requiredStr > 0)
            tooltip += "Required Str: " + requiredStr + Environment.NewLine;
        if (requiredDex > 0)
            tooltip += "Required Dex: " + requiredDex + Environment.NewLine;
        if (requiredInt > 0)
            tooltip += "Required Int: " + requiredInt + Environment.NewLine;
        if (flavorText != "")
            tooltip += '"' + flavorText + '"' + Environment.NewLine;
        tooltip += "Sell Value: " + sellValue + "G" + Environment.NewLine;
        return tooltip;
    }

    // Get Armor by item id from list
    public Armor GetArmorById(List<Armor> list, int id)
    {
        Armor a = new Armor();
        if ((list.Find(armor => armor.itemId == id) != null))
            a = list.Find(armor => armor.itemId == id);
        return a;
    }

    // Get a list of all items in the tier
    public List<Armor> GetListByTier(List<Armor> list, int tier)
    {
        List<Armor> tierList = new List<Armor>();
        foreach (Armor a in list)
        {
            // Check if the first digit of the id matches the tier
            if ((Int32.Parse(a.itemId.ToString("0000").Substring(0, 1))) == tier)
                tierList.Add(a);
        }
        if (tierList.Count == 0)
            tierList = list;
        return tierList;
    }

    // Get a list of all the rarities specified 
    public List<Armor> GetListByRarity(List<Armor> list, Rarity rarity)
    {
        List<Armor> rarityList = new List<Armor>();
        foreach (Armor a in list)
        {
            if (a.rarity == rarity)
                rarityList.Add(a);
        }
        // Check if the list is empty
        if (rarityList.Count == 0)
            rarityList = list;
        return rarityList;
    }

    // List of all armor
    public List<Armor> ArmorList()
    {
        List<Armor> armorList = new List<Armor>();

        Armor armor;

        /***************/
        /*** TIER 0 ***/
        /*************/
        armor = new Armor
        {
            itemId = 0001,
            name = "Loose Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Common,
            buyValue = 10,
            sellValue = 5,
            requiredLevel = 1,
            armorType = ArmorType.Head,
            strength = 1,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 0002,
            name = "Loose Shirt",
            spritePath = "Icons/chest",
            rarity = Rarity.Common,
            buyValue = 10,
            sellValue = 5,
            requiredLevel = 1,
            armorType = ArmorType.Chest,
            strength = 1,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 0003,
            name = "Loose Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Common,
            buyValue = 10,
            sellValue = 5,
            requiredLevel = 1,
            armorType = ArmorType.Gloves,
            strength = 1,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 0004,
            name = "Loose Legs",
            spritePath = "Icons/legs",
            rarity = Rarity.Common,
            buyValue = 10,
            sellValue = 5,
            requiredLevel = 1,
            armorType = ArmorType.Legs,
            strength = 1,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 0005,
            name = "Loose Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Common,
            buyValue = 10,
            sellValue = 5,
            requiredLevel = 1,
            armorType = ArmorType.Boots,
            strength = 1,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,

        };
        armorList.Add(armor);

        /***************/
        /*** TIER 1 ***/
        /*************/
        armor = new Armor
        {
            itemId = 1101,
            name = "Iron Helm",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Head,
            strength = 3,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1102,
            name = "Iron Breastplate",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Chest,
            strength = 3,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,
  

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1103,
            name = "Iron Gauntlets",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Gloves,
            strength = 3,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,
     

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1104,
            name = "Iron Greaves",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Legs,
            strength = 3,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,
     

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1105,
            name = "Iron Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Boots,
            strength = 3,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,
      

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1201,
            name = "Leather Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Head,
            strength = 1,
            dexterity = 3,
            intelligence = 1,
            critChance = 0,
            ilvl = 5,
            tier = 1,
    

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1202,
            name = "Leather Vest",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Chest,
            strength = 1,
            dexterity = 3,
            intelligence = 1,
            critChance = 0,
            ilvl = 5,
            tier = 1,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1203,
            name = "Leather Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Gloves,
            strength = 1,
            dexterity = 3,
            intelligence = 1,
            critChance = 0,
            ilvl = 5,
            tier = 1,
 

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1204,
            name = "Leather Leggings",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Legs,
            strength = 1,
            dexterity = 3,
            intelligence = 1,
            critChance = 0,
            ilvl = 5,
            tier = 1,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1205,
            name = "Leather Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Boots,
            strength = 1,
            dexterity = 3,
            intelligence = 1,
            critChance = 0,
            ilvl = 5,
            tier = 1,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1301,
            name = "Cloth Hood",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Head,
            strength = 1,
            dexterity = 1,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1302,
            name = "Cloth Robes",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Chest,
            strength = 1,
            dexterity = 1,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1303,
            name = "Cloth Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Gloves,
            strength = 1,
            dexterity = 1,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1304,
            name = "Cloth Pants",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Legs,
            strength = 1,
            dexterity = 1,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1305,
            name = "Cloth Shoes",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 300,
            sellValue = 100,
            requiredLevel = 1,
            armorType = ArmorType.Boots,
            strength = 1,
            dexterity = 1,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 5,
            tier = 1,


        };
        armorList.Add(armor);

        /********************/
        /*** LEGENDARIES ***/
        /******************/
        armor = new Armor
        {
            itemId = 1111,
            name = "Flashlight Hardhat",
            spritePath = "Icons/helm",
            flavorText = "Safety first, kids",
            rarity = Rarity.Legendary,
            buyValue = 2500,
            sellValue = 500,
            requiredLevel = 1,
            armorType = ArmorType.Head,
            strength = 5,
            dexterity = 1,
            intelligence = 1,
            critChance = 0,
            critDamage = 0,
            bonusPhysical = 1,
            itemFind = 2,
            magicFind = 2,
            ilvl = 10,
            tier = 1,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1213,
            name = "Duelist Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Set,
            buyValue = 2500,
            sellValue = 500,
            requiredLevel = 1,
            armorType = ArmorType.Gloves,
            strength = 1,
            dexterity = 3,
            intelligence = 1,
            critChance = 3,
            critDamage = 10,
            ilvl = 10,
            tier = 1,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1212,
            name = "Duelist Cuirass",
            spritePath = "Icons/chest",
            rarity = Rarity.Set,
            buyValue = 2500,
            sellValue = 500,
            requiredLevel = 1,
            armorType = ArmorType.Chest,
            strength = 1,
            dexterity = 3,
            intelligence = 1,
            critChance = 3,
            critDamage = 10,
            ilvl = 10,
            tier = 1,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1215,
            name = "Duelist Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Set,
            buyValue = 2500,
            sellValue = 500,
            requiredLevel = 1,
            armorType = ArmorType.Boots,
            strength = 1,
            dexterity = 3,
            intelligence = 1,
            critChance = 3,
            critDamage = 10,
            ilvl = 10,
            tier = 1,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 1313,
            name = "Skeletal Gloves",
            flavorText = "Clammy",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Legendary,
            buyValue = 2500,
            sellValue = 500,
            requiredLevel = 1,
            armorType = ArmorType.Gloves,
            strength = 1,
            dexterity = 1,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 0,
            bonusPhysical = 3,
            ilvl = 10,
            tier = 1,

        };
        armorList.Add(armor);
        /********************/
        /***   TIER 2    ***/
        /******************/
        armor = new Armor
        {
            itemId = 2101,
            name = "Steel Helm",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredStr = 10,
            armorType = ArmorType.Head,
            strength = 5,
            dexterity = 2,
            intelligence = 2,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,
 

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2102,
            name = "Steel Breastplate",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredStr = 10,
            armorType = ArmorType.Chest,
            strength = 5,
            dexterity = 2,
            intelligence = 2,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,
    

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2103,
            name = "Steel Gauntlets",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredStr = 10,
            armorType = ArmorType.Gloves,
            strength = 5,
            dexterity = 2,
            intelligence = 2,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,
 

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2104,
            name = "Steel Greaves",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredStr = 10,
            armorType = ArmorType.Legs,
            strength = 5,
            dexterity = 2,
            intelligence = 2,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,
  

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2105,
            name = "Steel Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredStr = 10,
            armorType = ArmorType.Boots,
            strength = 5,
            dexterity = 2,
            intelligence = 2,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,
 

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2201,
            name = "Studded Leather Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredDex = 10,
            armorType = ArmorType.Head,
            strength = 2,
            dexterity = 5,
            intelligence = 2,
            critChance = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2202,
            name = "Studded Leather Vest",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredDex = 10,
            armorType = ArmorType.Chest,
            strength = 2,
            dexterity = 5,
            intelligence = 2,
            critChance = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2203,
            name = "Studded Leather Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredDex = 10,
            armorType = ArmorType.Gloves,
            strength = 2,
            dexterity = 5,
            intelligence = 2,
            critChance = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2204,
            name = "Studded Leather Leggings",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredDex = 10,
            armorType = ArmorType.Legs,
            strength = 2,
            dexterity = 5,
            intelligence = 2,
            critChance = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2205,
            name = "Studded Leather Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredDex = 10,
            armorType = ArmorType.Boots,
            strength = 2,
            dexterity = 5,
            intelligence = 2,
            critChance = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2301,
            name = "Fine Cloth Hood",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredInt = 10,
            armorType = ArmorType.Head,
            strength = 2,
            dexterity = 2,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2302,
            name = "Fine Cloth Robes",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredInt = 10,
            armorType = ArmorType.Chest,
            strength = 2,
            dexterity = 2,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2303,
            name = "Fine Cloth Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredInt = 10,
            armorType = ArmorType.Gloves,
            strength = 2,
            dexterity = 2,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2304,
            name = "Fine Cloth Pants",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredInt = 10,
            armorType = ArmorType.Legs,
            strength = 2,
            dexterity = 2,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2305,
            name = "Fine Cloth Shoes",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 1000,
            sellValue = 250,
            requiredLevel = 3,
            requiredInt = 10,
            armorType = ArmorType.Boots,
            strength = 2,
            dexterity = 2,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 25,
            tier = 2,


        };
        armorList.Add(armor);
        /********************/
        /***   LEYENDOS  ***/
        /******************/
        armor = new Armor
        {
            itemId = 2311,
            name = "Necromancer Hood",
            spritePath = "Icons/helm",
            rarity = Rarity.Set,
            buyValue = 5000,
            sellValue = 1250,
            requiredLevel = 3,
            requiredInt = 10,
            armorType = ArmorType.Head,
            strength = 2,
            dexterity = 2,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            bonusPhysical = 5,
            bonusMagical = -3,
            armorPen = 8,
            ilvl = 35,
            tier = 2,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2312,
            name = "Necromancer Robes",
            spritePath = "Icons/chest",
            rarity = Rarity.Set,
            buyValue = 5000,
            sellValue = 1250,
            requiredLevel = 3,
            requiredInt = 10,
            armorType = ArmorType.Chest,
            strength = 2,
            dexterity = 2,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            bonusPhysical = 5,
            bonusMagical = -3,
            armorPen = 8,
            ilvl = 35,
            tier = 2,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2213,
            name = "Pickpocket's Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Legendary,
            buyValue = 5000,
            sellValue = 1250,
            requiredLevel = 3,
            requiredDex = 10,
            armorType = ArmorType.Gloves,
            strength = 2,
            dexterity = 3,
            intelligence = 2,
            critChance = 0,
            critDamage = 0,
            bonusGold = 35,
            ilvl = 35,
            tier = 2,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2113,
            name = "Kiddie Gloves",
            spritePath = "Icons/gloves2",
            flavorText = "You'll be a big kid someday",
            rarity = Rarity.Set,
            buyValue = 5000,
            sellValue = 1250,
            requiredLevel = 3,
            armorType = ArmorType.Gloves,
            strength = 1,
            dexterity = 1,
            intelligence = 1,
            bonusExp = 30,
            ilvl = 35,
            tier = 2,
        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2111,
            name = "Dunce Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Set,
            buyValue = 5000,
            sellValue = 1250,
            requiredLevel = 3,
            requiredStr = 30,
            armorType = ArmorType.Head,
            strength = 10,
            dexterity = 0,
            intelligence = -10,
            critChance = 0,
            critDamage = 0,
            bonusPhysical = 10,
            bonusExp = -10,
            ilvl = 35,
            tier = 2,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 2102,
            name = "Dwarven Steel Breastplate",
            spritePath = "Icons/chest",
            flavorText = "It's a tight fit, but comes with a comfy beard-holster",
            rarity = Rarity.Legendary,
            buyValue = 5000,
            sellValue = 1250,
            requiredLevel = 3,
            requiredStr = 30,
            armorType = ArmorType.Chest,
            strength = 10,
            dexterity = 0,
            intelligence = 0,
            critChance = 0,
            critDamage = 0,
            bonusPhysical = 0,
            armorPen = 8,
            ilvl = 35,
            tier = 2,

        };
        armorList.Add(armor);
        /********************/
        /***   TIER 3    ***/
        /******************/
        armor = new Armor
        {
            itemId = 3101,
            name = "Silver Helm",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredStr = 30,
            armorType = ArmorType.Head,
            strength = 7,
            dexterity = 3,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3102,
            name = "Silver Breastplate",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredStr = 30,
            armorType = ArmorType.Chest,
            strength = 7,
            dexterity = 3,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3103,
            name = "Silver Gauntlets",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredStr = 30,
            armorType = ArmorType.Gloves,
            strength = 7,
            dexterity = 3,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3104,
            name = "Silver Greaves",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredStr = 30,
            armorType = ArmorType.Legs,
            strength = 7,
            dexterity = 3,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3105,
            name = "Silver Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredStr = 30,
            armorType = ArmorType.Boots,
            strength = 7,
            dexterity = 3,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3201,
            name = "Snakeskin Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredDex = 30,
            armorType = ArmorType.Head,
            strength = 3,
            dexterity = 7,
            intelligence = 3,
            critChance = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3202,
            name = "Snakeskin Vest",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredDex = 30,
            armorType = ArmorType.Chest,
            strength = 3,
            dexterity = 7,
            intelligence = 3,
            critChance = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3203,
            name = "Snakeskin Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredDex = 30,
            armorType = ArmorType.Gloves,
            strength = 3,
            dexterity = 7,
            intelligence = 3,
            critChance = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3204,
            name = "Snakeskin Leggings",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredDex = 30,
            armorType = ArmorType.Legs,
            strength = 3,
            dexterity = 7,
            intelligence = 3,
            critChance = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3205,
            name = "Snakeskin Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredDex = 30,
            armorType = ArmorType.Boots,
            strength = 3,
            dexterity = 7,
            intelligence = 3,
            critChance = 0,
            ilvl = 50,
            tier = 3,

 

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3301,
            name = "Spidersilk Hood",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredInt = 30,
            armorType = ArmorType.Head,
            strength = 3,
            dexterity = 3,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3302,
            name = "Spidersilk Robes",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredInt = 30,
            armorType = ArmorType.Chest,
            strength = 3,
            dexterity = 3,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3303,
            name = "Spidersilk Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredInt = 30,
            armorType = ArmorType.Gloves,
            strength = 3,
            dexterity = 3,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3304,
            name = "Spidersilk Pants",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredInt = 30,
            armorType = ArmorType.Legs,
            strength = 3,
            dexterity = 3,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3305,
            name = "Spidersilk Shoes",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 2500,
            sellValue = 650,
            requiredLevel = 5,
            requiredInt = 30,
            armorType = ArmorType.Boots,
            strength = 3,
            dexterity = 3,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 50,
            tier = 3,



        };
        armorList.Add(armor);

        /****************/
        /*** LEYENDOS ***/
        /****************/
        armor = new Armor
        {
            itemId = 3111,
            name = "Paladin Helm",
            spritePath = "Icons/helm",
            rarity = Rarity.Set,
            buyValue = 15000,
            sellValue = 2000,
            requiredLevel = 5,
            requiredStr = 30,
            armorType = ArmorType.Head,
            strength = 8,
            dexterity = 3,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            bonusPhysical = -5,
            bonusMagical = 10,
            ilvl = 75,
            tier = 3,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3112,
            name = "Paladin Plate",
            spritePath = "Icons/chest",
            rarity = Rarity.Set,
            buyValue = 15000,
            sellValue = 2000,
            requiredLevel = 5,
            requiredStr = 30,
            armorType = ArmorType.Chest,
            strength = 8,
            dexterity = 3,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            bonusPhysical = -5,
            bonusMagical = 10,
            ilvl = 75,
            tier = 3,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3114,
            name = "Paladin Greaves",
            spritePath = "Icons/legs",
            rarity = Rarity.Set,
            buyValue = 15000,
            sellValue = 2000,
            requiredLevel = 5,
            requiredStr = 30,
            armorType = ArmorType.Legs,
            strength = 8,
            dexterity = 3,
            intelligence = 3,
            critChance = 0,
            critDamage = 0,
            bonusPhysical = -5,
            bonusMagical = 10,
            ilvl = 75,
            tier = 3,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3315,
            name = "Winged Shoes",
            spritePath = "Icons/boots2",
            flavorText = "Requires 2 AA batteries",
            rarity = Rarity.Legendary,
            buyValue = 15000,
            sellValue = 2000,
            requiredLevel = 5,
            requiredInt = 30,
            armorType = ArmorType.Boots,
            strength = 3,
            dexterity = 3,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 10,
            bonusPhysical = -5,
            ilvl = 75,
            tier = 3,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3214,
            name = "Assassin's Leggings",
            spritePath = "Icons/legs",
            rarity = Rarity.Legendary,
            buyValue = 15000,
            sellValue = 2000,
            requiredLevel = 5,
            requiredDex = 30,
            armorType = ArmorType.Legs,
            strength = 3,
            dexterity = 5,
            intelligence = 3,
            critChance = 1,
            critDamage = 35,
            ilvl = 75,
            tier = 3,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 3215,
            name = "Shadowhide Boots",
            spritePath = "Icons/boots2",
            flavorText = "I walk in the valley of the shadow of death",
            rarity = Rarity.Legendary,
            buyValue = 15000,
            sellValue = 2000,
            requiredLevel = 5,
            requiredDex = 30,
            armorType = ArmorType.Boots,
            strength = 3,
            dexterity = 7,
            intelligence = 3,
            critChance = 5,
            critDamage = 15,
            ilvl = 75,
            tier = 3,

        };
        armorList.Add(armor);
        /********************/
        /***   TIER 4    ***/
        /******************/
        armor = new Armor
        {
            itemId = 4101,
            name = "Mithril Helm",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredStr = 50,
            armorType = ArmorType.Head,
            strength = 10,
            dexterity = 5,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4102,
            name = "Mithril Breastplate",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredStr = 50,
            armorType = ArmorType.Chest,
            strength = 10,
            dexterity = 5,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,
            


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4103,
            name = "Mithril Gauntlets",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredStr = 50,
            armorType = ArmorType.Gloves,
            strength = 10,
            dexterity = 5,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4104,
            name = "Mithril Greaves",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredStr = 50,
            armorType = ArmorType.Legs,
            strength = 10,
            dexterity = 5,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4105,
            name = "Mithril Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredStr = 50,
            armorType = ArmorType.Boots,
            strength = 10,
            dexterity = 5,
            intelligence = 5,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4201,
            name = "Raptorhide Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredDex = 50,
            armorType = ArmorType.Head,
            strength = 5,
            dexterity = 10,
            intelligence = 5,
            critChance = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4202,
            name = "Raptorhide Vest",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredDex = 50,
            armorType = ArmorType.Chest,
            strength = 5,
            dexterity = 10,
            intelligence = 5,
            critChance = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4203,
            name = "Raptorhide Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredDex = 50,
            armorType = ArmorType.Gloves,
            strength = 5,
            dexterity = 10,
            intelligence = 5,
            critChance = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4204,
            name = "Raptorhide Leggings",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredDex = 50,
            armorType = ArmorType.Legs,
            strength = 5,
            dexterity = 10,
            intelligence = 5,
            critChance = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4205,
            name = "Raptorhide Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredDex = 50,
            armorType = ArmorType.Boots,
            strength = 5,
            dexterity = 10,
            intelligence = 5,
            critChance = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4301,
            name = "Mageweave Hood",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredInt = 50,
            armorType = ArmorType.Head,
            strength = 5,
            dexterity = 5,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4302,
            name = "Mageweave Robes",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredInt = 50,
            armorType = ArmorType.Chest,
            strength = 5,
            dexterity = 5,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4303,
            name = "Mageweave Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredInt = 50,
            armorType = ArmorType.Gloves,
            strength = 5,
            dexterity = 5,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4304,
            name = "Mageweave Pants",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredInt = 50,
            armorType = ArmorType.Legs,
            strength = 5,
            dexterity = 5,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 4305,
            name = "Mageweave Shoes",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 7500,
            sellValue = 1500,
            requiredLevel = 10,
            requiredInt = 50,
            armorType = ArmorType.Boots,
            strength = 5,
            dexterity = 5,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 100,
            tier = 4,

        };
        armorList.Add(armor);
        
        /****************/
        /****LEYENDOS****/
        /****************/
        armor = new Armor
        {
            itemId = 4315,
            name = "Nightmage Shoes",
            spritePath = "Icons/boots2",
            rarity = Rarity.Set,
            buyValue = 60000,
            sellValue = 3500,
            requiredLevel = 10,
            requiredInt = 50,
            armorType = ArmorType.Boots,
            strength = 5,
            dexterity = 5,
            intelligence = 10,
            critChance = 5,
            critDamage = 25,
            bonusMagical = 0,
            ilvl = 150,
            tier = 4,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 4312,
            name = "Nightmage Robes",
            spritePath = "Icons/chest",
            rarity = Rarity.Set,
            buyValue = 60000,
            sellValue = 3500,
            requiredLevel = 10,
            requiredInt = 50,
            armorType = ArmorType.Chest,
            strength = 5,
            dexterity = 5,
            intelligence = 10,
            critChance = 5,
            critDamage = 25,
            bonusMagical = 0,
            ilvl = 150,
            tier = 4,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 4311,
            name = "Nightmage Hood",
            spritePath = "Icons/helm",
            rarity = Rarity.Set,
            buyValue = 60000,
            sellValue = 3500,
            requiredLevel = 10,
            requiredInt = 50,
            armorType = ArmorType.Head,
            strength = 5,
            dexterity = 5,
            intelligence = 10,
            critChance = 5,
            critDamage = 25,
            bonusMagical = 0,
            ilvl = 150,
            tier = 4,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 4213,
            name = "Fingerless Nomad Gloves",
            spritePath = "Icons/gloves2",
            flavorText = "You totally look cool",
            rarity = Rarity.Legendary,
            buyValue = 60000,
            sellValue = 3500,
            requiredLevel = 10,
            requiredDex = 50,
            armorType = ArmorType.Gloves,
            strength = 5,
            dexterity = 25,
            intelligence = 5,
            critChance = 0,
            critDamage = 20,
            ilvl = 150,
            tier = 4,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 4114,
            name = "Crotchless Chaps",
            spritePath = "Icons/legs",
            flavorText = "Dick cleavage",
            rarity = Rarity.Legendary,
            buyValue = 60000,
            sellValue = 3500,
            requiredLevel = 10,
            requiredStr = 50,
            armorType = ArmorType.Legs,
            strength = 10,
            dexterity = 5,
            intelligence = 5,
            armorPen = 25,
            bonusPhysical = 0,
            ilvl = 150,
            tier = 4,
        };
        armorList.Add(armor);
        
        /********************/
        /***   TIER 5    ***/
        /******************/
        armor = new Armor
        {
            itemId = 5101,
            name = "Darksteel Helm",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredStr = 75,
            armorType = ArmorType.Head,
            strength = 15,
            dexterity = 7,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5102,
            name = "Darksteel Breastplate",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredStr = 75,
            armorType = ArmorType.Chest,
            strength = 15,
            dexterity = 7,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5103,
            name = "Darksteel Gauntlets",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredStr = 75,
            armorType = ArmorType.Gloves,
            strength = 15,
            dexterity = 7,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5104,
            name = "Darksteel Greaves",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredStr = 75,
            armorType = ArmorType.Legs,
            strength = 15,
            dexterity = 7,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5105,
            name = "Darksteel Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredStr = 75,
            armorType = ArmorType.Boots,
            strength = 15,
            dexterity = 7,
            intelligence = 7,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5201,
            name = "Drakehide Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredDex = 75,
            armorType = ArmorType.Head,
            strength = 7,
            dexterity = 15,
            intelligence = 7,
            critChance = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5202,
            name = "Drakehide Vest",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredDex = 75,
            armorType = ArmorType.Chest,
            strength = 7,
            dexterity = 15,
            intelligence = 7,
            critChance = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5203,
            name = "Drakehide Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredDex = 75,
            armorType = ArmorType.Gloves,
            strength = 7,
            dexterity = 15,
            intelligence = 7,
            critChance = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5204,
            name = "Drakehide Leggings",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredDex = 75,
            armorType = ArmorType.Legs,
            strength = 7,
            dexterity = 15,
            intelligence = 7,
            critChance = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5205,
            name = "Drakehide Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredDex = 75,
            armorType = ArmorType.Boots,
            strength = 7,
            dexterity = 15,
            intelligence = 7,
            critChance = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5301,
            name = "Sagesilk Hood",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredInt = 75,
            armorType = ArmorType.Head,
            strength = 7,
            dexterity = 7,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5302,
            name = "Sagesilk Robes",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredInt = 75,
            armorType = ArmorType.Chest,
            strength = 7,
            dexterity = 7,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5303,
            name = "Sagesilk Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredInt = 75,
            armorType = ArmorType.Gloves,
            strength = 7,
            dexterity = 7,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5304,
            name = "Sagesilk Pants",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
           buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredInt = 75,
            armorType = ArmorType.Legs,
            strength = 7,
            dexterity = 7,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 5305,
            name = "Sagesilk Shoes",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 25000,
            sellValue = 4000,
            requiredLevel = 15,
            requiredInt = 75,
            armorType = ArmorType.Boots,
            strength = 7,
            dexterity = 7,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 200,
            tier = 5,

        };
        armorList.Add(armor);
        
        /****************/
        /****LEYENDOS****/
        /****************/
        armor = new Armor
        {
            itemId = 5112,
            name = "Comfy T-shirt",
            spritePath = "Icons/chest",
            flavorText = "Just came out of the dryer",
            rarity = Rarity.Set,
            buyValue = 150000,
            sellValue = 10000,
            requiredLevel = 20,
            armorType = ArmorType.Chest,
            strength = 20,
            dexterity = 20,
            intelligence = 20,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 3,
            bonusPhysical = 3,
            ilvl = 300,
            tier = 5,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 5214,
            name = "Jeans",
            spritePath = "Icons/legs",
            flavorText = "The tanned leather of the Denimdragon provides excellent protection",
            rarity = Rarity.Set,
            buyValue = 150000,
            sellValue = 10000,
            requiredLevel = 20,
            armorType = ArmorType.Legs,
            strength = 20,
            dexterity = 20,
            intelligence = 20,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 3,
            bonusPhysical = 3,
            ilvl = 300,
            tier = 5,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 5311,
            name = "Baseball Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Set,
            buyValue = 150000,
            sellValue = 10000,
            requiredLevel = 20,
            armorType = ArmorType.Head,
            strength = 20,
            dexterity = 20,
            intelligence = 20,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 3,
            bonusPhysical = 3,
            ilvl = 300,
            tier = 5,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 5213,
            name = "No Gloves",
            spritePath = "Icons/gloves2",
            flavorText = "Wearing gloves is lame tbh",
            rarity = Rarity.Set,
            buyValue = 150000,
            sellValue = 10000,
            requiredLevel = 20,
            armorType = ArmorType.Gloves,
            strength = 20,
            dexterity = 20,
            intelligence = 20,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 3,
            bonusPhysical = 3,
            ilvl = 300,
            tier = 5,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 5115,
            name = "Light-up Skechers",
            spritePath = "Icons/boots2",
            flavorText = "Hell yeah",
            rarity = Rarity.Set,
            buyValue = 150000,
            sellValue = 10000,
            requiredLevel = 20,
            armorType = ArmorType.Boots,
            strength = 20,
            dexterity = 20,
            intelligence = 20,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 3,
            bonusPhysical = 3,
            ilvl = 300,
            tier = 5,
        };
        armorList.Add(armor);
        
        /********************/
        /***   TIER 6    ***/
        /******************/
        armor = new Armor
        {
            itemId = 6101,
            name = "Thorium Helm",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredStr = 100,
            armorType = ArmorType.Head,
            strength = 22,
            dexterity = 10,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6102,
            name = "Thorium Breastplate",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredStr = 100,
            armorType = ArmorType.Chest,
            strength = 22,
            dexterity = 10,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6103,
            name = "Thorium Gauntlets",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredStr = 100,
            armorType = ArmorType.Gloves,
            strength = 22,
            dexterity = 10,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6104,
            name = "Thorium Greaves",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredStr = 100,
            armorType = ArmorType.Legs,
            strength = 22,
            dexterity = 10,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6105,
            name = "Thorium Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredStr = 100,
            armorType = ArmorType.Boots,
            strength = 22,
            dexterity = 10,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6201,
            name = "Giantskin Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredDex = 100,
            armorType = ArmorType.Head,
            strength = 10,
            dexterity = 22,
            intelligence = 10,
            critChance = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6202,
            name = "Giantskin Vest",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredDex = 100,
            armorType = ArmorType.Chest,
            strength = 7,
            dexterity = 22,
            intelligence = 7,
            critChance = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6203,
            name = "Giantskin Gloves",
            spritePath = "Icons/gloves2",
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredDex = 100,
            armorType = ArmorType.Gloves,
            strength = 7,
            dexterity = 22,
            intelligence = 7,
            critChance = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6204,
            name = "Giantskin Leggings",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredDex = 100,
            armorType = ArmorType.Legs,
            strength = 7,
            dexterity = 22,
            intelligence = 7,
            critChance = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6205,
            name = "Giantskin Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredDex = 100,
            armorType = ArmorType.Boots,
            strength = 7,
            dexterity = 22,
            intelligence = 7,
            critChance = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6301,
            name = "Arachnesilk Hood",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredInt = 100,
            armorType = ArmorType.Head,
            strength = 10,
            dexterity = 10,
            intelligence = 22,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6302,
            name = "Arachnesilk Robes",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredInt = 100,
            armorType = ArmorType.Chest,
            strength = 10,
            dexterity = 10,
            intelligence = 22,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6303,
            name = "Arachnesilk Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredInt = 100,
            armorType = ArmorType.Gloves,
            strength = 10,
            dexterity = 10,
            intelligence = 22,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6304,
            name = "Arachnesilk Pants",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredInt = 100,
            armorType = ArmorType.Legs,
            strength = 10,
            dexterity = 10,
            intelligence = 22,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 6305,
            name = "Arachnesilk Shoes",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 80000,
            sellValue = 10000,
            requiredLevel = 20,
            requiredInt = 100,
            armorType = ArmorType.Boots,
            strength = 10,
            dexterity = 10,
            intelligence = 22,
            critChance = 0,
            critDamage = 0,
            ilvl = 400,
            tier = 6,

        };
        armorList.Add(armor);
        
        /****************/
        /****LEYENDOS****/
        /****************/
        armor = new Armor
        {
            itemId = 6112,
            name = "Irradiated Thorium Platemail",
            spritePath = "Icons/chest",
            flavorText = "You are rapidly dying",
            rarity = Rarity.Set,
            buyValue = 500000,
            sellValue = 35000,
            requiredLevel = 20,
            requiredStr = 100,
            armorType = ArmorType.Chest,
            strength = 35,
            dexterity = 10,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 5,
            bonusPhysical = -5,
            magicPen = 15,
            armorPen = 15,
            ilvl = 600,
            tier = 6,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 6113,
            name = "Irradiated Thorium Gloves",
            spritePath = "Icons/gloves2",
            flavorText = "You are rapidly dying",
            rarity = Rarity.Set,
            buyValue = 500000,
            sellValue = 35000,
            requiredLevel = 20,
            requiredStr = 100,
            armorType = ArmorType.Gloves,
            strength = 35,
            dexterity = 10,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 5,
            bonusPhysical = -5,
            magicPen = 15,
            armorPen = 15,
            ilvl = 600,
            tier = 6,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 6111,
            name = "Irradiated Thorium Helm",
            spritePath = "Icons/helm",
            flavorText = "You are rapidly dying",
            rarity = Rarity.Set,
            buyValue = 500000,
            sellValue = 35000,
            requiredLevel = 20,
            requiredStr = 100,
            armorType = ArmorType.Head,
            strength = 35,
            dexterity = 10,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 5,
            bonusPhysical = -5,
            magicPen = 15,
            armorPen = 15,
            ilvl = 600,
            tier = 6,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 6214,
            name = "Tasteful Thong",
            spritePath = "Icons/legs",
            flavorText = "Gender-neutral",
            rarity = Rarity.Legendary,
            buyValue = 500000,
            sellValue = 35000,
            requiredLevel = 20,
            armorType = ArmorType.Legs,
            strength = 10,
            dexterity = 30,
            intelligence = 10,
            critChance = 3,
            critDamage = 10,
            bonusMagical = -10,
            bonusPhysical = 8,
            ilvl = 600,
            tier = 6,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 6215,
            name = "Fuzzy Slippers",
            spritePath = "Icons/boots2",
            rarity = Rarity.Legendary,
            buyValue = 150000,
            sellValue = 10000,
            requiredLevel = 20,
            armorType = ArmorType.Boots,
            strength = 10,
            dexterity = 30,
            intelligence = 10,
            critChance = 0,
            critDamage = 0,
            magicPen = 25,
            armorPen = 25,
            ilvl = 600,
            tier = 6,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 6313,
            name = "Spider-person-silk Gloves",
            spritePath = "Icons/gloves2",
            flavorText = "Banned in 37 states",
            rarity = Rarity.Legendary,
            buyValue = 150000,
            sellValue = 10000,
            requiredLevel = 20,
            armorType = ArmorType.Gloves,
            strength = 10,
            dexterity = 10,
            intelligence = 35,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 5,
            ilvl = 600,
            tier = 6,

        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 6312,
            name = "Robe of the Magi King",
            spritePath = "Icons/chest",
            flavorText = "You feel smart enough to win an internet argument",
            rarity = Rarity.Legendary,
            buyValue = 150000,
            sellValue = 10000,
            requiredLevel = 20,
            armorType = ArmorType.Chest,
            strength = 10,
            dexterity = 10,
            intelligence = 60,
            critChance = 0,
            critDamage = 0,
            bonusMagical = -10,
            bonusPhysical = -10,
            ilvl = 600,
            tier = 6,
        };
        armorList.Add(armor);
        
        /********************/
        /***   TIER 6    ***/
        /******************/
        armor = new Armor
        {
            itemId = 7101,
            name = "Titanium Helm",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredStr = 125,
            armorType = ArmorType.Head,
            strength = 33,
            dexterity = 15,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7102,
            name = "Titanium Breastplate",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredStr = 125,
            armorType = ArmorType.Chest,
            strength = 33,
            dexterity = 15,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7103,
            name = "Titanium Gauntlets",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredStr = 125,
            armorType = ArmorType.Gloves,
            strength = 33,
            dexterity = 15,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7104,
            name = "Titanium Greaves",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredStr = 125,
            armorType = ArmorType.Legs,
            strength = 33,
            dexterity = 15,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,


        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7105,
            name = "Titanium Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredStr = 125,
            armorType = ArmorType.Boots,
            strength = 33,
            dexterity = 15,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7201,
            name = "Sauridhide Cap",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredDex = 125,
            armorType = ArmorType.Head,
            strength = 15,
            dexterity = 33,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7202,
            name = "Sauridhide Vest",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredDex = 125,
            armorType = ArmorType.Chest,
            strength = 15,
            dexterity = 33,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7203,
            name = "Sauridhide Gloves",
            spritePath = "Icons/gloves2",
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredDex = 125,
            armorType = ArmorType.Gloves,
            strength = 15,
            dexterity = 33,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7204,
            name = "Sauridhide Leggings",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredDex = 125,
            armorType = ArmorType.Legs,
            strength = 15,
            dexterity = 33,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7205,
            name = "Sauridhide Boots",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredDex = 125,
            armorType = ArmorType.Boots,
            strength = 15,
            dexterity = 33,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7301,
            name = "Celestial Hood",
            spritePath = "Icons/helm",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredInt = 125,
            armorType = ArmorType.Head,
            strength = 15,
            dexterity = 15,
            intelligence = 33,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7302,
            name = "Celestial Robes",
            spritePath = "Icons/chest",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredInt = 125,
            armorType = ArmorType.Chest,
            strength = 15,
            dexterity = 15,
            intelligence = 33,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7303,
            name = "Celestial Gloves",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredInt = 125,
            armorType = ArmorType.Gloves,
            strength = 15,
            dexterity = 15,
            intelligence = 33,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7304,
            name = "Celestial Pants",
            spritePath = "Icons/legs",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredInt = 125,
            armorType = ArmorType.Legs,
            strength = 15,
            dexterity = 15,
            intelligence = 33,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);

        armor = new Armor
        {
            itemId = 7305,
            name = "Celestial Shoes",
            spritePath = "Icons/boots2",
            rarity = Rarity.Uncommon,
            buyValue = 250000,
            sellValue = 25000,
            requiredLevel = 25,
            requiredInt = 125,
            armorType = ArmorType.Boots,
            strength = 15,
            dexterity = 15,
            intelligence = 33,
            critChance = 0,
            critDamage = 0,
            ilvl = 800,
            tier = 7,

        };
        armorList.Add(armor);
        
        /****************/
        /****LEYENDOS****/
        /****************/
        armor = new Armor
        {
            itemId = 7212,
            name = "9-Tailed Foxhide Jerkin",
            spritePath = "Icons/chest",
            flavorText = "Idk what a jerkin looks like",
            rarity = Rarity.Set,
            buyValue = 1500000,
            sellValue = 100000,
            requiredLevel = 25,
            requiredDex = 125,
            armorType = ArmorType.Chest,
            strength = 15,
            dexterity = 33,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 0,
            bonusPhysical = 0,
            magicPen = 75,
            armorPen = 75,
            ilvl = 1200,
            tier = 7,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 7214,
            name = "9-Tailed Foxhide Jorts",
            spritePath = "Icons/legs",
            flavorText = "I don't even know where I'm going with this",
            rarity = Rarity.Set,
            buyValue = 1500000,
            sellValue = 100000,
            requiredLevel = 25,
            requiredDex = 125,
            armorType = ArmorType.Legs,
            strength = 15,
            dexterity = 33,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 0,
            bonusPhysical = 0,
            magicPen = 75,
            armorPen = 75,
            ilvl = 1200,
            tier = 7,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 7211,
            name = "9-Tailed Foxhide Cowboy Hat",
            spritePath = "Icons/helm",
            rarity = Rarity.Set,
            buyValue = 1500000,
            sellValue = 100000,
            requiredLevel = 25,
            requiredDex = 125,
            armorType = ArmorType.Head,
            strength = 15,
            dexterity = 33,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            bonusMagical = 0,
            bonusPhysical = 0,
            magicPen = 75,
            armorPen = 75,
            ilvl = 1200,
            tier = 7,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 7113,
            name = "Meteoric Titanium Gauntlets",
            spritePath = "Icons/gloves2",
            rarity = Rarity.Legendary,
            buyValue = 500000,
            sellValue = 35000,
            requiredLevel = 20,
            armorType = ArmorType.Gloves,
            strength = 50,
            dexterity = 15,
            intelligence = 15,
            critChance = 10,
            critDamage = 25,
            bonusMagical = -10,
            bonusPhysical = -10,
            ilvl = 1200,
            tier = 7,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 7115,
            name = "Magnetic Magisteel Boots",
            spritePath = "Icons/boots2",
            flavorText = "Forged by the Dwarfpire Draculon Ironaxe so he could hang from the ceiling of his mountain.",
            rarity = Rarity.Legendary,
            buyValue = 500000,
            sellValue = 35000,
            requiredLevel = 20,
            armorType = ArmorType.Boots,
            strength = 50,
            dexterity = 15,
            intelligence = 15,
            critChance = 0,
            critDamage = 0,
            magicPen = 175,
            armorPen = -75,
            ilvl = 1200,
            tier = 7,
        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 7311,
            name = "The Third Eye",
            spritePath = "Icons/helm",
            flavorText = "Sees through magical barriers and fake friends",
            rarity = Rarity.Legendary,
            buyValue = 500000,
            sellValue = 35000,
            requiredLevel = 20,
            armorType = ArmorType.Head,
            strength = 15,
            dexterity = 15,
            intelligence = 50,
            critChance = 5,
            critDamage = 50,
            magicPen = 15,
            armorPen = 0,
            ilvl = 1200,
            tier = 7,

        };
        armorList.Add(armor);
        
        armor = new Armor
        {
            itemId = 7314,
            name = "Khakis of the White King",
            spritePath = "Icons/legs",
            flavorText = "Adventurer-casual",
            rarity = Rarity.Legendary,
            buyValue = 500000,
            sellValue = 35000,
            requiredLevel = 20,
            armorType = ArmorType.Legs,
            strength = 15,
            dexterity = 15,
            intelligence = 50,
            bonusMagical = -25,
            bonusPhysical = 25,
            magicPen = 100,
            armorPen = -100,
            ilvl = 1200,
            tier = 7,
        };
        armorList.Add(armor);
        
        return armorList;
    }

}
