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
        [SerializeField] private float _reviveArrowFadeDuration = 0.25f;
        [SerializeField] private float _reviveArrowPulseDuration = 0.6f;
        [SerializeField] private float _reviveArrowPulseScale = 1.2f;
        [SerializeField] private float _reviveArrowFlyOffDistance = 400f;
        [SerializeField] private float _reviveArrowFlyOffDuration = 0.35f;
        [SerializeField] private Vector2 _reviveArrowFlyOffDirection = new Vector2(0f, 1f);
        [SerializeField] private float _reviveHeartDarkenDuration = 0.25f;
        
        public float MinigameStartAppearDuration => _minigameStartAppearDuration;
        public float MinigameContentScaleDuration => _minigameContentScaleDuration;
        public float DurationBeforeHearthTravel => _durationBeforeHearthTravel;
        public float HeartTravelDuration => _heartTravelDuration;
        public float CloseFadeDuration => _closeFadeDuration;
        public float ReviveArrowFadeDuration => _reviveArrowFadeDuration;
        public float ReviveArrowPulseDuration => _reviveArrowPulseDuration;
        public float ReviveArrowPulseScale => _reviveArrowPulseScale;
        public float ReviveArrowFlyOffDistance => _reviveArrowFlyOffDistance;
        public float ReviveArrowFlyOffDuration => _reviveArrowFlyOffDuration;
        public Vector2 ReviveArrowFlyOffDirection => _reviveArrowFlyOffDirection;
        public float ReviveHeartDarkenDuration => _reviveHeartDarkenDuration;
    }
}