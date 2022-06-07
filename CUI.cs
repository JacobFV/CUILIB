using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUI
{
    public static class CUI
    {
        public static void wait()
        {
            Console.Read();
        }
        public static void write(string text)
        {
            Console.Write(text);
        }
        public static void writeLine(string text)
        {
            Console.WriteLine(text);
        }
        public static void writeTwoTexts(string left, string right)
        {
            write(left);
            for (int i = 0; i < Console.BufferWidth - left.Length - right.Length; i++)
            {
                write(" ");
            }
            write(right);
        }
        /*public static void getNumberList(ref List<number> inputNumbers, string prefix)
        {
            for (int numberNum = 0; numberNum < inputNumbers.Count; numberNum++)
            {
                inputNumbers[numberNum].value = getDouble(prefix + numberNum.ToString());
            }
        }*/
        public static void getDataList(string title, DataListItem[] items)
        {
            Action<string, DataListItem[]> updateScreen =
                (string updateTitle, DataListItem[] updateItems) =>
            {
                Console.Clear();
                if (title?.Length > 0)
                {
                    Console.Title = title;
                    Console.WriteLine(updateTitle);
                }
                foreach(DataListItem item in updateItems)
                {
                    Console.WriteLine(item.name + ":" + item.consoleValue);
                }
            };

            bool titleExists = title?.Length > 0;
            int itemIndexOfset = titleExists ? 1 : 0;
            int index = itemIndexOfset;
            int minIndex = itemIndexOfset;
            int maxIndex = items.Length + itemIndexOfset;

            while (true)
            {
                updateScreen(title, items);
                Console.SetCursorPosition(
                    items[index -itemIndexOfset].name.Length + 1,
                    index);
                string consoleValue = Console.ReadLine();
                if (consoleValue != null)
                {
                    items[index - itemIndexOfset].consoleValue = consoleValue;
                    index++;
                    if (index - itemIndexOfset >= items.Length)
                    {
                        return;
                    }
                }
            }

            /*OLD*/ /*
            while (true)
            {
                updateScreen(title, items);
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        index--;
                        if(index < minIndex)
                        {
                            index++;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        index++;
                        if (index > maxIndex)
                        {
                            index--;
                        }
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Tab:
                        index++;
                        if (index > maxIndex)
                        {
                            return;
                        }
                        break;
                    case ConsoleKey.Delete:
                        string value = items[index + itemIndexOfset].consoleValue;
                        if(value.Length > 0)
                        {
                            value = value.Substring(0, value.Length - 1);
                            items[index + itemIndexOfset].consoleValue = value;
                        }
                        break;
                    default:
                        items[index + itemIndexOfset].consoleValue += key;
                        break;
                }
            }
            */
            //handle keystrokes until last enter
        }
        public class DataListItem
        {
            public enum DataType { tString, tDouble, tInt }

            public DataListItem(string name, DataType type)
            {
                this.name = name;
                this.type = type;
                this.consoleValue = "";
            }

            public DataType type;
            public string name;
            public string consoleValue;
            public object getValue
            {
                get
                {
                    switch (type)
                    {
                        case DataType.tDouble:
                            double doubleValue = 0;
                            double.TryParse(consoleValue, out doubleValue);
                            return doubleValue;
                            break;
                        case DataType.tInt:
                            int intValue = 0;
                            int.TryParse(consoleValue, out intValue);
                            return intValue;
                            break;
                        case DataType.tString:
                            return consoleValue;
                            break;
                    }
                    throw new NotImplementedException();
                }
            }
        }
        public static double getDouble(string valueName)
        {
            double result = 0;
            double.TryParse(getText(valueName), out result);
            return result;
        }
        public static string getText(string valueName)
        {
            Console.Write(valueName + ":");
            return Console.ReadLine(); //Makke sure this works and doesn't return "value:1"
        }
        public static List<int> getIntArray(string valueName)
        {
            List<int> ints = new List<int>();
            List<string> strings = getText(valueName).Split(new char[2] { ',', ' ' }).ToList();
            foreach (string singleString in strings)
            {
                int i = 0;
                int.TryParse(singleString, out i);
                ints.Add(i);
            }
            return ints;
        }
        public static void optionList<T>(List<Action<T>> actions,
                                      T data, List<string> captions, string title)
        {
            int selectedItem = 0;
            while (true)
            {
                drawList(captions, selectedItem, title);
                selectedItem += upDownEnter(actions[selectedItem], data);
                limitRange(ref selectedItem, 0, actions.Count() - 1);
            }
        }
        static void drawList(List<string> captions, int selectedItem, string title)
        {
            Console.Clear();
            writeLine(title);
            for (int itemNum = 0; itemNum < captions.Count; itemNum++)
            {
                string startString = "  ";
                if (itemNum == selectedItem)
                {
                    startString = "> ";
                }
                writeLine(startString + captions[itemNum]);
                startString = "  ";
            }
        }
        static void limitRange(ref int val, int min, int max)
        {
            if (val < min)
            {
                val = min;
            }
            if (val > max)
            {
                val = max;
            }
        }
        static int upDownEnter<T>(Action<T> enter, T data)
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Enter:
                    enter(data);
                    return 0;
                    break;
                case ConsoleKey.DownArrow:
                    return 1;
                    break;
                case ConsoleKey.UpArrow:
                    return -1;
                    break;
                default:
                    return upDownEnter(enter, data);
                    break;
            }
        }
        public static void quit()
        {
            quit(new Random().Next(10) > 5 ? "goodbye" : "shutting down");
        }
        public static void quit(string goodbye)
        {
            Console.Clear();
            foreach (char character in goodbye)
            {
                Console.Write(character);
                System.Threading.Thread.Sleep(45);
            }
            System.Threading.Thread.Sleep(225);
            Console.Write(" :)");
            System.Threading.Thread.Sleep(400);
            System.Threading.Thread.CurrentThread.Abort();
        }
    }
}
