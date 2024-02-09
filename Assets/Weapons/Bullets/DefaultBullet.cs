using UnityEngine;

public class DefaultBullet : BulletBehaviour
{
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Props") || collider.CompareTag("Target"))
        {
            if (collider.CompareTag("Target"))
            {
                ZombieBehaviour zombie = collider.GetComponent<ZombieBehaviour>();
                zombie.reduceHealth(bulletDamage);
                zombie.push((zombie.transform.position - GameManager.instance.player.transform.position).normalized, 1f, bulletForce);
            }
            Destroy(this.gameObject);
        }
    }
}