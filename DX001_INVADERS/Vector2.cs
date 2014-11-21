using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DxFramework
{
	public struct Vector2//沢山作って捨てるタイプと思われるので、構造体にした
				　//参照型ではなく、値型になる　なので、自身を編集する関数は作らない
	{
		public  static readonly Vector2 O = new Vector2(0, 0);

		public float x, y;
		public Vector2(Vector2 v){
			this.x = v.x;
			this.y = v.y;
		}
		public Vector2(float x, float y){
			this.x = x;
			this.y = y;
		}
		public Vector2(double x, double y)
		{
			this.x = (float)x;
			this.y = (float)y;
		}
		public static Vector2 newins2(double angle,double length){
			return new Vector2(Math.Cos(angle)*length,Math.Sin(angle)*length);
		}
		public static bool operator ==(Vector2 v, Vector2 w){
			return (v.x == w.x && v.y == w.y);
			//doubleにイコールをつかうのはちょっとこわい
			//距離が十分近い、に直した方がいいかも
		}
		public override bool Equals(object obj){
			return (this.x == ((Vector2)obj).x && this.y == ((Vector2)obj).y);
			//同じく
		}
		public override int GetHashCode(){
			return (x.GetHashCode() + y.GetHashCode());
			//コンパイラに作れと言われた 使わない
		}
		public static bool operator !=(Vector2 v, Vector2 w){
			return (!(v == w));
		}
		public static Vector2 operator+(Vector2 v, Vector2 w){
			return (new Vector2(v.x + w.x, v.y + w.y));
		}
		public static Vector2 operator -(Vector2 v, Vector2 w){
			return (new Vector2(v.x - w.x, v.y - w.y));
		}
		public static Vector2 operator -(Vector2 v){
			return (new Vector2(-v.x, -v.y));
		}
		public static Vector2 operator +(Vector2 v){
			return (new Vector2(v.x,v.y));
		}
		public static Vector2 operator *(Vector2 v, float d){
			return (new Vector2(v.x * d, v.y * d));
		}
		public static Vector2 operator /(Vector2 v, float d){
			return (new Vector2(v.x / d, v.y / d));
		}
		public float distance(Vector2 v){
			return ((this - v).length());
		}
		public float length(){
			return ((float)Math.Sqrt(x * x + y * y));
		}
		public Vector2 unit(){
			float len = length();
			return len == 0 ? this : this / len;
		}
		public double angle(){
			return Math.Atan2(y,x);
		}
		public double direction(Vector2 a)
		{
			return (a - this).angle();
		}
		public override string ToString(){//デバッグ用文字列化
			return ("(" + x.ToString() + "," + y.ToString() + ")");
		}
		public static Vector2 parse(string s){//デバッグ用文字列読みとり
			char[] spl={' ',',','(',')'};//
			var inp = s.Split(spl,StringSplitOptions.RemoveEmptyEntries);
			if (inp.Length != 2) return (new Vector2(0, 0));
			return (new Vector2(float.Parse(inp[0]), float.Parse(inp[1])));
		}
		public bool isinRec(Vector2 top, Vector2 bottom)
		{
			return (bottom.x < x && x < top.x && bottom.y < y && y < top.y);
		}

		public bool isinRec(Vector2 top, Vector2 bottom,double asobi)
		{
			return (bottom.x-asobi < x && x < top.x +asobi&& bottom.y-asobi < y && y < top.y+asobi);
		}

		public static bool RectRectHit(Vector2 apos, Vector2 asize, Vector2 bpos, Vector2 bsize)
		{
			var atop = apos + asize / 2;
			var abottom = apos - asize / 2;
			var btop = bpos + bsize / 2;
			var bbottom = bpos - bsize / 2;

			return (abottom.x < btop.x && bbottom.x < atop.x &&
				abottom.y < btop.y && bbottom.y < atop.y);
		}
		public Vector2 pushx(double x,bool right)
		{
			Vector2 newvec = this;
			if ((right&&this.x < x) || (right==false &&x< this.x)) newvec.x =(float) x;
			return newvec;
		}
		public Vector2 pushy(double y, bool down)
		{
			Vector2 newvec = this;
			if ((down && this.y < y) || (down == false && y < this.y)) newvec.y = (float)y;
			return newvec;
		}
	}

}
