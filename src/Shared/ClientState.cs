using System;
using System.Collections.Generic;
using System.Text;


namespace TheQ.DiceRoller.Shared
{
    public enum State
    {
        MustAuthenticate = 0,
        MustAuthenticateSent = 1,
        MustIdentify = 2,
        MustIdentifySent = 3,
        Ready = 4
    }

    public class ClientState
    {
        public string Room { get; set; }
        public string Id { get; set; }
        public State CurrentState { get; set; }
    }
}