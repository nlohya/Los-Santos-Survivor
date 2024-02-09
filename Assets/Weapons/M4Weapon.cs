
using System.Collections;
using UnityEngine;

public class M4Weapon : WeaponBehaviour
{

	public int burstAmount;
	
    public override string getName() {
		return "M4";
	}
    
	public override void shoot(bool canShoot)
	{
		if (!canShoot) return;

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			StartCoroutine(burstShoot());
		}
	}

	IEnumerator burstShoot()
	{
		for (int i = 0; i < burstAmount; i++)
		{
			Instantiate(bullet, bulletSpawn.position, transform.rotation);
			yield return new WaitForSeconds(0.1f);
		}
		
		yield return null;
	}
}