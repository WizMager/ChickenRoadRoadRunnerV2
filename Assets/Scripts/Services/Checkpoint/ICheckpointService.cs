using System;
using UnityEngine;

namespace Services.Checkpoint
{
    public interface ICheckpointService
    {
        Action OnLastCheckpointReached { get; set; }
        Action OnCheckpointReached { get; set; }
        int GetCurrentCheckpoint { get; }
        Vector2 GetNextCheckpointPosition { get; }
        Vector2 GetCurrentCheckpointPosition { get; }
        Vector2 GetStartPosition { get; }
        Vector2 GetEndPosition { get; }
    }
}