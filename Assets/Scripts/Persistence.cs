using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Persistence : MonoBehaviour {
	public GameManager gm;
    [HideInInspector]
	public Dictionary<string, int> itemsCount;
    [HideInInspector]
    public Dictionary<string, Items> itemsList;
    [HideInInspector]
    public List<Enemies> enemyList;
    [HideInInspector]
    public Dictionary<int, string> mapList;
    [HideInInspector]
    public static Persistence persistence;

    private VRInput vrInput;

	void Awake() {
        if (!persistence)
        {
            persistence = this;
            DontDestroyOnLoad(gameObject);
            CreateItemsList();
            CreateEnemyList();
            CreateMapList();
        }
        else if (persistence && !gm)
        {
            gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        else {
			Destroy (gameObject);
		}
	}
		
	#region Public
	public Items FindItem(string itemName)
	{
        Items itemFound = itemsList[itemName];
		if (itemFound != null) {
			return itemFound;
		}
		return null;
	}

    public int? GetItemCount(string itemName)
    {
        int? count = itemsCount[itemName];
        if (count != null)
        {
            return count;
        }
        return null;
    }

    public Enemies FindEnemy(string enName)
	{
        foreach (Enemies enemy in enemyList)
        {
            if (enName.Contains(enemy.enemyName))
                return enemy;
        }
		return null;
	}

	#endregion

	#region Private


	void CreateItemsList() {
        itemsList = new Dictionary<string, Items>();
        itemsCount = new Dictionary<string, int>();

        Items hands = CreateItem ("hands", 0, Items.handabilityClass.pocket, Items.iType.gun, false, false);
		hands = CreateGun (hands, 5f, 16f, 50f, 1f, 0, 0, Items.gType.hands);
		itemsList.Add ("hands", hands);
        itemsCount.Add("hands", 0);
        Items bat = CreateItem("bat", 0, Items.handabilityClass.onehand, Items.iType.gun, true, false);
        bat = CreateGun(bat, 10f, 16f, 100f, 2f, 0, 0, Items.gType.bat);
        itemsList.Add("bat", bat);
        itemsCount.Add("bat", 0);
        Items handgun = CreateItem ("handgun", 1, Items.handabilityClass.onehand, Items.iType.gun, true, false);
		handgun = CreateGun (handgun, 33.4f, 25f, 150f, 15f, 13, 13, Items.gType.handgun);
		itemsList.Add ("handgun", handgun);
        itemsCount.Add("handgun", 0);
        Items shotgun = CreateItem ("shotgun", 3, Items.handabilityClass.twohand, Items.iType.gun, true, false);
		shotgun = CreateGun (shotgun, 150f, 6f, 400f, 10f, 6, 6, Items.gType.shotgun);
		itemsList.Add ("shotgun", shotgun);
        itemsCount.Add("shotgun", 0);
        Items m4 = CreateItem ("m4", 3, Items.handabilityClass.twohand, Items.iType.gun, true, false);
		m4 = CreateGun (m4, 25f, 45f, 250f, 35f, 30, 30, Items.gType.m4);
		itemsList.Add ("m4" , m4);
        itemsCount.Add("m4", 0);
        Items sniper = CreateItem ("sniper", 1, Items.handabilityClass.body, Items.iType.gun, true, false);
		sniper = CreateGun (sniper, 200f, 5f, 500f, 150f, 4, 4, Items.gType.sniper);
		itemsList.Add ("sniper", sniper);
        itemsCount.Add("sniper", 0);

        Items handgunAmmo = CreateItem("handgunAmmo", 1, Items.handabilityClass.backpack, Items.iType.ammo, true, true);
        handgunAmmo.ammoType = Items.amType.handgunAmmo;
        itemsList.Add("handgunAmmo", handgunAmmo);
        itemsCount.Add("handgunAmmo", 0);
        Items shotgunAmmo = CreateItem("shotgunAmmo", 1, Items.handabilityClass.backpack, Items.iType.ammo, true, true);
        shotgunAmmo.ammoType = Items.amType.shotgunAmmo;
        itemsList.Add("shotgunAmmo", shotgunAmmo);
        itemsCount.Add("shotgunAmmo", 0);
        Items m4Ammo = CreateItem("m4Ammo", 1, Items.handabilityClass.backpack, Items.iType.ammo, true, true);
        m4Ammo.ammoType = Items.amType.m4Ammo;
        itemsList.Add("m4Ammo", m4Ammo);
        itemsCount.Add("m4Ammo", 0);
        Items sniperAmmo = CreateItem("sniperAmmo", 1, Items.handabilityClass.backpack, Items.iType.ammo, true, true);
        sniperAmmo.ammoType = Items.amType.sniperAmmo;
        itemsList.Add("sniperAmmo", sniperAmmo);
        itemsCount.Add("sniperAmmo", 0);

        Items bandage = CreateItem("bandage", 0, Items.handabilityClass.pocket, Items.iType.health, false, false);
        bandage.healthType= Items.hType.bandage;
        bandage.SetHealing(25);
        itemsList.Add("bandage", bandage);
        itemsCount.Add("bandage", 0);
        Items stimpack = CreateItem("stimpack", 0, Items.handabilityClass.pocket, Items.iType.health, false, false);
        stimpack.healthType = Items.hType.stimpack;
        stimpack.SetHealing(50);
        itemsList.Add("stimpack", stimpack);
        itemsCount.Add("stimpack", 0);
        Items medkit = CreateItem("medkit", 0, Items.handabilityClass.pocket, Items.iType.health, false, false);
        medkit.healthType = Items.hType.medkit;
        medkit.SetHealing(100);
        itemsList.Add("medkit", medkit);
        itemsCount.Add("medkit", 0);

        Items helmet= CreateItem("helmet", 0, Items.handabilityClass.onehand, Items.iType.armor, false, false);
        helmet.armorType = Items.arType.helmet;
        helmet.SetArmor(25);
        itemsList.Add("helmet", helmet);
        itemsCount.Add("helmet", 0);
        Items jacket= CreateItem("jacket", 0, Items.handabilityClass.onehand, Items.iType.armor, false, false);
        jacket.armorType = Items.arType.jacket;
        jacket.SetArmor(50);
        itemsList.Add("jacket", jacket);
        itemsCount.Add("jacket", 0);
        Items full = CreateItem("full", 0, Items.handabilityClass.twohand, Items.iType.armor, false, false);
        full.armorType = Items.arType.full;
        full.SetArmor(100);
        itemsList.Add("full", full);
        itemsCount.Add("full", 0);
    }

	Items CreateItem(string name, int weight, Items.handabilityClass handability, Items.iType type, 
		bool isDroppable, bool isStackable) {
        Items temp = ScriptableObject.CreateInstance("Items") as Items;
        temp.init();

        temp.itemName = name;
		temp.weight = weight;
		temp.handability = handability;
		temp.itemType = type;
		temp.isDroppable = isDroppable;
		temp.isStackable = isStackable;
		return temp;
	}

	Items CreateGun(Items item, float damage, float fireRate, float impactForce, 
		float range, int clipSize, int bullets, Items.gType gunType) {
        Items temp = ScriptableObject.CreateInstance("Items") as Items;
        temp.init();
        temp.itemName = item.itemName;
		temp.weight = item.weight;
		temp.handability = item.handability;
		temp.itemType = item.itemType;
		temp.isDroppable = item.isDroppable;
		temp.isStackable = item.isStackable;

		temp.SetDamage(damage);
		temp.SetFireRate(fireRate);
		temp.SetImpactForce(impactForce);
		temp.SetRange(range);
		temp.SetClipSize(clipSize);
		temp.SetBullets(bullets);
		temp.gunType = gunType;

		return temp;
	}
    
	void CreateEnemyList()
	{
		enemyList = new List<Enemies> ();

		Enemies cactus = CreateEnemy ("Snuggles", Enemies.eType.monster, 150, 2.5f, 20, Enemies.atkType.melee, 2f, 10, 17f); //8.5 a sec
		enemyList.Add (cactus);
		Enemies whale = CreateEnemy ("Hunter", Enemies.eType.monster, 50, 21f, 7, Enemies.atkType.melee, 3f, 15, 40f); //7 a sec
		enemyList.Add (whale);
		Enemies alien = CreateEnemy ("Io", Enemies.eType.monster, 100, 7.5f, 12, Enemies.atkType.range, 10f, 25, 24f); //7 a sec
		enemyList.Add (alien);
		Enemies trog = CreateEnemy ("Trog", Enemies.eType.monster, 250, 5f, 40, Enemies.atkType.melee, 4f, 40, 10f); //10 a sec
		enemyList.Add (trog);

        Enemies caltrop = CreateEnemy("Caltrop", Enemies.eType.trap, 200, 0f, 5, Enemies.atkType.melee, .5f, 10, 5f);//5 once
        enemyList.Add(caltrop);
        Enemies spike = CreateEnemy("SpikeTrap", Enemies.eType.trap, 100, 0f, 10, Enemies.atkType.melee, 1f, 20, 20f); //5 a sec
        enemyList.Add(spike);
        Enemies smoke = CreateEnemy("GasTrap", Enemies.eType.trap, 100, 0f, 5, Enemies.atkType.melee, 5f, 40, 4f); //every 10 sec
        enemyList.Add(smoke);
        Enemies mine = CreateEnemy("MineTrap", Enemies.eType.trap, 50, 30f, 100, Enemies.atkType.melee, .5f, 80, 100f);//100 within half a sec
        enemyList.Add(mine);
    }

	Enemies CreateEnemy(string name, Enemies.eType enemyType, int health, float speed, int damage, 
		Enemies.atkType attack, float range, int cost, float atkSpd)
	{
        Enemies temp = ScriptableObject.CreateInstance("Enemies") as Enemies;
        temp.init();

        temp.enemyName = name;
		temp.enemyType= enemyType;
		temp.attackType = attack;

		temp.SetHealth(health);
		temp.SetDamage(damage);
		temp.SetSpeed(speed);
		temp.SetRange(range);
		temp.SetCost (cost);
        temp.SetAttackSpeed(atkSpd);
        return temp;
	}

    void CreateMapList()
    {
        mapList = new Dictionary<int, string>();
        mapList.Add(1, "town");
        mapList.Add(2, "farm");
        mapList.Add(3, "island");
        mapList.Add(4, "spaceship");
        mapList.Add(5, "lab");
        mapList.Add(6, "city");
        mapList.Add(7, "mine");
        mapList.Add(8, "beach");
    }
        #endregion
    }
