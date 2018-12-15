using Milkitic.OsbLib.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MGLib.Osu.Reader.Osb
{
    public struct Element
    {
        public int ElementType; // 0 - sprite, 1 - animation
        public LayerType Layer;
        public OriginType Origin;
        public string TexturePath;
        public (int StartTime, int EndTime) Position;
        //for Animation
        public int FrameCount;
        public int FrameDelay;
        public LoopType LoopType;
        //Commands
        public IEnumerable<Command> Commands;
    }

    public struct Command
    {
        public EventEnum CommandType;
        public EasingType EasingType;
        public (int StartTime, int EndTime) Time;
        public IEnumerable<float> Params;
        public IEnumerable<Command> SubCommands;
        //For Loop Command
        public int LoopCount;
        //For Trigger
        public string Trigger;
    }

    public class OsbElementList : IEnumerable<Element>
    {
        private readonly string FilePath;
        private StreamReader Reader;
        private readonly bool IsLargeOSB;
        public OsbElementList(string filePath)
        {
            FilePath = filePath;
            Reader = new StreamReader(new FileStream(filePath, FileMode.OpenOrCreate));
            if (Reader.BaseStream.Length > 5000000) IsLargeOSB = true;
        }

        private bool IsLf()
        {
            if (PeekChar() == '\r') SkipChar();
            return PeekChar() == '\n';
        }

        private void SkipChar() => Reader.Read();

        private char ReadChar()
        {
            return (char)Reader.Read();
        }

        private char PeekChar()
        {
            return (char)Reader.Peek();
        }

        private string ReadLine()
        {
            return Reader.ReadLine();
        }

        private string ReadUntil(char ch, bool eat = false)
        {
            return ReadUntil((cur) => cur == ch);
        }

        private string ReadUntil(Func<char, bool> pred, bool eat = false)
        {
            StringBuilder sb = new StringBuilder();
            var current = PeekChar();
            while (!pred(current))
            {
                current = ReadChar();
                sb.Append(current);
                current = PeekChar();
            }
            if (eat) Reader.Read();
            return sb.ToString();
        }

        private void SkipUntil(Func<char, bool> pred, bool eat = false)
        {
            var current = PeekChar();
            while (!pred(current))
            {
                SkipChar();
                current = PeekChar();
            }
            if (eat) Reader.Read();
        }

        private void SkipWhile(Func<char, bool> pred, bool eat = false) => SkipUntil(ch => !pred(ch), eat);

        private string ReadWhile(Func<char, bool> pred, bool eat = false) => ReadUntil((ch) => !pred(ch), eat);

        private IEnumerable<char> StreamReadWhile(Func<char, bool> pred, bool eat = false) => StreamReadUntil(ch => !pred(ch), eat);

        private IEnumerable<char> StreamReadUntil(Func<char, bool> pred, bool eat = false)
        {
            var current = PeekChar();
            while (!pred(current))
            {
                current = ReadChar();
                yield return current;
                current = PeekChar();
            }
            if (eat) Reader.Read();
        }

        private string ReadLiteralString()
        {
            var current = PeekChar();
            //如果是'"'就需要读到下一个'"'
            if (current == '"')
            {
                //读到'"'并吃掉
                SkipChar();
                var result = ReadUntil('"', true);
                //再吃掉结尾的'"'
                SkipChar();
                return result;
            }
            if (!char.IsLetter(current)) return "";
            return ReadWhile(ch => char.IsLetterOrDigit(ch));
        }

        private bool VerifyEvents()
        {
            switch (ReadLiteralString())
            {
                case "Events":
                    return true;
            }
            return false;
        }

        //跳过其他内容找到[Events]这行
        private bool SeekToElementStart()
        {
            var current = Reader.Peek();
            switch (current)
            {
                case '[': //如果是[，证明是osb的开头
                    Reader.Read();
                    if (VerifyEvents()) return true;
                    break;
            }
            //其他情况就跳过这行
            ReadLine();
            return false;
        }

        private int ReadElementType()
        {
            int result = -1;
            switch (ReadLiteralString())
            {
                case "Animation":
                    result = 1;
                    break;
                case "Sprite":
                    result = 0;
                    break;
            }
            //吃掉','
            Reader.Read();
            return result;
        }

        private T EnumParseT<T>(string stringEnum)
        {
            return (T)Enum.Parse(typeof(T), stringEnum, true);
        }

        //参数中专用的Literal string reader，会吃掉下一个','(如果是的话
        private string ReadParamLiteralString()
        {
            string result = ReadLiteralString();
            if (PeekChar() == ',') Reader.Read();
            return result;
        }

        private int ReadInt()
        {
            int result = 0;
            int sng = 1;
            if (PeekChar() == '-')
            {
                SkipChar();
                sng = -1;
            }
            if (!(PeekChar() == ','))
            {
                //整数部分
                int integer = 0;
                integer = StreamReadUntil(ch => !char.IsDigit(ch))
                .Aggregate(integer, (val, ch) => 10 * val + (ch - 48));
                result = integer;
            }
            if (PeekChar() == ',') Reader.Read();
            return result * sng;
        }

        //参数中专用的float读取，支持省略
        private float ReadFloat()
        {
            //省略了这个参数
            float result = 0;
            float sng = 1;
            if (PeekChar() == '-')
            {
                SkipChar();
                sng = -1;
            }
            if (!(PeekChar() == ','))
            {
                //整数部分
                int integer = 0;
                integer = StreamReadUntil(ch => !char.IsDigit(ch))
                .Aggregate(integer, (val, ch) => 10 * val + (ch - 48));
                result = integer;
                //小数部分
                if (PeekChar() == '.')
                {
                    SkipChar(); //吃掉'.'
                    float dec = 0;
                    dec = StreamReadUntil(ch => !char.IsDigit(ch))
                    .Reverse()
                    .Aggregate(dec, (val, ch) => (val + (ch - 48) / 10));
                    result += dec;
                }
            }
            if (PeekChar() == '∞')
            {
                result = float.Parse("∞");
                SkipChar();
            }
            if (PeekChar() == ',') Reader.Read();
            return result * sng;
        }

        private IEnumerable<float> ReadParams()
        {
            List<float> list = new List<float>(5);
            //读到行尾
            while (!IsLf())
            {
                list.Add(ReadFloat());
            }
            return list;
        }

        private List<Command> ReadCommands()
        {
            var result = new List<Command>(IsLargeOSB ? 20000 : 100);
            //命令是空格开始的，如果不是空格，说明命令读完了。。
            while (PeekChar() == ' ')
            {
                SkipWhile(ch => ch == ' ');
                var type = ReadParamLiteralString().ToCommandType();
                //需要对Loop和Trigger特殊处理
                switch (type)
                {
                    case EventEnum.Loop:
                        result.Add(new Command
                        {
                            CommandType = type,
                            Time = (ReadInt(), 0),
                            LoopCount = ReadInt(),
                            //不能继续IEnumerable了，需要求值
                            SubCommands = ReadCommands(),
                        });
                        break;
                    case EventEnum.Trigger:
                        result.Add(new Command
                        {
                            CommandType = type,
                            Trigger = ReadParamLiteralString(),
                            Time = (ReadInt(), 0),
                            SubCommands = ReadCommands(),
                        });
                        break;
                    default:
                        result.Add(new Command
                        {
                            CommandType = type,
                            EasingType = (EasingType)ReadFloat(),
                            Time = (ReadInt(), ReadInt()),
                            Params = ReadParams(),
                        });
                        break;
                }
                ReadLine();
            }
            return result;
        }

        //跳过注释，找到元素的开始
        private Element ReadElement()
        {
            //Element基本信息
            var result = new Element
            {
                ElementType = ReadElementType(),
                Layer = EnumParseT<LayerType>(ReadParamLiteralString()),
                Origin = EnumParseT<OriginType>(ReadParamLiteralString()),
                TexturePath = ReadParamLiteralString(),
                Position = (ReadInt(), ReadInt()),

            };
            //如果是Animation，就把剩下三个参数读一读
            if (result.ElementType == 1)
            {
                //读到换行符为止
                if (!IsLf())
                {
                    result.FrameCount = ReadInt();
                    if (!IsLf())
                    {
                        result.FrameDelay = ReadInt();
                        if (!IsLf())
                        {
                            result.LoopType = EnumParseT<LoopType>(ReadParamLiteralString());
                        }
                    }
                }
            }
            ReadLine();
            //Commands需要立即求值，不允许IEnumerable
            result.Commands = ReadCommands();
            return result;
        }

        private IEnumerator<Element> ReadElements()
        {
            bool reading = true;
            var current = PeekChar();
            while (reading && !Reader.EndOfStream)
            {
                switch (current)
                {
                    //元素只能是'S'或者'A'开头
                    case 'S':
                    case 'A':
                        yield return ReadElement();
                        break;
                    //如果是'['说明读到了下一个Section
                    case '[':
                        reading = false;
                        break;
                    case '/':
                    default:
                        //注释是'/'开头，加上其他情况，跳过这行
                        ReadLine();
                        break;
                }
                current = PeekChar();
            }
        }

        public List<Element> Parse()
        {
            while (!SeekToElementStart()) ;
            ReadLine();
            List<Element> result = new List<Element>(IsLargeOSB ? 10000 : 100);
            bool reading = true;
            var current = PeekChar();
            while (reading && !Reader.EndOfStream)
            {
                switch (current)
                {
                    //元素只能是'S'或者'A'开头
                    case 'S':
                    case 'A':
                        result.Add(ReadElement());
                        break;
                    //如果是'['说明读到了下一个Section
                    case '[':
                        reading = false;
                        break;
                    case '/':
                    default:
                        //注释是'/'开头，加上其他情况，跳过这行
                        ReadLine();
                        break;
                }
                current = PeekChar();
            }
            return result;
        }

        public IEnumerator<Element> GetEnumerator()
        {
            while (!SeekToElementStart()) ;
            ReadLine();
            return ReadElements();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class TestOsbReader
    {
        OsbElementList elementList;
        public TestOsbReader()
        {
            elementList = new OsbElementList(@"z:\test.osb");
            foreach (var element in elementList)
            {
                //item 的 commands;
                foreach (var command in element.Commands)
                {
                    if (command.CommandType == EventEnum.Loop)
                    {
                        foreach (var subCommand in command.SubCommands)
                        {

                        }
                    }
                }
            }
        }
    }
}
