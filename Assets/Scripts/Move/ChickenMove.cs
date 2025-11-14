using System;
using Db;
using DG.Tweening;
using Services.Audio;
using Services.Checkpoint;
using Ui;
using UnityEngine;
using Views;
using ESoundType = Db.Sound.ESoundType;

namespace Move
{
    public class ChickenMove : IChickenMove, IDisposable
    {
        private readonly GameHudWindow _gameHudWindow;
        private readonly ICheckpointService _checkpointService;
        private readonly Chicken _chicken;
        private readonly GameData _gameData;
        private readonly AudioService _audioService;

        private Sequence _sequence;
        
        public ChickenMove(
            GameHudWindow gameHudWindow, 
            ICheckpointService checkpointService, 
            Chicken chicken, 
            GameData gameData,
            AudioService audioService
        )
        {
            _gameHudWindow = gameHudWindow;
            _checkpointService = checkpointService;
            _chicken = chicken;
            _gameData = gameData;
            _audioService = audioService;

            _gameHudWindow.OnNextPressed += OnNextCheckpointPressed;
        }

        private void OnNextCheckpointPressed()
        {
            _chicken.StartJumpAnimation();
            _audioService?.PlaySound(ESoundType.ChickenJump);
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            var stepDuration = _gameData.TimeToStepMove;
            var movePosition = _checkpointService.GetNextCheckpointPosition;
            
            _sequence.Append(_chicken.transform.DOMoveX(movePosition.x, stepDuration));
            _sequence.Join(_chicken.transform.DOMoveY(movePosition.y + 1, stepDuration / 2));
            _sequence.Insert(stepDuration / 2, _chicken.transform.DOMoveY(movePosition.y, stepDuration / 2));
        }

        public void GoToLastCheckpoint()
        {
            _chicken.StartJumpAnimation();
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            var stepDuration = _gameData.TimeToStepMove;
            var movePosition = _checkpointService.GetEndPosition;
            
            _sequence.Append(_chicken.transform.DOMoveX(movePosition.x, stepDuration));
            _sequence.Join(_chicken.transform.DOMoveY(movePosition.y + 1, stepDuration / 2));
            _sequence.Insert(stepDuration / 2, _chicken.transform.DOMoveY(movePosition.y, stepDuration / 2));
        }

        public void RevertJump()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            var stepDuration = _gameData.TimeToStepMove;
            var movePosition = _checkpointService.GetCurrentCheckpointPosition;
            
            _sequence.Append(_chicken.transform.DOMoveX(movePosition.x, stepDuration / 3 * 2));
            _sequence.Join(_chicken.transform.DOMoveY(movePosition.y, stepDuration / 3 * 2));
        }

        public void Reset()
        {
            _sequence?.Kill();
            _chicken.transform.position = _checkpointService.GetStartPosition;
            _chicken.InterruptJumpAnimation();
        }

        public void Dispose()
        {
            _gameHudWindow.OnNextPressed -= OnNextCheckpointPressed;
        }
    }
}