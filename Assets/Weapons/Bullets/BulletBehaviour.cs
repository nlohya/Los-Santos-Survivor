using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;

    public float bulletSpeed = 4f;
    
	public float bulletDamage = 0.5f;

    public float bulletForce = 4f;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(transform.position + transform.up * bulletSpeed * Time.fixedDeltaTime);
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
        yield return null;
    }
}
