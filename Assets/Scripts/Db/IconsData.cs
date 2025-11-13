using System.Collections.Generic;
using UnityEngine;

namespace Db
{
    [CreateAssetMenu(fileName = "IconsData", menuName = "Data/IconsData")]
    public class IconsData : ScriptableObject
    {
        [SerializeField] private List<Sprite> _cars;
        [SerializeField] private List<Sprite> _sewerSprites;

        public Sprite GetRandomCar()
        {
            return _cars[Random.Range(0, _cars.Count)];
        }

        public Sprite GetSewerSprite(bool isClosed)
        {
            return _sewerSprites[isClosed ? 0 : 1];
        }
    }
}