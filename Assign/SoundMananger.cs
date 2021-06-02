using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RC_Framework;

namespace Assign
{
    public class SoundManager : RC_GameStateParent
    {
        private static SoundEffect music;
        private static SoundEffectInstance musicIn;
        private static SoundEffect boom;
        private static LimitSound limSound;


        public override void LoadContent()
        {
            music = Content.Load<SoundEffect>("2");
            musicIn = music.CreateInstance();

            boom = Content.Load<SoundEffect>("Sound2");
            limSound = new LimitSound(boom, 3);
        }
        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spriteBatch.End();
        }

        public static void playMusic()
        {
            musicIn.Play();
        }

        public static void stopMusic()
        {
            musicIn.Stop();
        }
        public static void playBoom()
        {
            limSound.playSoundIfOk();
        }

        /// <summary>
        /// Returns a random shot sound effect
        /// </summary>
        /// <returns></returns>

    }
}
