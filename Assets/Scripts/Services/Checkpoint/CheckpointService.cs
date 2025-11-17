using System;
using System.Collections;
using System.Collections.Generic;
using Db;
using Ui;
using UnityEngine;

namespace Services.Checkpoint
{
    public class CheckpointService : ICheckpointService
    {
        private readonly List<Views.Checkpoint> _checkpoints;
        private readonly GameHudWindow _gameHudWindow;
        private readonly GameData _gameData;
        
        private int _currentCheckpoint;
        
        public Action OnLastCheckpointReached { get; set; }
        public Action OnCheckpointReached { get; set; }
        public bool IsLastCheckpoint { get; private set; }
        public int GetCurrentCheckpoint => _currentCheckpoint;
        public Vector2 GetNextCheckpointPosition => _checkpoints[_currentCheckpoint + 1].transform.position;
        public Vector2 GetCurrentCheckpointPosition => _checkpoints[_currentCheckpoint].transform.position;
        public Vector2 GetStartPosition => _checkpoints[0].transform.position;
        public Vector2 GetEndPosition => _checkpoints[_checkpoints.Count - 1].transform.position;

        public CheckpointService(
            List<Views.Checkpoint> checkpoints, 
            GameHudWindow gameHudWindow, 
            GameData gameData
        )
        {
            _checkpoints = checkpoints;
            _gameHudWindow = gameHudWindow;
            _gameData = gameData;

            _gameHudWindow.OnNextPressed += NextCheckpoint;
        }

        private void NextCheckpoint()
        {
            if (IsLastCheckpoint)
                return;
            
            if (_currentCheckpoint > 0)
            {
                if (_currentCheckpoint != _gameData.LoseAfterCheckpoint + 1)
                {
                    _checkpoints[_currentCheckpoint].JumpFrom();
                }
            }

            _checkpoints[_currentCheckpoint + 1].Stay();
            
            _gameHudWindow.StartCoroutine(WaitAndNextCheckpoint());
        }

        private IEnumerator WaitAndNextCheckpoint()
        {
            yield return new WaitForSeconds(_gameData.TimeToStepMove - 0.05f);
            
            _currentCheckpoint++;
            
            if (_currentCheckpoint == _checkpoints.Count - 2)
            {
                IsLastCheckpoint = true;
                OnLastCheckpointReached?.Invoke();
                
                yield break;
            }
            
            OnCheckpointReached?.Invoke();
        }
    }
}