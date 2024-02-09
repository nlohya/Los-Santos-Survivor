using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponBehaviour : MonoBehaviour
{
    
    public BulletBehaviour bullet;
    
    public Transform bulletSpawn;

    public bool defaultUnlock;

    private bool _unlocked;

    public float priceUnlock = 0;


    void Awake()
    {
        _unlocked = defaultUnlock;
    }
    
	public virtual string getName() {
		return "";
	}
    
    public virtual void shoot(bool canShoot)
    {
        if (!canShoot) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var bulletInstance = Instantiate(bullet, bulletSpawn.position, transform.rotation);
        }
    }

    public bool isUnlocked()
    {
        return _unlocked;
    }

    public void setUnlocked(bool value)
    {
        _unlocked = value;
    }

}
