using UnityEngine;

namespace Db
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [SerializeField] private float _timeToStepMove = 0.7f;
        [SerializeField] private float _startCarTimer = 3f;
        [SerializeField] private float _carMoveAllPathTime = 2.5f;
        
        public float TimeToStepMove => _timeToStepMove;
        public float StartCarTimer => _startCarTimer;
        public float CarMoveAllPathTime => _carMoveAllPathTime;
    }
}