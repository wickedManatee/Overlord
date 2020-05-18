using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour {
    public GunSwitch weaponHolster;
    public PlayerCamera playerCam;
    public GameObject crosshair;

    int maxHealthArmor = 100;
	int health;
	int armor;
    float origSpeed;
    float timeToSlowPlayer = 0f;

    public InventoryController inv;
    bool toggleInventory;
    bool discardSlot1;
    bool discardSlot2;
    bool discardSlot3;
    bool discardSlot4;
    bool discardSlot5;
    bool discardSlot6;
    bool discardSlot7;
    bool discardSlot8;
    bool discardSlot9;
    bool isPanic;
    bool isReload;
    bool isAim;

    Rigidbody rbody;

    private void Awake()
    {
        if (weaponHolster == null || playerCam == null || crosshair == null)
            Debug.LogError("Gameobject not fully formed - " + this.name);
        rbody = GetComponent<Rigidbody>();

        origSpeed = GetComponent<ThirdPersonCharacter>().GetMovementSpeedMultiplier();
    }
	
	// Update is called once per frame
	void Update () {
        if (timeToSlowPlayer != 0f && Time.time >= timeToSlowPlayer)
            SlowPlayerEnd();

        toggleInventory = Input.GetButtonDown("Inventory");
        if (toggleInventory)
            inv.ToggleInv();

        isReload = Input.GetButtonDown("Reload");
        if (isReload)
            Reload();

        isAim = Input.GetButton("Aim");
        if (isAim)
            Aimming(true);
        else
            Aimming(false);

        discardSlot1 = Input.GetButtonDown("Discard1");
        if (discardSlot1)
            inv.DropItem(1);
        discardSlot2 = Input.GetButtonDown("Discard2");
        if (discardSlot2)
            inv.DropItem(2);
        discardSlot3 = Input.GetButtonDown("Discard3");
        if (discardSlot3)
            inv.DropItem(3);
        discardSlot4 = Input.GetButtonDown("Discard4");
        if (discardSlot4)
            inv.DropItem(4);
        discardSlot5 = Input.GetButtonDown("Discard5");
        if (discardSlot5)
            inv.DropItem(5);
        discardSlot6 = Input.GetButtonDown("Discard6");
        if (discardSlot6)
            inv.DropItem(6);
        discardSlot7 = Input.GetButtonDown("Discard7");
        if (discardSlot7)
            inv.DropItem(7);
        discardSlot8 = Input.GetButtonDown("Discard8");
        if (discardSlot8)
            inv.DropItem(8);
        discardSlot9 = Input.GetButtonDown("Discard9");
        if (discardSlot9)
            inv.DropItem(9);
        isPanic = Input.GetButtonDown("Panic");
        if (isPanic)
            inv.DropAllItems();
    }

    #region Helpers
    public void TakeDamage(int amount)
    {
        if (health <= 0)
        {
            Die();
            return;
        }
        if (armor > 0 && amount <= armor)
            armor -= amount;
        else if (armor > 0)
        {
            int temp = amount - armor;
            armor = 0;
            health -= temp;
        } else if (armor == 0)
            health -= amount;

        Persistence.persistence.gm.UpdateHUDStats();

        if (health <= 0)
            Die();
    }

    public IEnumerator TakePoisonDamage(int amount)
    {
        int dmgPerSec = amount / 5;

        for (int i=0; i < 5; i++)
        {
            TakeDamage(dmgPerSec);
            yield return new WaitForSeconds(1f);
        }
    }

    public void SlowPlayerStart(float slowedTime)
    {        
        GetComponent<ThirdPersonCharacter>().SetMovementSpeedMultiplier(origSpeed * .5f);
        if (timeToSlowPlayer == 0f)
            timeToSlowPlayer = Time.time + slowedTime;
        else
            timeToSlowPlayer += slowedTime;
    }

    void SlowPlayerEnd()
    {
        GetComponent<ThirdPersonCharacter>().SetMovementSpeedMultiplier(origSpeed);
        timeToSlowPlayer = 0f;
    }

    private void Die()
    {
        //play sound
        this.gameObject.GetComponent<Animator>().Play("Death");
    }

    public Items GetCurrentWeapon()
    {
        Items temp = null;
        if (weaponHolster != null && weaponHolster.selectedGuns != null)
            temp = Persistence.persistence.itemsList[weaponHolster.selectedGuns[weaponHolster.selectedGun]];
        return temp;
    }

    public GunController GetCurrentWeaponController(string wep)
    {
        GunController gc = null;
        foreach (Transform t in weaponHolster.gameObject.transform)
        {
            if (t.name == wep)
            {
                gc = t.GetComponent<GunController>();
            }
        }
        return gc;
    }

    public void Reload()
    {
        Items wep = GetCurrentWeapon();
        GunController wepCont = GetCurrentWeaponController(wep.itemName);
        AudioSource audio = wepCont.gameObject.GetComponent<AudioSource>();

        if (Persistence.persistence.itemsCount[wep.itemName + "Ammo"] > (wep.GetClipSize() - wep.GetBullets()))
        {
            Persistence.persistence.itemsCount[wep.itemName + "Ammo"] -= (wep.GetClipSize() - wep.GetBullets());
            wep.SetBullets(wep.GetClipSize());
            audio.clip = wepCont.sounds[1];
            audio.Play();
            Persistence.persistence.gm.UpdateHUDStats();
            inv.UpdateBagSlotUINumber(Persistence.persistence.FindItem(wep.itemName+"Ammo").inventorySlot, Persistence.persistence.itemsCount[wep.itemName + "Ammo"]);
        }
        else if (Persistence.persistence.itemsCount[wep.itemName + "Ammo"] > 0 &&
          Persistence.persistence.itemsCount[wep.itemName + "Ammo"] < (wep.GetClipSize()-wep.GetBullets()))
        {
            wep.SetBullets(Persistence.persistence.itemsCount[wep.itemName + "Ammo"] + wep.GetBullets());
            Persistence.persistence.itemsCount[wep.itemName + "Ammo"] = 0;
            audio.clip = wepCont.sounds[1];
            audio.Play();
            Persistence.persistence.gm.UpdateHUDStats();
            inv.UpdateBagSlotUINumber(Persistence.persistence.FindItem(wep.itemName + "Ammo").inventorySlot, Persistence.persistence.itemsCount[wep.itemName + "Ammo"]);
        }
        else if (Persistence.persistence.itemsCount[wep.itemName + "Ammo"] == 0)
            return;
    }

    public int GetHealth() {
		return health;
	}

	public void AddHealth(int number) {
		health += number;
		if (health > maxHealthArmor)
			health = maxHealthArmor;
	}

    public void AddHealth(string healthType)
    {
        Items item = Persistence.persistence.FindItem(healthType);
        AddHealth(item.GetHealing());
    }

    public int GetArmor() {
		return armor;
	}

	public void AddArmor(int number) {
		armor += number;
		if (armor > maxHealthArmor)
			armor = maxHealthArmor;
	}

    public void AddArmor(string armorType)
    {
        Items item = Persistence.persistence.FindItem(armorType);
        AddArmor(item.GetArmor());
    }

    void Aimming(bool state)
    {
        playerCam.SetAimming(state);
        //this.gameObject.GetComponent<Animator>().SetBool("IsAim", state);

        if (state)
        {
            this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().Walk();
            RotatePlayer();
        }
        crosshair.SetActive(state);
    }

    // Rotate the player to match correct orientation, according to camera.
    void RotatePlayer()
    {
        Vector3 forward = playerCam.transform.TransformDirection(Vector3.forward);
        // Player is moving on ground, Y component of camera facing is not relevant.
        forward.y = 0.0f;
        forward = forward.normalized;

        // Always rotates the player according to the camera forward in aim mode.
        Quaternion targetRotation = Quaternion.LookRotation(forward);

        Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, 15 * Time.deltaTime);
        rbody.MoveRotation (newRotation);
    }

    #endregion
}
