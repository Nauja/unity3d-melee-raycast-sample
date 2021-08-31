using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Component for quickly flashing a Renderer</summary>
    public class FlashController : MonoBehaviour
    {
        [Tooltip("Reference to the Renderer")]
        [SerializeField]
        private Renderer _renderer;
        public Renderer renderer
        {
            get { return _renderer; }
        }

        [Tooltip("Flash color")]
        [SerializeField]
        private Color _color;

        [Tooltip("Time in seconds for the flash to be fully visible")]
        [SerializeField]
        private float _fadeInDuration;

        [Tooltip("Time in seconds for the flash to disappear")]
        [SerializeField]
        private float _fadeOutDuration;

        private Coroutine _flashCoro;

        private void Start()
        {
            foreach (var _ in _renderer.materials)
            {
                _.EnableKeyword("_EMISSION");
            }
        }

        public virtual void Flash()
        {
            if (_flashCoro != null)
            {
                StopCoroutine(_flashCoro);
            }
            _flashCoro = StartCoroutine(FlashCoroutine());
        }

        private IEnumerator FlashCoroutine()
        {
            yield return FadeColor(Color.black, _color, _fadeInDuration);
            yield return FadeColor(_color, Color.black, _fadeOutDuration);
        }

        private IEnumerator FadeColor(Color from, Color to, float duration)
        {
            var initTime = Time.time;
            var maxTime = initTime + duration;
            while(true)
            {
                var time = Time.time;
                var progress = Mathf.Clamp((time - initTime) / duration, 0.0f, 1.0f);

                SetColor(Color.Lerp(from, to, progress));
                yield return new WaitForSeconds(0);

                if (time >= maxTime)
                {
                    SetColor(to);
                    break;
                }
            }
        }

        private void SetColor(Color color)
        {
            foreach (var _ in _renderer.materials)
            {
                _.SetColor("_EmissionColor", color);
            }
        }
    }
}