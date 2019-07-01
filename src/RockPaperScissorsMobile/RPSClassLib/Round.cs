using System;

namespace RPS {
    public class Round {
        public Pick HumanPick { get; set; }

        public Pick ComputerPick { get; set; }

        public Player? Winner {
            get {
                if (HumanPick == ComputerPick)
                    return null;
                else if ((HumanPick == Pick.Paper && ComputerPick == Pick.Rock) ||
                    (HumanPick == Pick.Rock && ComputerPick == Pick.Scissor) ||
                    (HumanPick == Pick.Scissor && ComputerPick == Pick.Paper))
                    return Player.Human;
                else if ((ComputerPick == Pick.Paper && HumanPick == Pick.Rock) ||
                    (ComputerPick == Pick.Rock && HumanPick == Pick.Scissor) ||
                    (ComputerPick == Pick.Scissor && HumanPick == Pick.Paper))
                    return Player.Computer;
                else
                    throw new NotImplementedException ("Pick not implemented");
            }
        }
    }
}