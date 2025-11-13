using System;
using UnityEngine;

namespace Db.Sound
{
    [Serializable]
    public struct SoundVo
    {
        public ESoundType Type;
        public AudioClip Clip;
    }
}