﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public bool dead = false;
    public SpriteRenderer sprt;
    public SpriteRenderer defBubble;
    public SpriteRenderer offBubble;
    HordeScript hordeScript;
    PlayerScript playerScript;
    GameData gameData;
    public PlayerStats stats;
    public bool isAttachedToKing = false;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Animator>().speed = UnityEngine.Random.Range(0.5f, 1.5f);
        hordeScript = GameObject.Find("Horde").GetComponent<HordeScript>();
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        playerScript = GameObject.Find("FAT player").GetComponent<PlayerScript>();
        stats = playerScript.stats;
    }

    // Update is called once per frame
    void Update()
    {
        // make it more red the deader it is
        Color clr = sprt.color;
        clr.g = health / maxHealth;
        clr.b = health / maxHealth;
        sprt.color = clr;

        if (dead) return;

        if (hordeScript.state == HordeState.DEFENSIVE)
        {
            offBubble.gameObject.SetActive(false);
            defBubble.gameObject.SetActive(true);
        }

        if (hordeScript.state == HordeState.OFFENSIVE)
        {
            offBubble.gameObject.SetActive(true);
            defBubble.gameObject.SetActive(false);
        }

        if (hordeScript.state == HordeState.NEUTRAL)
        {
            offBubble.gameObject.SetActive(false);
            defBubble.gameObject.SetActive(false);
        }

        if (health < maxHealth) health += hordeScript.regenerationSpeed;
    }

    public bool Hit(float hp)
    {
        if (dead) return false;

        health -= hp * hordeScript.defensiveStat;
        if (health <= 0) {
            Kill();
            return true;
        }

        // detach from king once we leave half health
        if (health / maxHealth < 0.5f && isAttachedToKing) {
            hordeScript.DetachZombie(this.gameObject);
        }

        return false;
    }

    public void Kill()
    {
        offBubble.gameObject.SetActive(false);
        defBubble.gameObject.SetActive(false);
        dead = true;
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<ParticleSystem>().Emit(10);
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameData == null) return;

        // if we touch another zombie while attached to the king, recruit that one too
        if (other.tag == "Zombie" && isAttachedToKing)
        {
            hordeScript.AttachZombie(other.gameObject);
        }

        if (other.tag == "KetchupBlob")
        {
            if (Hit(gameData.ketchupDamage)) stats.kills++;
        }
    }
}
