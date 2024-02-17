using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP = 100f;
    public bool immune = false;
    public GameObject winScreen;
    public GameObject gameUI;

    float hp;
    protected GameObject player;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (!player)
        {
            Debug.LogError("ERROR: Enemy script did not detect a Player present!");
            return;
        }
        hp = maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (!immune)
        {
            float newHP = Mathf.Clamp(hp - damage, 0f, maxHP);
            hp = newHP;
            if (hp == 0f)
            {
                winScreen.SetActive(true);
                gameUI.SetActive(false);
                Time.timeScale = 0.0f;
                Destroy(gameObject);
            }
        }
    }

    public virtual bool Immune { get; set; }
}
