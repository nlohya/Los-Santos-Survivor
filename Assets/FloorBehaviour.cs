using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBehaviour : MonoBehaviour
{

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameManager.instance.player.dieFromVoid();
        }

        if (collider.CompareTag("Target"))
        {
            Destroy(collider.gameObject);
        }
    }
}
