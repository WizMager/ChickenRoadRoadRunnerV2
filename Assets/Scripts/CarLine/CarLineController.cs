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
        private readonly IconsData _iconsData;

        public CarLineController(
            GameHudWindow gameHudWindow, 
            List<Views.CarLine> carLines, 
            ICheckpointService checkpointService, 
            GameData gameData, 
            IconsData iconsData
        )
        {
            _gameHudWindow = gameHudWindow;
            _carLines = carLines;
            _checkpointService = checkpointService;
            _gameData = gameData;
            _iconsData = iconsData;

            _gameHudWindow.OnNextPressed += OnNext;
        }

        private void OnNext()
        {
            var currentCheckpointIndex = _checkpointService.GetCurrentCheckpoint;
            var animationTime = _gameData.TimeToStepMove;
            
            _carLines[currentCheckpointIndex].StartBarrier(animationTime);
            
            _gameHudWindow.StartCoroutine(WaitAndCarHandle(currentCheckpointIndex));
        }

        private IEnumerator WaitAndCarHandle(int checkpointIndex)
        {
            yield return new WaitForSeconds(_gameData.TimeToStepMove);
            
            _carLines[checkpointIndex].StartCar(_gameData.CarDriveTimeBeforeBarrier, _iconsData.GetRandomCar());
        }
    }
}