﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 선택지의 한 항목 UI
    /// </summary>
    public class UISelectItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private Image _border = null;
        [SerializeField]
        private Image _bg = null;
        [SerializeField]
        private Text _text = null;
        [SerializeField]
        private Color _enteredColor = default;
        [SerializeField]
        private Color _selectedColor = default;
        [SerializeField]
        private Color _selectedTextColor = default;
        [SerializeField]
        private Vector3 _enterScale = default;
        [SerializeField]
        private float _tweenTime = 0.1f; // 온전히 트윈하는데 걸리는 시간
        private Color _normalColor;
        private Vector3 _normalScale;
        private float _TweenSpeed { get { return 1.0f / _tweenTime; } }
        private const string _enterTweenName = "OnEnterTween";
        private const string _exitTweenName = "OnExitTween";
        private bool _selected = false;
        private enum TweenState
        {
            Exited,
            Entering,
            Entered,
            Exiting,
        }
        private TweenState _tweenState = TweenState.Exited;
        private UnityAction _onPointerEnter;
        private UnityAction _onPointerExit;
        private UnityAction _onClick;

        private void Awake()
        {
            _normalColor = _bg.color;
            _normalScale = _bg.rectTransform.localScale;
        }

        public void SetText(string text) => _text.text = text;

        public void SetEventListener(UnityAction onPointerEnter, UnityAction onPointerExit, UnityAction onClick)
        {
            _onPointerEnter += onPointerEnter;
            _onPointerExit += onPointerExit;
            _onClick += onClick;
        }

        public void OnPointerEnter(PointerEventData ped)
        {
            _onPointerEnter();
        }

        public void OnPointerExit(PointerEventData ped)
        {
            _onPointerExit();
        }

        public void OnPointerClick(PointerEventData ped)
        {
            _onClick();
        }

        public void SetPointerEntered() => DoEnterTween();

        public void SetPointerExited() => DoExitTween();

        public void SetSelected()
        {
            _selected = true;
            if (!DoEnterTween())
            {
                _bg.color = _selectedColor;
                _border.color = _selectedTextColor;
            }
            _text.color = _selectedTextColor;
        }

        private bool DoEnterTween()
        {
            if (_tweenState == TweenState.Entering || _tweenState == TweenState.Entered)
                return false;
            if (_tweenState == TweenState.Exiting)
                StopCoroutine(_exitTweenName);
            StartCoroutine(_enterTweenName);
            return true;
        }

        private void DoExitTween()
        {
            if (_tweenState == TweenState.Exiting || _tweenState == TweenState.Exited)
                return;
            if (_tweenState == TweenState.Entering)
                StopCoroutine(_enterTweenName);
            StartCoroutine(_exitTweenName);
        }

        private Color GetEnterEndColor() => (_selected) ? _selectedColor : _enteredColor;

        private IEnumerator OnEnterTween()
        {
            _tweenState = TweenState.Entering;
            float rate = (_bg.rectTransform.localScale.x - _normalScale.x) / (_enterScale.x - _normalScale.x);
            while (rate < 1.0f)
            {
                rate += (_TweenSpeed * Time.deltaTime);
                _bg.rectTransform.localScale = Vector3.Lerp(_normalScale, _enterScale, rate);
                _bg.color = Color.Lerp(_normalColor, GetEnterEndColor(), rate);
                _border.color = Color.Lerp(_normalColor, GetEnterEndColor(), rate);
                yield return null;
            }
            _bg.rectTransform.localScale = _enterScale;
            _bg.color = GetEnterEndColor();
            _border.color = GetEnterEndColor();
            _tweenState = TweenState.Entered;
        }

        private IEnumerator OnExitTween()
        {
            _tweenState = TweenState.Exiting;
            float rate = (_bg.rectTransform.localScale.x - _enterScale.x) / (_normalScale.x - _enterScale.x);
            while (rate < 1.0f)
            {
                rate += (_TweenSpeed * Time.deltaTime);
                _bg.rectTransform.localScale = Vector3.Lerp(_enterScale, _normalScale, rate);
                _bg.color = Color.Lerp(_enteredColor, _normalColor, rate);
                _border.color = Color.Lerp(_enteredColor, _normalColor, rate);
                yield return null;
            }
            _bg.rectTransform.localScale = _normalScale;
            _bg.color = _normalColor;
            _border.color = _normalColor;
            _tweenState = TweenState.Exited;
        }
    }
}