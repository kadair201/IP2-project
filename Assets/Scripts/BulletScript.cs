﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed;
    }


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player") return;

        if (collision.tag == "Zombie")
        {
            collision.gameObject.GetComponent<ZombieScript>().Hit(gameData.throwingKnifeDamage);

         
        }
        Destroy(gameObject);
    }
}
