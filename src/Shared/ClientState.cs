using System;
using System.Collections.Generic;
using System.Text;


namespace TheQ.DiceRoller.Shared
{
    public enum State
    {
        MustAuthenticate = 0,
        MustIdentify = 1,
        Ready = 2
    }

    public class ClientState
    {
        public string Id { get; set; }
        public State CurrentState { get; set; }
    }
}