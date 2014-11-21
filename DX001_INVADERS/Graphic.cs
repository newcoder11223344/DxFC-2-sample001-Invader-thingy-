using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using DxFramework;

namespace DX001_INVADERS
{
	public class Graphic
	{
		int dxgrhandle;
		
		public Graphic(string imagefilename)
		{
			dxgrhandle=DX.LoadGraph(imagefilename);
		}
		public Graphic clone() { return (Graphic)this.MemberwiseClone(); }
		public virtual void update(){;}
		public virtual void draw(Vector2 pos)
		{
			int xsize, ysize;
			DX.GetGraphSize(dxgrhandle, out xsize, out ysize);
			DX.DrawGraphF(pos.x - xsize / 2,pos.y-  ysize / 2, dxgrhandle, DX.TRUE);
		}
		public virtual void draw(Vector2 pos,double muki)
		{
			DX.DrawRotaGraphF(pos.x, pos.y,1,muki, dxgrhandle, DX.TRUE);
		}
	}
}
