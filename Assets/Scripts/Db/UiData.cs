using UnityEngine;

namespace Db
{
    [CreateAssetMenu(fileName = "UiData", menuName = "Data/UiData")]
    public class UiData : ScriptableObject
    {
        [SerializeField] private float _minigameStartAppearDuration = 0.2f;
        [SerializeField] private float _minigameContentScaleDuration = 1f;
        [SerializeField] private float _durationBeforeHearthTravel = 0.1f;
        [SerializeField] private float _heartTravelDuration = 0.7f;
        [SerializeField] private float _closeFadeDuration = 0.3f;
        
        public float MinigameStartAppearDuration => _minigameStartAppearDuration;
        public float MinigameContentScaleDuration => _minigameContentScaleDuration;
        public float DurationBeforeHearthTravel => _durationBeforeHearthTravel;
        public float HeartTravelDuration => _heartTravelDuration;
        public float CloseFadeDuration => _closeFadeDuration;
    }
}