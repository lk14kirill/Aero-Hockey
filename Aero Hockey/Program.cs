using SFML;

namespace Aero_Hockey
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.GameCycle();
        }
    }
}
