using System;

namespace ExpertWaves
{
    public class GameManager
    {
        private double score;

        private float angle;

        private DIRECTION direction;

        private Random randomGenerator;

        public GameManager()
        {
            this.randomGenerator = new Random();
            this.Reset();
        }

        public void Reset()
        {
            this.score = 0;
            this.angle = 0;
            this.direction = DIRECTION.RIGHT;
        }

        public float getRandomAngle()
        {
            double rightAngle = 90;
            int scaler = randomGenerator.Next(0, 4);
            this.angle = (float)(scaler * rightAngle);
            switch (scaler)
            {
                case 0: //  0
                    this.direction = DIRECTION.RIGHT;
                    break;
                case 1: // 90
                    this.direction = DIRECTION.UP;
                    break;
                case 2: // 180
                    this.direction = DIRECTION.LEFT;
                    break;
                case 3: // 270
                    this.direction = DIRECTION.DOWN;
                    break;
                default: // 360
                    break;
            }

            return this.angle;
        }

        public DIRECTION getDirection()
        {
            return this.direction;
        }

        public float getAngle()
        {
            return this.angle;
        }

        public double getScore()
        {
            return this.score;
        }
    }
}
