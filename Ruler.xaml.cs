using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
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
    /// <summary>
    /// refer website: https://edi.wang/post/2018/11/5/build-pixel-ruler-uwp
    /// for details on how to implement this ruler functionality
    /// </summary>
    public sealed partial class Ruler : UserControl, INotifyPropertyChanged
    {
        //////////////////////

        public static readonly DependencyProperty ScaleMarkPositionProperty = DependencyProperty.Register(
            nameof(ScaleMarkPosition),
            typeof(int),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public int ScaleMarkPosition
        {
            get => (int)GetValue(ScaleMarkPositionProperty);
            set => SetValue(ScaleMarkPositionProperty, value);
        }

        //////////////////////

        public static readonly DependencyProperty RulerLengthProperty = DependencyProperty.Register(
            nameof(RulerLength),
            typeof(int),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public int RulerLength
        {
            get => (int)GetValue(RulerLengthProperty);
            set => SetValue(RulerLengthProperty, value);
        }

        //////////////////////

        public static readonly DependencyProperty SmallDividerLengthProperty = DependencyProperty.Register(
            nameof(SmallDividerLength),
            typeof(int),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public int SmallDividerLength
        {
            get => (int)GetValue(SmallDividerLengthProperty);
            set => SetValue(SmallDividerLengthProperty, value);
        }

        //////////////////////

        public static readonly DependencyProperty LargeDividerLengthProperty = DependencyProperty.Register(
            nameof(LargeDividerLength),
            typeof(int),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public int LargeDividerLength
        {
            get => (int)GetValue(LargeDividerLengthProperty);
            set => SetValue(LargeDividerLengthProperty, value);
        }

        //////////////////////

        public static readonly DependencyProperty RulerWidthProperty = DependencyProperty.Register(
            nameof(RulerWidth),
            typeof(int),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public int RulerWidth
        {
            get => (int)GetValue(RulerWidthProperty);
            set => SetValue(RulerWidthProperty, value);
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
            get => (Color) GetValue(BackgroundColorProperty);
            set
            {
                SetValue(BackgroundColorProperty, value);
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        //////////////////////

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            nameof(TextColor),
            typeof(Color),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set
            {
                SetValue(TextColorProperty, value);
                OnPropertyChanged();
            }
        }

        //////////////////////

        public static DependencyProperty PixelsPerUnitProperty = DependencyProperty.Register(
            nameof(PixelsPerUnit),
            typeof(int),
            typeof(Ruler),
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

        public static DependencyProperty HorizontalRulerProperty = DependencyProperty.Register(
            nameof(HorizontalRuler),
            typeof(Visibility),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public Visibility HorizontalRuler
        {
            get => (Visibility)GetValue(HorizontalRulerProperty);
            set => SetValue(HorizontalRulerProperty, value);
        }

        //////////////////////

        public static DependencyProperty VerticalRulerProperty = DependencyProperty.Register(
            nameof(VerticalRuler),
            typeof(Visibility),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public Visibility VerticalRuler
        {
            get => (Visibility)GetValue(VerticalRulerProperty);
            set => SetValue(VerticalRulerProperty, value);
        }

        //////////////////////

        public event PropertyChangedEventHandler PropertyChanged;

        private double zoom = 0.0;

        private delegate void DrawLine(CanvasDrawingSession session, float pos, int dividerLevel);
        private delegate void DrawText(CanvasDrawingSession session, float pos, string txt);

        ///////////////////////////////////////////////////////////////////////////////////////////////

        public Ruler()
        {
            Color WHITE = Color.FromArgb(255, 200, 200, 200);
            Color BLACK = Color.FromArgb(255, 0, 0, 0);

            // initialize
            ScaleMarkPosition = 35;
            RulerLength = 2000;
            SmallDividerLength = 5;
            LargeDividerLength = 10;
            RulerWidth = 35;
            PixelsPerUnit = 10;
            UnitsPerScale = 4;
            BackgroundColor = WHITE;
            DividerColor = BLACK;
            TextColor = BLACK;
            HorizontalRuler = Visibility.Collapsed;
            VerticalRuler = Visibility.Collapsed;

            this.InitializeComponent();

            SizeChanged += WindowSizeChanged;
        }

        private async void WindowSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (HorizontalRuler == Visibility.Visible)
            {
                RulerLength = (int)(args.NewSize.Width);
            }
            else if (VerticalRuler == Visibility.Visible)
            {
                RulerLength = (int)(args.NewSize.Height);
            }
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DrawHorizontalRuler(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var session = args.DrawingSession;
            session.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Aliased;

            float scale = (float)(1024.0f * this.zoom); // should be multiple of 8

            DrawHorizontalRulerBorder(session);

            DrawLines(session, (float)0, scale, DrawHorizontalLine, DrawHorizontalText);
        }

        private void DrawVerticalRuler(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var session = args.DrawingSession;
            session.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Aliased;

            float scale = (float)(1024.0f * this.zoom); // should be multiple of 8

            DrawVerticalRulerBorder(session);

            DrawLines(session, (float)0, scale, DrawVerticalLine, DrawVerticalText);
        }


        private void DrawHorizontalRulerBorder(CanvasDrawingSession session)
        {
            session.DrawLine(0, RulerWidth, RulerLength, RulerWidth, DividerColor);
            session.DrawLine(RulerWidth, 0, RulerWidth, RulerWidth, DividerColor);
        }

        private void DrawVerticalRulerBorder(CanvasDrawingSession session)
        {
            session.DrawLine(RulerWidth, 0, RulerWidth, RulerLength, DividerColor);
            session.DrawLine(0, RulerWidth, RulerWidth, RulerWidth, DividerColor);
        }



        private void DrawLines(CanvasDrawingSession session, float begin, float scale, DrawLine drawLine, DrawText drawText)
        {
            if (scale < 10) return;

            int value = (int)Math.Round(begin / this.zoom);

            int dividerLevel = GetDividerLevel(value);

            float offset = ScaleMarkPosition + begin;

            if (offset > ScaleMarkPosition)
            {
                drawLine(session, offset, dividerLevel);

                if (dividerLevel == 0)
                {
                    drawText(session, offset, value.ToString());
                }
            }

            float half = scale * 0.5f;

            DrawLines(session, begin - half, half, drawLine, drawText);
            DrawLines(session, begin + half, half, drawLine, drawText);
        }

        private int GetDividerLevel(int value)
        {
            int dividerLevel = 2;

            if (value % 10 == 0) dividerLevel = 0;
            else if (value % 5 == 0) dividerLevel = 1;

            return dividerLevel;
        }

        private void DrawHorizontalLine(CanvasDrawingSession session, float x, int dividerLevel)
        {
            switch (dividerLevel)
            {
                case 0:
                    session.DrawLine(x, RulerWidth, x, 0, DividerColor);
                    break;
                case 1:
                    session.DrawLine(x, RulerWidth, x, RulerWidth - LargeDividerLength, DividerColor);
                    break;
                default:
                    session.DrawLine(x, RulerWidth, x, RulerWidth - SmallDividerLength, DividerColor);
                    break;
            }
        }

        private void DrawVerticalLine(CanvasDrawingSession session, float y, int dividerLevel)
        {
            switch (dividerLevel)
            {
                case 0:
                    session.DrawLine(RulerWidth, y, 0, y, DividerColor);
                    break;
                case 1:
                    session.DrawLine(RulerWidth, y, RulerWidth - LargeDividerLength, y, DividerColor);
                    break;
                default:
                    session.DrawLine(RulerWidth, y, RulerWidth - SmallDividerLength, y, DividerColor);
                    break;
            }
        }

        private void DrawHorizontalText(CanvasDrawingSession session, float x, string txt)
        {
            var format = new CanvasTextFormat()
            {
                FontSize = (float)12,
                FontFamily = new FontFamily("Courier New").Source,
                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                VerticalAlignment = CanvasVerticalAlignment.Bottom,
                WordWrapping = CanvasWordWrapping.NoWrap
            };

            float xLoc = x + 3;
            float yLoc = RulerWidth - 5;
            CanvasTextLayout textLayout = new CanvasTextLayout(session, txt, format, 0.0f, 0.0f);
            Rect theRectYouAreLookingFor = new Rect(xLoc + textLayout.DrawBounds.X, yLoc + textLayout.DrawBounds.Y, textLayout.DrawBounds.Width, textLayout.DrawBounds.Height);
            session.DrawTextLayout(textLayout, xLoc, yLoc, TextColor);
        }

        private void DrawVerticalText(CanvasDrawingSession session, float y, string txt)
        {
            var format = new CanvasTextFormat()
            {
                FontSize = (float)12,
                FontFamily = new FontFamily("Courier New").Source,
                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                VerticalAlignment = CanvasVerticalAlignment.Bottom,
                WordWrapping = CanvasWordWrapping.NoWrap
            };

            float xLoc = RulerWidth - 5;
            float yLoc = y + 3;
            CanvasTextLayout textLayout = new CanvasTextLayout(session, txt, format, 0.0f, 0.0f);
            Rect theRectYouAreLookingFor = new Rect(xLoc + textLayout.DrawBounds.X, yLoc + textLayout.DrawBounds.Y, textLayout.DrawBounds.Width, textLayout.DrawBounds.Height);

            session.Transform = Matrix3x2.CreateRotation((float)-Math.PI * 0.5f, new Vector2(xLoc, yLoc));
            session.DrawTextLayout(textLayout, xLoc, yLoc, TextColor);
            session.Transform = Matrix3x2.Identity;
        }


        //private void DrawVerticalRuler(CanvasControl sender, CanvasDrawEventArgs args)
        //{
        //    var format = new CanvasTextFormat()
        //    {
        //        FontSize = (float)12,
        //        FontFamily = new FontFamily("Courier New").Source,
        //        HorizontalAlignment = CanvasHorizontalAlignment.Right,
        //        VerticalAlignment = CanvasVerticalAlignment.Top,
        //        WordWrapping = CanvasWordWrapping.NoWrap
        //    };

        //    var session = args.DrawingSession;
        //    session.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Aliased;

        //    session.DrawLine(RulerWidth, 0, RulerWidth, RulerLength, DividerColor);

        //    for (int unit = 0, y = 0; y < RulerLength - ScaleMarkPosition; y += PixelsPerUnit)
        //    {
        //        int offset = ScaleMarkPosition + y;

        //        if (y % (PixelsPerUnit * UnitsPerScale) == 0)
        //        {
        //            session.DrawLine(RulerWidth, offset, 0, offset, DividerColor);

        //            float xLoc = RulerWidth - 19;
        //            float yLoc = offset + 3;
        //            CanvasTextLayout textLayout = new CanvasTextLayout(session, unit.ToString(), format, 0.0f, 0.0f);
        //            Rect theRectYouAreLookingFor = new Rect(xLoc + textLayout.DrawBounds.X, yLoc + textLayout.DrawBounds.Y, textLayout.DrawBounds.Width, textLayout.DrawBounds.Height);

        //            session.Transform = Matrix3x2.CreateRotation((float)-Math.PI * 0.5f, new Vector2(xLoc, yLoc));
        //            session.DrawTextLayout(textLayout, xLoc, yLoc, TextColor);
        //            session.Transform = Matrix3x2.Identity;

        //            ++unit;
        //        }
        //        else
        //        {
        //            if (y % (PixelsPerUnit * UnitsPerScale / 2) == 0)
        //            {
        //                session.DrawLine(RulerWidth, offset, RulerWidth - LargeDividerLength, offset, DividerColor);
        //            }
        //            else
        //            {
        //                session.DrawLine(RulerWidth, offset, RulerWidth - SmallDividerLength, offset, DividerColor);
        //            }
        //        }
        //    }
        //}

        public void Zoom(float zoom)
        {
            this.zoom = zoom;

            if(HorizontalRuler == Visibility.Visible)
            {
                HorizontalCanvas.Invalidate();
            }

            if (VerticalRuler == Visibility.Visible)
            {
                VerticalCanvas.Invalidate();
            }
        }

        public void Scroll(double offset)
        {
            if (HorizontalRuler == Visibility.Visible)
            {
                HorizontalCanvas.Invalidate();
            }

            if (VerticalRuler == Visibility.Visible)
            {
                VerticalCanvas.Invalidate();
            }
        }
    }
}
