using SkiaSharp;
using SkiaSharp.Views.Android;
using SkiaSharp.Views.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Snek
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {


        GameData gameData;
        

        public MainPage()
        {
            InitializeComponent();
            gameData = new GameData ( 20, 20 );
        }

        void OnSwiped(object sender, SwipedEventArgs e)
        {
            gameData.setDirection(e.Direction);
            gameData.gameTick();
            render();
        }

        private void render()
        {
            double[,] field = new double[(int)gameData.playingField.Y,(int)gameData.playingField.X];

            GameData.SnakeChunk[] snake = gameData.snake.ToArray<GameData.SnakeChunk>();
            
            foreach(GameData.SnakeChunk chunk in snake)
            {
                field[chunk.y,chunk.x] = chunk.sprite;
                
            }
            
            
            String myStr = "";
            int i = 0;
            foreach(int spriteID in field)
            {

                myStr += spriteID / 100;
                i = (i + 1) % (int)gameData.playingField.X;
                if (i == 0) myStr += '\n';
            }
            labeltje.Text = myStr;
            //gameGrid.Children.Add()
            //throw new NotImplementedException();
            SKCanvasView canvasView = new SKCanvasView();//Moest de IDE de laatste versie van SkiaSharp.Views en SkiaSharp.Views.Forms laten zoeken en installeren

            SKBitmap resourceBitmap; //Moest de IDE de laatste versie van SkiaSharp laten zoeken en installeren
        }
    }

}
