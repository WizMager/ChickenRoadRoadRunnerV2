using UnityEngine;

namespace Db
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [SerializeField] private float _timeToStepMove = 0.7f;
        [SerializeField] private float _carDriveTimeBeforeBarrier = 0.2f;
        [SerializeField] private float _carDriveTimeFullPath = 1f;
        
        public float TimeToStepMove => _timeToStepMove;
        public float CarDriveTimeBeforeBarrier => _carDriveTimeBeforeBarrier;
        public float CarDriveTimeFullPath => _carDriveTimeFullPath;
    }
}