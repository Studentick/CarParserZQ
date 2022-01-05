using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularX
{
    class ConsoleProgressBar
    {
        int c_left, c_top;
        int c_width;

        public ConsoleProgressBar()
        {
            GetWidth(); 
        }

        public void SetPbPosition(int c_left, int c_top)
        {
            this.c_left = c_left;
            this.c_top = c_top;
        }

        public void GetWidth()
        {
            this.c_width = Console.WindowWidth-2;
        }

        public void SetWidth(int w)
        {
            this.c_width = w - 2;
        }

        public void WritePercent(int progress, int complete, bool need_callback = true)
        {
            string line = "║";
            int pos_left = Console.CursorLeft;
            var pos_top = Console.CursorTop;
            //c_width
            Console.SetCursorPosition(c_left, c_top);

            int percent = 100 * progress / complete;
            //int gr_step = percent * 100 / c_width;
            int gr_step = c_width * percent / 100;

            for (int i = 0; i < this.c_width - 1; i++)
            {
                if (i <= gr_step && i !=0)
                {
                    line += "▓";
                }
                else line += " ";
            }
            line += "║";
            Console.WriteLine(line);
            //Console.WriteLine();
            Console.WriteLine($"Выполнено: {percent}%     \n");
            if(need_callback)
            Console.SetCursorPosition(pos_left, pos_top);
        }



    }
}
