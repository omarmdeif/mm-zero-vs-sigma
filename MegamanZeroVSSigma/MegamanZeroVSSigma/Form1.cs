// important roles
/*scroll - w obsticales
elevator
laser
good looking map
enemy diff funcs
hero multi attack*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegamanZeroVSSigma
{
    public partial class Form1 : Form
    {
        public class slice
        {
            public Rectangle rcDst;
            public Rectangle rcSrc;
        }
        public class elevator
        {
            public int x, y;
            public Hero zero;
            public Bitmap img;
            public Boolean hasHero = false;

        }
        public class bullet
        {
            public int x, y, state = 0;
            public List<Bitmap> img = new List<Bitmap>();
            public Boolean facingRight = false;
        }
        public class character {
            public int x, y, state = 0, moving = 1;
            public Boolean facingRight = true, rotated = false, ele = false;
            public List<Bitmap> img = new List<Bitmap>();
            public List<bullet> myb = new List<bullet>();
        }
        public class Hero : character
        {
            public Boolean r = false, l = false, u = false, d = false, s = false, sho = false, ele = false;
        }
        public class enemytyperegular : character
        {
        }
        public class Level
        {
            public Bitmap bg, pf;
            public int xpf, ypf;
        }
        public enemytyperegular bat = new enemytyperegular();
        public enemytyperegular smash = new enemytyperegular();
        public int mr = 0, ml = 0;
        public Timer tt = new Timer();
        public int ree = 0, lee = 0, ctrele = 0;
        public Boolean goingRight = false, goingLeft = false;
        public List<slice> slices = new List<slice>();
        public Hero zero = new Hero();
        public elevator ele = new elevator();
        Level l1 = new Level();
        Bitmap off, big;
        public Form1()
        {
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.WindowState = FormWindowState.Maximized;
            tt.Tick += Tt_Tick;
            tt.Start();
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            //if()
            for (int i = 0; i < zero.myb.Count; i++)
            {
                if (zero.myb[i].facingRight)
                {
                    zero.myb[i].x += 40;
                }
                else
                {
                    zero.myb[i].x -= 40;
                }
            }
            if (ele.hasHero)
            {
                if (ele.y > (ClientSize.Height / 2) - (ele.img.Height) && ele.y <= l1.ypf - ele.img.Height && ele.hasHero)
                {
                    if ((ele.y - 20) < (ClientSize.Height / 2) - (ele.img.Height))
                    {
                        ele.y = (ClientSize.Height / 2) - (ele.img.Height);
                        zero.y = ele.y + 76 - zero.img[0].Height;
                        zero.ele = false;
                        ele.hasHero = false;
                    }
                    else
                    {
                        ele.y -= 20;
                        zero.y -= 20;
                    }
                }
                else if (ele.y < l1.ypf - ele.img.Height && ele.hasHero)
                {
                    if ((ele.y + 20) > l1.ypf - ele.img.Height)
                    {
                        ele.y = l1.ypf - ele.img.Height;
                        zero.y = ele.y + 76 - zero.img[0].Height;
                        zero.ele = false;
                        ele.hasHero = false;
                    }
                    else
                    {

                        ele.y += 20;
                        zero.y += 20;
                    }
                }

            }
            movehero();
            movebat();
            movesmash();
            dd(this.CreateGraphics());
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    zero.r = false;
                    zero.state = 0;
                    zero.y = l1.ypf - zero.img[zero.state].Height;
                    break;
                case Keys.Left:
                    zero.l = false;
                    zero.state = 0;
                    zero.y = l1.ypf - zero.img[zero.state].Height;
                    break;
                case Keys.Up:
                    zero.u = false;
                    zero.d = true;
                    zero.state = 4;
                    break;
                case Keys.Z:
                    zero.s = false;
                    zero.state = 0;
                    zero.y = l1.ypf - zero.img[zero.state].Height;
                    break;
                case Keys.X:
                    zero.sho = false;
                    zero.state = 0;
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && !zero.ele)
            {
                slices = null;
                createslice();
                slice pnn = new slice();
                pnn.rcDst = slices[slices.Count - 1].rcDst;
                //pnn.rcSrc = slices[slices.Count - 1].rcSrc;
                for (int i = slices.Count - 1; i > 0; i--)
                {
                    //slices[i].rcSrc = slices[i - 1].rcSrc;
                    slices[i].rcDst = slices[i - 1].rcDst;
                }
                slices[0].rcDst = pnn.rcDst;
                //slices[0].rcDst.X = slices[1].rcDst.X - 40;
                zero.facingRight = false;
                zero.l = true;
            }
            if (e.KeyCode == Keys.Right && !zero.ele)
            {
                slices = null;
                createslice();
                slice pnn = new slice();
                pnn.rcDst = slices[0].rcDst;
                //pnn.rcSrc = slices[0].rcSrc;
                for (int i = 0; i < slices.Count - 1; i++)
                {
                    //slices[i].rcSrc = slices[i + 1].rcSrc;
                    slices[i].rcDst = slices[i + 1].rcDst;
                }
                slices[slices.Count - 1].rcDst = pnn.rcDst;
                //slices[slices.Count - 1].rcDst.X = slices[slices.Count - 2].rcDst.X + 40;
                zero.facingRight = true;
                zero.r = true;
            }
            if (e.KeyCode == Keys.Up && !zero.ele)
            {
                int x = 0, y = 0, w = ClientSize.Width, h = 40, xd;
                slices = null;
                slices = new List<slice>();
                for (int i = 0; i <= ClientSize.Height / 40; i++)
                {
                    slice pnn = new slice();
                    pnn.rcDst = new Rectangle(x, y, w, h);
                    pnn.rcSrc = new Rectangle(x, y, w, h);
                    y += 40;
                    slices.Add(pnn);
                }
                slice pnn0 = new slice();
                pnn0.rcDst = slices[0].rcDst;
                //pnn.rcSrc = slices[0].rcSrc;
                for (int i = 0; i < slices.Count - 1; i++)
                {
                    //slices[i].rcSrc = slices[i + 1].rcSrc;
                    slices[i].rcDst = slices[i + 1].rcDst;
                }
                slices[slices.Count - 1].rcDst = pnn0.rcDst;
                zero.u = true;
            }
            if (e.KeyCode == Keys.Z && !zero.ele)
            {
                zero.s = true;
            }
            if (e.KeyCode == Keys.X && !zero.ele)
            {
                zero.sho = true;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if(zero.x + zero.img[zero.state].Width > ele.x && zero.x < ele.x + ele.img.Width
                    && zero.y >= ele.y && zero.y < ele.y + ele.img.Height){ zero.ele = true;
                    ele.hasHero = true; }
                
            }

            //dd(this.CreateGraphics());
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            dd(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            createLevel();
            createsmash();
            createbat();
            createZero();
            createslice();
            createelevator();
            dd(this.CreateGraphics());
        }
        void createelevator()
        {
            ele.img = new Bitmap(@"U:\project\e1.bmp");
            ele.x = ClientSize.Width / 2 - ele.img.Width / 2;
            ele.y = l1.ypf - ele.img.Height;
            ele.zero = null;
        }
        void movebat()
        {
            if (zero.x > bat.x)
            {
                bat.x += 20;
            }
            else
            {
                bat.x -= 20;
            }
            if (zero.x >= bat.x - 10 && zero.x <= bat.x + 10 && bat.state == 4)
            {
                for (int i = 0; i < 2; i++)
                {
                    Bitmap img = new Bitmap(@$"U:\project\bb{i + 1}.bmp");
                    bullet pnn = new bullet();
                    pnn.x = bat.x + bat.img[4].Width / 2;
                    pnn.y = bat.y + bat.img[4].Height;
                    pnn.img.Add(img);
                    bat.myb.Add(pnn);
                }
            }
            if (bat.facingRight)
            {
                bat.state += 1;
                if (bat.state == 4)
                {
                    bat.facingRight = false;
                }
            }
            else
            {
                bat.state -= 1;
                if (bat.state == 0)
                {
                    bat.facingRight = true;
                }
            }

        }
        void movebatbull()
        {
            for (int i = 0; i < bat.myb.Count; i++)
            {
                if (bat.myb[i].y < l1.ypf - bat.myb[i].img[0].Height)
                {
                    bat.myb[i].y += 10;
                }
                if (bat.myb[i].state == 0) {
                    bat.myb[i].state = 1;
                }
                else
                {
                    bat.myb[i].state = 0;
                }
            }
        }
        void movehero()
        {
            if (zero.r)
            {
                zero.x += 20;
                zero.state = zero.moving;
                zero.y = l1.ypf - zero.img[zero.state].Height;
                if (zero.facingRight)
                {
                    if (zero.rotated)
                    {
                        for (int i = 0; i < zero.img.Count; i++)
                        {
                            zero.img[i].RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        zero.rotated = false;
                    }
                }

                if (zero.moving == 1)
                {
                    zero.moving = 2;
                }
                else
                {
                    zero.moving = 1;
                }
            }
            if (zero.u)
            {
                zero.y -= 20;
                zero.state = 3;
                if (zero.s)
                {
                    zero.state = 7;


                }

            }
            if (zero.d)
            {
                zero.y += 20;
                if (zero.y + zero.img[zero.state].Height >= l1.ypf - 10)
                {
                    zero.d = false;

                    zero.state = 0;
                    zero.y = l1.ypf - zero.img[zero.state].Height;
                }
            }
            if (zero.s && !zero.u)
            {

                zero.state = 6;
                zero.y = l1.ypf - zero.img[zero.state].Height;


            }
            if (zero.l)
            {
                zero.x -= 20;
                zero.state = zero.moving;
                zero.y = l1.ypf - zero.img[zero.state].Height;
                if (!zero.facingRight)
                {
                    if (!zero.rotated) {
                        for (int i = 0; i < zero.img.Count; i++)
                        {
                            zero.img[i].RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        zero.rotated = true;
                    }
                }
                if (zero.moving == 1)
                {
                    zero.moving = 2;
                }
                else
                {
                    zero.moving = 1;
                }
            }
            if (zero.sho)
            {
                //5 - 20
                if (zero.state == 0)
                {
                    zero.state = 5;

                    //for (int i = 0; i < 4; i++)
                    //{
                    zero.y = l1.ypf - zero.img[zero.state].Height;
                    if (zero.state == 5)
                    {
                        bullet pnn = new bullet();
                        pnn.y = zero.y;
                        pnn.img.Add(new Bitmap(@"U:\project\bullet.bmp"));
                        if (zero.rotated)
                        {
                            pnn.facingRight = false;
                            pnn.x = zero.x - (zero.img[zero.state].Width / 16);
                            pnn.img[0].RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        else
                        {
                            pnn.x = zero.x + (zero.img[zero.state].Width / 4);
                            pnn.facingRight = true;
                        }
                        zero.myb.Add(pnn);

                    }

                    //}
                }
            }
        }
        void movesmash()
        {
            if (zero.x > smash.x)
            {
                smash.x += 20;
            }
            else
            {
                smash.x -= 20;
            }
            if (smash.facingRight)
            {
                smash.state += 1;
                if (smash.state == 3)
                {
                    smash.facingRight = false;
                }
            }
            else
            {
                smash.state -= 1;
                if (smash.state == 0)
                {
                    smash.facingRight = true;
                }
            }
            smash.y = l1.ypf - smash.img[smash.state].Height;
        }
        void createsmash()
        {
            for (int i = 0; i < 4; i++)
            {
                smash.x = ClientSize.Width / 5 * 4;
                smash.state = 0;
                Bitmap img = new Bitmap(@$"U:\project\ms{1 + i}.bmp");
                smash.img.Add(img);
                smash.y = l1.ypf - smash.img[smash.state].Height;
            }
            if (smash.x > zero.x)
            {
                for (int i = 0; i < smash.img.Count; i++)
                {
                    smash.img[i].RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
            }
        }
        void createLevel()
        {
            l1.bg = new Bitmap(@"U:\project\bg1.bmp");
            l1.pf = new Bitmap(@"U:\project\pf.bmp");
            l1.xpf = 0;
            l1.ypf = ClientSize.Height - l1.pf.Height;
            big = new Bitmap(l1.bg, ClientSize.Width, ClientSize.Height);
        }
        void createslice()
        {
            int x = 0, y = 0, w = 40, h = ClientSize.Height, xd;
            for (int i = 0; i <= ClientSize.Width / 40; i++)
            {
                slice pnn = new slice();
                pnn.rcDst = new Rectangle(x, y, w, h);
                pnn.rcSrc = new Rectangle(x, y, w, h);
                x += 40;
                ctrele++;
                slices.Add(pnn);
            }
        //ree = ctrele;
        }
        void createbat()
        {
            Bitmap img;
            bat.x = ClientSize.Width / 2;
            for (int i = 0; i < 5; i++)
            {
                img = new Bitmap(@$"U:\project\b{1 + i}.bmp");
                bat.img.Add(img);
            }
            bat.y = bat.img[4].Height;
        }
        void createZero()
        {
            //idle =  0
            Bitmap img = new Bitmap(@"U:\project\z.bmp");
            zero.img.Add(img);
            //moving  = 1~2
            for (int i = 0; i < 2; i++)
            {
                img = new Bitmap(@$"U:\project\zm{i + 1}.bmp");
                zero.img.Add(img);
            }
            //juming = 3~4
            for (int i = 0; i < 2; i++)
            {
                img = new Bitmap(@$"U:\project\zj{i + 1}.bmp");
                zero.img.Add(img);
            }
            //shooting = 5
            img = new Bitmap(@"U:\project\zb.bmp");
            zero.img.Add(img);
            //sword idle = 6
            img = new Bitmap(@"U:\project\zs.bmp");
            zero.img.Add(img);

            //sword juming = 7

            img = new Bitmap(@$"U:\project\zjs.bmp");
            zero.img.Add(img);

            zero.y = l1.ypf - zero.img[0].Height;
            zero.x = 150;
        }
        //public Bitmap MirrorImage(Bitmap source)
        //{
        //    Bitmap mirrored = new Bitmap(source.Width, source.Height);
        //    mirrored.RotateFlip(RotateFlipType.RotateNoneFlipY);
        //    return mirrored;
        //}
        void dd(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            d(g2);
            g.DrawImage(off, 0, 0);
        }
        void d(Graphics g)
        {
            g.Clear(Color.Black);
            for (int i = 0; i < slices.Count; i++)
            {
                g.DrawImage(big, slices[i].rcDst, slices[i].rcSrc, GraphicsUnit.Pixel);
            }

            //g.DrawImage(l1.bg, 0, 0, ClientSize.Width, ClientSize.Height);
            l1.pf.MakeTransparent(l1.pf.GetPixel(0, 0));
            g.DrawImage(l1.pf, l1.xpf, l1.ypf, ClientSize.Width, l1.pf.Height);
            smash.img[smash.state].MakeTransparent(smash.img[smash.state].GetPixel(0, 0));
            g.DrawImage(smash.img[smash.state], smash.x, smash.y);
            if (!zero.ele)
            {
                zero.img[zero.state].MakeTransparent(zero.img[zero.state].GetPixel(0, 0));
                g.DrawImage(zero.img[zero.state], zero.x, zero.y);
                for (int i = 0; i < zero.myb.Count; i++)
                {
                    zero.myb[i].img[0].MakeTransparent(zero.myb[i].img[0].GetPixel(0, 0));
                    g.DrawImage(zero.myb[i].img[0], zero.myb[i].x, zero.myb[i].y);
                }
            }
            bat.img[bat.state].MakeTransparent(bat.img[bat.state].GetPixel(0, 0));
            g.DrawImage(bat.img[bat.state], bat.x, bat.y);
            for (int i = 0; i < bat.myb.Count; i++)
            {
                bat.myb[i].img[bat.myb[i].state].MakeTransparent(bat.myb[i].img[bat.myb[i].state].GetPixel(0, 0));
                g.DrawImage(bat.myb[i].img[bat.myb[i].state], bat.myb[i].x, bat.myb[i].y);
            }
            ele.img.MakeTransparent(ele.img.GetPixel(0, 0));
            g.DrawImage(ele.img, ele.x, ele.y);
            if (ele.hasHero)
            {
                int yy = ele.y + 76;
                yy -= (zero.img[0].Height);
                int xx = ele.x + ele.img.Width / 2;
                xx -= (zero.img[0].Width / 2);
                zero.img[0].MakeTransparent(zero.img[0].GetPixel(0, 0));
                g.DrawImage(zero.img[0], xx, yy);
            }
        }
    }
}
