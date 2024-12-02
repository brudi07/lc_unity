using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Battle scene, this code attached to Canvas
public class Battle : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public GameObject attackAnimation;
    private Player player;
    private Enemy enemy;
    private World world;
    private int playerFrame = 0; // Current frame of player animation
    private int enemyFrame = 0; // Current frame of enemy animation
    private float clickTimer = 0.0f; // Click timer that resets every second
    private int clickCount = 0; // Anti cheat click count
    private bool clickLimit = false; // Check for clickCount > 15
    private bool timer = false; // Check for bossTime < 0
    private float bossTime = 15; // Time left to defeat boss
    private List<Damage> dotTimers = new List<Damage>(); // DoT timers list
    private List<Damage> deadTimers = new List<Damage>(); // DoT timers that have ended list

    // Use this for initialization
    void Start()
    {
        // Set GameManger properties
        player = GameManager.gm.player;
        enemy = GameManager.gm.enemy;
        world = GameManager.gm.world;

        // Play music
        SoundManager.sm.PlayMusic(Resources.Load<AudioClip>(world.soundPath));

        // World info
        GameObject.Find("WorldLabel").GetComponent<Text>().text = "World: " + world.worldName;
        GameObject.Find("StageLabel").GetComponent<Text>().text = "Stage: " + world.currentStage.ToString();
        GameObject.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>(world.backgroundPath);

        // Instnatiate Enemy
        int listCount = world.enemyList.Count;
        int rng = UnityEngine.Random.Range(0, listCount);
        int id = world.enemyList[rng].enemyId;
        enemy = enemy.GetEnemyById(enemy.EnemyList(), id).ScaleDifficulty(world);

        // Enemy Info
        GameObject.Find("EnemyNameLabel").GetComponent<Text>().text = enemy.enemyName;
        GameObject.Find("EnemyImage").GetComponent<Image>().sprite = Resources.Load<Sprite>(enemy.spriteList[0]);
        GameObject.Find("EnemyHealthLabel").GetComponent<Text>().text = "HP: " + enemy.health.ToString();
        GameObject.Find("EnemyHealthBar").GetComponent<Slider>().maxValue = enemy.health;
        GameObject.Find("EnemyHealthBar").GetComponent<Slider>().value = enemy.health;
        GameObject.Find("EnemyArmor").GetComponent<Text>().text = "A: " + enemy.armor + " MR: " + enemy.magicResist;
        GameObject.Find("EnemyResists").GetComponent<Text>().text = "W: " + string.Join(",", enemy.weaknesses.ToArray()) +
            Environment.NewLine + "R: " + string.Join(",", enemy.resistances.ToArray());

        // Weapon Buttons
        GameObject.Find("Slot1Button").GetComponentInChildren<Text>().text = player.weapon1.name;
        GameObject.Find("Slot1Button").GetComponentInChildren<Text>().color = player.weapon1.GetRarityColor();
        GameObject.Find("Slot2Button").GetComponentInChildren<Text>().text = player.weapon2.name;
        GameObject.Find("Slot2Button").GetComponentInChildren<Text>().color = player.weapon2.GetRarityColor();
        GameObject.Find("Slot3Button").GetComponentInChildren<Text>().text = player.weapon3.name;
        GameObject.Find("Slot3Button").GetComponentInChildren<Text>().color = player.weapon3.GetRarityColor();
        if (player.equippedWeapon.name == GameObject.Find("Slot1Button").GetComponentInChildren<Text>().text)
            GameObject.Find("Slot1Button").GetComponentInChildren<Outline>().effectColor = Color.white;
        else if (player.equippedWeapon.name == GameObject.Find("Slot2Button").GetComponentInChildren<Text>().text)
            GameObject.Find("Slot2Button").GetComponentInChildren<Outline>().effectColor = Color.white;
        else if (player.equippedWeapon.name == GameObject.Find("Slot3Button").GetComponentInChildren<Text>().text)
            GameObject.Find("Slot3Button").GetComponentInChildren<Outline>().effectColor = Color.white;

        // Create a Repeating Check methods
        InvokeRepeating("DotDamage", 0f, 0.5f);
        InvokeRepeating("AnimateEnemy", 0f, 0.033f);
        InvokeRepeating("AnimatePlayer", 0f, 0.033f);
    }

    // Update is called once per frame
    void Update()
    {
        // Keybinds
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            EquipWeapon1();
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            EquipWeapon2();
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            EquipWeapon3();
        if (Input.GetKeyDown(KeyCode.T))
            GoToTown();
        if (Input.GetKeyDown(KeyCode.L))
            LevelUp();
        if (Input.GetKeyDown(KeyCode.I))
            SpawnItem();
        if (Input.GetKeyDown(KeyCode.S))
            GameManager.gm.Save();

        // Anti-cheat, check if user clicks over 15 times in a second
        AntiCheat();
        // Boss Timer
        BossTimer();

        // Check if enemy dies
        if (enemy.health <= 0)
        {
            // DoT's do not persist across enemies
            dotTimers.Clear();
            EnemyKilled();
        }

        // Check for level up
        if (player.xpTNL <= 0)
            LevelUp();

    }

    // Whenever player clicks the enemy
    private void ClickDamage()
    {
        // Check for anti cheat before doing damage
        if (!clickLimit)
        {
            // Create new damage object
            Damage damage = new Damage();

            // Deal an instance of damage for number of apc's the weapon has
            for (int i = 0; i < player.equippedWeapon.apc; i++)
            {
                // Calculate daamge
                damage.Calculate(player, enemy);

                // Trigger floating text
                ShowDamageText(damage);

                // Apply damage to enemy
                enemy.health -= damage.value;
                GameObject.Find("EnemyHealthLabel").GetComponent<Text>().text = "HP: " + enemy.health.ToString();
                GameObject.Find("EnemyHealthBar").GetComponent<Slider>().value = enemy.health;

                // Instatiate attack animation
                GameObject attack = Instantiate(attackAnimation, Input.mousePosition + new Vector3(0, -100, 0), UnityEngine.Random.rotation, transform);
                attack.transform.SetParent(GameObject.Find("EnemyPanel").transform);
                attack.transform.localScale = new Vector3(1.5f, 1.5f, 0);
            }

            // Trigger sound fx
            SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>(player.equippedWeapon.soundPath), damage.weak, damage.resist, damage.crit);

            // Check for combo
            if (player.comboCount > 5)
                GameObject.Find("ComboLabel").GetComponent<Text>().text = player.comboCount.ToString() + " HITS";
            else
                GameObject.Find("ComboLabel").GetComponent<Text>().text = "";
            // ULTRAAAAAA COMBOOOOOOOO
            if (player.comboCount == 15)
                SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/ultracombosfx"));

            // DoT check
            if (player.equippedWeapon.isDot)
            {
                damage.value = damage.value / 2;
                damage.weak = false;
                damage.resist = false;
                damage.crit = false;
                damage.dot = true;
                damage.dotTimer = 3.0f;
                dotTimers.Add(damage);
            }
            // Sunder check
            int sunderValue = 1;
            if (player.equippedWeapon.isSundering)
            {
                if (enemy.armor - sunderValue >= 0)
                    enemy.armor -= sunderValue;
                else
                    enemy.armor = 0;
            }
            // Negate check
            int negateValue = 1;
            if (player.equippedWeapon.isNegating)
            {
                if (enemy.magicResist - negateValue > 0)
                    enemy.magicResist -= negateValue;
                else
                    enemy.magicResist = 0;
            }
        }
    }

    private void DotDamage()
    {
        // DoT Damage
        if (dotTimers.Count > 0)
        {
            foreach (Damage dot in dotTimers)
            {
                Debug.Log("Dot damage");
                // Trigger floating text
                ShowDamageText(dot);

                // Apply damage to enemy
                enemy.health -= dot.value;
                GameObject.Find("EnemyHealthLabel").GetComponent<Text>().text = "HP: " + enemy.health.ToString();
                GameObject.Find("EnemyHealthBar").GetComponent<Slider>().value = enemy.health;

                dot.dotTimer--;
                if (dot.dotTimer < 0)
                    deadTimers.Add(dot);
            }
        }

        // Remove timers that have expired
        if (deadTimers.Count > 0)
        {
            foreach (Damage deadDot in deadTimers)
                dotTimers.Remove(deadDot);
            deadTimers.Clear();
        }
    }

    // Show floating numbers on screen
    private void ShowDamageText(Damage damage)
    {
        // Create location for floating text
        Vector3 location = GameObject.Find("EnemyPanel").transform.position;
        //Vector3 offset = new Vector3(UnityEngine.Random.Range(-100, 100), 100, 0);
        Vector3 offset = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(75, 110), 0);
        if (damage.dot)
            offset = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(100, 120), 0);
        // Instantiate floatingText
        GameObject floatingText = Instantiate(floatingTextPrefab, location + offset, Quaternion.identity, transform);
        floatingText.transform.SetParent(GameObject.Find("EnemyPanel").transform);

        // Change text if resistance
        if (damage.resist)
        {
            floatingText.GetComponentInChildren<Outline>().effectColor = Color.cyan;
            floatingText.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
            floatingText.GetComponentInChildren<Text>().fontSize = 40;
        }
        // Change text if weakness
        if (damage.weak)
        {
            floatingText.GetComponentInChildren<Outline>().effectColor = Color.magenta;
            floatingText.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
            floatingText.GetComponentInChildren<Text>().fontSize = 40;
        }
        // Change text if critical strike
        if (damage.crit)
        {
            floatingText.GetComponentInChildren<Outline>().effectColor = Color.yellow;
            floatingText.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
            floatingText.GetComponentInChildren<Text>().fontSize = 40;
        }
        // Change text if DoT
        if (damage.dot)
        {
            floatingText.GetComponentInChildren<Outline>().effectColor = Color.gray;
            floatingText.GetComponentInChildren<Text>().fontStyle = FontStyle.Italic;
            floatingText.GetComponentInChildren<Text>().fontSize = 28;
        }

        // Set text of floating text to damage
        floatingText.GetComponentInChildren<Text>().text = damage.value.ToString();
    }

    // Information to update when an enemy is killed
    private void EnemyKilled()
    {
        // Update player with killed enemy values
        player.xpTNL -= Convert.ToInt32(enemy.xpToGive * (player.bonusExp / 100));
        GameObject.Find("TNL").GetComponentInChildren<Text>().text = "XP TNL: " + player.xpTNL.ToString();
        player.gold += Convert.ToInt32(enemy.goldToGive * (player.bonusGold / 100));
        GameObject.Find("Gold").GetComponentInChildren<Text>().text = "Gold: " + player.gold.ToString();
        // Increment the stage
        GameManager.gm.world.currentStage++;
        GameObject.Find("StageLabel").GetComponentInChildren<Text>().text = "Stage: " + GameManager.gm.world.currentStage.ToString();

        // Check player drop chance for item spawn
        if (UnityEngine.Random.Range(0, 100) < player.itemFind)
            SpawnItem();

        // Spawn new enemy
        if (world.currentStage == 50)
            enemy = world.miniBoss.ScaleDifficulty(world);
        else if (world.currentStage == 100)
        {
            enemy = world.boss.ScaleDifficulty(world);
            timer = true;
        }
        else
        {
            int listCount = world.enemyList.Count;
            int rng = UnityEngine.Random.Range(0, listCount);
            int id = world.enemyList[rng].enemyId;
            enemy = enemy.GetEnemyById(enemy.EnemyList(), id).ScaleDifficulty(world);
        }

        // If boss is beat go to town
        if (world.currentStage == 101)
        {
            Transform[] transform = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
            foreach (Transform t in transform)
            {
                if (t.name == "CompletePanel")
                {
                    // Stop the timer
                    timer = false;

                    // Update next world gate for player
                    world.UpdateWorldGate(player, world);

                    // Stop music and play the victory sound!
                    SoundManager.sm.StopMusic();
                    SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/finalfantasy"));
                    // Set overlay to active
                    t.gameObject.SetActive(true);
                }
            }
        }

        // Update Enemy Info
        enemyFrame = 0;
        GameObject.Find("EnemyNameLabel").GetComponentInChildren<Text>().text = enemy.enemyName;
        GameObject.Find("EnemyImage").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(enemy.spriteList[0]);
        GameObject.Find("EnemyHealthLabel").GetComponentInChildren<Text>().text = "HP: " + enemy.health.ToString();
        GameObject.Find("EnemyHealthBar").GetComponent<Slider>().maxValue = enemy.health;
        GameObject.Find("EnemyHealthBar").GetComponent<Slider>().value = enemy.health;
        GameObject.Find("EnemyArmor").GetComponent<Text>().text = "A: " + enemy.armor + " MR: " + enemy.magicResist;
        GameObject.Find("EnemyResists").GetComponent<Text>().text = "W: " + string.Join(",", enemy.weaknesses.ToArray()) + Environment.NewLine + "R: " + string.Join(",", enemy.resistances.ToArray());
    }

    // Level Up!
    public void LevelUp()
    {
        // Player level up
        player.LevelUp();
        // Play sound
        SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/levelupsfx"));
        // Update the gui
        GameObject.Find("Level").GetComponentInChildren<Text>().text = "Level: " + player.level.ToString();
        GameObject.Find("TNL").GetComponentInChildren<Text>().text = "XP TNL: " + player.xpTNL.ToString();
        GameObject.Find("TalentButton").GetComponentInChildren<Text>().text = "TALENTS: " + player.talentPoints;
    }

    // Spawn an item and rng check to add magical properties
    public void SpawnItem()
    {
        //Debug.Log("Spawn item");
        Item item = Item.SpawnItem(player, world);

        // Add item to inventory
        if (player.inventory.Count < 15)
        {
            player.inventory.Add(item);
            SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/pickupitemsfx"));
        }
        else
        {
            SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/dropsfx"));
        }

        // Update Inventory (Set item border color to match rarity)
        for (int i = 0; i < player.inventory.Count; i++)
        {
            GameObject.Find("Item" + i).GetComponent<ItemPanel>().item = player.inventory[i];
            GameObject.Find("Item" + i).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(player.inventory[i].spritePath);
            GameObject.Find("ItemPanel" + i).GetComponentInChildren<Outline>().effectColor = player.inventory[i].GetRarityColor();
        }
    }

    // Create a timer that starts when the boss spawns
    // If the player is unable to kill the boss in time they fail the stage
    private void BossTimer()
    {
        if (timer)
        {
            bossTime -= Time.deltaTime;
            if (bossTime < 0)
            {
                timer = false;
                // Have to traverse through Canvas to get disabled UI elements
                Transform[] transform = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
                foreach (Transform t in transform)
                {
                    if (t.name == "CompletePanel")
                    {
                        SoundManager.sm.StopMusic();
                        SoundManager.sm.PlaySoundFX(Resources.Load<AudioClip>("Sfx/failsfx"));
                        t.gameObject.SetActive(true);
                        GameObject.Find("CompleteText").GetComponent<Text>().text = "YOU FAILED";
                    }
                }
            }
            float seconds = bossTime % 60;//Use the euclidean division for the seconds.
            float fraction = (bossTime * 100) % 100;
            // Update the label value
            GameObject.Find("BossTimer").GetComponent<Text>().text = string.Format("{0:00} : {1:00}", seconds, fraction);
        }
    }

    // Anti-cheat, check if user clicks over 15 times in a second
    private void AntiCheat()
    {
        const int maxClicks = 15;
        // Checks in 1 second intervals
        if (clickTimer < 1.0f)
        {
            // Check if user clicks, add it to click count
            if (Input.GetMouseButtonDown(0))
            {
                clickCount++;
                //Debug.Log("clickCount: " + clickCount);
            }
            // Check if user has gone over max click amount
            if (clickCount > maxClicks)
            {
                clickLimit = true;
                player.name = "Cheater";
                GameObject.Find("Name").GetComponentInChildren<Text>().text = "Name: " + player.name;
            }
        }
        // Reset anti-cheat variables after 1 second
        else
        {
            clickTimer = 0.0f;
            clickCount = 0;
            clickLimit = false;
        }
        // Update click timer time
        clickTimer += Time.deltaTime;
    }

    private void AnimatePlayer()
    {
        List<string> playerList = new List<string>(){ "Player/charmander/charmander (1)", "Player/charmander/charmander (2)", "Player/charmander/charmander (3)", "Player/charmander/charmander (4)", "Player/charmander/charmander (5)", "Player/charmander/charmander (6)", "Player/charmander/charmander (7)", "Player/charmander/charmander (8)", "Player/charmander/charmander (9)", "Player/charmander/charmander (10)", "Player/charmander/charmander (11)", "Player/charmander/charmander (12)", "Player/charmander/charmander (13)", "Player/charmander/charmander (14)", "Player/charmander/charmander (15)", "Player/charmander/charmander (16)", "Player/charmander/charmander (17)", "Player/charmander/charmander (18)", "Player/charmander/charmander (19)", "Player/charmander/charmander (20)", "Player/charmander/charmander (21)", "Player/charmander/charmander (22)", "Player/charmander/charmander (23)", "Player/charmander/charmander (24)", "Player/charmander/charmander (25)", "Player/charmander/charmander (26)", "Player/charmander/charmander (27)", "Player/charmander/charmander (28)", "Player/charmander/charmander (29)", "Player/charmander/charmander (30)", "Player/charmander/charmander (31)", "Player/charmander/charmander (32)", "Player/charmander/charmander (33)", "Player/charmander/charmander (34)", "Player/charmander/charmander (35)", "Player/charmander/charmander (36)", "Player/charmander/charmander (37)", "Player/charmander/charmander (38)" };
        GameObject.Find("PlayerImage").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(playerList[playerFrame]);
        playerFrame++;
        if (playerFrame >= playerList.Count)
            playerFrame = 0;
    }

    private void AnimateEnemy()
    {
        GameObject.Find("EnemyImage").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(enemy.spriteList[enemyFrame]);
        enemyFrame++;
        if (enemyFrame >= enemy.spriteList.Count)
            enemyFrame = 0;
    }

    // Equip player weapon1
    private void EquipWeapon1()
    {
        player.SetEquippedWeapon(player.weapon1);
        // Update outline
        GameObject.Find("Slot1Button").GetComponentInChildren<Outline>().effectColor = Color.white;
        GameObject.Find("Slot2Button").GetComponentInChildren<Outline>().effectColor = Color.black;
        GameObject.Find("Slot3Button").GetComponentInChildren<Outline>().effectColor = Color.black;
        CharacterPanel.UpdateSheet();
        Debug.Log(player.equippedWeapon.name + " equipped!");
    }

    // Equip player weapon2
    private void EquipWeapon2()
    {
        player.SetEquippedWeapon(player.weapon2);
        // Update outline
        GameObject.Find("Slot1Button").GetComponentInChildren<Outline>().effectColor = Color.black;
        GameObject.Find("Slot2Button").GetComponentInChildren<Outline>().effectColor = Color.white;
        GameObject.Find("Slot3Button").GetComponentInChildren<Outline>().effectColor = Color.black;
        CharacterPanel.UpdateSheet();
        Debug.Log(player.equippedWeapon.name + " equipped!");
    }

    // Equip player weapon3
    private void EquipWeapon3()
    {
        player.SetEquippedWeapon(player.weapon3);
        // Update outline
        GameObject.Find("Slot1Button").GetComponentInChildren<Outline>().effectColor = Color.black;
        GameObject.Find("Slot2Button").GetComponentInChildren<Outline>().effectColor = Color.black;
        GameObject.Find("Slot3Button").GetComponentInChildren<Outline>().effectColor = Color.white;
        CharacterPanel.UpdateSheet();
        Debug.Log(player.equippedWeapon.name + " equipped!");
    }

    // Load Town scene
    public void GoToTown()
    {
        SoundManager.sm.StopMusic();
        GameManager.gm.LoadScene("Town");
    }

    // Summon Boss
    public void SummonBoss()
    {
        Debug.Log("Summon Boss");
        world.currentStage = 99;
        EnemyKilled();
    }

}
