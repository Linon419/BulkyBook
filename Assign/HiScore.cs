using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC_Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Collections;

namespace Assign
{
    class HiScore
    {

        //int mark;
        ArrayList list = new ArrayList();
        //Marks.getmark();

        public HiScore(int mark)
        {

            // level = new int[count];
            // this.count = count;
            //scoreLs[0] = count;
            //for (int i = 0; i < 3; i++)
            //{
            //    list.Add(0);

            //    if ((int)list[i] < mark)
            //    {
            //        list[i] = mark;
            //    }
            //}
            list.Add(mark);
            list.Sort();
            list.Reverse();
            Console.WriteLine(list[0]);


        }

        public object getMark1()
        {
            return list[0];
                
        }
        public object getMark2()
        {
            if (list.Count > 1)
            {
                return list[1];
            }
            else return 0;
           

        }
        public object getMark3()
        {
            if (list.Count > 2)
            {
                return list[2];
            }else return 0;

        }


    }
}
