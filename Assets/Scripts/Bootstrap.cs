using System.Collections;
using System.Collections.Generic;
using Camera;
using Car;
using Db;
using Move;
using RoadBarrier;
using Services.Audio;
using Services.Checkpoint;
using Ui;
using UnityEngine;
using Views;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> _checkpoints;
    [SerializeField] private Chicken _chicken;
    [SerializeField] private Transform _camera;
    [SerializeField] private List<CarLine> _carLines;
    [SerializeField] private List<Views.RoadBarrier> _roadBarriers;
    //UI
    [SerializeField] private GameHudWindow _gameHudWindow;
    [SerializeField] private MinigamePopupWindow _minigamePopupWindow;
    [SerializeField] private WinPopupWindow _winPopupWindow;
    //DATA
    [SerializeField] private GameData _gameData;
    [SerializeField] private IconsData _iconsData;
    //AUDIO
    [SerializeField] private AudioService _audioService;

    private ICheckpointService _checkpointService;
    private IChickenMove _chickenMove;
    private ICarController _carController;
    private IRoadBarrierController _roadBarrierController;
    private ICameraFollow _cameraFollow;

    private void Awake()
    {
        _checkpointService = new CheckpointService(_checkpoints);
        _checkpointService.OnLastCheckpointReached += OnLastCheckpointReached;
        _chickenMove = new ChickenMove(_gameHudWindow, _checkpointService, _chicken, _gameData, _audioService);
        _carController = new CarController(_carLines, _checkpointService, _gameHudWindow, _gameData, this, _iconsData, _audioService, _roadBarriers);
        _carController.OnChickenHit += OnChickenHit;
        _carController.Initialize();
        _roadBarrierController = new RoadBarrierController(_roadBarriers, _checkpointService, _gameData, _gameHudWindow, _carController);
        _cameraFollow = new CameraFollow(_camera, _chicken.transform, 3.080001f);
        
        _gameHudWindow.Initialize(_gameData, _checkpoints.Count - 1);
        _winPopupWindow.Initialize(_audioService);
    }

    private void OnCompleteMiniGame()
    {
        _chickenMove.GoToLastCheckpoint();
        _winPopupWindow.Show();
    }

    private void OnLastCheckpointReached()
    {
		
    }

    private void OnChickenHit()
    {
        _chickenMove.Reset();
        _carController.Reset();
        _checkpointService.Reset();
        _gameHudWindow.Reset();

        StartCoroutine(SkipFrame());
    }

    private IEnumerator SkipFrame()
    {
        yield return null;
        
        _roadBarrierController.Reset();
    }

    private void Update()
    {
        _cameraFollow.Update();
    }
}