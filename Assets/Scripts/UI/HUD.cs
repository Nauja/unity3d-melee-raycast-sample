using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private Text _actionNameText;

        [SerializeField]
        private RectTransform _barTransform;

        [SerializeField]
        private RectTransform _inputPhaseTransform;

        [SerializeField]
        private RectTransform _comboPhaseTransform;

        [SerializeField]
        private RectTransform _execPhaseTransform;

        [SerializeField]
        private RectTransform _cursorTransform;

        [SerializeField]
        private InputController _inputController;

        [SerializeField]
        private GameObject _buttonPrefab;

        private Action _action;

        private List<GameObject> _buttons;

        private void Awake()
        {
            GameEvents.OnActionStarted += HandleActionStarted;
            GameEvents.OnInputPressed += HandleInputPressed;
            _buttons = new List<GameObject>();
            gameObject.SetActive(false);
        }

        private void HandleActionStarted(Action action)
        {
            Clear();
            _actionNameText.text = action.data.displayName;
            _action = action;
            gameObject.SetActive(true);

            bool hasCombo = action.data.combo.duration > 0.0f && action.data.duration > 0.0f;
            _comboPhaseTransform.gameObject.SetActive(hasCombo);
            _inputPhaseTransform.gameObject.SetActive(hasCombo);
            if (hasCombo)
            {
                var delay = action.data.combo.delay;
                var duration = action.data.combo.duration;
                _comboPhaseTransform.sizeDelta = new Vector2((duration / action.data.duration) * _barTransform.rect.width, 20.0f);
                _comboPhaseTransform.anchoredPosition = new Vector2((delay / action.data.duration) * _barTransform.rect.width, 0.0f);
                _inputPhaseTransform.sizeDelta = new Vector2(((duration + _inputController.inputLifetime) / action.data.duration) * _barTransform.rect.width, 20.0f);
                _inputPhaseTransform.anchoredPosition = new Vector2(((delay - _inputController.inputLifetime) / action.data.duration) * _barTransform.rect.width, 0.0f);
            }

            if (action.data.exec.duration > 0.0f)
            {
                _execPhaseTransform.gameObject.SetActive(true);
                _execPhaseTransform.sizeDelta = new Vector2((action.data.exec.duration / action.data.duration) * _barTransform.rect.width, 20.0f);
                _execPhaseTransform.anchoredPosition = new Vector2((action.data.exec.delay / action.data.duration) * _barTransform.rect.width, 0.0f);
            }
            else
            {
                _execPhaseTransform.gameObject.SetActive(false);
            }
        }

        private void HandleInputPressed(EInput input)
        {
            if (input != EInput.Attack || _action == null)
            {
                return;
            }

            var o = Instantiate(_buttonPrefab);
            o.transform.SetParent(_barTransform.transform);
            o.GetComponent<RectTransform>().anchoredPosition = new Vector2((_action.timeRunning / _action.data.duration) * _barTransform.rect.width, 0.0f);
            _buttons.Add(o);
        }

        private void Update()
        {
            var dX = 0.0f;
            if (_action != null && _action.data.duration > 0.0f)
            {
                dX = _action.timeRunning / _action.data.duration;
            }

            if (dX > 1.0f)
            {
                dX = 1.0f;
                Clear();
            }

            _cursorTransform.anchoredPosition = new Vector2(dX * _barTransform.rect.width, 0.0f);
        }

        private void Clear()
        {
            _action = null;
            foreach (var _ in _buttons)
            {
                Destroy(_);
            }
            _buttons.Clear();
            gameObject.SetActive(false);
        }
    }
}