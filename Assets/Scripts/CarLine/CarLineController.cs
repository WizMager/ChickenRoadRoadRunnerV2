using System.Collections.Generic;
using Services.Checkpoint;
using Ui;

namespace CarLine
{
    public class CarLineController
    {
        private readonly GameHudWindow _gameHudWindow;
        private readonly List<Views.CarLine> _carLines;
        private readonly ICheckpointService _checkpointService;

        public CarLineController(
            GameHudWindow gameHudWindow, 
            List<Views.CarLine> carLines, 
            ICheckpointService checkpointService
        )
        {
            _gameHudWindow = gameHudWindow;
            _carLines = carLines;
            _checkpointService = checkpointService;

            _gameHudWindow.OnNextPressed += OnNext;
        }

        private void OnNext()
        {
            var currentCheckpointIndex = _checkpointService.GetCurrentCheckpoint;
            
            
        }
    }
}