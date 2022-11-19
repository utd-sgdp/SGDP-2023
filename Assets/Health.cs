using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;

public class Health : MonoBehaviour
{
    private Image HealthBar;
    public float CurrentHealth;
    private float maxHealth;
    Damageable Player;
    // Start is called before the first frame update
    void Start()
    {
        HealthBar = GetComponent<Image>();
        Player = FindObjectOfType<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHealth = Player._health;
        maxHealth = Player._maxHealth;
        HealthBar.fillAmount = CurrentHealth/maxHealth;
    }
}
