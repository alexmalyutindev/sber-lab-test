using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{
    public Action OnPlay;
    
    [SerializeField] private Text _playerListField;

    [SerializeField] private Button _play;

    private void Awake() => 
        _play.onClick.AddListener(PlayHandler);

    private void PlayHandler() => OnPlay?.Invoke();

    public void SetPlayersList(List<string> players) => 
        _playerListField.text = String.Join("\n", players);
}