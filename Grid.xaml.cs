using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame
{
    public sealed partial class Grid : UserControl
    {
        private class Line
        {
            Point from;
            Point to;

            public double X1 { get => from.X; set => from.X = value; }
            public double Y1 { get => from.Y; set => from.Y = value; }
            public double X2 { get => to.X; set => to.X = value; }
            public double Y2 { get => to.Y; set => to.Y = value; }

            public Line()
            {
                from = new Point(0.0, 0.0);
                to = new Point(0.0, 0.0);
            }
        }

        private Grid.Line[] cursorLines = new Grid.Line[4];
        private PointerState pointerState = PointerState.Exited;
        private KeyState pointerKeyState = KeyState.Released;

        ////////////////////////////

        public static readonly DependencyProperty CursorStartXProperty = DependencyProperty.Register(
            nameof(CursorStartX),
            typeof(int),
            typeof(Grid),
            new PropertyMetadata(null)
        );

        public int CursorStartX
        {
            get => (int)GetValue(CursorStartXProperty);
            set => SetValue(CursorStartXProperty, value);
        }

        ////////////////////////////

        public static readonly DependencyProperty CursorStartYProperty = DependencyProperty.Register(
            nameof(CursorStartY),
            typeof(int),
            typeof(Grid),
            new PropertyMetadata(null)
        );

        public int CursorStartY
        {
            get => (int)GetValue(CursorStartYProperty);
            set => SetValue(CursorStartYProperty, value);
        }

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

        //////////////////////

        public static readonly DependencyProperty SnapToDividerProperty = DependencyProperty.Register(
            nameof(SnapToDivider),
            typeof(bool),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public bool SnapToDivider
        {
            get => (bool)GetValue(SnapToDividerProperty);
            set
            {
                SetValue(SnapToDividerProperty, value);
            }
        }

        //////////////////////

        public static readonly DependencyProperty SnapToSubDividerProperty = DependencyProperty.Register(
            nameof(SnapToSubDivider),
            typeof(bool),
            typeof(Ruler),
            new PropertyMetadata(null)
        );

        public bool SnapToSubDivider
        {
            get => (bool)GetValue(SnapToSubDividerProperty);
            set
            {
                SetValue(SnapToSubDividerProperty, value);
            }
        }

        ////////////////////////////

        public static readonly DependencyProperty GridWidthProperty = DependencyProperty.Register(
            nameof(GridWidth),
            typeof(int),
            typeof(Grid),
            new PropertyMetadata(null)
        );

        public int GridWidth
        {
            get => (int)GetValue(GridWidthProperty);
            set => SetValue(GridWidthProperty, value);
        }

        ////////////////////////////

        public static readonly DependencyProperty GridHeightProperty = DependencyProperty.Register(
            nameof(GridHeight),
            typeof(int),
            typeof(Grid),
            new PropertyMetadata(null)
        );

        public int GridHeight
        {
            get => (int)GetValue(GridHeightProperty);
            set => SetValue(GridHeightProperty, value);
        }

        ////////////////////////////

        private double zoom = 0.0;

        ////////////////////////////

        public Grid()
        {
            Color WHITE = Color.FromArgb(255, 255, 255, 255);
            Color GRAY = Color.FromArgb(255, 200, 200, 200);
            Color LIGHT_GRAY = Color.FromArgb(255, 100, 100, 100);
            Color BLACK = Color.FromArgb(255, 0, 0, 0);
            Color CYAN = Color.FromArgb(255, 0, 204, 204);

            PixelsPerUnit = 10;
            UnitsPerScale = 4;
            SnapToDivider = false;
            SnapToSubDivider = false;
            BackgroundColor = BLACK;
            DividerColor = GRAY;
            SubDividerColor = LIGHT_GRAY;
            CursorStartX = 0;
            CursorStartY = 0;
            GridWidth = (int)Width;
            GridHeight = (int)Height;

            for (int i = 0; i < cursorLines.Length; i++)
            {
                // index 0, 1 : cursor movement tracking lines
                // index 2, 3 : cursor click tracking lines
                cursorLines[i] = new Grid.Line();
            }            

            this.InitializeComponent();

            PointerEntered += PointerEnteredGrid;
            PointerMoved += PointerMovedInGrid;
            PointerExited += PointerExitedGrid;
            PointerPressed += PointerPressedOnGrid;
            PointerReleased += PointerReleasedOnGrid;

            SizeChanged += WindowSizeChanged;
        }

        private async void WindowSizeChanged(object sender, SizeChangedEventArgs args)
        {
            GridWidth = (int)(ActualWidth);
            GridHeight = (int)(ActualHeight);
        }

        private void DrawGrid(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var session = args.DrawingSession;
            session.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Aliased;

            float scale = (float)(1024.0f * this.zoom); // should be multiple of 8


            var pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
            var x = pointerPosition.X - Window.Current.Bounds.X;
            var y = pointerPosition.Y - Window.Current.Bounds.Y;

            DrawHorizontalLines(session, (float)x, scale);
            DrawVerticalLines(session, (float)y, scale);

            DrawCursorLines(session);
        }

        public void Zoom(float zoom)
        {
            this.zoom = zoom;

            GridCanvas.Invalidate();
        }

        private void DrawCursorLines(CanvasDrawingSession session)
        {
            if (pointerKeyState == KeyState.Pressed || pointerState == PointerState.Entered)
            {
                session.DrawLine((float)cursorLines[0].X1, (float)cursorLines[0].Y1, (float)cursorLines[0].X2, (float)cursorLines[0].Y2, Colors.Cyan);
                session.DrawLine((float)cursorLines[1].X1, (float)cursorLines[1].Y1, (float)cursorLines[1].X2, (float)cursorLines[1].Y2, Colors.Cyan);
            }

            if (pointerKeyState == KeyState.Pressed)
            {
                var vecPos = new Vector2((float)(cursorLines[2].X1), (float)(cursorLines[2].Y1));
                var vecLabel = new Vector2((float)(cursorLines[2].X2 - cursorLines[2].X1), (float)(cursorLines[3].Y2 - cursorLines[3].Y1));
                string vecPosText = "(" + vecLabel.X + " x " + vecLabel.Y + ")";
                DrawText(vecPosText, vecPos, Colors.Cyan, Colors.Black, session);

                session.DrawLine((float)cursorLines[2].X1, (float)cursorLines[2].Y1, (float)cursorLines[2].X2, (float)cursorLines[2].Y2, Colors.Cyan);
                session.DrawLine((float)cursorLines[3].X1, (float)cursorLines[3].Y1, (float)cursorLines[3].X2, (float)cursorLines[3].Y2, Colors.Cyan);
            }
        }

        private void DrawHorizontalLines(CanvasDrawingSession session, float begin, float scale)
        {
            if (scale < 10) return;

            if (begin > 0)
            {
                DrawHorizontalLine(session, begin, GetDividerLevel(scale));
            }

            float half = scale * 0.5f;

            DrawHorizontalLines(session, begin - half, half);
            DrawHorizontalLines(session, begin + half, half);
        }

        private void DrawVerticalLines(CanvasDrawingSession session, float begin, float scale)
        {
            if (scale < 10) return;

            if (begin > 0)
            {
                DrawVerticalLine(session, begin, GetDividerLevel(scale));
            }

            float half = scale * 0.5f;

            DrawVerticalLines(session, begin - half, half);
            DrawVerticalLines(session, begin + half, half);
        }

        private void DrawHorizontalLine(CanvasDrawingSession session, float x, int dividerLevel)
        {
            Color color = dividerLevel == 0 ? DividerColor : SubDividerColor;

            session.DrawLine(x, 0, x, (int)GridHeight, color);
        }

        private void DrawVerticalLine(CanvasDrawingSession session, float y, int dividerLevel)
        {
            Color color = dividerLevel == 0 ? DividerColor : SubDividerColor;

            session.DrawLine(0, y, (int)GridWidth, y, color);
        }

        private int GetDividerLevel(float scale)
        {
            int dividerLevel = 0;

            if (scale < 10) dividerLevel = 2;
            else if (scale < 100) dividerLevel = 1;

            return dividerLevel;
        }

        private void PointerEnteredGrid(object sender, PointerRoutedEventArgs args)
        {
            var pos = SanitizePointerPosition(args.GetCurrentPoint(this).Position);

            UpdateCursorLines(pos);

            pointerState = PointerState.Entered;

            this.GridCanvas.Invalidate();
        }

        private void PointerMovedInGrid(object sender, PointerRoutedEventArgs args)
        {
            var pos = SanitizePointerPosition(args.GetCurrentPoint(this).Position);

            UpdateCursorLines(pos);

            this.GridCanvas.Invalidate();
        }

        private void PointerExitedGrid(object sender, PointerRoutedEventArgs args)
        {
            pointerState = PointerState.Exited;

            this.GridCanvas.Invalidate();
        }

        private void PointerPressedOnGrid(object sender, PointerRoutedEventArgs args)
        {
            var pos = SanitizePointerPosition(args.GetCurrentPoint(this).Position);

            cursorLines[2].X1 = cursorLines[2].X2 = pos.X;
            cursorLines[2].Y1 = cursorLines[2].Y2 = pos.Y;

            cursorLines[3].X1 = cursorLines[3].X2 = pos.X;
            cursorLines[3].Y1 = cursorLines[3].Y2 = pos.Y;

            pointerKeyState = KeyState.Pressed;

            this.GridCanvas.Invalidate();
        }

        private void PointerReleasedOnGrid(object sender, PointerRoutedEventArgs args)
        {
            pointerKeyState = KeyState.Released;

            this.GridCanvas.Invalidate();
        }

        private void UpdateCursorLines(Point pointer)
        {
            // Pointer Movement Lines

            cursorLines[0].X1 = pointer.X;
            cursorLines[0].Y1 = 0;
            cursorLines[0].X2 = pointer.X;
            cursorLines[0].Y2 = GridHeight;

            cursorLines[1].X1 = 0;
            cursorLines[1].Y1 = pointer.Y;
            cursorLines[1].X2 = GridWidth;
            cursorLines[1].Y2 = pointer.Y;

            // Key Press Lines

            cursorLines[2].X2 = pointer.X;
            cursorLines[2].Y2 = cursorLines[2].Y1;

            cursorLines[3].X2 = cursorLines[3].X1;
            cursorLines[3].Y2 = pointer.Y;
        }

        private Point SanitizePointerPosition(Point pos)
        {
            pos.X = Math.Max(CursorStartX, Math.Min(pos.X, GridWidth));
            pos.Y = Math.Max(CursorStartY, Math.Min(pos.Y, GridHeight));

            if (SnapToDivider)
            {
                pos.X = Math.Round(pos.X / (PixelsPerUnit * UnitsPerScale)) * (PixelsPerUnit * UnitsPerScale);
                pos.Y = Math.Round(pos.Y / (PixelsPerUnit * UnitsPerScale)) * (PixelsPerUnit * UnitsPerScale);
            }
            else if (SnapToSubDivider)
            {
                pos.X = Math.Round(pos.X / PixelsPerUnit) * PixelsPerUnit;
                pos.Y = Math.Round(pos.Y / PixelsPerUnit) * PixelsPerUnit;
            }

            return pos;
        }

        private void DrawText(string text, Vector2 pos, Color bgColor, Color fgColor, CanvasDrawingSession session)
        {
            CanvasTextFormat format = new CanvasTextFormat
            {
                FontFamily = FontFamily.Source,
                FontSize = 14,
                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                VerticalAlignment = CanvasVerticalAlignment.Bottom,
                WordWrapping = CanvasWordWrapping.NoWrap
            };

            CanvasTextLayout textLayout = new CanvasTextLayout(session, text, format, 0.0f, 0.0f);
            Rect rect = new Rect(pos.X + textLayout.LayoutBounds.X, pos.Y + textLayout.LayoutBounds.Y, textLayout.LayoutBounds.Width, textLayout.LayoutBounds.Height);
            session.FillRectangle(rect, bgColor);
            session.DrawTextLayout(textLayout, pos, fgColor);
        }
    }
}
