using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Camera;
using CarLine;
using Db;
using Db.Checkpoint;
using Move;
using Services.Audio;
using Services.Checkpoint;
using Ui;
using UnityEngine;
using Views;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> _checkpoints;
    [SerializeField] private List<Views.CarLine> _carLines;
    [SerializeField] private Chicken _chicken;
    [SerializeField] private UnityEngine.Camera _camera;
   
    //UI
    [SerializeField] private GameHudWindow _gameHudWindow;
    [SerializeField] private MinigamePopupWindow _minigamePopupWindow;
    [SerializeField] private WinPopupWindow _winPopupWindow;
    
    //DATA
    [SerializeField] private GameData _gameData;
    [SerializeField] private IconsData _iconsData;
    [SerializeField] private CheckpointData _checkpointData;
    
    [SerializeField] private AudioService _audioService;
    [SerializeField] private CameraFollow _cameraFollow;

    private ICheckpointService _checkpointService;
    private IChickenMove _chickenMove;
    //private ICarLineController _carLineController;

    private void Awake()
    {
        for (var i = 0; i < _checkpoints.Count; i++)
        {
            _checkpoints[i].Initialize(_iconsData, "x" + _checkpointData.GetCheckpointData(i).Multiply.ToString(CultureInfo.InvariantCulture), _gameData);
        }

        _checkpointService = new CheckpointService(_checkpoints, _gameHudWindow, _gameData);
        _chickenMove = new ChickenMove(_gameHudWindow, _checkpointService, _chicken, _gameData, _audioService);
        new CarLineController(_gameHudWindow, _carLines, _checkpointService, _gameData, _iconsData);
    }
}