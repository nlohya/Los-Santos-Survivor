using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconBehaviour : MonoBehaviour
{
    public float offsetX;

    public float offsetY;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(GameManager.instance.player.isStuned());
        
        transform.position = new Vector3(
            GameManager.instance.player.transform.position.x + offsetX,
            GameManager.instance.player.transform.position.y + offsetY,
            transform.position.z
        );

        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
