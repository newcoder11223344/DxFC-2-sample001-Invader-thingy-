using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using DxFramework;
namespace DX001_INVADERS
{


	static class Test
	{
		[STAThread]
		static void Main()
		{
			//--------------------------initializing dxlib--------------------------
			Vector2 screenSize = new Vector2(160, 120);//スクリーンサイズ設定
			int windowSizeRate = 4;					//ウィンドウサイズ設定

			Vector2 windowSize = screenSize * windowSizeRate;
			DX.ChangeWindowMode(DX.TRUE);
			DX.SetGraphMode((int)windowSize.x,(int)windowSize.y, 32);
			if (DX.DxLib_Init() == -1) return;
			int smallscreen=DX.MakeScreen((int)screenSize.x,(int)screenSize.y);
			DX.SetDrawScreen(smallscreen);
			

			//--------------------------loading stuff----------------------------
			//これをstaticコンストラクタでやればいいと思ったが、Dxinit前に呼ばれるとまずい
			Player.load();
			Shot.load();
			Enemy.load();
			Bullet.load();

			//--------------------------↑の後に行う必要がある-------------------
			World.makeins();
			
			//--------------------------test用初期化-------------------------------



			while (DX.ScreenFlip()==0 && DX.ProcessMessage() == 0 && DX.ClearDrawScreen() == 0)
			{
				//-----------------------------mainloop---------------------------
				BasicInput.update();
				World.ins.testupdate();
				World.ins.update();
				World.ins.draw();
				//+++++++++++++++++++++++++++++++mainloop+++++++++++++++++++++++++


				//一旦描写してから拡大
				DX.SetDrawScreen(DX.DX_SCREEN_BACK);
				DX.DrawRotaGraph((int)windowSize.x / 2, (int)windowSize.y / 2, windowSizeRate, 0, smallscreen, 0);
				DX.SetDrawScreen(smallscreen);
			}
			DX.DxLib_End();
		}
	}
}



