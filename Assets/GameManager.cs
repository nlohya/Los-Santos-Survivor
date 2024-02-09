using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
   public static GameManager instance;

   public Image healthBar;

   public PlayerBehaviour player;

   public List<GameObject> spawners;

   private float _timer;
   
   private float _scoreTimer;

   public float spawnDelay;

   public ZombieBehaviour zombiePrefab;

   public ZombieBehaviour megaZombiePrefab;

   public MedkitBehaviour medkitPrefab;

   public TextMeshProUGUI scoreText;

   public TextMeshProUGUI weaponText;

   public TextMeshProUGUI alertText;
   
   private float _gameScore;
   
   void Awake()
   {
      if (instance != null)
         Destroy(this.gameObject);

      instance = this;
      _gameScore = 0;
      alertText.text = "";
   }

   void Start()
   {
      _scoreTimer = 0f;
      _timer = 0f;
      updateScore();
      StartCoroutine(spawnZombies());
   }

   void Update()
   {
      _timer += Time.deltaTime;
      _scoreTimer += Time.deltaTime;

      if (spawnDelay > 1) {
         spawnDelay -= Time.deltaTime / 10;
      }

      if (!player.isDead())
         _gameScore += (Time.deltaTime / spawnDelay) * 10.0f;

      unlockWeapons(_gameScore);

      if (_scoreTimer >= 1)
      {
         updateScore();
         _scoreTimer = 0f;
      }

      if (_timer >= spawnDelay)
      {
         StartCoroutine(spawnZombies());
         _timer = 0f;
      }
      
      updateHealthBar();
   }

   private void unlockWeapons(float score)
   {
      foreach (var playerAvailableWeapon in player.availableWeapons)
      {
         if (!playerAvailableWeapon.isUnlocked() && (score >= playerAvailableWeapon.priceUnlock))
         {
            player.unlockWeaponByName(playerAvailableWeapon.getName());
         }
      }
   }

   IEnumerator spawnZombies()
   {
      foreach (var spawner in spawners)
      {
         if ((Random.Range(0, 10) % 2) == 0)
         {
            Instantiate(zombiePrefab, spawner.transform.position, new Quaternion(0, 0, 0, 0));
         }
         else if (Random.Range(0, 10) == 5)
         {
            Instantiate(megaZombiePrefab, spawner.transform.position, new Quaternion(0, 0, 0, 0));
         }
      }
      
      yield return null;
   }

   void updateHealthBar()
   {
      healthBar.fillAmount = player.getHealth() / PlayerBehaviour.MAX_HEALTH;
   }

   public void updateScore()
   {
      scoreText.text = $"{(int) _gameScore} $";
   }

   public void updateWeaponName()
   {
      weaponText.text = player.getCurrentWeapon().getName();
   }

   public float getScore()
   {
      return _gameScore;
   }

   public void addScorePoints(float amount)
   {
      _gameScore += amount;
      updateScore();
   }

   public void spawnMedkit(Vector3 position)
   {
      Instantiate(medkitPrefab, position, new Quaternion(0, 0, 0, 0));
   }

   public void loseGame()
   {
      player.StopAllCoroutines();
      SceneManager.LoadScene("End screen");
   }

   public void sendAlert(string message)
   {
      StartCoroutine(alertRoutine(message));
   }

   IEnumerator alertRoutine(string message)
   {
      alertText.text = "";
      for (int i = 0; i < message.Length; i++)
      {
         alertText.text += message[i];
         yield return new WaitForSeconds(0.1f);
      }

      yield return new WaitForSeconds(3f);
      alertText.text = "";
      
      yield return null;
   } 
}
