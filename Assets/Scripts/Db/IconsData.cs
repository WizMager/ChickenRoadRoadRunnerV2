using System.Collections.Generic;
using UnityEngine;

namespace Db
{
    [CreateAssetMenu(fileName = "IconsData", menuName = "Data/IconsData")]
    public class IconsData : ScriptableObject
    {
        [SerializeField] private List<Sprite> _icons;
        
        public Sprite GetRandomIcon()
        {
            return _icons[Random.Range(0, _icons.Count)];
        }
    }
}