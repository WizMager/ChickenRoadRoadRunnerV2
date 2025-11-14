using System.Collections;
using System.Collections.Generic;
using Db;
using Services.Checkpoint;
using Ui;
using UnityEngine;

namespace CarLine
{
    public class CarLineController : ICarLineController
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
            _gameHudWindow.OnRevivePress += OnRevive;
        }

        private void OnNext()
        {
            var currentCheckpointIndex = _checkpointService.GetCurrentCheckpoint;

            if (currentCheckpointIndex == _gameData.LoseAfterCheckpoint)
            {
                _carLines[currentCheckpointIndex].StartFullRunCar(_gameData.CarDriveTimeFullPath, _iconsData.GetRandomCar());
                
                return;
            }
            
            _carLines[currentCheckpointIndex].StartBarrier(_gameData.TimeToStepMove);
            
            _gameHudWindow.StartCoroutine(WaitAndCarHandle(currentCheckpointIndex));
        }

        private IEnumerator WaitAndCarHandle(int checkpointIndex)
        {
            yield return new WaitForSeconds(_gameData.TimeToStepMove);

            var chance = Mathf.Clamp01(_gameData.CarSpawnChancePercent / 100f);
            
            if (Random.value <= chance)
            {
                _carLines[checkpointIndex].StartCar(_gameData.CarDriveTimeBeforeBarrier, _iconsData.GetRandomCar());
            }
        }
        
        private void OnRevive()
        {
            _gameHudWindow.StartCoroutine(WaitAndDownBarricade());
            
            return;
            
            IEnumerator WaitAndDownBarricade()
            {
                yield return null;
                
                var currentCheckpointIndex = _checkpointService.GetCurrentCheckpoint;
                _carLines[currentCheckpointIndex - 1].StartBarrier(_gameData.TimeToStepMove);
            }
        }
    }
}