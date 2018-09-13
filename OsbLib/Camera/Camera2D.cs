namespace Milkitic.OsbLib.Camera
{
    class Camera2D
    {
        private Element[] Objects;
        public Element[] NewObjects { get; set; }
        private int fps = 15;


        public Camera2D(params Element[] Objects)
        {
            this.Objects = Objects;
        }

        public void Rotate(float degree, int starttime, int endtime, int easing = 0)
        {
            int lasttime = endtime - starttime;
            int totalframes = lasttime / 1000 * fps;
            foreach (var obj in Objects)
            {
                //List<ActionClass.ActionDouble> mv = obj.MoveList;
                //obj.MoveList.listDouble.Clear();
                //for (int j = 0; j < mv.Count; j++)
                //{
                //    for (int i = 0; i < totalframes; i++)
                //    {
                //        float Ddegree = degree / (float)totalframes;
                //        //obj.Move.Add( obj.Move[j].X1 * Math.Cos(Ddegree) - obj.Move[j].Y1 * Math.Sin(Ddegree);
                //        //obj.Move[j].Y1 = obj.Move[j].X1 * Math.Sin(Ddegree) + obj.Move[j].Y1 * Math.Cos(Ddegree);
                //        //Console.WriteLine("{" + obj.X1 + "," + obj.Y1 + "}");
                //    }
                //}
            }
        }

        public int Fps { get { return fps; } set { fps = value; } }
    }
}
