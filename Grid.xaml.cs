using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame
{
    public sealed partial class Grid : UserControl
    {
        private Line[] cursorLines = new Line[2];
        
        ////////////////////////////

        public static readonly DependencyProperty PixelsPerUnitProperty = DependencyProperty.Register(
            nameof(PixelsPerUnit),
            typeof(int),
            typeof(Grid),
            new PropertyMetadata(null)
        );

        public int PixelsPerUnit
        {
            get => (int)GetValue(PixelsPerUnitProperty);
            set => SetValue(PixelsPerUnitProperty, value);
        }

        //////////////////////

        public static DependencyProperty UnitsPerScaleProperty = DependencyProperty.Register(
            nameof(UnitsPerScale),
            typeof(int),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public int UnitsPerScale
        {
            get => (int)GetValue(UnitsPerScaleProperty);
            set => SetValue(UnitsPerScaleProperty, value);
        }

        //////////////////////

        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            nameof(BackgroundColor),
            typeof(Color),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set
            {
                SetValue(BackgroundColorProperty, value);
            }
        }

        //////////////////////

        public static readonly DependencyProperty DividerColorProperty = DependencyProperty.Register(
            nameof(DividerColor),
            typeof(Color),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public Color DividerColor
        {
            get => (Color)GetValue(DividerColorProperty);
            set
            {
                SetValue(DividerColorProperty, value);
            }
        }

        //////////////////////

        public static readonly DependencyProperty SubDividerColorProperty = DependencyProperty.Register(
            nameof(SubDividerColor),
            typeof(Color),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public Color SubDividerColor
        {
            get => (Color)GetValue(SubDividerColorProperty);
            set
            {
                SetValue(SubDividerColorProperty, value);
            }
        }

        ////////////////////////////

        public Grid()
        {
            Color WHITE = Color.FromArgb(255, 255, 255, 255);
            Color GRAY = Color.FromArgb(255, 200, 200, 200);
            Color LIGHT_GRAY = Color.FromArgb(255, 100, 100, 100);
            Color BLACK = Color.FromArgb(255, 0, 0, 0);
            Color CYAN = Color.FromArgb(255, 0, 204, 204);

            this.InitializeComponent();

            PixelsPerUnit = 10;
            UnitsPerScale = 4;
            BackgroundColor = BLACK;
            DividerColor = GRAY;
            SubDividerColor = LIGHT_GRAY;

            cursorLines[0] = new Line();
            cursorLines[0].Stroke = new SolidColorBrush(CYAN);
            cursorLines[1] = new Line();            
            cursorLines[1].Stroke = new SolidColorBrush(CYAN);

            PointerEntered += PointerEnteredGrid;
            PointerMoved += PointerMovedInGrid;
            PointerExited += PointerExitedGrid;
        }

        private void DrawGrid(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var session = args.DrawingSession;
            session.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Aliased;

            for (int x = 0; x <= Width; x += PixelsPerUnit)
            {
                if(x % (PixelsPerUnit * UnitsPerScale) == 0)
                {
                    session.DrawLine(x, 0, x, (int)Height, DividerColor);
                }
                else
                {
                    session.DrawLine(x, 0, x, (int)Height, SubDividerColor);
                }                
            }

            for (int y = 0; y <= Height; y += PixelsPerUnit)
            {
                if (y % (PixelsPerUnit * UnitsPerScale) == 0)
                {
                    session.DrawLine(0, y, (int)Width, y, DividerColor);
                }
                else
                {
                    session.DrawLine(0, y, (int)Width, y, SubDividerColor);
                }
            }
        }

        private void PointerEnteredGrid(object sender, PointerRoutedEventArgs args)
        {
            var pos = args.GetCurrentPoint(this).Position;
            
            cursorLines[0].X1 = pos.X;
            cursorLines[0].Y1 = 0;
            cursorLines[0].X2 = pos.X;
            cursorLines[0].Y2 = Height;
            
            cursorLines[1].X1 = 0;
            cursorLines[1].Y1 = pos.Y;
            cursorLines[1].X2 = Width;
            cursorLines[1].Y2 = pos.Y;

            GridGrid.Children.Add(cursorLines[0]);
            GridGrid.Children.Add(cursorLines[1]);
        }

        private void PointerMovedInGrid(object sender, PointerRoutedEventArgs args)
        {
            var pos = args.GetCurrentPoint(this).Position;

            cursorLines[0].X1 = pos.X;
            cursorLines[0].Y1 = 0;
            cursorLines[0].X2 = pos.X;
            cursorLines[0].Y2 = Height;

            cursorLines[1].X1 = 0;
            cursorLines[1].Y1 = pos.Y;
            cursorLines[1].X2 = Width;
            cursorLines[1].Y2 = pos.Y;
        }

        private void PointerExitedGrid(object sender, PointerRoutedEventArgs args)
        {
            GridGrid.Children.Remove(cursorLines[0]);
            GridGrid.Children.Remove(cursorLines[1]);
        }
    }
}
