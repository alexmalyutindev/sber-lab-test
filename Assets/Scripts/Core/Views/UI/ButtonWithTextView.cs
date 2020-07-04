using System;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithTextView : MonoBehaviour
{
    public Action OnClick;

    [SerializeField] private Text _textField;
    [SerializeField] private Button _button;

    private void Awake() => _button.onClick.AddListener(OnClickHandler);
    private void OnClickHandler() => OnClick?.Invoke();
    public void SetText(string text) => _textField.text = text;
}