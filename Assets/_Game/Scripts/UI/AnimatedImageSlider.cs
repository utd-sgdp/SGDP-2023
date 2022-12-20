using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(Image))]
    public class AnimatedImageSlider : MonoBehaviour
    {
        [SerializeField, Min(0)] float _speed = 5;
        [SerializeField, Min(0)] float _delay = 0;
        [SerializeField] bool _skipDelayIfMidUpdate = true;
        
        Image _image;

        float _normalizedSpeed => _speed * Time.deltaTime;
        float _currentFraction => _image.fillAmount;

        Promise _animPromise;
        Coroutine _animCoroutine;
        
        void Awake()
        {
            _image = GetComponent<Image>();
        }
        
        [Button(Mode = ButtonMode.InPlayMode)]
        public void UpdateUI(float newFraction)
        {
            bool animationIsPlaying = _animPromise != null && !_animPromise.State.IsSettled();
            if (animationIsPlaying)
            {
                // stop animation
                StopCoroutine(_animCoroutine);
            }

            bool noDelay = _delay <= 0 || (_skipDelayIfMidUpdate && animationIsPlaying);
            _animPromise = noDelay
                ? Promise.Start(callback: Animate, onProgress: SetSlider)
                : Promise.Start(callback: Delay)
                    .Then(onFulfill: Animate, onProgress: SetSlider);
            
            void Delay(Promise promise)
            {
                _animCoroutine = StartCoroutine(Coroutines.Wait(promise, _delay));
            }
            
            void Animate(Promise promise)
            {
                _animCoroutine = StartCoroutine(Coroutines.Lerp(promise, _currentFraction, newFraction, _normalizedSpeed));
            }
        }

        void SetSlider(float fraction)
        {
            _image.fillAmount = fraction;
        }
    }
}