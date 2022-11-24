using System;
using Game.Play.Level;
using Game.Play;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class HUDHealth : MonoBehaviour
    {
        [SerializeField] Damageable _target;
        
        Image _healthBar;
    
        void Awake()
        {
            _healthBar = GetComponent<Image>();
        }

        void OnEnable()
        {
            if (!_target)
            {
                this.enabled = false;
                return;
            }
            
            _target.OnChange.AddListener(UpdateUI);
            UpdateUI();
        }
        void OnDisable()
        {
            if (!_target) return;
            
            _target.OnChange.RemoveListener(UpdateUI);
        }

        void UpdateUI() => UpdateUI(0);
        void UpdateUI(float currentHealth)
        {
            _healthBar.fillAmount = _target.HealthFraction;
        }
    }
}