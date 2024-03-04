using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MM_Large_Game
{
    public partial class Form1 : Form
    {
        int health = 6;
        Bitmap heart = new Bitmap("Heart.png");

        int coins = 0;

        public class Actor
        {
            public int X, Y;
            public bool climb = false;
            public Bitmap img;
            public int movdir = 0;
            public int anim = 0;
            public bool idle = true;
            public bool jump = false;
            public int jy = 0; // initial jump position.
            public bool grounded = false;
            public int ctshoot=0;
        }
        public class Bullet
        {
            public int X, Y;
            public int dir;
            public bool hit = false;
            public bool hitplayer = false;
            public Bitmap img;
        }
        public class Ladder
        {
            public int X, Y;
            public Bitmap img;
        }

        public class elevator
        {
            public int X, Y;
            public int W, H;
            public int startY, endY;
            public bool horiz = false;
            public bool up = false;
            public Bitmap img;
        }

        public class Enemy
        {
            public int X, Y;
            public int S, E;
            public int anim = 0;
            public bool movOp = false;
            public bool still = false;
            public bool canshoot = false;
            public bool canshootlaser = false;
            public bool shoot = false;
            public int shoottimer = 0;
            public int laserrange = 0;
            public Bitmap img;
        }

        public class Map
        {
            public int X, Y;
            public Bitmap img;
        }

        public class CActor
        {
            public Rectangle rcdst;
            public Rectangle rcsrc;
            public Bitmap img;
        }
        public class Coins
        {
            public int X, Y;
            public Bitmap img;
        }

        Timer tt = new Timer();
        Bitmap off;
        Actor Player;
        int XScroll = 0;
        int YScroll = 0;

        List<Actor> LActors = new List<Actor>();
        List<Map> LMap = new List<Map>();
        List<Ladder> Lladder = new List<Ladder>();
        List<elevator> Lelev = new List<elevator>();
        List<Bullet> LBullet = new List<Bullet>();
        List<Enemy> Lenemies = new List<Enemy>();
        List<Coins> Lcoins = new List<Coins>();
        List<CActor> Lworld = new List<CActor>();
        //bool test=false;
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.Load += new EventHandler(Load_game);
            this.KeyDown += new KeyEventHandler(Key_Down);
            this.KeyUp += new KeyEventHandler(Key_Up);
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            //player.SoundLocation = @"D:\CS\CS\CS232 (Multimedia)\1ST Time\Large Game\GameProjectMM\GameProjectMM\GameProjectMM\bin\01 ROCKMAN1-3.wav";
            //player.Load();
            //player.Play();
            tt.Tick += new EventHandler(tt_Tick);
            tt.Interval = 70;
            tt.Start();
        }

        void Load_game(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            Player = new Actor();
            Player.X = this.ClientSize.Width / 2+100;
            Player.Y = this.ClientSize.Height - 160;
            Player.img = new Bitmap("player\\idle0R.png");
            LActors.Add(Player);
            CreateWorld();

            //Runner
            /////////////////////bossssssssssssss//////////////
            Enemy enem1 = new Enemy();
            enem1.X = this.ClientSize.Width   - 1650;
            enem1.Y = this.ClientSize.Height / 2 - 80;
            enem1.S = enem1.X - 300;
            enem1.E = enem1.X + 300;
            enem1.img = new Bitmap("runner\\1.png");
            enem1.img.MakeTransparent(enem1.img.GetPixel(0, 0));
            Lenemies.Add(enem1);

            enem1 = new Enemy();
            enem1.X = this.ClientSize.Width + 750;
            enem1.Y = this.ClientSize.Height - 160;
            enem1.S = enem1.X - 300;
            enem1.E = enem1.X + 300;
            enem1.img = new Bitmap("runner\\1.png");
            enem1.img.MakeTransparent(enem1.img.GetPixel(0, 0));
            Lenemies.Add(enem1);

            //FlyingEnemy
            //lower
            Enemy enem2 = new Enemy();
            enem2.X = this.ClientSize.Width / 2 +200 ;
            enem2.Y = this.ClientSize.Height / 2 + 100;
            enem2.canshootlaser = true;
            enem2.S = enem2.X - 300;
            enem2.E = enem2.X + 300;
            enem2.laserrange = 250;
            enem2.img = new Bitmap("enemy\\1.png");
            enem2.img.MakeTransparent(enem2.img.GetPixel(0, 0));
            Lenemies.Add(enem2);

            //BOMBER
            //lower
            enem2 = new Enemy();
            enem2.X = -50;
            enem2.Y = this.ClientSize.Height - 160;
            enem2.canshoot = true;
            enem2.still = true;
            enem2.S = enem2.X - 300;
            enem2.E = enem2.X + 300;
            enem2.laserrange = 250;
            enem2.img = new Bitmap("bomber\\norm.png");
            enem2.img.MakeTransparent(enem2.img.GetPixel(0, 0));
            Lenemies.Add(enem2);
            
            //FlyingEnemy

            enem2 = new Enemy();
            enem2.X = this.ClientSize.Width / 2 + 100;
            enem2.Y = this.ClientSize.Height / 2 - 270;
            enem2.canshootlaser = true;
            enem2.S = enem2.X - 100;
            enem2.E = enem2.X + 100;
            enem2.laserrange = 250;
            enem2.img = new Bitmap("enemy\\1.png");
            enem2.img.MakeTransparent(enem2.img.GetPixel(0, 0));
            Lenemies.Add(enem2);
            //boss
            Enemy enem3 = new Enemy();
            enem3.X = this.ClientSize.Width + 750;
            enem3.Y = this.ClientSize.Height - 500;
            enem3.S = enem3.X - 200;
            enem3.E = enem3.X + 200;
            enem3.img = new Bitmap("Boss\\1L.png");
            enem3.img.MakeTransparent(enem3.img.GetPixel(0, 0));
            Lenemies.Add(enem3);

            //MAP
            //LBoarder
            int x = -1000;
            int y = this.ClientSize.Height - 80;
            for (int i = 0; i < 12; i++)
            {
                Map pnn = new Map();
                pnn.X = x;
                pnn.Y = y;
                pnn.img = new Bitmap(new Bitmap("map\\4.png"));
                y -= pnn.img.Height;
                LMap.Add(pnn);
            }

            //BaseFloor
            x = -1000 + LMap[0].img.Width;
            y = this.ClientSize.Height - 80;
            for (int i = 0; i < 50; i++)
            {
                Map pnn = new Map();
                pnn.X = x;
                pnn.Y = y;
                pnn.img = new Bitmap(new Bitmap("map\\4.png"));
                x += pnn.img.Width;
                LMap.Add(pnn);
            }

            //RBoarder 
            x = LMap[LMap.Count - 1].X + 72;
            y = this.ClientSize.Height - 80;
            for (int i = 0; i < 20; i++)
            {
                Map pnn = new Map();
                pnn.X = x;
                pnn.Y = y;
                pnn.img = new Bitmap(new Bitmap("map\\5.png"));
                y -= pnn.img.Height;
                LMap.Add(pnn);
            }

            //COINS
            Coins pnnCn = new Coins();
            pnnCn.X = this.ClientSize.Width / 2 - 400;
            pnnCn.Y = this.ClientSize.Height - 160;
            pnnCn.img = new Bitmap(new Bitmap("coin.png"));
            Lcoins.Add(pnnCn);

            pnnCn = new Coins();
            pnnCn.X = this.ClientSize.Width + 400;
            pnnCn.Y = this.ClientSize.Height - 160;
            pnnCn.img = new Bitmap(new Bitmap("coin.png"));
            Lcoins.Add(pnnCn);

            pnnCn = new Coins();
            pnnCn.X = this.ClientSize.Width / 2 + 1800;
            pnnCn.Y = this.ClientSize.Height - 160;
            pnnCn.img = new Bitmap(new Bitmap("coin.png"));
            Lcoins.Add(pnnCn);

            x = this.ClientSize.Width / 2 + 500;
            y = this.ClientSize.Height / 2 - 80;
            for (int i = 0; i < 2; i++)
            {
                pnnCn = new Coins();
                pnnCn.X = x;
                pnnCn.Y = y;
                pnnCn.img = new Bitmap(new Bitmap("coin.png"));
                x += pnnCn.img.Width + 10;
                Lcoins.Add(pnnCn);
            }

            x = this.ClientSize.Width / 2 + 1750;
            y = this.ClientSize.Height / 2 - 80;
            for (int i = 0; i < 2; i++)
            {
                pnnCn = new Coins();
                pnnCn.X = x;
                pnnCn.Y = y;
                pnnCn.img = new Bitmap(new Bitmap("coin.png"));
                x += pnnCn.img.Width + 10;
                Lcoins.Add(pnnCn);
            }

            //map
            //2d flor right
            x = 645;
            y = this.ClientSize.Height / 2;
            for (int i = 0; i < 13; i++)
            {
                Map pnn = new Map();
                pnn.X = x;
                pnn.Y = y;
                pnn.img = new Bitmap(new Bitmap("map\\4.png"));
                x += pnn.img.Width;
                LMap.Add(pnn);
            }
            //right ladder
            x = LMap[LMap.Count - 1].X + 70;
            y = LMap[LMap.Count - 1].Y+20;
            for (int i = 0; i < 3; i++)
            {
                Ladder pnn = new Ladder();
                pnn.X = x;
                pnn.Y = y;
                Image image = Image.FromFile("map\\ladder.png");
                pnn.img = new Bitmap(new Bitmap(image, 72, 80));
                y += pnn.img.Height;
                Lladder.Add(pnn);
            }
            //map
            x = LMap[LMap.Count - 1].X + 140;
            y = LMap[LMap.Count - 1].Y;
            for (int i = 0; i < 14; i++)
            {
                Map pnn = new Map();
                pnn.X = x;
                pnn.Y = y;
                pnn.img = new Bitmap(new Bitmap("map\\4.png"));
                x += pnn.img.Width;
                LMap.Add(pnn);
            }



            x = -930 + LMap[0].img.Width;
            y = this.ClientSize.Height / 2;
            for (int i =0 ; i < 19; i++)
            {
                Map pnn = new Map();
                pnn.X = x;
                pnn.Y = y;
                pnn.img = new Bitmap(new Bitmap("map\\4.png"));
                x += pnn.img.Width;
                LMap.Add(pnn);
            }

            for (int i = 0; i < 7; i++)
            {
                Map pnn = new Map();
                pnn.X = x;
                pnn.Y = y;
                pnn.img = new Bitmap(new Bitmap("map\\5.png"));
                y -= pnn.img.Height;
                LMap.Add(pnn);
            }
            x += 70;
            y = this.ClientSize.Height / 2+20;
            for (int i = 0; i < 3; i++)
            {
                Ladder pnn = new Ladder();
                pnn.X = x;
                pnn.Y = y;
                Image image = Image.FromFile("map\\ladder.png");
                pnn.img = new Bitmap(new Bitmap(image, 72, 80));
                y += pnn.img.Height;
                Lladder.Add(pnn);
            }

            x = -1000 + LMap[0].img.Width;
            y = this.ClientSize.Height / 2 - 80;
            for (int i = 0; i < 3; i++)
            {
                Coins pnnC = new Coins();
                pnnC.X = x+1250;
                pnnC.Y = y;
                pnnC.img = new Bitmap(new Bitmap("coin.png"));
                x += pnnC.img.Width;
                Lcoins.Add(pnnC);
            }


            x = -1000 + LMap[0].img.Width;
            y = this.ClientSize.Height - 80;

            elevator pnnL = new elevator();
            pnnL.X = x;
            pnnL.Y = y;
            pnnL.startY = y;
            pnnL.endY = LMap[LMap.Count - 1].Y;
            pnnL.img = new Bitmap(new Bitmap("map\\elevator.png"));
            y += pnnL.img.Height;
            Lelev.Add(pnnL);

            DrawDubb(this.CreateGraphics());
        }

        int runAnimframes = 4;
        int idleAnimframes = 2;
        int climbAnimFrames = 4;
        int framect = 0;

        int gravity = 10;

        bool movL;
        bool movR;
        bool isclimbu = false;
        bool isclimbd = false;

        int elevframe = 0;

        int enemframe = 0;

        bool onelev = false;
        bool stopelev = false;
        bool shoot = false;



        void playergravity()
        {
            // check oif player is not on elevator
            if (!onelev && !Player.climb)
            {
                if (Player.jump)
                {
                    if (Player.Y > Player.jy - 80)
                    {
                        Player.Y -= gravity;

                        if (Player.movdir == 1)
                        {
                            Player.img = new Bitmap("player\\jump0R.png");

                        }
                        else if (Player.movdir == -1)
                        {
                            Player.img = new Bitmap("player\\jump0.png");

                        }
                    }
                    else
                    {
                        Player.jump = false;
                    }
                }

                for (int k = 0; k < LMap.Count; k++)
                {
                    //check if player on ground   
                    if ((LMap[k].Y == Player.Y + (Player.img.Height) || (LMap[k].Y <= Player.Y + (Player.img.Height)
                       && LMap[k].Y + LMap[k].img.Height >= Player.Y + (Player.img.Height))) && (LMap[k].X <= Player.X + (Player.img.Width / 2)
                       && LMap[k].X + LMap[k].img.Width >= Player.X + (Player.img.Width / 2)))
                    {
                        Player.Y = LMap[k].Y - Player.img.Height;
                        gravity = 15;
                        Player.grounded = true;

                        if (Player.idle)
                        {
                            Player.anim = 0;
                            if (Player.movdir == -1)
                            {
                                Player.img = new Bitmap("player\\idle0.png");
                            }
                            else if (Player.movdir == 1)
                            {
                                Player.img = new Bitmap("player\\idle0R.png");
                            }
                        }

                        break;
                    }
                    else
                    {
                        Player.grounded = false;
                    }
                }
                if (Player.grounded)
                {
                    return;
                }
                else
                {
                    if (!Player.jump)
                    {
                        if (Player.Y < this.ClientSize.Height)
                        {
                            if (!Player.grounded)
                            {
                                Player.Y += gravity;
                            }

                        }
                        else
                        {
                            MessageBox.Show("dead.");
                            tt.Stop();
                        }
                    }
                }

            }
        }

        void elevators()
        {
            for (int i = 0; i < Lelev.Count; i++)
            {

                if ((Player.X + Player.img.Width / 2 > Lelev[i].X
                    && Player.X + Player.img.Width / 2 < Lelev[i].X + Lelev[i].img.Width)
                  && (Player.Y + Player.img.Height == Lelev[i].Y
                    && Player.Y + Player.img.Height < Lelev[i].Y + Lelev[i].img.Height))
                {


                    Player.grounded = true;
                    //check if player on elevator
                    onelev = true;
                    if (Lelev[i].Y >= Lelev[i].startY)
                    {
                        Lelev[i].up = true;
                        stopelev = true;
                    }
                    if (Lelev[i].Y <= Lelev[i].endY)
                    {
                        Lelev[i].up = false;
                        stopelev = true;
                    }

                    if (elevframe % 20 == 0)
                    {
                        stopelev = false;
                    }


                    if (!stopelev)
                    {
                        if (Lelev[i].up)
                        {
                            Player.Y -= 10;
                            Lelev[i].Y -= 10;
                        }
                        else
                        {
                            Player.Y += 10;
                            Lelev[i].Y += 10;
                        }
                    }

                    elevframe++;
                }
                else
                {
                    elevframe = 0;

                    if (Lelev[i].Y != Lelev[i].startY)
                    {
                        Lelev[i].Y += 10;
                    }
                    else
                    {
                        Lelev[i].up = false;
                    }

                    onelev = false;
                }
            }
        }

        void playerladders()
        {
            for (int i = 0; i < Lladder.Count; i++)
            {
                if ((Player.X + Player.img.Width / 2 > Lladder[i].X
                    && Player.X + Player.img.Width / 2 < Lladder[i].X + Lladder[i].img.Width)
                  && (Player.Y + Player.img.Height / 2 > Lladder[i].Y
                    && Player.Y + Player.img.Height / 2 < Lladder[i].Y + Lladder[i].img.Height))
                {
                    Player.climb = true;

                    if (isclimbu)
                    {
                        isclimbd = false;
                        Player.Y -= 10;

                        Player.anim++;
                        if (Player.anim >= climbAnimFrames)
                        { Player.anim = 0; }
                        Player.img = new Bitmap("player\\climb" + Player.anim + ".png");
                    }
                    else if (isclimbd)
                    {
                        isclimbu = false;
                        Player.Y += 10;

                        Player.anim++;
                        if (Player.anim >= climbAnimFrames)
                        { Player.anim = 0; }
                        Player.img = new Bitmap("player\\climb" + Player.anim + ".png");
                    }
                }

            }
        }

        void playermove()
        {
            if (Player.idle && !Player.jump && !Player.climb)
            {
                if (Player.grounded)
                {
                    if (Player.movdir == 1)
                    {
                        if (framect % 10 == 0)
                        {
                            Player.anim++;
                            if (Player.anim >= idleAnimframes)
                            { Player.anim = 0; }
                            Player.img = new Bitmap("player\\idle" + Player.anim + "R.png");
                        }
                    }
                    else if (Player.movdir == -1)
                    {
                        if (framect % 10 == 0)
                        {
                            Player.anim++;
                            if (Player.anim >= idleAnimframes)
                            { Player.anim = 0; }
                            Player.img = new Bitmap("player\\idle" + Player.anim + ".png");
                        }
                    }
                    framect++;

                }
                else
                {
                    if (Player.movdir == 1)
                    {
                        Player.img = new Bitmap("player\\jump0R.png");
                        //Player.X += 10;
                    }
                    else if (Player.movdir == -1)
                    {
                        Player.img = new Bitmap("player\\jump0.png");
                        //Player.X -= 10;
                    }
                }


            }
            else
            {
                movL = true;
                movR = true;

                if (!Player.grounded && !Player.climb)
                {
                    if (Player.movdir == 1)
                    {
                        Player.img = new Bitmap("player\\jump1R.png");

                        for (int i = 0; i < LMap.Count; i++)
                        {
                            if ((LMap[i].X > Player.X + Player.img.Width / 2 && LMap[i].X - 20 < Player.X + Player.img.Width / 2)
                                && (LMap[i].Y + LMap[i].img.Height > Player.Y + Player.img.Height / 2 && LMap[i].Y < Player.Y + Player.img.Height / 2))
                            {
                                movL = true;
                                movR = false;
                            }
                        }
                        if (movR)
                        {
                            if (Player.X + Player.img.Width + 144 < this.ClientSize.Width)
                            {
                                Player.X += 10;
                            }
                            for (int i = 0; i < LMap.Count; i++)
                            {
                                LMap[i].X -= 10;
                            }

                            for (int i = 0; i < Lcoins.Count; i++)
                            {
                                Lcoins[i].X -= 10;
                            }

                            for (int i = 0; i < Lenemies.Count; i++)
                            {
                                Lenemies[i].X -= 10;
                                Lenemies[i].S -= 10;
                                Lenemies[i].E -= 10;
                            }

                            for (int i = 0; i < Lladder.Count; i++)
                            {
                                Lladder[i].X -= 10;
                            }

                            for (int i = 0; i < Lelev.Count; i++)
                            {
                                Lelev[i].X -= 10;
                            }

                            for (int i = 0; i < LBullet.Count; i++)
                            {
                                LBullet[i].X -= 10;
                            }
                        }

                    }
                    else if (Player.movdir == -1)
                    {
                        Player.img = new Bitmap("player\\jump1.png");

                        for (int i = 0; i < LMap.Count; i++)
                        {
                            if ((LMap[i].X + LMap[i].img.Width > Player.X + (Player.img.Width / 2 - 20) && LMap[i].X + 20 < Player.X + (Player.img.Width / 2 - 20))
                                && (LMap[i].Y + LMap[i].img.Height > Player.Y + Player.img.Height / 2 && LMap[i].Y < Player.Y + Player.img.Height / 2))
                            {
                                movL = false;
                                movR = true;
                            }
                        }

                        if (movL)
                        {
                            if (Player.X > 144)
                            {
                                Player.X -= 10;
                            }
                            for (int i = 0; i < LMap.Count; i++)
                            {
                                LMap[i].X += 10;
                            }

                            for (int i = 0; i < Lcoins.Count; i++)
                            {
                                Lcoins[i].X += 10;
                            }

                            for (int i = 0; i < Lenemies.Count; i++)
                            {
                                Lenemies[i].X += 10;
                                Lenemies[i].S += 10;
                                Lenemies[i].E += 10;
                            }

                            for (int i = 0; i < Lladder.Count; i++)
                            {
                                Lladder[i].X += 10;
                            }

                            for (int i = 0; i < Lelev.Count; i++)
                            {
                                Lelev[i].X += 10;
                            }

                            for (int i = 0; i < LBullet.Count; i++)
                            {
                                LBullet[i].X += 10;
                            }
                        }
                    }
                }
                else
                {
                    if (!Player.climb)
                    {
                        if (Player.movdir == 1)
                        {
                            Player.anim++;
                            if (Player.anim >= runAnimframes)
                            { Player.anim = 0; }
                            Player.img = new Bitmap("player\\run" + Player.anim + "R.png");

                            for (int i = 0; i < LMap.Count; i++)
                            {
                                if ((LMap[i].X > Player.X + Player.img.Width / 2 && LMap[i].X - 20 < Player.X + Player.img.Width / 2)
                                    && (LMap[i].Y + LMap[i].img.Height > Player.Y + Player.img.Height / 2 && LMap[i].Y < Player.Y + Player.img.Height / 2))
                                {
                                    movL = true;
                                    movR = false;
                                }
                            }
                            if (movR)
                            {
                                if (Player.X + Player.img.Width + 144 < this.ClientSize.Width)
                                {
                                    Player.X += 10;
                                }

                                for (int i = 0; i < LMap.Count; i++)
                                {
                                    LMap[i].X -= 10;
                                }

                                for (int i = 0; i < Lcoins.Count; i++)
                                {
                                    Lcoins[i].X -= 10;
                                }

                                for (int i = 0; i < Lenemies.Count; i++)
                                {
                                    Lenemies[i].X -= 10;
                                    Lenemies[i].S -= 10;
                                    Lenemies[i].E -= 10;
                                }

                                for (int i = 0; i < Lladder.Count; i++)
                                {
                                    Lladder[i].X -= 10;
                                }

                                for (int i = 0; i < Lelev.Count; i++)
                                {
                                    Lelev[i].X -= 10;
                                }

                                for (int i = 0; i < LBullet.Count; i++)
                                {
                                    LBullet[i].X -= 10;
                                }
                            }
                        }
                        else if (Player.movdir == -1)
                        {
                            Player.anim++;
                            if (Player.anim >= runAnimframes)
                            { Player.anim = 0; }
                            Player.img = new Bitmap("player\\run" + Player.anim + ".png");

                            for (int i = 0; i < LMap.Count; i++)
                            {
                                if ((LMap[i].X + LMap[i].img.Width > Player.X + (Player.img.Width / 2 - 20) && LMap[i].X + 20 < Player.X + (Player.img.Width / 2 - 20))
                                    && (LMap[i].Y + LMap[i].img.Height > Player.Y + Player.img.Height / 2 && LMap[i].Y < Player.Y + Player.img.Height / 2))
                                {
                                    movL = false;
                                    movR = true;
                                }
                            }

                            if (movL)
                            {
                                if (Player.X > 144)
                                {
                                    Player.X -= 10;
                                }
                                for (int i = 0; i < LMap.Count; i++)
                                {
                                    LMap[i].X += 10;
                                }

                                for (int i = 0; i < Lcoins.Count; i++)
                                {
                                    Lcoins[i].X += 10;
                                }

                                for (int i = 0; i < Lenemies.Count; i++)
                                {
                                    Lenemies[i].X += 10;
                                    Lenemies[i].S += 10;
                                    Lenemies[i].E += 10;
                                }

                                for (int i = 0; i < Lladder.Count; i++)
                                {
                                    Lladder[i].X += 10;
                                }

                                for (int i = 0; i < Lelev.Count; i++)
                                {
                                    Lelev[i].X += 10;
                                }

                                for (int i = 0; i < LBullet.Count; i++)
                                {
                                    LBullet[i].X += 10;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Player.movdir == 1)
                        {
                            Player.X += 10;
                            Player.movdir = 0;
                        }
                        else if (Player.movdir == -1)
                        {
                            Player.X -= 10;
                            Player.movdir = 0;
                        }
                    }
                }
            }
        }

        void movenemies()
        {
            int ctshoot = 0;
            
            for (int i = 0; i < Lenemies.Count; i++)
            {
                if (!Lenemies[i].still)
                {
                    if (Lenemies[i].X >= Lenemies[i].E)
                    {
                        Lenemies[i].movOp = true;
                    }
                    if (Lenemies[i].X <= Lenemies[i].S)
                    {
                        Lenemies[i].movOp = false;
                    }

                    if (Lenemies[i].movOp)
                    {
                        if (!Lenemies[i].canshoot && !Lenemies[i].canshootlaser)
                        {
                            Lenemies[i].anim++;
                            if (Lenemies[i].anim >= 4)
                            { Lenemies[i].anim = 1; }
                            Lenemies[i].img = new Bitmap("runner\\" + Lenemies[i].anim + ".png");
                            Lenemies[i].img.MakeTransparent(Lenemies[i].img.GetPixel(0, 0));
                        }
                        Lenemies[i].X -= 5;


                    }
                    else
                    {
                        Lenemies[i].X += 5;

                        if (!Lenemies[i].canshoot && !Lenemies[i].canshootlaser)
                        {
                            Lenemies[i].anim++;
                            if (Lenemies[i].anim >= 4)
                            { Lenemies[i].anim = 1; }
                            Lenemies[i].img = new Bitmap("runner\\" + Lenemies[i].anim + "R.png");
                            Lenemies[i].img.MakeTransparent(Lenemies[i].img.GetPixel(0, 0));
                        }

                    }

                    
                }
            }
            int j = 0;
            if (!Lenemies[j].canshoot && !Lenemies[j].canshootlaser)
            {
                ctshoot++;
                Lenemies[j].anim++;
                if (Lenemies[j].anim >= 5)
                { Lenemies[j].anim = 1; }
                Lenemies[j].img = new Bitmap("Boss\\" + Lenemies[j].anim + "L.png");

                Lenemies[j].img.MakeTransparent(Lenemies[j].img.GetPixel(0, 0));
            }
            else
            {
                Lenemies[j].anim++;
                if (Lenemies[j].anim >= 5)
                { Lenemies[j].anim = 1; }
                Lenemies[j].img = new Bitmap("Boss\\" + Lenemies[j].anim + "R.png");
                Lenemies[j].img.MakeTransparent(Lenemies[j].img.GetPixel(0, 0));
            }
        }

        void bullets()
        {
            //hero bullet
            for (int i = 0; i < LBullet.Count; i++)
            {
                LBullet[i].X += 10 * (LBullet[i].dir);
                for (int j = 0; j < LMap.Count; j++)
                {//mess shot
                    if ((LBullet[i].X + LBullet[i].img.Width / 2 >= LMap[j].X
                            && LBullet[i].X + LBullet[i].img.Width / 2 <= LMap[j].X + LMap[j].img.Width)
                        && (LBullet[i].Y + LBullet[i].img.Height / 2 >= LMap[j].Y
                            && LBullet[i].Y + LBullet[i].img.Height / 2 <= LMap[j].Y + LMap[j].img.Height))
                    {
                        //test = true;
                        LBullet[i].hit = true;
                        break;
                    }

                }
                //shot confermed
                if (!LBullet[i].hit)
                {
                    for (int j = 0; j < Lenemies.Count; j++)
                    {
                        if ((LBullet[i].X + LBullet[i].img.Width / 2 >= Lenemies[j].X
                                && LBullet[i].X + LBullet[i].img.Width / 2 <= Lenemies[j].X + Lenemies[j].img.Width)
                            && (LBullet[i].Y + LBullet[i].img.Height / 2 >= Lenemies[j].Y
                                && LBullet[i].Y + LBullet[i].img.Height / 2 <= Lenemies[j].Y + Lenemies[j].img.Height))
                        {
                            //test = true;
                            LBullet[i].hit = true;
                            Lenemies.RemoveAt(j);
                            break;
                        }
                    }

                    //enymy bullets
                    if ((LBullet[i].X + LBullet[i].img.Width / 2 >= Player.X + Player.img.Width / 2 - 10
                                && LBullet[i].X + LBullet[i].img.Width / 2 <= Player.X + Player.img.Width / 2 + 10)
                            && (LBullet[i].Y + LBullet[i].img.Height / 2 >= Player.Y
                                && LBullet[i].Y + LBullet[i].img.Height / 2 <= Player.Y + Player.img.Height))
                    {
                        if (!LBullet[i].hitplayer)
                        {
                            //hero hit
                            LBullet[i].hitplayer = true;
                            health--;
                            if (LBullet[i].dir == 1)
                            { Player.img = new Bitmap("player\\hurt.png"); }
                            else
                            { Player.img = new Bitmap("player\\hurtR.png"); }
                            LBullet[i].hit = true;
                        }

                        break;
                    }
                }
                else if (LBullet[i].hit)
                {
                    LBullet.RemoveAt(i);
                    break;
                }
            }
        }
        //anime hit  time
        bool graceperiod = false;
        void tt_Tick(object sender, EventArgs e)
        {
            if (health == 0)
            {
                tt.Stop();
                MessageBox.Show("you died :'(");
            }

            if (coins == 10)
            {
                tt.Stop();
                MessageBox.Show("you win! :)");
            }

            // test = false;
            movL = true;
            movR = true;
            Player.climb = false;

            movenemies();
            playerladders();
            playergravity();
            elevators();


            for (int i = 0; i < Lenemies.Count; i++)
            {
                if (graceperiod)
                {
                    enemframe++;
                    //bullet didnt hit hero
                    if (enemframe % 20 == 0)
                    {
                        graceperiod = false;
                    }
                }

                if ((Player.X + Player.img.Width / 2 >= Lenemies[i].X - 5
                        && Player.X + Player.img.Width / 2 <= Lenemies[i].X + Lenemies[i].img.Width + Player.img.Width / 2)
                    && (Player.Y == Lenemies[i].Y))
                {
                    if (!graceperiod)
                    {
                        enemframe = 0;
                        health--;
                        graceperiod = true;
                        if (Player.movdir == 1)
                        { Player.img = new Bitmap("player\\hurtR.png"); }
                        else
                        { Player.img = new Bitmap("player\\hurt.png"); }
                    }
                }

                if (Lenemies[i].canshootlaser)
                {
                    if (Player.X > Lenemies[i].S && Player.X < Lenemies[i].E)
                    {
                        Lenemies[i].shoot = true;

                        if (Player.X > Lenemies[i].X)
                        {
                            Lenemies[i].img = new Bitmap("enemy\\2.png");
                            Lenemies[i].img.MakeTransparent(Lenemies[i].img.GetPixel(0, 0));
                        }
                        else if (Player.X < Lenemies[i].X)
                        {
                            Lenemies[i].img = new Bitmap("enemy\\2L.png");
                            Lenemies[i].img.MakeTransparent(Lenemies[i].img.GetPixel(0, 0));
                        }
                        if (Player.X + Player.img.Width / 2 >= Lenemies[i].X + Lenemies[i].img.Width / 2 - 5
                                && Player.X + Player.img.Width / 2 <= Lenemies[i].X + Lenemies[i].img.Width / 2 + 5
                            && Player.Y + Player.img.Height / 2 <= Lenemies[i].Y + Lenemies[i].img.Height + Lenemies[i].laserrange
                                && Player.Y + Player.img.Height / 2 >= Lenemies[i].Y + Lenemies[i].img.Height)
                        {
                            if (!graceperiod)
                            {
                                enemframe = 0;
                                health--;
                                graceperiod = true;
                                if (Player.movdir == 1)
                                { Player.img = new Bitmap("player\\hurtR.png"); }
                                else
                                { Player.img = new Bitmap("player\\hurt.png"); }
                            }

                        }
                    }
                    else
                    {
                        if (Player.X > Lenemies[i].X)
                        {
                            Lenemies[i].img = new Bitmap("enemy\\1.png");
                        }
                        else
                        {
                            Lenemies[i].img = new Bitmap("enemy\\1L.png");
                        }
                        Lenemies[i].img.MakeTransparent(Lenemies[i].img.GetPixel(0, 0));
                        Lenemies[i].shoot = false;
                    }
                }
                else if (Lenemies[i].canshoot)
                {
                    if (Player.X > Lenemies[i].S && Player.X < Lenemies[i].E)
                    {
                        Lenemies[i].shoot = true;
                        Lenemies[i].shoottimer++;

                        if (Lenemies[i].shoottimer % 20 == 0)
                        {
                            Bullet pnn = new Bullet();
                            pnn.img = new Bitmap("bomber\\bomb.png");
                            pnn.img.MakeTransparent(pnn.img.GetPixel(0, 0));
                            pnn.Y = Lenemies[i].Y - 5;

                            if (Player.X > Lenemies[i].X)
                            {
                                pnn.X = Lenemies[i].X + 30;
                                pnn.dir = 1;
                            }
                            else if (Player.X < Lenemies[i].X)
                            {
                                pnn.X = Lenemies[i].X - 50;
                                pnn.dir = -1;
                            }

                            LBullet.Add(pnn);
                        }


                        if (Player.X > Lenemies[i].X)
                        {
                            Lenemies[i].img = new Bitmap("bomber\\throwR.png");
                            Lenemies[i].img.MakeTransparent(Lenemies[i].img.GetPixel(0, 0));
                        }
                        else if (Player.X < Lenemies[i].X)
                        {
                            Lenemies[i].img = new Bitmap("bomber\\throw.png");
                            Lenemies[i].img.MakeTransparent(Lenemies[i].img.GetPixel(0, 0));
                        }


                        if (Player.X + Player.img.Width / 2 >= Lenemies[i].X + Lenemies[i].img.Width / 2 - 5
                                && Player.X + Player.img.Width / 2 <= Lenemies[i].X + Lenemies[i].img.Width / 2 + 5
                            && Player.Y + Player.img.Height / 2 <= Lenemies[i].Y + Lenemies[i].img.Height + Lenemies[i].laserrange
                                && Player.Y + Player.img.Height / 2 >= Lenemies[i].Y + Lenemies[i].img.Height)
                        {
                            if (!graceperiod)
                            {
                                enemframe = 0;
                                health--;
                                graceperiod = true;
                                if (Player.movdir == 1)
                                { Player.img = new Bitmap("player\\hurtR.png"); }
                                else
                                { Player.img = new Bitmap("player\\hurt.png"); }
                            }
                        }
                    }
                    else
                    {
                        if (Player.X > Lenemies[i].X)
                        {
                            Lenemies[i].img = new Bitmap("bomber\\normR.png");
                        }
                        else
                        {
                            Lenemies[i].img = new Bitmap("bomber\\norm.png");
                        }
                        Lenemies[i].img.MakeTransparent(Lenemies[i].img.GetPixel(0, 0));
                        Lenemies[i].shoot = false;
                    }
                }

            }


            for (int i = 0; i < Lcoins.Count; i++)
            {
                if (Player.X + Player.img.Width / 2 >= Lcoins[i].X
                        && Player.X + Player.img.Width / 2 <= Lcoins[i].X + Lcoins[i].img.Width
                    && Player.Y == Lcoins[i].Y)
                {
                    coins++;
                    Lcoins.RemoveAt(i);
                    break;
                }
            }


            if (!shoot)
            {
                playermove();
            }
            bullets();

            DrawDubb(this.CreateGraphics());

        }



        bool jumponce = true;
        void Key_Down(object sender, KeyEventArgs e)
        {
            Player.idle = false;

            if (e.KeyCode == Keys.Space)
            {
                Bullet pnn = new Bullet();
                pnn.img = new Bitmap("player\\bullet.png");
                pnn.Y = Player.Y - 5;
                shoot = true;

                if (Player.movdir == 1)
                {
                    Player.img = new Bitmap("player\\shootR.png");
                    pnn.X = Player.X + 30;
                    pnn.dir = 1;
                }
                else
                {
                    Player.img = new Bitmap("player\\shoot.png");
                    pnn.X = Player.X - 50;
                    pnn.dir = -1;
                }

                LBullet.Add(pnn);
            }

            else if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                if (Player.climb)
                {
                    isclimbu = true;
                }
                else
                {
                    if (jumponce)
                    {
                        Player.jump = true;
                        Player.jy = Player.Y;
                        jumponce = false;
                    }
                }
            }

            else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                if (Player.climb)
                {
                    isclimbd = true;
                }
            }

            else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                if (XScroll - 5 >= 0)
                {
                    XScroll -= 15;
                    Player.movdir = -1;
                }

            }

            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                if (XScroll + 5 <= (Lworld[0].img.Width - ClientSize.Width))
                {
                    XScroll += 15;
                    Player.movdir = 1;
                }

            }
            ModifyRects();
        }

        void Key_Up(object sender, KeyEventArgs e)
        {
            isclimbu = false;
            isclimbd = false;
            Player.idle = true;


            if (e.KeyCode == Keys.Space)
            { shoot = false; }

            if (Player.climb)
            {
                if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                {
                    Player.anim = 0;
                    Player.img = new Bitmap("player\\climb0.png");
                }

                if (e.KeyCode == Keys.D || e.KeyCode == Keys.Down)
                {
                    Player.anim = 0;
                    Player.img = new Bitmap("player\\climb0.png");
                }

            }
            else
            {
                if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                {
                    jumponce = true;
                }
            }

            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                Player.anim = 0;

            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                Player.anim = 0;
                // Player.img = new Bitmap("player\\idle0R.png");
            }
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);

        }
        void ModifyRects()
        {
            Lworld[0].rcsrc = new Rectangle(XScroll, YScroll, ClientSize.Width, ClientSize.Height);
        }
        void CreateWorld()
        {
            CActor pnn = new CActor();
            pnn.img = new Bitmap("map//bk1.png");
            pnn.rcdst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            pnn.rcsrc = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Lworld.Add(pnn);
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.Gray);
            SolidBrush b = new SolidBrush(Color.Black);


            for (int i = 0; i < Lworld.Count; i++)
            {
                g.DrawImage(Lworld[i].img, Lworld[i].rcdst, Lworld[i].rcsrc, GraphicsUnit.Pixel);
            }

            for (int i = 0; i < Lcoins.Count; i++)
            {
                g.DrawImage(Lcoins[i].img, Lcoins[i].X, Lcoins[i].Y);
            }

            for (int i = 0; i < Lenemies.Count; i++)
            {
                if (Lenemies[i].canshootlaser && Lenemies[i].shoot)
                {
                    g.DrawLine(new Pen(Color.Yellow, 10), Lenemies[i].X + Lenemies[i].img.Width / 2 - 5, Lenemies[i].Y + Lenemies[i].img.Height / 2, Lenemies[i].X + Lenemies[i].img.Width / 2 - 5, Lenemies[i].Y -80+ Lenemies[i].img.Height + Lenemies[i].laserrange);
                }
            }


            for (int i = 0; i < Lenemies.Count; i++)
            {
                g.DrawImage(Lenemies[i].img, Lenemies[i].X, Lenemies[i].Y, 72, 80);
            }

            for (int i = 0; i < LMap.Count; i++)
            {
                g.DrawImage(LMap[i].img, LMap[i].X, LMap[i].Y);
            }



            for (int i = 0; i < Lladder.Count; i++)
            {
                g.DrawImage(Lladder[i].img, Lladder[i].X, Lladder[i].Y);
            }


            for (int i = 0; i < Lelev.Count; i++)
            {
                g.DrawImage(Lelev[i].img, Lelev[i].X, Lelev[i].Y);
            }

            for (int i = 0; i < LBullet.Count; i++)
            {
                g.DrawImage(LBullet[i].img, LBullet[i].X, LBullet[i].Y);
            }

            g.DrawImage(Player.img, Player.X, Player.Y, 72, 80);
            

            for (int i = 0; i < health; i++)
            {
                g.DrawImage(heart, 1 * (50 * i), 0, 50, 50);
            }

            Font drawFont = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            g.DrawString("Coins: " + coins + "/10", drawFont, drawBrush, this.ClientSize.Width / 2, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
