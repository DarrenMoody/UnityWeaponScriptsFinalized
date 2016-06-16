﻿using UnityEngine;
using System.Collections;

public class Smite : MeleeAttack {

	[SerializeField]
	private float range = 50;
	[SerializeField]
	private float damage = 25;
	[SerializeField]
	private float arcDegree = 30.0f;
	[SerializeField]
	private float duration;
	[SerializeField]
	private ParticleSystem WeaponVFX;

	public override void Fire ()
	{
		NetworkAnimations netanim = GetComponentInChildren<NetworkAnimations> ();
		netanim.SetBool("Melee", true);
	}

	public void PlayVFX()
	{
		StartCoroutine(attack());
	}
	private IEnumerator attack()
	{
		WeaponVFX.Play();

		yield return new WaitForSeconds (duration * 0.5f);

		if(networkView.isMine) 
		{
			GameObject target = null;
			Collider[] ListOfObjects = Physics.OverlapSphere (this.transform.position, range);
			
			//Single Target.
			foreach (var obj in ListOfObjects) {
				if(obj.gameObject.tag == "Dino" || obj.gameObject.tag == "Ai"){
					//if(obj.gameObject.tag == "Dino" && obj.gameObject.networkView.isMine){
					//}
					float angle = Vector3.Angle(obj.gameObject.transform.position - transform.position, transform.forward);
					if(angle < arcDegree){
						if (target == null){
							target = obj.gameObject;
						}
						var distance = Vector3.Distance(transform.position, obj.gameObject.transform.position);
						var distance2 = Vector3.Distance(transform.position, target.transform.position);
						if (distance < distance2){
							target = obj.gameObject;
							Debug.Log (target.name);
						}
					}
				}
			}

			if (target != null) {
				Health health = target.GetComponent<Health> ();
				health.Damage (damage);
			}

			//Multiple Targets.
			/*		foreach (var obj in ListOfObjects) {
				float angle = Vector3.Angle(obj.gameObject.transform.position - transform.position, transform.forward);
				if(angle < arcDegree){
					Health health = obj.gameObject.GetComponent<Health>();
					if (health != null)
						health.Damage(damage);
				}
			}*/
		}

		yield return new WaitForSeconds (duration * 0.5f);
	}
}
