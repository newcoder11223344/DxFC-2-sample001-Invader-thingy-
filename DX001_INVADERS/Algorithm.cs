using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DX001_INVADERS
{
	class OnOffCounter
	{
		int row;
		public bool pressed { get; private set; }
		public int lastpushed { get; private set; }//押したタイミングの次から１　それまで加算
		public int lastreleased { get; private set; }//０にはならない!

		public bool pushed { get { return (row==1)&&(pressed==true); }}
		public bool released { get { return (row == 1) && (pressed == false); } }
		public int onInARow { get { return (pressed ? row : 0); } }
		public int offInARow { get { return (pressed ? 0 : row); } }
		public OnOffCounter()
		{
			row = 0;
			pressed = false;
		}
		public void update(bool a)
		{
			if (pushed) lastpushed = 1;
			else lastpushed++;
			if (released) lastreleased = 1;
			else lastreleased++;

			if (pressed == a) row++;//実質updateタイミング
			else { pressed = a; row = 1; }
		}
	}
	class Counter
	{
		public int count{get; private set;}
		public void reset() { count=0;}
		public void update(){count++;}
	}
}
