using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Checkpoint
{
    public class CheckpointService : ICheckpointService
    {
        private readonly List<Views.Checkpoint> _checkpoints;
        
        private int _currentCheckpoint;
        
        public Action OnLastCheckpointReached { get; set; }
        public Action OnCheckpointReached { get; set; }
        public int GetCurrentCheckpoint => _currentCheckpoint;
        public Vector2 GetNextCheckpointPosition => _checkpoints[_currentCheckpoint + 1].transform.position;

        public Vector2 GetStartPosition => _checkpoints[0].transform.position;
        public Vector2 GetEndPosition => _checkpoints[_checkpoints.Count - 1].transform.position;

        public CheckpointService(List<Views.Checkpoint> checkpoints)
        {
            _checkpoints = checkpoints;
        }
        
        public void NextCheckpoint()
        {
            _currentCheckpoint++;
            
            if (_currentCheckpoint == _checkpoints.Count - 2)
            {
                OnLastCheckpointReached?.Invoke();
                
                return;
            }
            
            OnCheckpointReached?.Invoke();
        }

        public void Reset()
        {
            _currentCheckpoint = 0;
        }
    }
}