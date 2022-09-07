using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    #region Properties
    private static GameLogic _gameLogicInstance;
    public static GameLogic GameLogicInstance
    {
        //Property Read is public by default and readys the instance
        get => _gameLogicInstance;
        private set
        {
            //Property private write sets the instance to the value if the instance is null
            if (_gameLogicInstance == null)
            {
                _gameLogicInstance = value;
            }
            //Property checks for already existing NetworkManagers and if the instance doesn't match it destroys it :)
            else if (_gameLogicInstance != value)
            {
                Debug.LogWarning($"{nameof(GameLogic)} instance already exists, destroy duplicate!");
                Destroy(value);
            }
        }
    }


    #region Variables
    public GameObject PlayerPrefab => _playerPrefab;
    public GameObject LocalPlayerPrefab => _localPlayerPrefab;


    [Header("Prefabs")]
    [SerializeField] private GameObject _localPlayerPrefab;
    [SerializeField] private GameObject _playerPrefab;


    #endregion


    #endregion

    private void Awake()
    {
        //sets the singleton to this
        GameLogicInstance = this;
    }
}
