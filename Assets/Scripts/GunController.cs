using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
	public Camera gunCam;
    [HideInInspector]
	public Items gun;
	public Items.gType gunType;
	public ParticleSystem muzzleFlash;
	public GameObject hitEffect;
	public AudioClip[] sounds;

	private float nextTimeToFire = 1f;

	void Start() {
		gun = Persistence.persistence.FindItem(gunType.ToString());
	}

	void Update() {
		if (Input.GetButton ("Fire1") && Time.time >=  nextTimeToFire && Input.GetButton("Aim")) {
            nextTimeToFire = Time.time + 5f / gun.GetFireRate();
            if (gun.GetBullets() > 0 || ((int)gun.itemType) < 2)
            {                
                Attack();
            }
            else
            {
                GetComponent<AudioSource>().clip = sounds[2];
                GetComponent<AudioSource>().Play();
            }
		} 
	}
    
	void Attack() {
        if ((int)gunType > 1)
            muzzleFlash.Play ();
		AudioSource audio = GetComponent<AudioSource> ();
		audio.clip = sounds [0];
		audio.Play();
        //TODO Play animation

        gun.SetBullets(gun.GetBullets() - 1);
        Persistence.persistence.gm.UpdateHUDStats();

        RaycastHit hitInfo;
        var layerMask = ~(1 << 11);
        if (Physics.Raycast (gunCam.transform.position, gunCam.transform.forward, out hitInfo, gun.GetRange(), layerMask)) {
			Debug.Log (hitInfo.transform.name + " hit");

			DestroyableObject obj = hitInfo.transform.GetComponent<DestroyableObject> ();
			if (obj != null) {
				obj.TakeDamage (gun.GetDamage());
			}
			EnemyController enemy = hitInfo.transform.GetComponent<EnemyController> ();
			if (enemy != null) {
				enemy.TakeDamage (gun.GetDamage());
			}

			if (hitInfo.rigidbody != null)
				hitInfo.rigidbody.AddForce (-hitInfo.normal * gun.GetImpactForce());
			
			GameObject impact = Instantiate (hitEffect, hitInfo.point, Quaternion.LookRotation (hitInfo.normal));
			Destroy (impact, 2f);
		}
	}
}
