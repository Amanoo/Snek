using SkiaSharp;
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
        static int IMAGESIZE=32;

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
            //render();
            playfieldCanvas.InvalidateSurface();
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
            //SKCanvasView playfieldCanvas = new SKCanvasView();//Moest de IDE de laatste versie van SkiaSharp.Views en SkiaSharp.Views.Forms laten zoeken en installeren

            //SKBitmap resourceBitmap; //Moest de IDE de laatste versie van SkiaSharp laten zoeken en installeren
        }

        private void playfieldCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            double[,] field = new double[(int)gameData.playingField.Y, (int)gameData.playingField.X];

            GameData.SnakeChunk[] snake = gameData.snake.ToArray<GameData.SnakeChunk>();

            foreach (GameData.SnakeChunk chunk in snake)
            {
                field[chunk.y, chunk.x] = chunk.sprite;

            }


            

            SKImage finalImage = null;

            using (var tempSurface = SKSurface.Create(new SKImageInfo(gameData.playingField.X * IMAGESIZE, gameData.playingField.Y * IMAGESIZE)))
            {
                var tempCanvas = tempSurface.Canvas;
                tempCanvas.Clear(SKColors.Black);

                int x = 0;
                int y = 0;
                foreach (int spriteID in field)
                {

                    
                    string resourceID = "Snek.sprites." + spriteID + ".png";
                    Assembly assembly = GetType().GetTypeInfo().Assembly;

                    using (Stream stream = assembly.GetManifestResourceStream(resourceID))
                    {
                        SKBitmap resourceBitmap = SKBitmap.Decode(stream);
                        tempCanvas.DrawBitmap(resourceBitmap, x * IMAGESIZE - 1, y * IMAGESIZE - 1);
                    }

                    x = (x + 1) % (int)gameData.playingField.X;
                    if (x == 0) y++;
                }
                finalImage = tempSurface.Snapshot();

                //scale final image
                var paint = new SKPaint
                {
                    FilterQuality = SKFilterQuality.High // high quality scaling
                };
                var pictureFrame = SKRect.Create(0, 0, playfieldCanvas.CanvasSize.Width, playfieldCanvas.CanvasSize.Height);
                var imageSize = new SKSize (finalImage.Width,finalImage.Height );
                var dest = pictureFrame.AspectFit(imageSize); // fit the size inside the rect

                //draw game into the canvas
                canvas.DrawImage(finalImage, dest, paint);
            }
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
           
                if (width > height)
                {
                    gameLayout.Orientation = StackOrientation.Horizontal;
                }
                else
                {
                    gameLayout.Orientation = StackOrientation.Vertical;
                }
            }
        }
    }

