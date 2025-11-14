using System.Collections.Generic;
using UnityEngine;

namespace Db.Checkpoint
{
    [CreateAssetMenu(fileName = "CheckpointData", menuName = "Data/CheckpointData")]
    public class CheckpointData : ScriptableObject
    {
        [SerializeField] private List<CheckpointVo> _checkpoints;

        public CheckpointVo GetCheckpointData(int checkpointIndex)
        {
            return _checkpoints[checkpointIndex];
        }
    }
}