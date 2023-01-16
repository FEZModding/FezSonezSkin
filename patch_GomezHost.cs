
using Common;
using FezEngine.Effects;
using FezEngine.Effects.Structures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FezGame.Components
{
    public class patch_GomezHost : GomezHost
    {
        public patch_GomezHost(Game game) : base(game) { }

        public extern void orig_Update(GameTime gameTime);
        public override void Update(GameTime gameTime)
        {
            orig_Update(gameTime);

            var f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            GomezEffect effect = this.GetType().GetField("effect", f).GetValue(this) as GomezEffect;

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
    }
}
