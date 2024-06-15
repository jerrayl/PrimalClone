using Primal.Entities;

namespace Primal.Models
{
    public class Position : IPosition
    {
        public Position(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }

        public int XPosition { get; set; }
        public int YPosition { get; set; }
    }

    public class BossPartPosition : Position
    {
        public BossPartPosition(int xPosition, int yPosition) : base(xPosition, yPosition) { }
        public Direction? Direction { get; set; }
        public Corner? Corner { get; set; }
        public string BossPart { get; set; }
    }
}