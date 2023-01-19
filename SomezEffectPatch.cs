using FezEngine.Effects;
using FezEngine.Effects.Structures;
using FezGame.Components;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace Sonez
{
    public class SonezEffectPatch : GameComponent
    {
        private static IDetour GomezHostUpdateDetour;

        public SonezEffectPatch(Game game) : base(game)
        {
            GomezHostUpdateDetour = new Hook(
                typeof(GomezHost).GetMethod("Update"),
                (Action<Action<GomezHost, GameTime>, GomezHost, GameTime>)delegate(Action<GomezHost, GameTime> original, GomezHost gomezHost, GameTime gameTime)
                {
                    UpdateHooked(original, gomezHost, gameTime);
                }
            );
        }

        private void UpdateHooked(Action<GomezHost, GameTime> original, GomezHost gomezHost, GameTime gameTime)
        {
            original(gomezHost, gameTime);

            var f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            GomezEffect effect = gomezHost.GetType().GetField("effect", f).GetValue(gomezHost) as GomezEffect;

            if (effect == null) return;

            SemanticMappedVector3 blackSwap = effect.GetType().GetField("blackSwap", f).GetValue(effect) as SemanticMappedVector3;

            switch (effect.ColorSwapMode)
            {
                case ColorSwapMode.VirtualBoy:
                    blackSwap.Set(new Vector3(101f / 255f, 1f / 255f, 0f));
                    break;
                case ColorSwapMode.Gameboy:
                    blackSwap.Set(new Vector3(82f / 255f, 127f / 255f, 19f / 85f));
                    break;
                case ColorSwapMode.Cmyk:
                    blackSwap.Set(new Vector3(0f, 92f / 255f, 127f / 255f));
                    break;
                default:
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            GomezHostUpdateDetour.Dispose();
        }
    }
}
