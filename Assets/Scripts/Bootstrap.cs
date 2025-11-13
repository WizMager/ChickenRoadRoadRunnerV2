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
    [SerializeField] private UnityEngine.Camera _camera;
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

    [SerializeField] private CameraFollow _cameraFollow;

    private ICheckpointService _checkpointService;
    private IChickenMove _chickenMove;
    private ICarController _carController;
    private IRoadBarrierController _roadBarrierController;

    private void Awake()
    {
        _checkpointService = new CheckpointService(_checkpoints);
        _checkpointService.OnLastCheckpointReached += OnLastCheckpointReached;
        _carController = new CarController(_carLines, _checkpointService, _gameHudWindow, _gameData, this, _iconsData, _audioService, _roadBarriers);
        _chickenMove = new ChickenMove(_gameHudWindow, _checkpointService, _chicken, _gameData, _audioService, _carController);
        _carController.OnChickenHit += OnChickenHit;
        _carController.Initialize();
        _roadBarrierController = new RoadBarrierController(_roadBarriers, _checkpointService, _gameData, _gameHudWindow, _carController);
        
        _cameraFollow.Initialize(_camera);
        _gameHudWindow.Initialize(_gameData, _checkpoints.Count - 1, _carController);
        _minigamePopupWindow.Initialize(_audioService);
        _winPopupWindow.Initialize(_audioService);
        _minigamePopupWindow.OnCompleteMiniGame += OnCompleteMiniGame;
    }

    private void OnCompleteMiniGame()
    {
        _chickenMove.GoToLastCheckpoint();
        _winPopupWindow.Show();
    }

    private void OnLastCheckpointReached()
    {
		_minigamePopupWindow.PlaySequence();
    }

    private void OnChickenHit()
    {
        StartCoroutine(SkipTime());
    }

    private IEnumerator SkipTime()
    {
        yield return new WaitForSeconds(_gameData.TimeToStepMove / 3);
        
        _chickenMove.RevertJump();

        if (_checkpointService.GetCurrentCheckpoint == 0)
        {
            _gameHudWindow.Reset();
            
            yield break;
        }
            
        
        _winPopupWindow.Show(true, _gameHudWindow.CurrentScore);
    }
}