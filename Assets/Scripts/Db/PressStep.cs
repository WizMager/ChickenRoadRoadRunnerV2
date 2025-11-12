using System;

namespace Db
{
    [Serializable]
    public struct PressStep
    {
        public int PressIndex;
        public HudEntry Values;
    }
}