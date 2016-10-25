using System;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
class man
{
    int pic = 1;
    public int jumping=0;
    public int fall = 0;
    public Bitmap image = new Bitmap(Image.FromFile("1.bmp"));
    public int life=100;
    public void draw(Bitmap bit)
    {
        image.MakeTransparent(Color.White);
        Graphics.FromImage(bit).DrawImage(image, new Rectangle(300, 100, 50, 50));
        if (jumping > 0)
        { image = new Bitmap(Image.FromFile("jump.bmp")); jumping--; }
        else if (fall > 0)
        { image = new Bitmap(Image.FromFile("fall.bmp")); fall--; }
        else
            image = new Bitmap(Image.FromFile(pic++ % 3 + 1 + ".bmp"));
        Graphics.FromImage(bit).DrawString("生命值:" + life, new Font("黑体", 20), new SolidBrush(Color.Black), new Point(350, 0));
    }
}
class hurdle
{
    public int x;
    Bitmap b=new Bitmap(10,20);
    public hurdle next;
    public bool passed;
    public hurdle()
    {
        passed = false;
        x = 599;
        next = null;
        Graphics.FromImage(b).Clear(Color.Black);
    }
    public void draw(Bitmap bit)
    {
        x-=10;
        Graphics.FromImage(bit).DrawImage(b, x, 130);
    }
}
class haha : Form
{
    static void Main()
    {
        Application.Run(new haha());
    }
    Bitmap bit;
    man me = new man();
    hurdle h;
    Random r = new Random();
    Timer t = new Timer();
    haha()
    {
        Text = "快点跑呀";
        ClientSize = new Size(600, 200);
        KeyDown += down;
        bit = new Bitmap(ClientSize.Width, ClientSize.Height);
        t.Tick += draw;
        init();
        MaximizeBox = false;
        MinimizeBox = false;
        //Opacity = 0.8;
    }
    void init()
    {
        t.Interval = 100;
        t.Start();
        h = new hurdle();
        me.life = 100;
    }
    void check()
    {
        hurdle i;
        for (i = h; i != null; i = i.next)
        {
            if (i.x < 350 && !i.passed)
            {
                i.passed = true;
                if (me.jumping <= 0)
                {
                    me.life -= 20;
                    me.fall = 10;
                    if(t.Interval<200)t.Interval += 10;
                    new SoundPlayer("wolf.wav").Play();
                }
                else
                {
                    me.life += 10;
                    if (t.Interval <= 50) t.Interval = 45;
                    t.Interval -= 10;
                    new SoundPlayer("lion.wav").Play();
                }
            }
        } 
    }
    void failed()
    {
        t.Stop();
        DialogResult result = MessageBox.Show("you failed the game!","OVER",MessageBoxButtons.RetryCancel);
        if (result == DialogResult.Cancel) Application.Exit();
        else   init(); 
    }
    void draw(object o, EventArgs e)
    {
        if (me.life < 0) failed();
        check();
        Graphics.FromImage(bit).Clear(Color.Green);
        Graphics.FromImage(bit).DrawLine(new Pen(Color.Black, 5), new Point(0, 150), new Point(600, 150));
        hurdle i;
        if (h.x < 10) h = h.next;
        for ( i = h; i != null; i = i.next) i.draw(bit);
        if (h == null) { h = new hurdle(); h.next = null; }
        me.draw(bit);
        for (i = h; i.next != null; i = i.next) ;
        if (r.Next() % 20== 0) i.next = new hurdle();
        CreateGraphics().DrawImage(bit, 0, 0);
    }
    void down(object o, KeyEventArgs e)
    {
        me.life -= 5;
        switch (e.KeyCode)
        {
            case Keys.Space: me.jumping = 500/t.Interval;
                break;
            case Keys.F1: t.Stop(); help(); t.Start(); break;
        }
    }
    void help()
    {
        MessageBox.Show("Made by weidiao.neu");
    }
}