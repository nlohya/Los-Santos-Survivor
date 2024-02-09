using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZombieBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;

    public float speed;
    
    private float currentDashTime;

    private bool _isStuned;

    public float maxHealth = 10;

    public float damageDealt = 0.5f;
    
    private float _health;
    
    private bool _dead;

    public float pushForce;
    
    void Start()
    {
        _dead = false;
        _health = maxHealth;
        _isStuned = false;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        zombieRotation();

        zombieWalk();
    }
    
    private void zombieRotation()
    {
        if (_isStuned || GameManager.instance.player.isDead()) return;
        
        Vector3 difference = GameManager.instance.player.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

    private void zombieWalk()
    {
        if (_isStuned  || GameManager.instance.player.isDead()) return;
        
        transform.position = Vector2.MoveTowards(transform.position, GameManager.instance.player.transform.position, (speed / 1000000) / Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (GameManager.instance.player.isDead()) return;
        
        if (collider.CompareTag("Player"))
        {
            GameManager.instance.player.receiveDamage(damageDealt);
            GameManager.instance.player.pushAwayFromTarget(transform.position, 1f, pushForce);

            if (GameManager.instance.player.getHealth() <= 0)
            {
                GameManager.instance.loseGame();
            }
        }
    }

    public void push(Vector2 direction, float duration, float intensity)
    {
        StartCoroutine(Dash(direction, duration, intensity));
    }
    
    public IEnumerator Dash(Vector2 direction, float duration, float intensity)
    {
        _isStuned = true;
        for (int i = 0; i < (int) (duration * 3f); i++)
        {
            _rb.velocity = direction * intensity * 0.5f;
            yield return new WaitForSeconds(duration / (duration * 3f));
        }
        _rb.velocity = new Vector2(0f, 0f);
        _isStuned = false;
        yield return null;
    }

    public float getHealth()
    {
        return _health;
    }

    public void reduceHealth(float amount)
    {
        _health -= amount;
        
        if (_health <= 0 && !_dead)
        {
            if (Random.Range(1, 10) == 5)
            {
                GameManager.instance.spawnMedkit(transform.position);
            }
                
            Destroy(gameObject);
            GameManager.instance.addScorePoints(5);
            
            StopAllCoroutines();
            
            _dead = true;
            return;
        }
    }
}
