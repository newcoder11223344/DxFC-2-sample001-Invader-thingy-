using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxFramework;
using DxLibDLL;

namespace DX001_INVADERS
{
	public abstract class DxObject {
		public DxObject(Vector2 p){
			pos = p;
			removemeflag = false;
		}
		public bool removemeflag { get;private set; }
		public virtual void removeme() { removemeflag = true; Console.WriteLine(GetType()); }
		public Vector2 pos { get; protected set; }
		public abstract void update();
		public abstract void draw();
		public abstract void draw(Vector2  pos);
	}

	public class DxLeaf:DxObject
	{
		protected Graphic graphic;
		public DxLeaf(Vector2 pos,Graphic graphic):base(pos)
		{
			this.graphic = graphic;
		}
		public override void update() { graphic.update(); }
		public override void draw() { draw(new Vector2(0, 0)); }
		public override void draw(Vector2 pos) { graphic.draw(this.pos+pos); }
	}

	public class DxBranch : DxObject
	{
		public DxBranch(Vector2 pos):base(pos)
		{
			list = new List<DxObject>();
		}
		public List<DxObject> list { get;private set; }
		virtual public void addChild(DxObject o)
		{
			list.Add(o);
		}
		override public void update()
		{
			list.RemoveAll(o=>o.removemeflag);
			foreach (DxObject o in list.ToList())
			{
				o.update();
			}
		}
		override public void draw(Vector2 pos)
		{
			foreach (DxObject o in list)
			{
				o.draw(pos);
			}
		}
		override public void draw(){
			draw(new Vector2 (0,0));
		}
		public override void removeme()
		{
			base.removeme();
			foreach (DxObject o in list)
			{
				o.removeme();
			}
		}
	}

	class World:DxBranch
	{
		public static Vector2 gameScreenSize = new Vector2(160,120);
		public static World ins { get; private set; }

		static public void makeins()
		{
			Player.makeins();
			ShotBranch.makeins();
			EnemyBranch.makeins();
			ins = new World(new Vector2(0, 0));
		}

		int cnt = 0;

		private World(Vector2 p):base(p)
		{
			addChild(Player.ins);
			addChild(ShotBranch.ins);
			addChild(EnemyBranch.ins);
		}
		public override void update()
		{
			cnt++;
			base.update();
		}

		public void testupdate()
		{
			if (cnt % 100 == 0) EnemyBranch.ins.addChild(new Enemy(new Vector2 (10,10)));
		}

		public override void addChild(DxObject o)
		{
			base.addChild(o);
		}
	}

	class Player : DxLeaf
	{
		public static Player ins { get; private set; }
		static Graphic gr;
		public static void load()
		{
			gr = new Graphic("resource/image/player.png");
		}
		public static void makeins(){
			ins = new Player();
		}

		OnOffCounter shotbutton=new OnOffCounter();
		Counter lastshot = new Counter();
		bool reserved;

		Player():base(new Vector2(100,100),gr.clone())
		{
			;
		}
		override public void update()
		{
			base.update();
			pos += BasicInput.arrowkeyDir()*1;
			shotbutton.update(BasicInput.getKey(DX.KEY_INPUT_Z));
			lastshot.update();
			if (shotbutton.pushed) reserved = true;
			if ((reserved || shotbutton.pressed)&& lastshot.count > 15)
			{
				reserved = false; lastshot.reset();
				ShotBranch.ins.addChild(new Shot(pos));
			}
		}
	}

	class Shot : DxLeaf
	{
		static Graphic gr;
		public static void load()
		{
			gr = new Graphic("resource/image/shot.png");
		}

		public  Vector2 hitsize;
		public Shot(Vector2 p):base(p,gr.clone())
		{
			hitsize=new Vector2(2,8);
		}
		override public void update()
		{
			base.update();
			pos += new Vector2(0, -2);
			if ((pos.isinRec(World.gameScreenSize, Vector2.O))==false)removeme();
		}

		public void hit()
		{
			removeme();
		}
	}

	class ShotBranch : DxBranch
	{
		public static ShotBranch ins{get;private set;}
		public static void makeins()
		{
			ins = new ShotBranch();
		}

		ShotBranch(): base(new Vector2(0,0)){;}
		public override void addChild(DxObject o)
		{
			if (list.Count >= 15) return;
			base.addChild(o);
		}
	}

	class EnemyBranch : DxBranch
	{
		public static EnemyBranch ins { get; private set; }

		public static void makeins()
		{
			ins = new EnemyBranch();

		}

		EnemyBranch():base(new Vector2 (0,0)){;}

		public override void update()
		{
			//hitcheck(ゴミ)
			foreach (Enemy e in list)
			{
				foreach (Shot s in ShotBranch.ins.list)
				{
					if (Vector2.RectRectHit(s.pos, s.hitsize, e.pos, e.hitsize)) { e.removeme(); s.hit(); }
				}
			}
			
			base.update();
		}

		public override void addChild(DxObject o)
		{
			if (list.Count >= 50) return;
			base.addChild(o);
		}

	}

	class Enemy : DxLeaf
	{
		static Graphic tempgr;
		public static void load (){
			tempgr = new Graphic("resource/image/enemy.png");
		}

		public  Vector2 hitsize;
		int cnt = 0;

		public Enemy(Vector2 p):base(p,tempgr.clone())
		{
			hitsize=new Vector2(8,8);
		}

		public override void update()
		{
			cnt++;
			pos+=new Vector2(Math.Sin(0.1*(double)cnt/3),0.3);
			if (cnt % 10 == 0) World.ins.addChild(new Bullet(pos,(Player.ins.pos-pos).unit()));

			base.update();

			if ((pos.isinRec(World.gameScreenSize, Vector2.O)) == false) removeme();
		}
	}

	class Bullet : DxLeaf
	{
		static Graphic tempgr;
		public static void load()
		{
			tempgr = new Graphic("resource/image/shot.png");
		}
		public Vector2 hitsize;
		public Vector2 speed;
		public Bullet(Vector2 p, Vector2 v):base(p,tempgr.clone())
		{
			speed = v;
			hitsize = new Vector2(2, 2);
		}
		public override void update()
		{
			pos += speed;
			base.update();

			if ((pos.isinRec(World.gameScreenSize, Vector2.O)) == false) removeme();
		}
		public override void draw(Vector2 p)
		{
			graphic.draw(p+pos, (double)speed.angle() -Math.PI/2);
		}
	}
}

//TODO
//enemy branch
//atari hantei

