using Game.Play;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] Damageable _target;
        [SerializeField] AnimatedImageSlider[] _sliders;

        #if UNITY_EDITOR
        void OnValidate()
        {
            _sliders = GetComponentsInChildren<AnimatedImageSlider>();
        }
        #endif

        void OnEnable()
        {
            if (_target) _target.OnChange.AddListener(UpdateSlidersForTarget);
        }

        void OnDisable()
        {
            if (_target) _target.OnChange.RemoveListener(UpdateSlidersForTarget);
        }

        void UpdateSlidersForTarget(float currentHealth) => UpdateSliders(_target.HealthFraction);

        [Button(Mode = ButtonMode.InPlayMode)]
        void UpdateSliders(float fraction)
        {
            foreach (AnimatedImageSlider slider in _sliders)
            {
                slider.UpdateUI(fraction);
            }
        }
    }
}
