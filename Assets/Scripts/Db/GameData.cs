using UnityEngine;

namespace Db
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [SerializeField] private float _timeToStepMove = 0.7f;
        
        public float TimeToStepMove => _timeToStepMove;
    }
}