using System;
using System.IO;

namespace Milkitic.OsbLib
{
    static class Demo
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText(
                @"D:\Program Files (x86)\osu!\Songs\455780 Kano - Sakura no Zenya\Kano - Sakura no Zenya (yf_bmp).osb");
            ElementGroup sb = ElementGroup.Parse(text, 0);
            Console.WriteLine(sb);
            Console.Read();
        }
    }
}
