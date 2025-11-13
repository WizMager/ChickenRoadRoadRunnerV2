using System;
using System.Collections.Generic;
using Car;
using Db;
using Services.Checkpoint;
using Ui;

namespace RoadBarrier
{
    public class RoadBarrierController : IRoadBarrierController, IDisposable
    {
        private readonly List<Views.RoadBarrier> _barriers;
        private readonly ICheckpointService _checkpointService;
        private readonly GameData _gameData;
        private readonly GameHudWindow _gameHudWindow;
        private readonly ICarController _carController;

        public RoadBarrierController(
            List<Views.RoadBarrier> barriers, 
            ICheckpointService checkpointService, 
            GameData gameData, 
            GameHudWindow gameHudWindow, 
            ICarController carController
        )
        {
            _barriers = barriers;
            _checkpointService = checkpointService;
            _gameData = gameData;
            _gameHudWindow = gameHudWindow;
            _carController = carController;

            _gameHudWindow.OnNextPressed += OnNextCheckpointPressed;
        }
        
        private void OnNextCheckpointPressed()
        {
            if (!_carController.IsSaveJump)
            {
                return;
            }
            
            var checkpoint = _checkpointService.GetCurrentCheckpoint;

            if (_barriers.Count - 1 < checkpoint)
                return;
            
            _barriers[checkpoint].MoveBarrier(_gameData.TimeToStepMove);
        }

        public void Reset()
        {
            foreach (var barrier in _barriers)
            {
                barrier.ResetState();
            }
        }
        
        public void Dispose()
        {
            _gameHudWindow.OnNextPressed -= OnNextCheckpointPressed;
        }
    }
}