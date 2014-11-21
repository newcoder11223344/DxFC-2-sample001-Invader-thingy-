using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DX001_INVADERS
{
	/*
	public abstract class Scene
	{
		SortedDictionary<int, List<DxLeaf>> dxObjList;

		public Scene()
		{
			dxObjList = new SortedDictionary<int, List<DxLeaf>>();
		}
		public void updateAll()
		{
			foreach (List<DxLeaf> list in dxObjList.Values.ToList())
			{
				foreach (DxLeaf obj in list.ToList())
				{
					obj.update();
				}
			}
		}
		public void drawAll()
		{
			foreach (List<DxLeaf> list in dxObjList.Values.ToList())
			{
				foreach (DxLeaf obj in list.ToList())
				{
					obj.draw();
				}
			}
		}

		public abstract void restart();
		public abstract void update();
		public abstract void draw();
	}

	public class GameScene : Scene
	{
		public void restart()
		{
			;
		}
		public void update()
		{
			updateAll();
		}
		public void draw()
		{
			drawAll();
		}
	}*/
}
