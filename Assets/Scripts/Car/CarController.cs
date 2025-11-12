using System;
using System.Collections;
using System.Collections.Generic;
using Db;
using Services.Audio;
using Services.Checkpoint;
using Ui;
using UnityEngine;
using Views;

namespace Car
{
    public class CarController : ICarController, IDisposable
    {
        private readonly List<CarLine> _carLines;
        private readonly ICheckpointService _checkpointService;
        private readonly GameHudWindow _gameHudWindow;
        private readonly GameData _gameData;
        private readonly Bootstrap _bootstrap;
        private readonly IconsData _iconsData;
        private readonly AudioService _audioService;
        private readonly List<Views.RoadBarrier> _roadBarriers;
        
        private CarLine _currentCarLine;
        private Coroutine _runCoroutine;
        private bool _isClicked;

        public Action OnChickenHit { get; set; }
        public bool IsSaveJump { get; private set; } = true;
        
        public CarController(
            List<CarLine> carLines, 
            ICheckpointService checkpointService, 
            GameHudWindow gameHudWindow, 
            GameData gameData, 
            Bootstrap bootstrap, 
            IconsData iconsData,
            AudioService audioService,
            List<Views.RoadBarrier> roadBarriers
        )
        {
            _carLines = carLines;
            _checkpointService = checkpointService;
            _gameHudWindow = gameHudWindow;
            _gameData = gameData;
            _bootstrap = bootstrap;
            _iconsData = iconsData;
            _audioService = audioService;
            _roadBarriers = roadBarriers;

            _gameHudWindow.OnNextPressed += OnNextCheckpointPressed;
            
            for (int i = 0; i < _carLines.Count; i++)
            {
                var carLine = _carLines[i];
                carLine.OnSafeZone += OnCarDroveSafeZone;
                var barrier = i < _roadBarriers.Count ? _roadBarriers[i] : null;
                carLine.Initialize(_audioService, barrier);
            }
        }

        private void OnNextCheckpointPressed()
        {
            _isClicked = true;
            _bootstrap.StartCoroutine(ResetClick());
            
            if (IsSaveJump)
            {
                _currentCarLine.StopCar(_gameData.CarMoveAllPathTime);
            }
            else
            {
                _bootstrap.StopCoroutine(_runCoroutine);
                OnChickenHit?.Invoke();
            }
        }

        private IEnumerator ResetClick()
        {
            yield return new WaitForSeconds(_gameData.TimeToStepMove);
            _isClicked = false;
        }
        
        private void OnCarDroveSafeZone(bool isSafe)
        {
            IsSaveJump = isSafe;
        }

        public void Initialize()
        {
            _runCoroutine = _bootstrap.StartCoroutine(RunCar());
            _currentCarLine = _carLines[0];
        }
        
        private IEnumerator RunCar()
        {
            while (true)
            {
                yield return new WaitForSeconds(_gameData.StartCarTimer);
                
                var checkpoint = _checkpointService.GetCurrentCheckpoint;
                
                if (_carLines.Count - 1 < checkpoint)
                    yield break;
                
                _currentCarLine = _carLines[checkpoint];
                
                if (_currentCarLine.IsCarStopped)
                    continue;
                
                IsSaveJump = true;
                 
                 if (_isClicked)
                 {
                     _currentCarLine.StopCar(_gameData.CarMoveAllPathTime);
                     continue;
                 }
                _currentCarLine.StartCar(_gameData.CarMoveAllPathTime, _iconsData.GetRandomIcon());
                
                yield return new WaitForSeconds(_gameData.CarMoveAllPathTime);
            }
        }

        public void Reset()
        {
            _runCoroutine = _bootstrap.StartCoroutine(RunCar());
            _currentCarLine = _carLines[0];
            IsSaveJump = true;
            
            foreach (var carLine in _carLines)
            {
                carLine.ResetState();
            }
        }
        
        public void Dispose()
        {
            _gameHudWindow.OnNextPressed -= OnNextCheckpointPressed;
        }
    }
}