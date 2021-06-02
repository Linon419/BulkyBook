using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC_Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
namespace Assign
{
    class GameLevels
    {
    }
    class Dir
    {
        public static string dir = Util.findDirWithFile("airplane.png");
    }

    class Marks
    {
        static int marks;
        internal static int getMark()
        {
            return marks;
        }


        internal static void setMark(int mark)
        {
            marks = mark;
        }
    }

    // -------------------------------------------------------- Game level 0 ----------------------------------------------------------------------------------

    class GameLevel_0 : RC_GameStateParent
    {
        Texture2D texBg;
        Sprite3 bg;
        public override void LoadContent()
        {
            //font1 = Content.Load<SpriteFont>("spritefont1");
            
        }

        public override void Update(GameTime gameTime)
        {

            if (Game1.keyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            {
                gameStateManager.setLevel(1);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            bg.draw(spriteBatch);
            spriteBatch.End();
        }
    }


    //---------------------------Game level 1
    class GameLevel_1 : RC_GameStateParent
    {
        Random RandomClass;
        Texture2D texAirplane;
        Texture2D texBullet;
        Texture2D texEnemy;
        Texture2D texLaser;
        Texture2D texFlash;

        Sprite3 airplane;
        Sprite3 bullet;
        Sprite3 enemy;
        Sprite3 laser;
        Sprite3 s;
        Vector2[] animAir;
        SpriteList bulletList;
        SpriteList enemyList;
        SpriteList laserList;
        WayPointList wlist;
        Texture2D texBg;
        ScrollBackGround bg;
        int life;
        int marks;
         SoundEffect music;
         SoundEffectInstance musicIn;
         SoundEffect boom;
        LimitSound limSound;
        bool showbb;
        public override void LoadContent()
        {
            RandomClass = new Random();
            music = Content.Load<SoundEffect>("1");
            musicIn = music.CreateInstance();

            boom = Content.Load<SoundEffect>("Sound2");
            limSound = new LimitSound(boom, 3);
            font1 = Content.Load<SpriteFont>("spritefont2");
            //font1 = Content.Load<SpriteFont>("spritefont1");
            texAirplane = Util.texFromFile(graphicsDevice, Dir.dir + "player.png");
            airplane = new Sprite3(true, texAirplane, 350, 500);
            
            texBullet = Util.texFromFile(graphicsDevice, Dir.dir + "bullet.png");
            bullet = new Sprite3(false, texBullet, 10, 10);

            texEnemy = Util.texFromFile(graphicsDevice, Dir.dir + "enemy.png");
            enemy = new Sprite3(false, texEnemy, 10, 10);

            texLaser = Util.texFromFile(graphicsDevice, Dir.dir + "laser.png");
            laser = new Sprite3(false, texLaser, 500, 100);

            texFlash = Util.texFromFile(graphicsDevice, Dir.dir + "flash.png");
            s = new Sprite3(false, texFlash, 0, 0);
            texBg = Util.texFromFile(graphicsDevice, Dir.dir + "sea.jpg");
            bg = new ScrollBackGround(texBg, new Rectangle(0, 0, 800, 600), new Rectangle(0, 0, 800, 600), 1.0f, 1);


            bulletList = new SpriteList();
            laserList = new SpriteList();
            enemyList = new SpriteList();
            enemyList.addSpriteReuse(enemy);
            marks = 0;
            life = 3;

            for (int i = 0; i < 20; i++)
            {
                enemy = new Sprite3(true, texEnemy, (float)( 1+RandomClass.NextDouble() * 700), -(float)(20 + RandomClass.NextDouble() * 1000));
                enemy.setDeltaSpeed(new Vector2(0,(float)(1 + RandomClass.NextDouble()))); // break up monotony with random speed
                //enemy.setWidthHeight(48, 60)
                enemy.setMoveAngleDegrees(90);
                enemy.setMoveSpeed(2.1f);
                enemyList.addSpriteReuse(enemy);
            }
            airplane.setWidthHeight(127/2,108/2);
            airplane.setXframes(3);
            airplane.setYframes(1);
            airplane.setBB(0,0,127, 108);
            animAir = new Vector2[3];
            animAir[0].X = 0; animAir[0].Y = 0;
            animAir[1].X = 1; animAir[1].Y = 0;
            animAir[2].X = 2; animAir[2].Y = 0;
    
            airplane.setAnimationSequence(animAir, 1, 1, 0);
            //airplane.setAnimFinished(2); // make it inactive and invisible
            airplane.animationStart();

        }

        public override void Update(GameTime gameTime)
        {

            if ((Game1.keyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))||(life<=0))
            {
                musicIn.Stop();
                gameStateManager.setLevel(2);
            }
            if (Game1.keyState.IsKeyDown(Keys.F1) && !Game1.prevKeyState.IsKeyDown(Keys.F1))
            {
                gameStateManager.pushLevel(7);
            }
            if (Game1.keyState.IsKeyDown(Keys.P) && !Game1.prevKeyState.IsKeyDown(Keys.P))
            {
                gameStateManager.pushLevel(5);
            }
            if (Game1.keyState.IsKeyDown(Keys.J) && Game1.prevKeyState.IsKeyUp(Keys.J))
            {
                createBullet(airplane.getPos());
            }

            if (Game1.keyState.IsKeyDown(Keys.W))
            {
                airplane.setDeltaSpeed(new Vector2(0, -3));
                airplane.moveByDeltaXY();
            }

            if (Game1.keyState.IsKeyDown(Keys.S))
            {
                airplane.setDeltaSpeed(new Vector2(0, 3));
                airplane.moveByDeltaXY();
            }
            if (Game1.keyState.IsKeyDown(Keys.A))
            {
                airplane.setDeltaSpeed(new Vector2(-3, 0));
                airplane.setAnimationSequence(animAir, 0, 0, 0);
                //airplane.setAnimFinished(2); // make it inactive and invisible
                airplane.animationStart();
                airplane.moveByDeltaXY();
            }
            else if (Game1.keyState.IsKeyDown(Keys.D))
            {
                airplane.setDeltaSpeed(new Vector2(3, 0));
                airplane.setAnimationSequence(animAir, 2, 2, 1);
                //airplane.setAnimFinished(2); // make it inactive and invisible
                airplane.animationStart();
                airplane.moveByDeltaXY();
            }
            else
            {
                airplane.setAnimationSequence(animAir, 1, 1, 0);
                airplane.animationStart();
            }
                //airplane.animationStart();
                bulletList.moveDeltaXY();
            //airplane.animationTick(gameTime);
            enemyList.moveDeltaXY();
            laser.moveTo(airplane.getPos(), 2, true);
            musicIn.Play();
            for (int i = 0; i < enemyList.count(); i++)
            {
                
                Sprite3 a = enemyList.getSprite(i);
                if (a == null) continue;
                if (airplane.collision(a))
                {
                    a.setVisible(false);
                    a.setPos(0, 0);
                    life--;
                }

                int colision = bulletList.collisionAA(a);
                if (colision == -1) continue;
                Sprite3 c = bulletList.getSprite(colision);
                c.setActive(false);
                a.setActive(false);
                createExplosion(a.getPosX(), a.getPosY());
                Marks.setMark(marks++);

            }
            if (Game1.keyState.IsKeyDown(Keys.B) && Game1.prevKeyState.IsKeyUp(Keys.B)) { showbb = !showbb; }

            bg.Update(gameTime);
            s.animationTick(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            bg.Draw(spriteBatch);
            airplane.draw(spriteBatch);
            bulletList.drawActive(spriteBatch);
            //laser.draw(spriteBatch);
            laserList.drawActive(spriteBatch);
            enemyList.drawActive(spriteBatch);
           // enemy.draw(spriteBatch);
            spriteBatch.DrawString(font1, "Life: " + life, new Vector2(340, 20), Color.Brown);
            spriteBatch.DrawString(font1, "Marks: " + Marks.getMark(), new Vector2(540, 20), Color.Brown);
            s.draw(spriteBatch);
            spriteBatch.DrawString(font1, "Level 1 Press N to next Level ", new Vector2(50, 50), Color.Brown);
            if (showbb)
            {
                bulletList.drawInfo(spriteBatch, Color.Red, Color.Red);
                //enemyList.drawInfo(spriteBatch, Color.Red, Color.Red);
                //enemyHeliList.drawInfo(spriteBatch, Color.Red, Color.Red);
                airplane.drawInfo(spriteBatch, Color.Red, Color.Black);
                //enemy2List.drawInfo(spriteBatch, Color.Red, Color.Black);
                enemyList.drawInfo(spriteBatch, Color.Red, Color.Black);

            }
            // boomList.drawActive(spriteBatch);
            spriteBatch.End();
        }

        private void createBullet(Vector2 pos){
               
            bullet = new Sprite3(true, texBullet, 10, 10);
            bullet.setWidthHeight(32, 32);
            bullet.setBBToTexture();
            bullet.active = true;
            bullet.setPos(pos);
            bullet.setDeltaSpeed(new Vector2(0, -5));
            bulletList.addSpriteReuse(bullet);              

        }

        private void createEnemyBullet(Sprite3 s)
        {
            laser = new Sprite3(true, texLaser, s.getPosX(), s.getPosY());
            laser.setWidthHeight(26, 74);
            laser.setBBToTexture();
            laserList.addSpriteReuse(laser);

            //laser.setPos(x, y);

        }

        void createExplosion(float x, float y)
        {
            limSound.playSoundIfOk();
            float scale = 0.6f;
            int xoffset = -5;
            int yoffset = 3;
            s = new Sprite3(true, texFlash, x + xoffset, y + yoffset);


            s.setXframes(4);
            s.setYframes(2);
            //s.setWidthHeight(896 / 7 * scale, 384 / 3 * scale);
            s.setWidthHeight(64, 64);

            Vector2[] anim = new Vector2[8];
            anim[0].X = 0; anim[0].Y = 0;
            anim[1].X = 1; anim[1].Y = 0;
            anim[2].X = 2; anim[2].Y = 0;
            anim[3].X = 3; anim[3].Y = 0;            
            anim[4].X = 0; anim[4].Y = 1;
            anim[5].X = 1; anim[5].Y = 1;
            anim[6].X = 2; anim[6].Y = 1;
            anim[7].X = 3; anim[7].Y = 1;
           
            s.setAnimationSequence(anim, 0, 7, 4);
            s.setAnimFinished(2); // make it inactive and invisible
            s.animationStart();

            

        }

        public static float ConvertToAngleAim(Vector2 v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }





    }

    //----------------------Game level2
    class GameLevel_2 : RC_GameStateParent
    {
        Random RandomClass;
        Texture2D texAirplane;
        Texture2D texBullet;
        Texture2D texEnemy;
        Texture2D texEnemyBullet;
        Texture2D texFlash;
        Texture2D texEnemyHeli;
        Texture2D texEnemyRed;
        Texture2D texEnemy2;
        Texture2D texCoin;
        Texture2D texTerraria;
        Texture2D texBg;
        Texture2D texLife;
        Texture2D texShield;
        Texture2D texShieldAir;

        ScrollBackGround bg;
        Sprite3 airplane;
        Sprite3 shieldAir;
        Sprite3 shield;
        Sprite3 bullet;
        Sprite3 enemy;
        Sprite3 EnemyBullet;
        Sprite3 s;
        Sprite3 enemyHeli;
        Sprite3 enemyRed;
        Sprite3 enemy2;
        Sprite3 coin;
        Sprite3 heart;

        Vector2[] animAir;
        Vector2[] animCoin;
        Vector2[] animEnemyHeli;
        Vector2[] animEnemyRed;
        Vector2[] animEnemy2;
        SpriteList bulletList;
        SpriteList enemyList;
        SpriteList enemyHeliList;
        SpriteList enemyRedList;
        SpriteList enemy2List;
        SpriteList coinList;
        SpriteList shieldList;
        SpriteList enemyBulletList;
        SpriteList laserList;
        WayPointList wlist;
        SpriteFont font2;
        public float _timer;
        int enemyNum = 0;
        int life = 3;
        int test;
        int k = 0;
        public bool showbb;

        SoundEffect music;
        SoundEffectInstance musicIn;
        SoundEffect boom;
        LimitSound limSound;
        int tick = 0;
        RC_RenderableList rlist = null;
        public float ShootingTimer = 1f;
        TextRenderableFade temp2;
        public override void LoadContent()
        {
            RandomClass = new Random();
           // temp2 = new TextRenderableFade("",s.getPos(),font2,Color.Red,,Color.White,0);
            music = Content.Load<SoundEffect>("1");
            musicIn = music.CreateInstance();

            boom = Content.Load<SoundEffect>("Sound2");
            limSound = new LimitSound(boom, 3);
            font2 = Content.Load<SpriteFont>("spritefont2");
            //font1 = Content.Load<SpriteFont>("spritefont1");
            texAirplane = Util.texFromFile(graphicsDevice, Dir.dir + "player.png");
            airplane = new Sprite3(true, texAirplane, 350, 500);

            texBullet = Util.texFromFile(graphicsDevice, Dir.dir + "bullet.png");
            bullet = new Sprite3(false, texBullet, 10, 10);

            texEnemy = Util.texFromFile(graphicsDevice, Dir.dir + "enemy.png");
           // enemy = new Sprite3(false, texEnemy, 10, 10);

            texEnemyBullet = Util.texFromFile(graphicsDevice, Dir.dir + "bullet2.png");
            EnemyBullet = new Sprite3(false, texEnemyBullet, 500, 100);

            texFlash = Util.texFromFile(graphicsDevice, Dir.dir + "flash.png");
            s = new Sprite3(false, texFlash, 0, 0);

            texEnemyHeli = Util.texFromFile(graphicsDevice, Dir.dir + "heli.png");
            enemyHeli = new Sprite3(false, texEnemyHeli, 10, 10);

            texEnemyRed = Util.texFromFile(graphicsDevice, Dir.dir + "enemyRed.png");
            enemyRed = new Sprite3(false, texEnemyRed, 10, 10);

            texEnemy2 = Util.texFromFile(graphicsDevice, Dir.dir + "enemy2.png");
            enemy2 = new Sprite3(false, texEnemy2, 10, 10);

            texCoin = Util.texFromFile(graphicsDevice, Dir.dir + "coin.png");
            coin = new Sprite3(false, texCoin, 10, 10);

            texShield = Util.texFromFile(graphicsDevice, Dir.dir + "shield.png");
          //  shield = new Sprite3(false, texShield, 10, 10);

            texBg = Util.texFromFile(graphicsDevice, Dir.dir + "sea.jpg");
            bg = new ScrollBackGround(texBg, new Rectangle(0, 0, 800, 600), new Rectangle(0, 0, 800, 600), 1.0f, 1);

            texLife = Util.texFromFile(graphicsDevice, Dir.dir + "life.png");
            heart = new Sprite3(true, texLife, 300, 20);

            texTerraria = Util.texFromFile(graphicsDevice, Dir.dir + "terraria.png");

            texShieldAir = Util.texFromFile(graphicsDevice, Dir.dir + "shieldAir.png");
            shieldAir = new Sprite3(false, texShieldAir, 10, 10);

            bulletList = new SpriteList();
            laserList = new SpriteList();
            enemyList = new SpriteList();
            enemyBulletList = new SpriteList();
            enemyHeliList = new SpriteList();
            enemyRedList = new SpriteList();
            enemy2List = new SpriteList();
            coinList = new SpriteList();
            shieldList = new SpriteList();
            life = 3;
            rlist = new RC_RenderableList();
            shield = new Sprite3(true, texShield, 500,-200);
            shield.setDeltaSpeed(new Vector2(0, 1f)); // break up monotony with random speed
                //enemy.setWidthHeight(48, 48);
                //enemy.setMoveAngleDegrees(90);
                //enemy.setMoveSpeed(2.1f);
                
            Marks.setMark(0);
            for (int i = 0; i < 1; i++)
            {
                enemy = new Sprite3(true, texEnemy, (float)(1 + RandomClass.NextDouble() * 700), -(float)(25 + RandomClass.NextDouble() * 2000));
                enemy.setDeltaSpeed(new Vector2(0, (float)(1 + RandomClass.NextDouble()))); // break up monotony with random speed
                enemy.setWidthHeight(48, 48);
                //enemy.setMoveAngleDegrees(90);
                //enemy.setMoveSpeed(2.1f);
                if (enemy.active == true)
                    enemyList.addSpriteReuse(enemy);
            }


            for (int i = 0; i < 5; i++)
            {
                enemyHeli = new Sprite3(true, texEnemyHeli, 300,-200-i*50);
                enemyHeli.setWidthHeight(305/7,420/7);
                enemyHeli.setXframes(3);
                enemyHeli.setYframes(1);
                enemyHeli.setBB(0, 0, 305, 420);
                animEnemyHeli = new Vector2[3];
                animEnemyHeli[0].X = 0; animEnemyHeli[0].Y = 0;
                animEnemyHeli[1].X = 1; animEnemyHeli[1].Y = 0;
                animEnemyHeli[2].X = 2; animEnemyHeli[2].Y = 0;
                enemyHeli.setAnimationSequence(animEnemyHeli, 0, 2, 4);
               // enemyHeli.setAnimFinished(2); // make it inactive and invisible
                enemyHeli.animationStart();
                enemyHeli.setDisplayAngleDegrees(0);
                enemyHeli.setMoveAngleDegrees(90);
                enemyHeli.setMoveSpeed(5.1f);
                enemyHeliList.addSpriteReuse(enemyHeli);

            }

            for (int i = 0; i<5; i++)
            {
                enemyRed = new Sprite3(true, texEnemyRed, 200 + i*80, -200);

                enemyRed.setWidthHeight(79/2, 68/2);
                enemyRed.setXframes(6);
                enemyRed.setBB(0,0,79,68);
                enemyRed.setYframes(1);
                animEnemyRed = new Vector2[6];
                animEnemyRed[0].X = 0; animEnemyRed[0].Y = 0;
                animEnemyRed[1].X = 1; animEnemyRed[1].Y = 0;
                animEnemyRed[2].X = 2; animEnemyRed[2].Y = 0;
                animEnemyRed[3].X = 3; animEnemyRed[3].Y = 0;
                animEnemyRed[4].X = 4; animEnemyRed[4].Y = 0;
                animEnemyRed[5].X = 5; animEnemyRed[5].Y = 0;
                enemyRed.setAnimationSequence(animEnemyRed, 0, 5, 6);
                //enemyRed.setAnimFinished(2); // make it inactive and invisible
                enemyRed.animationStart();
                enemyRed.setDisplayAngleDegrees(0);
                enemyRed.setMoveAngleDegrees(90);
                enemyRed.setMoveSpeed(2.1f);
                enemyRedList.addSpriteReuse(enemyRed);
            }

            for (int i = 0; i < 5; i++)
            {
                coin = new Sprite3(true, texCoin, 100 + (float)RandomClass.NextDouble() * 700, -200);
                coin.setWidthHeight(47, 68);
                coin.setXframes(6);
                coin.setYframes(1);
                animCoin = new Vector2[6];
                animCoin[0].X = 0; animCoin[0].Y = 0;
                animCoin[1].X = 1; animCoin[1].Y = 0;
                animCoin[2].X = 2; animCoin[2].Y = 0;
                animCoin[3].X = 3; animCoin[3].Y = 0;
                animCoin[4].X = 4; animCoin[4].Y = 0;
                animCoin[5].X = 5; animCoin[5].Y = 0;
                coin.setAnimationSequence(animCoin, 0, 5, 4);
                //coin.setAnimFinished(2); // make it inactive and invisible
                coin.animationStart();
                coin.setDisplayAngleDegrees(0);
                coin.setMoveAngleDegrees(90);
                coin.setMoveSpeed(2.1f);
                coinList.addSpriteReuse(coin);
            }

            for (int i = 0; i < 5; i++)
            {
                enemy2 = new Sprite3(true, texEnemy2, -200-i*100, -200 - i * 100);
                enemy2.setWidthHeight(86, 100);
                //coin.setAnimFinished(2); // make it inactive and invisible
                enemy2.setDisplayAngleDegrees(0);
                enemy2.setMoveAngleDegrees(45);
                enemy2.setMoveSpeed(2.1f);
                enemy2List.addSpriteReuse(enemy2);
            }

            airplane.setWidthHeight(127 / 2, 108 / 2);
            airplane.setXframes(3);
            airplane.setYframes(1);
            airplane.setBB(0,0,127,108);
            animAir = new Vector2[3];
            animAir[0].X = 0; animAir[0].Y = 0;
            animAir[1].X = 1; animAir[1].Y = 0;
            animAir[2].X = 2; animAir[2].Y = 0;
            
        }

        public override void Update(GameTime gameTime)
        {
            if (Game1.keyState.IsKeyDown(Keys.P) && !Game1.prevKeyState.IsKeyDown(Keys.P))
            {
                gameStateManager.pushLevel(5);
            }
            if (Game1.keyState.IsKeyDown(Keys.F1) && !Game1.prevKeyState.IsKeyDown(Keys.F1))
            {
                gameStateManager.pushLevel(7);
            }

            if (Game1.keyState.IsKeyDown(Keys.B) && Game1.prevKeyState.IsKeyUp(Keys.B)) { showbb = !showbb; }
            if (Game1.keyState.IsKeyDown(Keys.J) && Game1.prevKeyState.IsKeyUp(Keys.J))
            {
                createBullet2(airplane.getPos());
            }

            if (Game1.keyState.IsKeyDown(Keys.W))
            {
                airplane.setDeltaSpeed(new Vector2(0, -3));
                airplane.moveByDeltaXY();
            }

            if (Game1.keyState.IsKeyDown(Keys.S))
            {
                airplane.setDeltaSpeed(new Vector2(0, 3));
                airplane.moveByDeltaXY();
            }
            if (Game1.keyState.IsKeyDown(Keys.A))
            {
                airplane.setDeltaSpeed(new Vector2(-3, 0));
                airplane.setAnimationSequence(animAir, 0, 0, 0);
                //airplane.setAnimFinished(2); // make it inactive and invisible
                airplane.animationStart();
                airplane.moveByDeltaXY();
            }
            else if (Game1.keyState.IsKeyDown(Keys.D))
            {
                airplane.setDeltaSpeed(new Vector2(3, 0));
                airplane.setAnimationSequence(animAir, 2, 2, 1);
                //airplane.setAnimFinished(2); // make it inactive and invisible
                airplane.animationStart();
                airplane.moveByDeltaXY();
            }
            else
            {
                airplane.setAnimationSequence(animAir, 1, 1, 0);
                airplane.animationStart();
            }

            if (life <= 0)
            {
                //SoundManager.stopMusic();
                musicIn.Stop();
               gameStateManager.pushLevel(8);
            }

            bulletList.moveDeltaXY();
            shield.moveByDeltaXY();
            enemyList.moveDeltaXY();

            //if (Marks.getMark() > 2000)
            //{
            //    test++;
            //    temp2 = new TextRenderableFade("Boss Coming", new Vector2(300, 400), font2, Color.Red, Color.Transparent, 220);
            //    if (test > 360)
            //    {
            //        musicIn.Stop();
            //        gameStateManager.getLevel(3).LoadContent();
            //        gameStateManager.pushLevel(3);
            //    }


            //}
            musicIn.Play();
            // my bullet shoot enemy explosion
            enemyDie(enemyList, gameTime);
            enemyDie(enemyHeliList, gameTime);
            enemyDie(enemyRedList, gameTime);
            enemyDie(enemy2List, gameTime);
            enemyDie(enemyBulletList, gameTime);

            bulletList.removeIfOutside(Game1.screenRect);
          

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            //reset position
            foreach (Sprite3 single in enemyList)
            {
                if (single.getPosY()>600 || single.visible == false)
                {
                    single.setVisible(true);
                    single.setPos((float)(1 + RandomClass.NextDouble() * 700), -(float)(25 + RandomClass.NextDouble() * 2000));
                    single.setDeltaSpeed(new Vector2(0, (float)(1 + RandomClass.NextDouble()))); // break up monotony with random speed
                                                                                                 //enemy.setWidthHeight(48, 60)
                    single.setMoveAngleDegrees(90);
                    single.setMoveSpeed(2.1f);
                }
            }

            foreach (Sprite3 single in enemyHeliList)
            {
                if (single.getPosY() > 600|| single.visible==false)
                {
                    single.setVisible(true);
                    single.setPos((float)(1 + RandomClass.NextDouble() * 700), -(float)(25 + RandomClass.NextDouble() * 2000));                                                                                                 //enemy.setWidthHeight(48, 60)
                    single.setMoveAngleDegrees(90);
                    single.setMoveSpeed(2.1f);
                }
            }

            //foreach (Sprite3 single in shieldList)
            //{
            //    if (single.getPosY() > 600 || single.visible == false)
            //    {
            //        single.setVisible(true);
            //        single.setPos((float)(1 + RandomClass.NextDouble() * 700), -(float)(250 + RandomClass.NextDouble() * 2000));                                                                                                 //enemy.setWidthHeight(48, 60)
            //        single.setMoveAngleDegrees(90);
            //        single.setMoveSpeed(2.1f);
            //    }
            //}

            if (shield.getPosY() > 600 || shield.visible == false)
            {
                shield.setVisible(true);
                shield.setPos((float)(1 + RandomClass.NextDouble() * 700), -(float)(250 + RandomClass.NextDouble() * 2000));                                                                                                 //enemy.setWidthHeight(48, 60)
                shield.setMoveAngleDegrees(90);
                shield.setMoveSpeed(2.1f);
            }
            //enemyRedList.
            foreach (Sprite3 single in enemyRedList)
            {
                if (single.getPosY() > 600 || single.visible == false)
                {
                    single.setVisible(true);
                    single.setPos((float)(1 + RandomClass.NextDouble() * 700), -(float)(25 + RandomClass.NextDouble() * 2000));                                                                                                 //enemy.setWidthHeight(48, 60)
                    single.setMoveAngleDegrees(90);
                    single.setMoveSpeed(2.1f);
                }
            }

            foreach (Sprite3 single in enemy2List)
            {
                if (single.getPosY() > 600 || single.visible == false)
                {
                    single.setVisible(true);
                    single.setPos(-200 - k * 100, -200 - k * 100);                                                                                                 //enemy.setWidthHeight(48, 60)
                    single.setMoveAngleDegrees(45);
                    single.setMoveSpeed(2.1f);
                    //enemy2List.addSpriteReuse(single);
                    k=k+3;
                }
            }

            if (shield.collision(airplane))
            {
                shield.setVisible(false);
                shield.setPos(0, 0);
                life++;
                //(float)GameTime.ElapsedGameTime.TotalSeconds；
                if (tick < 1)
                {
                    //shieldAir.setPos(airplane.getPos());
                    shieldAir.setVisible(true);
                    shieldAir.setColor(new Color(256, 256, 256, 120));
                }

            }
           
            
            if (shieldAir.getVisible() == true)
            {
                tick = tick + 1;
            }
            
                if (tick >= 36)
                {
                    tick = 0;
                    shieldAir.setVisible(false);
                    //airplane.setColor(new Color(256, 256, 256, 0));
                }
            shieldAir.setPos(new Vector2(airplane.getPosX()-50,airplane.getPosY()-50));

            //for (int i = 0; i < 5; i++)
            //{
            //    enemy2 = new Sprite3(true, texEnemy2, -200 - i * 100, -200 - i * 100);
            //    enemy2.setWidthHeight(86, 100);
            //    //coin.setAnimFinished(2); // make it inactive and invisible
            //    enemy2.setDisplayAngleDegrees(0);
            //    enemy2.setMoveAngleDegrees(45);
            //    enemy2.setMoveSpeed(2.1f);
            //    enemy2List.addSpriteReuse(enemy2);
            //}




            //enemy shoot bullet
            if ( _timer >= ShootingTimer)
                {
                    for (int i = 0; i < enemyList.count(); i++)
                    {
                        Sprite3 a = enemyList.getSprite(i);
                        if (a == null) continue;
                    if (a.getPosY() > 0 && a.active == true && a.visible == true)
                        createEnemyBullet(a);
                        _timer = 0;
                    }
                }

            //Console.WriteLine(i);
            bg.Update(gameTime);
            enemyBulletList.moveDeltaXY();
            enemyHeliList.moveByAngleSpeed();
            enemy2List.moveByAngleSpeed();
            enemyRedList.moveByAngleSpeed();
            //shieldList.moveDeltaXY(); 
            coinList.moveByAngleSpeed();
            s.animationTick(gameTime);
            enemyHeli.animationTick(gameTime);
            enemyHeliList.animationTick(gameTime);
            enemyRedList.animationTick(gameTime);
            coinList.animationTick(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            bg.Draw(spriteBatch);
            airplane.draw(spriteBatch);
            //temp2.Draw(spriteBatch);
            //shieldList.drawActive(spriteBatch);
            shield.draw(spriteBatch);
            enemy2List.drawActive(spriteBatch);
            enemyRedList.drawActive(spriteBatch);
            enemyHeliList.drawActive(spriteBatch);
            bulletList.drawActive(spriteBatch);
            enemyList.drawActive(spriteBatch);         
            enemyBulletList.drawActive(spriteBatch);
            s.draw(spriteBatch);
            heart.draw(spriteBatch);
            spriteBatch.DrawString(font1, " "+life, new Vector2(340, 20), Color.Brown);
            spriteBatch.DrawString(font1, "Marks: " + Marks.getMark(), new Vector2(540, 20), Color.Brown);
            shieldAir.draw(spriteBatch);
            spriteBatch.DrawString(font1, "Level 2 ", new Vector2(50, 20), Color.Brown);
            if (showbb)
            {
                bulletList.drawInfo(spriteBatch, Color.Red, Color.Red);
                enemyRedList.drawInfo(spriteBatch, Color.Red, Color.Red);
                enemyHeliList.drawInfo(spriteBatch, Color.Red, Color.Red);
                airplane.drawInfo(spriteBatch, Color.Red, Color.Black);
                enemy2List.drawInfo(spriteBatch, Color.Red, Color.Black);
                enemyList.drawInfo(spriteBatch, Color.Red, Color.Black);

            }
            if (Marks.getMark() > 2000)
            {
                test++;
                //temp2 = new TextRenderableFade("Boss Coming", new Vector2(300, 400), font2, Color.Red, Color.Transparent, 220);
                spriteBatch.DrawString(font2, "Boss Coming ", new Vector2(300, 300), Color.Brown);
                if (test > 106)
                {
                    musicIn.Stop();
                    gameStateManager.getLevel(3).LoadContent();
                    gameStateManager.pushLevel(3);
                }


            }
            spriteBatch.End();
        }

        private void createBullet(Vector2 pos)
        {

            bullet = new Sprite3(true, texBullet, 10, 10);
            bullet.setWidthHeight(32, 32);
            bullet.setBBToTexture();
            bullet.setPos(pos);
            bullet.setDeltaSpeed(new Vector2(0, -5));
            bulletList.addSpriteReuse(bullet);

        }

        private void createBullet2(Vector2 pos)
        {
            Sprite3 bullet2 = new Sprite3(true, texTerraria, pos.X+15, pos.Y);
            Sprite3 bullet3 = new Sprite3(true, texTerraria, pos.X + 15, pos.Y);
            bullet = new Sprite3(true, texTerraria, pos.X + 15, pos.Y);
            bullet.setWidthHeight(14, 28);
            bullet.setBBToTexture();
            //bullet.setPos(pos);
            //bullet2.setPos(pos);
            //bullet3.setPos(pos);
            bullet2.setDisplayAngleDegrees(-30f);
            bullet3.setDisplayAngleDegrees(30f);
            bullet.setDeltaSpeed(new Vector2(0, -5));
            bullet2.setDeltaSpeed(new Vector2(-3, -5));
            bullet3.setDeltaSpeed(new Vector2(3, -5));
            bulletList.addSprite(bullet);
            bulletList.addSprite(bullet2);
            bulletList.addSprite(bullet3);
        }

        private void createEnemyBullet(Sprite3 s)
        {            
            EnemyBullet = new Sprite3(true, texEnemyBullet,s.getPosX()+7, s.getPosY()+10);
            EnemyBullet.setBB(0,0,9,9);
            //EnemyBullet.setPos(s.getPos());
            EnemyBullet.setDeltaSpeed(new Vector2(0, 3f));
            enemyBulletList.addSpriteReuse(EnemyBullet);                              
        }

        void createExplosion(float x, float y)
        {
            limSound.playSoundIfOk();
            //SoundManager.playBoom();
            float scale = 0.6f;
            int xoffset = -5;
            int yoffset = 3;
            s = new Sprite3(true, texFlash, x + xoffset, y + yoffset);


            s.setXframes(4);
            s.setYframes(2);
            //s.setWidthHeight(896 / 7 * scale, 384 / 3 * scale);
            s.setWidthHeight(64, 64);

            Vector2[] anim = new Vector2[8];
            anim[0].X = 0; anim[0].Y = 0;
            anim[1].X = 1; anim[1].Y = 0;
            anim[2].X = 2; anim[2].Y = 0;
            anim[3].X = 3; anim[3].Y = 0;
            anim[4].X = 0; anim[4].Y = 1;
            anim[5].X = 1; anim[5].Y = 1;
            anim[6].X = 2; anim[6].Y = 1;
            anim[7].X = 3; anim[7].Y = 1;

            s.setAnimationSequence(anim, 0, 7, 4);
            s.setAnimFinished(2); // make it inactive and invisible
            s.animationStart();
        }

        void enemyDie(SpriteList sl, GameTime gameTime)
        {
            for (int i = 0; i < sl.count(); i++)
            {
                Sprite3 b = sl.getSprite(i);
                if (b == null) continue;
                 if (b.collision(airplane))
                {
                    createExplosion(airplane.getPosX()-5, airplane.getPosY()-6);
                    b.setPos(0, 0);
                    b.setActive(false);
                    b.setVisible(false);
                    if (life > 0)
                    {
                        life = life - 1;
                        //(float)GameTime.ElapsedGameTime.TotalSeconds；
                    }
                    
                }
                //createEnemyBullet(b);
                int colision = bulletList.collisionAA(b);
                if (colision == -1) continue;
                Sprite3 c = bulletList.getSprite(colision);
                c.setActive(false);
                b.setVisible(false);
                createExplosion(b.getPosX(), b.getPosY());
                Marks.setMark(Marks.getMark() + 100);

            }
        }

    }
    //-----------------Game level3
    class GameLevel_3 : RC_GameStateParent
    {
        Texture2D texBoss;
        Sprite3 boss;
        Texture2D texBullet;
        Texture2D texBg;
        Sprite3 bullet;
        Texture2D texBullet2;
        Sprite3 bossBullet;
        SpriteList bulletList;
        SpriteList bulletList2;
        WayPointList wlist;
        ScrollBackGround bg;
        Sprite3 airplane;
        Texture2D texAirplane;
        Vector2[] animAir;
        Sprite3 s;
        Texture2D texFlash;
        public float _timer;
        public float ShootingTimer = 1f;
        int life = 1;
        HealthBar hb;
        SoundEffect music;
        SoundEffectInstance musicIn;
        SoundEffect boom;
        LimitSound limSound;
        int test;
        bool showbb;
        public override void LoadContent()
        {
            test = 0;
            //font1 = Content.Load<SpriteFont>("spritefont1");
            music = Content.Load<SoundEffect>("2");
            musicIn = music.CreateInstance();

            boom = Content.Load<SoundEffect>("Sound2");
            limSound = new LimitSound(boom, 3);
            texBoss = Util.texFromFile(graphicsDevice, Dir.dir + "boss_green.png");
            texBullet = Util.texFromFile(graphicsDevice, Dir.dir + "laser.png");
            texFlash = Util.texFromFile(graphicsDevice, Dir.dir + "flash.png");
            s = new Sprite3(false, texFlash, 0, 0);
            texBg = Util.texFromFile(graphicsDevice, Dir.dir + "sea.jpg");
            bg = new ScrollBackGround(texBg, new Rectangle(0, 0, 800, 600), new Rectangle(0, 0, 800, 600), 1.0f, 1);
            //font1 = Content.Load<SpriteFont>("spritefont1");
            texAirplane = Util.texFromFile(graphicsDevice, Dir.dir + "player.png");
            airplane = new Sprite3(true, texAirplane, 350, 500);
            texBullet2 = Util.texFromFile(graphicsDevice, Dir.dir + "bullet.png");
            bullet = new Sprite3(false, texBullet2, 10, 10);
            bulletList = new SpriteList();
            bulletList2 = new SpriteList();
            bossBullet = new Sprite3(true, texBullet, 0, -10);
            boss = new Sprite3(true, texBoss, 350, 50);
            //boss.setWidthHeight();
            WayPoint wp = new WayPoint(50,50, 2);
            wlist = new WayPointList();
            wlist.add(wp);
            wp = new WayPoint(600,50, 2);
            wlist.add(wp);
            boss.wayList = wlist;
            wlist.wayFinished = 2;
            boss.moveToStartOfWayPoints();

            airplane.setWidthHeight(127 / 2, 108 / 2);
            airplane.setXframes(3);
            airplane.setYframes(1);
            airplane.setBB(0, 0, 127, 108);
            animAir = new Vector2[3];
            animAir[0].X = 0; animAir[0].Y = 0;
            animAir[1].X = 1; animAir[1].Y = 0;
            animAir[2].X = 2; animAir[2].Y = 0;
            life = 3;
            hb = new HealthBar(Color.Green, Color.Black, Color.Red, true);
            hb.setHp(100);
            hb.currentHp = 100;
            //hb.setWidthHeight(100, 10);
            hb.setPos(100, 100);
            hb.bounds = new Rectangle(20, 10, 200, 20);
        }

        public override void Update(GameTime gameTime)
        {
            if (Game1.keyState.IsKeyDown(Keys.P) && !Game1.prevKeyState.IsKeyDown(Keys.P))
            {
                gameStateManager.pushLevel(3);
            }
            if (Game1.keyState.IsKeyDown(Keys.F1) && !Game1.prevKeyState.IsKeyDown(Keys.F1))
            {
                gameStateManager.pushLevel(7);
            }
            //bulletList.animationTick(gameTime);
            //if (Game1.keyState.IsKeyDown(Keys.N) && !Game1.prevKeyState.IsKeyDown(Keys.N))
            //{
            //    gameStateManager.setLevel(1);
            //}

            if (Game1.keyState.IsKeyDown(Keys.B) && Game1.prevKeyState.IsKeyUp(Keys.B)) { showbb = !showbb; }
            if (Game1.keyState.IsKeyDown(Keys.J) && Game1.prevKeyState.IsKeyUp(Keys.J))
            {
                createBullet(airplane.getPos());
            }

            if (Game1.keyState.IsKeyDown(Keys.W))
            {
                airplane.setDeltaSpeed(new Vector2(0, -3));
                airplane.moveByDeltaXY();
            }

            if (Game1.keyState.IsKeyDown(Keys.S))
            {
                airplane.setDeltaSpeed(new Vector2(0, 3));
                airplane.moveByDeltaXY();
            }
            if (Game1.keyState.IsKeyDown(Keys.A))
            {
                airplane.setDeltaSpeed(new Vector2(-3, 0));
                airplane.setAnimationSequence(animAir, 0, 0, 0);
                //airplane.setAnimFinished(2); // make it inactive and invisible
                airplane.animationStart();
                airplane.moveByDeltaXY();
            }
            else if (Game1.keyState.IsKeyDown(Keys.D))
            {
                airplane.setDeltaSpeed(new Vector2(3, 0));
                airplane.setAnimationSequence(animAir, 2, 2, 1);
                //airplane.setAnimFinished(2); // make it inactive and invisible
                airplane.animationStart();
                airplane.moveByDeltaXY();
            }
            else
            {
                airplane.setAnimationSequence(animAir, 1, 1, 0);
                airplane.animationStart();
            }
            //SoundManager.playMusic();
            musicIn.Play();

            if (life <= 0)
            {
                musicIn.Stop();
                //SoundManager.stopMusic();
               gameStateManager.pushLevel(8);
                
            }

            bulletList.moveDeltaXY();
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            boss.moveWayPointList(false);
            if (_timer >= ShootingTimer)
            {
                createBossBullet(boss);
                 _timer = 0;
               
            }
            //if (hb.currentHp<=0)
            //{
            //    gameStateManager.setLevel(6);
            //}
            enemyDie(bulletList);
            //bulletList.animationTick(gameTime);
            bulletList.moveDeltaXY();
            bulletList2.moveDeltaXY();
            s.animationTick(gameTime);
            bg.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            bg.Draw(spriteBatch);
            boss.draw(spriteBatch);
            bulletList.drawActive(spriteBatch);
            bulletList2.drawActive(spriteBatch);
            //bulletList.drawInfo(spriteBatch, Color.Red, Color.Red);
            airplane.draw(spriteBatch);
            spriteBatch.DrawString(font1, "Life: " + life, new Vector2(340, 20), Color.Brown);
            spriteBatch.DrawString(font1, "Marks: " + Marks.getMark(), new Vector2(540, 20), Color.Brown);
            s.draw(spriteBatch);
            hb.Draw(spriteBatch);
            spriteBatch.DrawString(font1, "Level 3 ", new Vector2(50, 40), Color.Brown);
            if (showbb)
            {
                bulletList.drawInfo(spriteBatch, Color.Red, Color.Red);
                airplane.drawInfo(spriteBatch, Color.Red, Color.Red);
                boss.drawInfo(spriteBatch, Color.Red, Color.Red);
            }
            if (hb.currentHp <= 0)
            {
                test++;
                spriteBatch.DrawString(font1, "You Win!!!!!! " , new Vector2(340, 350), Color.Brown);
                hb.setVisible(false);
                bulletList.setVisible(false);
                if (test > 97)
                {
                    musicIn.Stop();
                    gameStateManager.pushLevel(6);
                }
            }
            spriteBatch.End();
        }

        void createBossBullet(Sprite3 s)
        {

            bossBullet = new Sprite3(true, texBullet,s.getPosX()+50,s.getPosY()+50);
    
            bossBullet.setDeltaSpeed(new Vector2(0, 3f));
            bulletList.addSpriteReuse(bossBullet); // add the sprite
            //bossBullet.animationTick(gameTime);
        }
        private void createBullet(Vector2 pos)
        {

            bullet = new Sprite3(true, texBullet2, pos.X+10, pos.Y+10);
            bullet.setWidthHeight(32, 32);
            bullet.setBBToTexture();
            //bullet.setPos(pos);
            bullet.setDeltaSpeed(new Vector2(0, -5));
            bulletList2.addSpriteReuse(bullet);

        }
        void createExplosion(float x, float y)
        {

            float scale = 0.6f;
            int xoffset = -5;
            int yoffset = 3;
            s = new Sprite3(true, texFlash, x , y );


            s.setXframes(4);
            s.setYframes(2);
            //s.setWidthHeight(896 / 7 * scale, 384 / 3 * scale);
            s.setWidthHeight(64, 64);

            Vector2[] anim = new Vector2[8];
            anim[0].X = 0; anim[0].Y = 0;
            anim[1].X = 1; anim[1].Y = 0;
            anim[2].X = 2; anim[2].Y = 0;
            anim[3].X = 3; anim[3].Y = 0;
            anim[4].X = 0; anim[4].Y = 1;
            anim[5].X = 1; anim[5].Y = 1;
            anim[6].X = 2; anim[6].Y = 1;
            anim[7].X = 3; anim[7].Y = 1;

            s.setAnimationSequence(anim, 0, 7, 4);
            s.setAnimFinished(2); // make it inactive and invisible
            s.animationStart();
            limSound.playSoundIfOk();
          //SoundManager.playMusic();
        }
        void enemyDie(SpriteList sl)
        {
            for (int i = 0; i < sl.count(); i++)
            {
                Sprite3 b = sl.getSprite(i);
                if (b == null) continue;
                if (b.collision(airplane))
                {
                    createExplosion(airplane.getPosX(), airplane.getPosY());
                    b.setPos(0, 0);
                    b.setVisible(false);
                    b.setActive(false);
                    if (life > 0)
                    {
                        life = life - 1;
                       // airplane.setColor(new Color(256, 256, 256, 120));
                        
                    }
                    

                }
                
                //createEnemyBullet(b);
                int colision = bulletList2.collisionAA(boss);
                if (colision == -1) continue;
                Sprite3 c = bulletList2.getSprite(colision);
                c.setActive(false);
                //b.setVisible(false);
                hb.currentHp = hb.currentHp - 2;
                createExplosion(c.getPosX(), c.getPosY()-60);
                Marks.setMark(Marks.getMark() + 100);

            }
        }

    }

    //Game level4 welcome page
    class GameLevel_4 : RC_GameStateParent
    {
        Texture2D texAbout;
        Texture2D texExit;
        Texture2D texHighScore;
        Texture2D texNewGame;
        Texture2D texBg;
        Texture2D mouse;
        Sprite3 about;
        Sprite3 exit;
        Sprite3 highScore;
        Sprite3 newGame;
        Sprite3 bg;
        float mouse_x, mouse_y;

        public override void LoadContent()
        {
            texAbout = Util.texFromFile(graphicsDevice, Dir.dir + "about.bmp");
            texExit = Util.texFromFile(graphicsDevice, Dir.dir + "exit.bmp");
            texHighScore = Util.texFromFile(graphicsDevice, Dir.dir + "highScore.bmp");
            texNewGame = Util.texFromFile(graphicsDevice, Dir.dir + "newGame.bmp");
            texBg = Util.texFromFile(graphicsDevice, Dir.dir + "bg.jpg");
            mouse = Util.texFromFile(graphicsDevice, Dir.dir + "mouse.png");
            bg = new Sprite3(true, texBg, 0, 0);
            about = new Sprite3(true, texAbout, 350, 310);
            exit = new Sprite3(true, texExit, 350, 380);
            highScore = new Sprite3(true, texHighScore, 350, 240);
            newGame = new Sprite3(true, texNewGame, 350, 170);
            about.setBBToTexture();
            exit.setBBToTexture();
            highScore.setBBToTexture();
            newGame.setBBToTexture();
        }

        public override void Update(GameTime gameTime)
        {
           // previouseMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            mouse_x = currentMouseState.X;
            mouse_y = currentMouseState.Y;
            var mouseRectangle = new Rectangle((int)mouse_x, (int)mouse_y, 1, 1);
            var aboutRect = new Rectangle((int)about.getPosX(),(int)about.getPosY(),107,32);
            var exitRect = new Rectangle((int)exit.getPosX(), (int)exit.getPosY(), 107, 32);
            var highScoreRect = new Rectangle((int)highScore.getPosX(), (int)highScore.getPosY(), 107, 32);
            var newGameRect = new Rectangle((int)newGame.getPosX(), (int)newGame.getPosY(), 107, 32);
            about.setColor(Color.White);
            exit.setColor(Color.White);
            highScore.setColor(Color.White);
            newGame.setColor(Color.White);

            if (mouseRectangle.Intersects(aboutRect))
             {
                 about.setColor(Color.Gray);
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    gameStateManager.pushLevel(7);
                }
            }
            if (mouseRectangle.Intersects(exitRect))
            {
                exit.setColor(Color.Gray);
            }
            if (mouseRectangle.Intersects(highScoreRect))
            {
                highScore.setColor(Color.Gray);
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    gameStateManager.setLevel(6);
                }
            }
            if (mouseRectangle.Intersects(newGameRect))
            {
                newGame.setColor(Color.Gray);

                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    gameStateManager.getLevel(1).LoadContent();
                    gameStateManager.setLevel(1);
                }
            }


        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);


