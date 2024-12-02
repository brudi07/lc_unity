using System;
using System.Collections.Generic;
using UnityEngine;

public class World {

    public string worldName = "";
    public double enemyScaling = 1;
    public int currentStage = 1;
    public string backgroundPath;
    public string soundPath;
    public List<Enemy> enemyList;
    public Enemy miniBoss;
    public Enemy boss;
    public List<Weapon> weaponList;
    public List<Armor> armorList;
    public List<Modifier> prefixList;
    public List<Modifier> suffixList;

    // Get World by name from list
    public World GetWorldByName(List<World> list, string name)
    {
        World w = new World();
        if ((list.Find(world => world.worldName == name) != null))
        {
            w = list.Find(world => world.worldName == name);
        }
        return w;
    }

    public void UpdateWorldGate(Player player, World world)
    {
        if (world.worldName == "Plains")
            player.world2 = true;
        if (world.worldName == "Forest")
            player.world3 = true;
        if (world.worldName == "Cave")
            player.world4 = true;
        if (world.worldName == "River")
            player.world5 = true;
        if (world.worldName == "Swamp")
            player.world6 = true;
        if (world.worldName == "Desert")
            player.world7 = true;
    }

    // List of all worlds
    public List<World> WorldList()
    {
        List<World> worldList = new List<World>();
        // Instantiate
        Enemy enemy = new Enemy();
        Weapon weapon = new Weapon();
        Armor armor = new Armor();
        Modifier mod = new Modifier();
        // Shortcut lists
        List<Enemy> el = enemy.EnemyList();
        List<Weapon> wl = weapon.WeaponList();
        List<Armor> al = armor.ArmorList();
        List<Modifier> pl = mod.PrefixList();
        List<Modifier> sl = mod.SuffixList();

        World world;

        // Create Plains
        world = new World
        {
            worldName = "Plains",
            enemyScaling = 0.8,
            backgroundPath = "Background/plains",
            soundPath = "Music/plains",
            enemyList = new List<Enemy> { enemy.GetEnemyById(el, 152), enemy.GetEnemyById(el, 153), enemy.GetEnemyById(el, 154), enemy.GetEnemyById(el, 155), enemy.GetEnemyById(el, 156),
            enemy.GetEnemyById(el, 157), enemy.GetEnemyById(el, 158), enemy.GetEnemyById(el, 159), enemy.GetEnemyById(el, 160), enemy.GetEnemyById(el, 161)},
            miniBoss = enemy.GetEnemyById(el, 162),
            boss = enemy.GetEnemyById(el, 163),
            weaponList = weapon.GetListByTier(wl, 01),
            armorList = armor.GetListByTier(al, 1),
            prefixList = mod.GetListByTier(pl, 1),
            suffixList = mod.GetListByTier(sl, 1)
        };
        worldList.Add(world);

        // Create Forest
        world = new World
        {
            worldName = "Forest",
            enemyScaling = 2.1,
            backgroundPath = "Background/forest1",
            soundPath = "Music/forestmusic",
            enemyList = new List<Enemy> { enemy.GetEnemyById(el, 164), enemy.GetEnemyById(el, 165), enemy.GetEnemyById(el, 166), enemy.GetEnemyById(el, 167),
            enemy.GetEnemyById(el, 168), enemy.GetEnemyById(el, 169), enemy.GetEnemyById(el, 170), enemy.GetEnemyById(el, 171), enemy.GetEnemyById(el, 172), enemy.GetEnemyById(el, 173)},
            miniBoss = enemy.GetEnemyById(el, 174),
            boss = enemy.GetEnemyById(el, 175),
            weaponList = weapon.GetListByTier(wl, 02),
            armorList = armor.GetListByTier(al, 2),
            prefixList = mod.GetListByTier(pl, 2),
            suffixList = mod.GetListByTier(sl, 2)

        };
        worldList.Add(world);

        // Create Cave
        world = new World
        {
            worldName = "Cave",
            enemyScaling = 3.3,
            backgroundPath = "Background/cave1",
            soundPath = "Music/cavemusic",
            enemyList = new List<Enemy> { enemy.GetEnemyById(el, 176), enemy.GetEnemyById(el, 177), enemy.GetEnemyById(el, 178), enemy.GetEnemyById(el, 179),
            enemy.GetEnemyById(el, 180), enemy.GetEnemyById(el, 181), enemy.GetEnemyById(el, 182), enemy.GetEnemyById(el, 183), enemy.GetEnemyById(el, 184),
            enemy.GetEnemyById(el, 184), enemy.GetEnemyById(el, 185)},
            miniBoss = enemy.GetEnemyById(el, 186),
            boss = enemy.GetEnemyById(el, 187),
            weaponList = weapon.GetListByTier(wl, 03),
            armorList = armor.GetListByTier(al, 3),
            prefixList = mod.GetListByTier(pl, 3),
            suffixList = mod.GetListByTier(sl, 3)
        };
        worldList.Add(world);

        // Create River
        world = new World
        {
            worldName = "River",
            enemyScaling = 4.7,
            backgroundPath = "Background/river",
            soundPath = "Music/rivermusic",
            enemyList = new List<Enemy> { enemy.GetEnemyById(el, 188), enemy.GetEnemyById(el, 189), enemy.GetEnemyById(el, 190), enemy.GetEnemyById(el, 191),
            enemy.GetEnemyById(el, 192), enemy.GetEnemyById(el, 193), enemy.GetEnemyById(el, 194), enemy.GetEnemyById(el, 195),
            enemy.GetEnemyById(el, 196), enemy.GetEnemyById(el, 197)},
            miniBoss = enemy.GetEnemyById(el, 198),
            boss = enemy.GetEnemyById(el, 199),
            weaponList = weapon.GetListByTier(wl, 04),
            armorList = armor.GetListByTier(al, 4),
            prefixList = mod.GetListByTier(pl, 4),
        suffixList = mod.GetListByTier(sl, 4)
        };
        worldList.Add(world);

        // Create Swamp
        world = new World
        {
            worldName = "Swamp",
            enemyScaling = 6.2,
            backgroundPath = "Background/forest2",
            soundPath = "Music/swampmusic",
            enemyList = new List<Enemy> { enemy.GetEnemyById(el, 200), enemy.GetEnemyById(el, 201), enemy.GetEnemyById(el, 202), enemy.GetEnemyById(el, 203),
            enemy.GetEnemyById(el, 204), enemy.GetEnemyById(el, 205), enemy.GetEnemyById(el, 206),
            enemy.GetEnemyById(el, 207), enemy.GetEnemyById(el, 208), enemy.GetEnemyById(el, 209) },
            miniBoss = enemy.GetEnemyById(el, 210),
            boss = enemy.GetEnemyById(el, 211),
            weaponList = weapon.GetListByTier(wl, 05),
            armorList = armor.GetListByTier(al, 5),
            prefixList = mod.GetListByTier(pl, 5),
            suffixList = mod.GetListByTier(sl, 5)
        };
        worldList.Add(world);

        // Create Desert
        world = new World
        {
            worldName = "Desert",
            enemyScaling = 7.8,
            backgroundPath = "Background/desert",
            soundPath = "Music/desertmusic",
            enemyList = new List<Enemy> { enemy.GetEnemyById(el, 212), enemy.GetEnemyById(el, 213), enemy.GetEnemyById(el, 214), enemy.GetEnemyById(el, 215),
            enemy.GetEnemyById(el, 216), enemy.GetEnemyById(el, 217), enemy.GetEnemyById(el, 218),
            enemy.GetEnemyById(el, 219), enemy.GetEnemyById(el, 220), enemy.GetEnemyById(el, 221) },
            miniBoss = enemy.GetEnemyById(el, 222),
            boss = enemy.GetEnemyById(el, 223),
            weaponList = weapon.GetListByTier(wl, 06),
            armorList = armor.GetListByTier(al, 6),
            prefixList = mod.GetListByTier(pl, 6),
            suffixList = mod.GetListByTier(sl, 6)
        };
        worldList.Add(world);

        return worldList;
    }

}


