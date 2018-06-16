﻿using System;

namespace Shared
{
    public enum DiceType
    {
        D2 = 2,
        D3 = 3,
        D4 = 4,
        D6 = 6,
        D8 = 8,
        D10 = 10,
        D12 = 12,
        D20 = 20,
        D100 = 100
    }

    public class RollMessage
    {
        public int AmountOfDice { get; set; }
        public DiceType Dice { get; set; }
    }
}
