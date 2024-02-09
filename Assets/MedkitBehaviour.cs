using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MedkitBehaviour : MonoBehaviour
{

    public float healingAmount;
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameManager.instance.player.healPlayer(healingAmount);
            Destroy(gameObject);
        }    
    }
    
}
