using System.Collections;
using System.Collections.Generic;
using Db;
using Services.Checkpoint;
using Ui;
using UnityEngine;

namespace CarLine
{
    public class CarLineController
    {
        private readonly GameHudWindow _gameHudWindow;
        private readonly List<Views.CarLine> _carLines;
        private readonly ICheckpointService _checkpointService;
        private readonly GameData _gameData;

        public CarLineController(
            GameHudWindow gameHudWindow, 
            List<Views.CarLine> carLines, 
            ICheckpointService checkpointService, 
            GameData gameData
        )
        {
            _gameHudWindow = gameHudWindow;
            _carLines = carLines;
            _checkpointService = checkpointService;
            _gameData = gameData;

            _gameHudWindow.OnNextPressed += OnNext;
        }

        private void OnNext()
        {
            var currentCheckpointIndex = _checkpointService.GetCurrentCheckpoint;
            var animationTime = _gameData.TimeToStepMove;
            
            
            _carLines[currentCheckpointIndex].StartBarrier(animationTime);
        }

        private IEnumerator WaitAndCarHandle(int checkpointIndex)
        {
            yield return new WaitForSeconds(_gameData.TimeToStepMove);

            var animationTime = _gameData.CarDriveTimeBeforeBarrier;
            
            _carLines[checkpointIndex + 1].StartCar(animationTime, null);
        }
    }
}