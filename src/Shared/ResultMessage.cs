using System;
using System.Collections;
using System.Collections.Generic;


namespace Shared
{
    public class ResultMessage
    {
        public string From { get; set; }
        public DiceType DiceType { get; set; }
        public IList<int> Dice { get; set; } = new List<int>();
    }
}
