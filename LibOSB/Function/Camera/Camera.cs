using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB.Function.Camera
{
    class Camera
    {
        private SBObject[] Objects;
        public SBObject[] NewObjects { get; set; }
        private int fps = 15;


        public Camera(params SBObject[] Objects)
        {
            this.Objects = Objects;
        }

        public void Rotate(double degree, int starttime, int endtime, int easing = 0)
        {
            int lasttime = endtime - starttime;
            int totalframes = lasttime / 1000 * fps;
            foreach (var obj in Objects)
            {
                List<ActionTypes.Move> mv = obj._Move.M.ToList();
                obj._Move.M.Clear();
                for (int j = 0; j < mv.Count; j++)
                {
                    for (int i = 0; i < totalframes; i++)
                    {
                        double Ddegree = degree / (double)totalframes;
                        //obj.Move.Add( obj.Move[j].X1 * Math.Cos(Ddegree) - obj.Move[j].Y1 * Math.Sin(Ddegree);
                        //obj.Move[j].Y1 = obj.Move[j].X1 * Math.Sin(Ddegree) + obj.Move[j].Y1 * Math.Cos(Ddegree);
                        //Console.WriteLine("{" + obj.X1 + "," + obj.Y1 + "}");
                    }
                }
            }
        }

        public int Fps { get { return fps; } set { fps = value; } }
    }
}