            bg.draw(spriteBatch);
            about.draw(spriteBatch);
            exit.draw(spriteBatch);
            highScore.draw(spriteBatch);
            newGame.draw(spriteBatch);
            spriteBatch.Draw(mouse, new Rectangle((int)mouse_x, (int)mouse_y, 32, 32), Color.White);
            spriteBatch.End();



        }
    }


    class GameLevel_5_Pause : RC_GameStateParent
    {

        Texture2D texPause; //         
        Sprite3 pause = null;
        SillyFont16 sf16;
        public override void LoadContent()
        {
            font1 = Content.Load<SpriteFont>("spritefont1");
            texPause = Util.texFromFile(graphicsDevice, Dir.dir + "pause.jpg");
            pause = new Sprite3(true, texPause, 100, 100);
            pause.setWidthHeight(600, 400);
            sf16 = new SillyFont16(graphicsDevice, Color.Transparent, Color.Red);
        }

        public override void Update(GameTime gameTime)
        {

            if (Game1.keyState.IsKeyDown(Keys.P) && Game1.prevKeyState.IsKeyUp(Keys.P)) // ***
            {
                gameStateManager.popLevel();
            }

        }

        public override void Draw(GameTime gameTime)
        {
            gameStateManager.prevStatePlayLevel.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            pause.Draw(spriteBatch);

            spriteBatch.End();

        }



    }
    // high mark
    class GameLevel_6 : RC_GameStateParent
    {
        Texture2D texHighScore;
        Sprite3 highScore;
        SpriteFont fonty;
        Texture2D texCancel;
        Texture2D texExit;
        Sprite3 cancel;
        Texture2D texBg;
        Sprite3 bg;
        Sprite3 exit;
        Texture2D mouse;
        HiScore hm;
        float mouse_x, mouse_y;
        public override void LoadContent()
        {
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            texHighScore = Util.texFromFile(graphicsDevice, Dir.dir + "score.png");
            highScore = new Sprite3(true, texHighScore, 200, 30);
            highScore.setWidthHeight(800 / 2, 346 / 2);
            texCancel = Util.texFromFile(graphicsDevice, Dir.dir + "cancel.bmp");
            cancel = new Sprite3(true, texCancel, 350, 400);
            texExit = Util.texFromFile(graphicsDevice, Dir.dir + "exit.bmp");
            exit = new Sprite3(true, texCancel, 550, 400);
            texBg = Util.texFromFile(graphicsDevice, Dir.dir + "bg.jpg");
            bg=new Sprite3(true, texBg, 0, 0);
            mouse = Util.texFromFile(graphicsDevice, Dir.dir + "mouse.png");
            //hm = new HiScore(Marks.getMark());


        }

        public override void Update(GameTime gameTime)
        {

            currentMouseState = Mouse.GetState();
            mouse_x = currentMouseState.X;
            mouse_y = currentMouseState.Y;
            var mouseRectangle = new Rectangle((int)mouse_x, (int)mouse_y, 1, 1);
            var cancelRect = new Rectangle((int)cancel.getPosX(), (int)cancel.getPosY(), 107, 32);
            var exitRect = new Rectangle((int)exit.getPosX(), (int)exit.getPosY(), 107, 32);
            cancel.setColor(Color.White);
            exit.setColor(Color.White);
            if (mouseRectangle.Intersects(cancelRect))
            {
                cancel.setColor(Color.Gray);

                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    gameStateManager.setLevel(4);
                }
            }
            //if (mouseRectangle.Intersects(exitRect))
            //{
            //    exit.setColor(Color.Gray);

            //    if (currentMouseState.LeftButton == ButtonState.Pressed)
            //    {
            //        Game1.Exit();
            //    }
            //}

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            bg.Draw(spriteBatch);
            highScore.Draw(spriteBatch);
            hm = new HiScore(Marks.getMark());
            
            spriteBatch.DrawString(font1, "No1: "+hm.getMark1(), new Vector2(340, 260), Color.Brown);
            spriteBatch.DrawString(font1, "No2: " + hm.getMark2(), new Vector2(340, 300), Color.Brown);
            spriteBatch.DrawString(font1, "No3: " + hm.getMark3(), new Vector2(340, 340), Color.Brown);
            cancel.Draw(spriteBatch);
           // exit.Draw(spriteBatch);
            spriteBatch.Draw(mouse, new Rectangle((int)mouse_x, (int)mouse_y, 32, 32), Color.White);
            spriteBatch.End();



        }
    }

    //help page
    class GameLevel_7 : RC_GameStateParent
    {
        Texture2D texInfo;
        Sprite3 info;
        Texture2D texCancel;
        Sprite3 cancel;
        Texture2D texBg;
        Sprite3 bg;
        Texture2D mouse;
        float mouse_x, mouse_y;
        public override void LoadContent()
        {
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            texInfo = Util.texFromFile(graphicsDevice, Dir.dir + "info.png");
            info = new Sprite3(true, texInfo, 350, 50);
            texCancel = Util.texFromFile(graphicsDevice, Dir.dir + "cancel.bmp");
            cancel = new Sprite3(true, texCancel, 350, 400);
            texBg = Util.texFromFile(graphicsDevice, Dir.dir + "bg.jpg");
            bg = new Sprite3(true, texBg, 0, 0);
            mouse = Util.texFromFile(graphicsDevice, Dir.dir + "mouse.png");


        }

        public override void Update(GameTime gameTime)
        {

            currentMouseState = Mouse.GetState();
            mouse_x = currentMouseState.X;
            mouse_y = currentMouseState.Y;
            var mouseRectangle = new Rectangle((int)mouse_x, (int)mouse_y, 1, 1);
            var cancelRect = new Rectangle((int)cancel.getPosX(), (int)cancel.getPosY(), 107, 32);
            cancel.setColor(Color.White);
            if (mouseRectangle.Intersects(cancelRect))
            {
                cancel.setColor(Color.Gray);

                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    gameStateManager.popLevel();
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            bg.Draw(spriteBatch);
            info.Draw(spriteBatch);
            spriteBatch.DrawString(font1, "Press W  Move Up ", new Vector2(80, 150), Color.Brown);
            spriteBatch.DrawString(font1, "Press S  Move Down ", new Vector2(80, 200), Color.Brown);
            spriteBatch.DrawString(font1, "Press A  Move Left ", new Vector2(80, 250), Color.Brown);
            spriteBatch.DrawString(font1, "Press D  Move Right ", new Vector2(80, 300), Color.Brown);
            spriteBatch.DrawString(font1, "Press J  Fire ", new Vector2(80, 350), Color.Brown);
            spriteBatch.DrawString(font1, "Press P  Pause ", new Vector2(450, 150), Color.Brown);
            spriteBatch.DrawString(font1, "Press F1  Help ", new Vector2(450, 200), Color.Brown);
            cancel.Draw(spriteBatch);
            spriteBatch.Draw(mouse, new Rectangle((int)mouse_x, (int)mouse_y, 32, 32), Color.White);
            spriteBatch.End();



        }
    }
    //dead page
    class GameLevel_8 : RC_GameStateParent
    {
        Texture2D texDead; // Green Guy
        Sprite3 dead; //
        SillyFont16 sf16;

        public override void LoadContent()
        {
            texDead = Util.texFromFile(graphicsDevice, Dir.dir + "dead.jpg");
            dead = new Sprite3(true, texDead, 100, 100);
            dead.setWidthHeight(600, 400);
            sf16 = new SillyFont16(graphicsDevice, Color.Transparent, Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            if (Game1.keyState.IsKeyDown(Keys.Space) && !Game1.prevKeyState.IsKeyDown(Keys.Space))
            {
                //gameStateManager.setLevel(2);
                gameStateManager.getLevel(2).LoadContent();
                gameStateManager.setLevel(2);
            }

        }

        public override void Draw(GameTime gameTime)
        {
            gameStateManager.prevStatePlayLevel.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);


            dead.Draw(spriteBatch);
            sf16.drawString(spriteBatch, "Press SPACE to restart", new Vector2(200, 400), Color.AntiqueWhite);

            sf16.drawString(spriteBatch, "MARKS: " + Marks.getMark(), new Vector2(500, 400), Color.AntiqueWhite);
            spriteBatch.End();
            Marks.setMark(0);
        }
    }





}
