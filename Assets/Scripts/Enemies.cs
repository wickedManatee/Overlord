using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : ScriptableObject
{
	[HideInInspector]
	public enum eType {monster, trap};
	[HideInInspector]
	public enum atkType {melee, range};

	public string enemyName;
	public eType enemyType;
	public atkType attackType;
    public float wanderTimer;
    public float wanderArea;

	private int cost = 1;
	private int health = 1;
	private int damage = 1;
	private float speed = 1f;
	private float range = 1f;
    private float attackSpeed = 1f;

    internal void init()
    {

    }

    public void SetCost(int num)
	{
		cost = num;
	}
	public int GetCost(){
		return cost;
	}

	public void SetHealth(int hp)
	{
		health = hp;
	}
	public int GetHealth(){
		return health;
	}

	public void SetDamage(int dmg)
	{
		damage = dmg;
	}
	public int GetDamage(){
		return damage;
	}

	public void SetSpeed(float spd)
	{
		speed = spd;
	}
	public float GetSpeed(){
		return speed;
	}

	public void SetRange(float rng)
	{
		range = rng;
	}
	public float GetRange(){
		return range;
	}

    public void SetAttackSpeed(float atkSpd)
    {
        attackSpeed = atkSpd;
    }
    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
}
