using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tet
{


    public partial class Form1 : Form
    {
        
        int[,] grid = new int[20, 10];
        Control[,] labels = new Control[20,10];
        int[,,] dat = new int[4, 4, 2];
        int[,] edgedat = new int[4,4];
        int dir;
        int[] spawn;
        int[] loc;
        int[] prevloc = new int[2];
        Color basecolor;
        Color current;
        Color[] colorlist = new Color[7] { System.Drawing.Color.Red,
                                           System.Drawing.Color.Blue,
                                           System.Drawing.Color.Green,
                                           System.Drawing.Color.Yellow,
                                           System.Drawing.Color.Orange,
                                           System.Drawing.Color.BlueViolet,
                                           System.Drawing.Color.Brown,};
        int[,,] block = new int[7, 4, 2] { { {0, 0 }, {0, 2 }, {0, -1 }, {0, 1 } },
                                           { {0, 0 }, {0, 1 }, {0, -1 }, {1, 1 } },
                                           { {0, 0 },{0, 1 },{0, -1 },{1, -1 } },
                                           { {0, 0 },{0, 1 },{1, 0 },{1, 1 }},
                                           { {0, 0 },{0, 1 },{1, -1 },{1, 0 }},
                                           { {0, 0 },{0, 1 },{0, -1 },{1, 0 }},
                                           { {0, 0 },{1, 1 },{0, -1 },{1, 0 }} };
        Random rand = new Random();
        private int[,,] CreateBlock()
        {
            int x = rand.Next(7);
            int[,,] dat = new int[4, 4, 2];
            for (int i = 0; i < 8; i++)
            {
                dat[0, i / 2, i % 2] = block[x, i / 2, i % 2];
            }


            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    dat[i, j, 0] = dat[i - 1, j, 1];
                    dat[i, j, 1] = -dat[i - 1, j, 0];
                }
            }
            return dat;
        }
        private int[,] EdgeData(int[,,] dat)
        {
            int[,] edgedat = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //0 = 아래 1 = 위 2 = 오른쪽 3 = 왼쪽
                    edgedat[i, 0] = (dat[i, j, 0] > edgedat[i, 0]) ? dat[i, j, 0] : edgedat[i, 0];
                    edgedat[i, 1] = (dat[i, j, 0] < edgedat[i, 1]) ? dat[i, j, 0] : edgedat[i, 1];
                    edgedat[i, 2] = (dat[i, j, 1] > edgedat[i, 2]) ? dat[i, j, 1] : edgedat[i, 2];
                    edgedat[i, 3] = (dat[i, j, 1] < edgedat[i, 3]) ? dat[i, j, 1] : edgedat[i, 3];
                }
            }
            return edgedat;
        }
        private int[] makespawn(int[,] e, int di)
        {
            return (new int[2] { 0, rand.Next(-e[di, 3], 10 - e[di, 2]) });
        }
        private bool check(int[,,] d, int di, int[] l, int[,] e)
        {
            for (int i = 0; i < 4; i++)
            {
                if (((l[0] + d[di, i, 0]) > 19) || ((l[1] + d[di, i, 1]) > 9) || ((l[1] + d[di, i, 1]) < 0))
                {
                    return false;
                }
                else if (grid[l[0] + d[di, i, 0], l[1] + d[di, i, 1]] == 1)
                {
                    return false;
                }
            }
            return true;
        }
        private void render(int[,,] d,int di, int[] l, int[] pl)
        {
            for (int i = 0; i < 4; i++)
            {
                if ((pl[0] + d[pl[2], i, 0]) >= 0)
                {
                    labels[pl[0] + d[pl[2], i, 0], pl[1] + d[pl[2], i, 1]].BackColor = basecolor;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if ((l[0] + d[di, i, 0]) >= 0)
                {
                    labels[l[0] + d[di, i, 0], l[1] + d[di, i, 1]].BackColor = current;
                }
            }
        }
        private void baserender()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (grid[i, j] == 1)
                    {
                        continue;
                    }
                    else
                    {
                        labels[i, j].BackColor = basecolor;
                    }
                }
            }
        }
        private bool detect(int[,,] d, int di, int[] l, int[,] e)
        {
            for(int i = 0; i < 4; i++)
            {
                if((l[0] + d[di, i, 0]) < 0) {
                    continue;
                }
                if ((l[0] + d[di, i, 0]) >= 19)
                {
                    return true;
                }else if(grid[l[0] + d[di, i, 0]+1, l[1] + d[di, i, 1]] == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private bool linechecksub(int x)
        {
            for(int i = 0; i < 10; i++)
            {
                if (grid[x, i] != 1)
                {
                    return false;
                }
            }
            return true;
        }
        private void linecheck()
        {
            for(int i = 19; i >= 0; i--)
            {
                if (linechecksub(i))
                {
                    for(int j = i; j > 0; j--)
                    {
                        for(int k = 0; k < 10; k++)
                        {
                            grid[j, k] = grid[j - 1, k] * 1;
                            labels[j, k].BackColor = labels[j - 1, k].BackColor;
                        }
                    }
                    i++;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            for(int i = 0; i < 20; i++)
            {
                for(int j =0; j < 10; j++)
                {
                    grid[i, j] = 0;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    labels[i, j] = Controls.Find("label"+ (1 + j + (i * 10)),true)[0];
                }
            }
            basecolor = labels[0,0].BackColor;
            current = colorlist[rand.Next(7)];
            dat = CreateBlock();
            edgedat = EdgeData(dat);
            dir = 0;
            spawn = makespawn(edgedat,dir);
            loc = spawn;
            prevloc = new int[3] {loc[0],loc[1],dir*1};
            baserender();
            render(dat, dir, loc, prevloc);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (detect(dat,dir,loc,edgedat))
            {
                for(int i = 0; i < 4; i++)
                {
                    grid[loc[0]+dat[dir,i,0],loc[1]+dat[dir,i,1]] = 1;
                }
                baserender();
                linecheck();
                baserender();
                current = colorlist[rand.Next(7)];
                dat = CreateBlock();
                edgedat = EdgeData(dat);
                dir = 0;
                spawn = makespawn(edgedat, dir);
                loc = spawn;
                prevloc = new int[3] { loc[0], loc[1], dir*1 };
                baserender();
                render(dat, dir, loc, prevloc);
            }
            else
            {
                loc[0] += 1;
                render(dat,dir,loc,prevloc);
                prevloc = new int[3] { loc[0], loc[1], dir*1 };
            }
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if(check(dat,dir,new int[2]{loc[0],loc[1]+1 }, edgedat))
                    {
                        loc[1] += 1;
                        render(dat, dir, loc, prevloc);
                        prevloc = new int[3] { loc[0], loc[1],dir*1 };
                    }
                    break;
                case Keys.Up:
                    if (check(dat, (dir+1)%4, new int[2] { loc[0], loc[1] - 1 }, edgedat))
                    {
                        dir = (dir + 1) % 4;
                        render(dat, dir, loc, prevloc);
                        prevloc = new int[3] { loc[0], loc[1],dir*1 };
                    }
                    break;
                case Keys.Left:
                    if (check(dat, dir, new int[2] { loc[0], loc[1] - 1 }, edgedat))
                    {
                        loc[1] -= 1;
                        render(dat, dir, loc, prevloc);
                        prevloc = new int[3] { loc[0], loc[1],dir*1 };
                    }
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                timer1.Interval = 50;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            timer1.Interval = 500;
        }
    }       

}
