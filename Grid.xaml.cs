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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame
{
    public sealed partial class Grid : UserControl
    {
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

        ////////////////////////////

        public Grid()
        {
            Color WHITE = Color.FromArgb(255, 200, 200, 200);
            Color BLACK = Color.FromArgb(255, 0, 0, 0);

            this.InitializeComponent();

            PixelsPerUnit = 10;
            BackgroundColor = BLACK;
            DividerColor = WHITE;            
        }

        private void DrawGrid(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var session = args.DrawingSession;

            for (int x = 0; x <= Width; x += PixelsPerUnit)
            {
                session.DrawLine(x, 0, x, (int)Height, DividerColor);
            }

            for (int y = 0; y <= Height; y += PixelsPerUnit)
            {
                session.DrawLine(0, y, (int)Width, y, DividerColor);
            }
        }
    }
}
