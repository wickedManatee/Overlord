using System;
using UnityEngine;

public class Items: ScriptableObject {

	[HideInInspector]
	public float weight;

	[HideInInspector]
	public bool isStackable;
    [HideInInspector]
    public int inventorySlot;
    
    //1-5lbs 1pt				6-10lbs 2pts
    //fits in pocket 1pt		needs backpack 2pts

    [HideInInspector]
	public enum iType {health, armor, gun, ammo, objective};
	public iType itemType;
	public string itemName;
	[HideInInspector]
	public enum handabilityClass {pocket = 0, backpack = 1, onehand = 2, twohand = 3, body = 4};
	public handabilityClass handability;
	public bool isDroppable;

    
    internal void init()
    {
        isStackable = false;
        weight = 0;
        isDroppable = false;
    }

    #region Health
    private int healing = 0;
	public enum hType {bandage, stimpack, medkit }
	public hType healthType;

	public int GetHealing(){
		return healing;
	}

    public void SetHealing(int amt)
    {
        healing = amt;
    }
	#endregion

	#region Armor
	private int protection = 0;
	public enum arType {helmet, jacket, full}
	public arType armorType;

    public int GetArmor()
    {
        return protection;
    }

    public void SetArmor(int amt)
    {
        protection = amt;
    }
	#endregion


	#region Ammo
	public int amount = 0;
	public enum amType {handgunAmmo, shotgunAmmo, m4Ammo, sniperAmmo}
	public amType ammoType;

    public int GetAmmoLeft() {
			return amount;
		}
    
    #endregion

    #region Gun
    private float damage = 1f;
		private float fireRate = 1f;
		private float impactForce = 1f;
		private float range = 1f;
		private int clipSize = 0;
		private int bullets = 0;
		public enum gType {hands = 0, bat = 1, handgun = 2, shotgun = 3, m4 = 4, sniper = 5}
		public gType gunType;

		public void SetDamage(float dmg)
		{
			damage = dmg;
		}
		public float GetDamage(){
			return damage;
		}

		public void SetFireRate(float rof)
		{
			fireRate = rof;
		}
		public float GetFireRate(){
			return fireRate;
		}

		public void SetImpactForce(float force)
		{
			impactForce= force;
		}
		public float GetImpactForce(){
			return impactForce;
		}

		public void SetRange(float dist)
		{
			range = dist;
		}
		public float GetRange(){
			return range;
		}

		public void SetClipSize(int size)
		{
			clipSize = size;
		}
		public int GetClipSize(){
			return clipSize;
		}

    public void SetBullets(int amt)
		{
			bullets = amt;
		}
		public int GetBullets(){
			return bullets;
		}
	#endregion Gun
    
	#region Objective
		public enum oType {pocket, backpack, oneHand, twoHand}
		public oType objType;
	#endregion
}
