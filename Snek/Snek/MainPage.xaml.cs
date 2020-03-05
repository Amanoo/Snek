using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
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
        private bool running;

        public MainPage()
        {
            InitializeComponent();
            gameData = new GameData ( 20, 20 );
            scoreLabel.Text = "Score: 0\n\nSwipe to start\n\n";
            running = false;
            //directionCanvas.HeightRequest = gameLayout.Height / 8;
            Device.StartTimer(TimeSpan.FromSeconds(0.4), () =>
            {
                
                    if(running)tick();
                return true;
            });
        }

        void tick()
        {
            gameData.gameTick();
            if (!gameData.snakeAlive())
            {
                scoreLabel.Text = "Score: " + gameData.score+"\n\nGAME OVER!\n\nSwipe to start";
                running = false;
            }
            else
            {
                scoreLabel.Text = "Score: " + gameData.score+"\n\n\n\n";
            }
            playfieldCanvas.InvalidateSurface();
        }
        void OnSwiped(object sender, SwipedEventArgs e)
        {
            if (running) { 
            gameData.setDirection(e.Direction);
            }
            else
            {
                scoreLabel.Text = "Score: 0\n\n\n\n";
                gameData = new GameData(20, 20);
                running = true;
            }
            directionCanvas.InvalidateSurface();
        }



        private void directionCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            if (gameLayout.Height > gameLayout.Width)
            {
                directionCanvas.HeightRequest = gameLayout.Width / 8;

            }
            else
            {
                directionCanvas.WidthRequest = gameLayout.Height / 8;

            }
            canvas.Clear();


            using (Stream stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Snek.images." + gameData.getNewDirection() + ".png"))
            {
                SKBitmap arrow = SKBitmap.Decode(stream);
                


                //scale final image
                var paint = new SKPaint
                {
                    FilterQuality = SKFilterQuality.High // high quality scaling
                };
                var pictureFrame = SKRect.Create(0, 0, directionCanvas.CanvasSize.Width, directionCanvas.CanvasSize.Height);
                var imageSize = new SKSize(arrow.Width, arrow.Height);
                var dest = pictureFrame.AspectFit(imageSize); // fit the size inside the rect

                //draw game into the canvas
                canvas.DrawBitmap(arrow, dest, paint);
            }
        }


            //SKCanvasView playfieldCanvas = new SKCanvasView();//Moest de IDE de laatste versie van SkiaSharp.Views en SkiaSharp.Views.Forms laten zoeken en installeren

            //SKBitmap resourceBitmap; //Moest de IDE de laatste versie van SkiaSharp laten zoeken en installeren


            private void playfieldCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();


            SKImage finalImage = null;

            using (var tempSurface = SKSurface.Create(new SKImageInfo(gameData.playingField.X * IMAGESIZE, gameData.playingField.Y * IMAGESIZE)))
            {
                var tempCanvas = tempSurface.Canvas;
                tempCanvas.Clear(SKColors.Black);

                LinkedListNode<GameData.SnakeChunk> currChunk = gameData.snake.Last;
                do
                {
                    string resourceID = "Snek.sprites." + currChunk.Value.sprite + ".png";
                    Assembly assembly = GetType().GetTypeInfo().Assembly;

                    using (Stream stream = assembly.GetManifestResourceStream(resourceID))
                    {
                        SKBitmap resourceBitmap = SKBitmap.Decode(stream);
                        tempCanvas.DrawBitmap(resourceBitmap, currChunk.Value.x * IMAGESIZE - 1, currChunk.Value.y * IMAGESIZE - 1);
                    }

                } while ((currChunk = currChunk.Previous)!=null);

                //draw food
                using (Stream stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Snek.sprites.1.png"))
                {
                    SKBitmap resourceBitmap = SKBitmap.Decode(stream);
                    tempCanvas.DrawBitmap(resourceBitmap, gameData.foodLocation.X * IMAGESIZE - 1, gameData.foodLocation.Y * IMAGESIZE - 1);
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
                    GUIlayout.Orientation = StackOrientation.Vertical;
                }
                else
                {
                    gameLayout.Orientation = StackOrientation.Vertical;
                    GUIlayout.Orientation = StackOrientation.Horizontal;
                }
            }
        }
    }

