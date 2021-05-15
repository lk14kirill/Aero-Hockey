using SFML.System;
using System;
using SFML.Graphics;
using System.Threading.Tasks;

namespace Aero_Hockey
{
    abstract class CircleItem : CircleObject
    {

        public abstract void UseFeature(CircleObject circle);

        //public abstract Vector2f Spawn();

        public Vector2f RandomSpawn(Vector2u window)
        {
            Random random  = new Random();
            return new Vector2f(random.Next(1, (int)window.X + 1), random.Next(1, (int)window.Y + 1));
        }
    }
    class InvisibleStopper : CircleItem
    {
        public InvisibleStopper()
        {
            gameObject.FillColor = Color.Green;
            gameObject.Radius = 20;
        }
        public static InvisibleStopper CreateStopper()
        {
            return new InvisibleStopper();
        }
        public async override void UseFeature(CircleObject circle)
        {
            circle.ChangeBoolCanMove(false);
            await Task.Delay(3000);
            circle.ChangeBoolCanMove(true);
        }
    }
}
