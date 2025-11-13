using System.Collections;
using System.Collections.Generic;
using Camera;
using Db;
using Move;
using Services.Audio;
using Services.Checkpoint;
using Ui;
using UnityEngine;
using Views;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> _checkpoints;
    [SerializeField] private Chicken _chicken;
    [SerializeField] private UnityEngine.Camera _camera;
   
    //UI
    [SerializeField] private GameHudWindow _gameHudWindow;
    [SerializeField] private MinigamePopupWindow _minigamePopupWindow;
    [SerializeField] private WinPopupWindow _winPopupWindow;
    //DATA
    [SerializeField] private GameData _gameData;
    //AUDIO
    [SerializeField] private AudioService _audioService;

    [SerializeField] private CameraFollow _cameraFollow;

    private ICheckpointService _checkpointService;
    private IChickenMove _chickenMove;

    private void Awake()
    {
        
    }
}