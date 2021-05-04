using SFML.System;

namespace Aero_Hockey
{
    public class Vector2Directions
    {
        public static Vector2f left = new Vector2f(-1, 0);
        public static Vector2f right = new Vector2f(1, 0);
        public static Vector2f up = new Vector2f(0, -1);
        public static Vector2f down = new Vector2f(0, 1);

        public static Vector2f upLeft = new Vector2f(-1, -1);
        public static Vector2f upRight = new Vector2f(1, -1);
        public static Vector2f downLeft = new Vector2f(-1, 1);
        public static Vector2f downRight = new Vector2f(1, 1);
    }
}
