using Sonez;

namespace FezGame.Services
{
    public class patch_PlayerManager : PlayerManager
    {

        public extern void orig_FillAnimations();
        public void FillAnimations()
        {
            SonezAnimationLoader.Load();

            orig_FillAnimations();
        }
    }
}
