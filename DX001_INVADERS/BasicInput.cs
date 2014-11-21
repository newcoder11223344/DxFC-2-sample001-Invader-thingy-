using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace DxFramework
{
	public struct MouseButton
	{
		public int button;
		public MouseButton(int button):this(){
			pressed = false;
			up = false;
			down = false;
			lastDown = new Vector2();
			this.button = button;
		}
		public  bool pressed{get;set;}//ボタンが押されている
		public  bool up { get; set; }//離された
		public  bool down { get; set; }//押された
		public  Vector2 lastDown { get; set; }
		public  Vector2 lastUp { get; set; }
		public void setInput(bool pressedthisframe,Vector2 pos){
			up = false; down = false;
			if (pressedthisframe){
				if (pressed == false) down = true;
			}
			else{
				if (pressed == true) up = true;
			}
			if (up) lastUp = pos;
			if (down) lastDown = pos;
			pressed = pressedthisframe;
		}
		public void setInputLog(bool input,Vector2 pos){
			;
		}//setinputlogの定義が謎　保留
		public string state(){
			var s = "";
			s += ("buttonid:" + button + "\n");
			s+=("pressed:"+pressed+"\n");
			s += ("up:" +up + "\n");
			s += ("down:" + down + "\n");
			s += ("lastdown:" + lastDown + "\n");
			s += ("lastup:" + lastUp + "\n");
			return s;
		}
	}
	public  struct Mouse
	{
		public  Vector2 position { get; set; }
		public  Vector2 speed { get; set; }
		public  int wheelRotVol { get; set; }
		public MouseButton left;
		public MouseButton right;
		public MouseButton middle;
		public string state(){
			var s = "";
			s += ("pos:" +position + "\n");
			s += ("speed:" + speed + "\n");
			s += ("wheelrotvol:" + wheelRotVol + "\n");
			s += ("leftkeystates:" +left.state()+"\n");
			s += ("rightkeystates:" + right.state() + "\n");
			s += ("middlekeystates:" + middle.state() + "\n");
			return s;
		}
	}
	public class BasicInput
	{
		public static Mouse mouse;
		static byte[] Buf;
		static BasicInput() {
			mouse.left = new MouseButton(DX.MOUSE_INPUT_LEFT);
			mouse.right = new MouseButton(DX.MOUSE_INPUT_RIGHT);
			mouse.middle = new MouseButton(DX.MOUSE_INPUT_MIDDLE);
			Buf = new byte[256];
			update() ;}
		public static void update(){
			int x, y;
			DX.GetMousePoint(out x, out y);
			mouse.speed =new Vector2(x, y)-mouse.position;
			mouse.position =new  Vector2(x, y);
			mouse.wheelRotVol = DX.GetMouseWheelRotVol();
			int input=DX.GetMouseInput();
			mouse.left.setInput((input&1)!=0, mouse.position);
			mouse.right.setInput((input & 2) != 0, mouse.position);
			mouse.middle.setInput((input&4)!=0, mouse.position);
			//DX.GetMouseInputLog(out input,out x,out y,1);
			DX.GetHitKeyStateAll(out Buf[0]);
			//Console.WriteLine(state());
		}
		public static bool getKey(int key)
		{
			return (Buf[key] == 1);
		}
		public static Vector2 arrowkeyDir()
		{
			Vector2 dir=new Vector2(0,0);
			if (getKey(DX.KEY_INPUT_RIGHT))		dir.x = 1;
			else if (getKey(DX.KEY_INPUT_LEFT)) dir.x = -1;
			if (getKey(DX.KEY_INPUT_UP))		dir.y = -1;
			else if (getKey(DX.KEY_INPUT_DOWN)) dir.y = 1;
			if (dir.length() > 1.1) dir=dir.unit();
			return dir;
		}
		public static string state(){
			var s = "";
			int mouseinput = DX.GetMouseInput();
			s += ("getmouseinput: " + mouseinput  + "\n");
			s += ("buttoninput & 1: " + (mouseinput & 1) + "\n");
			s += ("buttoninput & 2: " + (mouseinput & 2) + "\n");
			s += ("buttoninput & 4: " + (mouseinput & 4) + "\n");
			s += mouse.state();
			return s;
		}
	}
}
