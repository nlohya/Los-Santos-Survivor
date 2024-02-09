using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    public float moveSpeed;

    public const float MAX_HEALTH = 20;

    private float _health;

    private Rigidbody2D _rb;
    
    private float currentDashTime;

    private bool _isStuned;

    private bool _dead;

    public List<WeaponBehaviour> availableWeapons;

    private List<WeaponBehaviour> _unlockedWeapons;

    private WeaponBehaviour _currentWeapon;
    
    void Start()
    {
        updateWeaponUnlock();
        _currentWeapon = _unlockedWeapons[0];
        _dead = false;
        _isStuned = false;
        _rb = GetComponent<Rigidbody2D>();
        _health = MAX_HEALTH;
        
        enableUsedWeapon();
        disableUnusedWeapons();
    }

    void Update()
    {
        playerMovement();

        playerRotation();

        weaponChange();

        _currentWeapon.shoot(!(_isStuned || _dead));
    }

    public void updateWeaponUnlock()
    {
        _unlockedWeapons = new List<WeaponBehaviour>();
        foreach (var weapon in availableWeapons)
        {
            if (weapon.isUnlocked())
            {
                _unlockedWeapons.Add(weapon);
            }
        }
    }

    public void unlockWeaponByName(string name)
    {
        foreach (var availableWeapon in availableWeapons)
        {
            if (availableWeapon.getName() == name)
            {
                availableWeapon.setUnlocked(true);
                GameManager.instance.sendAlert($"Unlocked new weapon : {name}");
            }
        }
        updateWeaponUnlock();
        enableUsedWeapon();
        disableUnusedWeapons();
    }

    private void weaponChange()
    {
        bool updateWeapon = false;
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            WeaponBehaviour first = _unlockedWeapons[0];
            _unlockedWeapons.RemoveAt(0);
            _unlockedWeapons.Add(first);
            enableUsedWeapon();
            disableUnusedWeapons();
            updateWeapon = true;
        }
        _currentWeapon = _unlockedWeapons[0];
        if (updateWeapon)
        {
            GameManager.instance.updateWeaponName();
            updateWeapon = false;
        }
    }

    private void enableUsedWeapon()
    {
        _unlockedWeapons[0].gameObject.SetActive(true);
    }

    private void disableUnusedWeapons()
    {
        foreach (var availableWeapon in availableWeapons)
        {
            if (!(_unlockedWeapons.Contains(availableWeapon)))
            {
                availableWeapon.gameObject.SetActive(false);
            }
        }
        if (_unlockedWeapons.Count <= 1) return;
        for (int i = 1; i < _unlockedWeapons.Count; i++)
        {
            _unlockedWeapons[i].gameObject.SetActive(false);
        }
    }

    public WeaponBehaviour getCurrentWeapon()
    {
        return _currentWeapon;
    }

    private void playerRotation()
    {
        if (_isStuned  || _dead) return;

        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void playerMovement()
    {
        if (_isStuned  || _dead) return;
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + (Vector3.up * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + (Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + (Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + (Vector3.down * moveSpeed * Time.deltaTime);
        }
    }

    public void receiveDamage(float amount)
    {
        this._health -= amount;
    }

    public float getHealth()
    {
        return this._health;
    }

    public void pushAwayFromTarget(Vector3 target, float duration, float intensity)
    {
        StartCoroutine(Dash((transform.position - target).normalized, duration, intensity));
    }
    
    IEnumerator Dash(Vector2 direction, float duration, float intensity)
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

    public void healPlayer(float amount)
    {
        if (MAX_HEALTH - _health > amount)
            _health += amount;
        else
            _health = MAX_HEALTH;
    }

    public bool isStuned()
    {
        return _isStuned;
    }

    public bool isDead()
    {
        return _dead;
    }

    public void dieFromVoid()
    {
        _dead = true;
        StartCoroutine(dieFromVoidRoutine());
    }

    IEnumerator dieFromVoidRoutine()
    {
        for (int i = 0; i < 20; i++)
        {
            transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            yield return new WaitForSeconds(0.15f);
        }
        
        GameManager.instance.loseGame();
        yield return null;
    }
}
