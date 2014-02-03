using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evac_Sim.AgentsLogic;
using Evac_Sim.WorldMap;
using System.Drawing;


namespace Evac_Sim.AppGUI
{
    internal class MapDrawing
    {
        private Pen pBlack;
        private Pen pBlue;

        private SolidBrush bWhite = new SolidBrush(Color.White),
            bGreen = new SolidBrush(Color.Green),
            bBlack = new SolidBrush(Color.Black),
            bRed = new SolidBrush(Color.Red);

        public int size;
        private Graph g;
        private Rectangle[] aRec;
        private Font font1 = new Font("Arial", 6, FontStyle.Bold, GraphicsUnit.Point);
        private Font font2 = new Font("Arial", 8, FontStyle.Underline, GraphicsUnit.Point);
        private float sizeF;
        private float sizeFH;
        private PointF[] ap;
        private Color[] currentColors;


        public MapDrawing(Graph g)
        {
            // grid = new List<Rectangle>();
            pBlack = new Pen(Color.Black, 0.01F);
            pBlue = new Pen(Color.Blue, 2);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(3, 5);
            pBlue.CustomEndCap = bigArrow;
            this.g = g;



            size = (int) Math.Round(Math.Min(((double) 800)/g.Height, ((double) 16000)/g.Width));

            if (size > 60)
                size = 60;

            // size = 20;

            sizeF = (float) size - 8;
            sizeFH = sizeF/2;
            PointF[] ap =
            {
                new PointF(sizeFH, sizeF), new PointF(sizeF, sizeF), new PointF(sizeF, sizeFH), new PointF(sizeF, 0),
                new PointF(sizeFH, 0), new PointF(0, 0), new PointF(0, sizeFH), new PointF(0, sizeF)
            };
            this.ap = ap;

            aRec = new Rectangle[g.GraphMap.Length];
            currentColors = new Color[g.GraphMap.Length];

            Rectangle r = new Rectangle();
            for (int i = 0; i < g.GraphMap.Length; i++)
            {
                State v = g.GraphMap[i];
                r = (new Rectangle((int) v.xLoc*size, (int) v.yLoc*size, size, size));
                aRec[i] = r;
            }

        }

        public Graphics zoomIn(ref Bitmap bm, bool indexing)
        {
            size++;

            if (size > 60)
                size = 60;

            bm = new Bitmap((int) g.Width*size + 20, (int) g.Height*size + 40);
            Graphics paper = Graphics.FromImage(bm);

            setSize(paper, indexing);

            return paper;
        }

        public Graphics zoomOut(ref Bitmap bm, bool indexing)
        {
            size--;

            if (size < 1)
                size = 1;


            bm = new Bitmap((int) g.Width*size + 20, (int) g.Height*size + 40);
            Graphics paper = Graphics.FromImage(bm);

            setSize(paper, indexing);

            return paper;

        }

        private void setSize(Graphics paper, bool indexing)
        {
            sizeF = (float) size - 8;
            sizeFH = sizeF/2;
            PointF[] ap =
            {
                new PointF(sizeFH, sizeF), new PointF(sizeF, sizeF), new PointF(sizeF, sizeFH), new PointF(sizeF, 0),
                new PointF(sizeFH, 0), new PointF(0, 0), new PointF(0, sizeFH), new PointF(0, sizeF)
            };
            this.ap = ap;

            Size s;
            for (int i = 0; i < aRec.Length; i++)
            {
                s = new Size(size, size);
                aRec[i].Size = s;
                aRec[i].X = (int) g.GraphMap[i].xLoc*size;
                aRec[i].Y = (int) g.GraphMap[i].yLoc*size;
            }
            rePaint(paper, indexing);
        }

        private void rePaint(Graphics paper, bool indexing)
        {
            paper.Clear(Color.Black);
            for (uint i = 0; i < aRec.Length; i++)
            {
                paper.FillRectangle(new SolidBrush(currentColors[i]), aRec[i]);
                DrawVector(paper, i, g.GraphMap[i].DVx, g.GraphMap[i].DVy);
                if (indexing)
                    printIndex(paper, i);
            }
            Rectangle r = new Rectangle();
            for (int i = 0; i < g.Blocked.Length; i++)
            {
                State v = g.Blocked[i];
                r = (new Rectangle((int) v.xLoc*size, (int) v.yLoc*size, size, size));
                paper.FillRectangle(bGreen, r);
            }
        }

        public void DrawMap(Graphics paper, bool indexing)
        {
            paper.Clear(Color.Black);
            for (uint i = 0; i < aRec.Length; i++)
            {

                paper.FillRectangle(bWhite, aRec[i]);
                currentColors[i] = Color.White;
                if (indexing)
                {
                    paper.DrawRectangle(pBlack, aRec[i]);
                    printIndex(paper, i);
                }
            }
            Rectangle r = new Rectangle();
            for (int i = 0; i < g.Blocked.Length; i++)
            {
                State v = g.Blocked[i];
                r = (new Rectangle((int) v.xLoc*size, (int) v.yLoc*size, size, size));
                paper.FillRectangle(bGreen, r);
            }
            //paper.DrawRectangles(p, aRec);
        }

        public State PaintRec(Graphics paper, Point cursorPos, Color c, bool indexing)
        {
            for (uint i = 0; i < aRec.Length; i++)
            {
                if (aRec[i].Contains(cursorPos))
                {
                    paper.FillRectangle(new SolidBrush(c), aRec[i]);
                    currentColors[i] = c;
                    if (indexing)
                        printIndex(paper, i);
                    return g.GraphMap[i];
                }
            }
            return null;
        }

        public bool IsStateFree(uint state)
        {
            return  currentColors[state] == Color.White;
        }

        public
            State getState(Graphics paper, Point cursorPos, Color c, bool indexing)
        {
            for (uint i = 0; i < aRec.Length; i++)
            {
                if (aRec[i].Contains(cursorPos))
                    return g.GraphMap[i];
            }
            return null;
        }



        public void PaintRec(Graphics paper, uint index, Color c, bool indexing)
        {
            paper.FillRectangle(new SolidBrush(c), aRec[index]);
            currentColors[index] = c;
            if (indexing)
            {
                paper.DrawRectangle(pBlack, aRec[index]);
                printIndex(paper, index);
            }
        }
        public void PaintEllipse(Graphics paper, uint index, Color c)
        {
            paper.FillEllipse(new SolidBrush(c),aRec[index]);
            currentColors[index] = c;
        }

        public void printIndex(Graphics paper, uint index)
        {
            paper.DrawString(index.ToString(), font2, bBlack, sizeFH + aRec[index].X - 5, sizeFH + aRec[index].Y);
            paper.DrawRectangle(pBlack, aRec[index]);
        }

        public void reIndex(Graphics paper, bool indexing)
        {
            if (!indexing)
                rePaint(paper, indexing);
            else
                for (uint i = 0; i < aRec.Length; i++)
                {
                    printIndex(paper, i);
                }
        }

        public void DrawLine(Graphics paper, uint fromRec, uint toRec)
        {
            paper.DrawLine(pBlue, aRec[fromRec].X + size/2, aRec[fromRec].Y + size/2, aRec[toRec].X + size/2,
                aRec[toRec].Y + size/2);
        }
        public Point CenterofRectangle(Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,
                             rect.Top + rect.Height / 2);
        }

        public void DrawVector(Graphics paper, uint placeState, double xdir,double ydir)
        {

            Point centerofRec = CenterofRectangle(aRec[placeState]);
            int lenx = (int)(centerofRec.X + xdir* sizeFH);
            int leny = (int)(centerofRec.Y + ydir* sizeFH);
            paper.DrawLine(pBlue, centerofRec.X, centerofRec.Y, lenx, leny);
            //paper.DrawString(index.ToString(), font2, bBlack, sizeFH + aRec[index].X - 5, sizeFH + aRec[index].Y);
           // paper.DrawLine(pBlue, aRec[fromRec]. + size / 2, aRec[fromRec].Y + size / 2, aRec[fromRec].X + size / 2,
            //    aRec[fromRec].Y + size / 2);
        }

        public void DrawLine(Graphics paper, Point from, Point to)
        {
            paper.DrawLine(pBlue, from, to);
        }
    }
}
