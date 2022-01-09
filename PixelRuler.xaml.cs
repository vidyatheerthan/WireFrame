using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    /// refer website: https://edi.wang/post/2018/11/5/build-pixel-ruler-uwp
    /// for details on how to implement this ruler functionality
    /// </summary>
    public sealed partial class PixelRuler : UserControl, INotifyPropertyChanged
    {
        //////////////////////

        public static readonly DependencyProperty ScaleMarkPositionProperty = DependencyProperty.Register(
            nameof(ScaleMarkPosition),
            typeof(int),
            typeof(PixelRuler),
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
            typeof(PixelRuler),
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
            typeof(PixelRuler),
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
            typeof(PixelRuler),
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
            typeof(PixelRuler),
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
            typeof(PixelRuler),
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
            typeof(PixelRuler),
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
            typeof(PixelRuler),
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
            typeof(PixelRuler),
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
            typeof(PixelRuler),
            new PropertyMetadata(null)
        );

        public int UnitsPerScale
        {
            get => (int)GetValue(UnitsPerScaleProperty);
            set => SetValue(UnitsPerScaleProperty, value);
        }

        //////////////////////

        public event PropertyChangedEventHandler PropertyChanged;

        ///////////////////////////////////////////////////////////////////////////////////////////////

        public PixelRuler()
        {
            Color WHITE = Color.FromArgb(255, 200, 200, 200);
            Color BLACK = Color.FromArgb(255, 0, 0, 0);

            this.InitializeComponent();

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
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RulerCanvas_OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var session = args.DrawingSession;

            session.DrawLine(0, 0, RulerLength, 0, DividerColor); // top
            session.DrawLine(0, RulerWidth, RulerLength, RulerWidth, DividerColor); // bottom

            for(int unit = 0, x=0; x < RulerLength - ScaleMarkPosition; x+=PixelsPerUnit)
            {
                int offset = ScaleMarkPosition + x;

                if (x % (PixelsPerUnit * UnitsPerScale) == 0)
                {
                    session.DrawLine(offset, 0, offset, LargeDividerLength, DividerColor);
                    session.DrawLine(offset, RulerWidth, offset, RulerWidth - LargeDividerLength, DividerColor);

                    session.DrawText(unit.ToString(), offset, RulerWidth / 2, TextColor, new CanvasTextFormat()
                    {
                        FontSize = (float)FontSize,
                        FontFamily = FontFamily.Source,
                        HorizontalAlignment = CanvasHorizontalAlignment.Center,
                        VerticalAlignment = CanvasVerticalAlignment.Center
                    });

                    ++unit;
                }
                else
                {
                    session.DrawLine(offset, 0, offset, SmallDividerLength, DividerColor);
                    session.DrawLine(offset, RulerWidth, offset, RulerWidth - SmallDividerLength, DividerColor);
                }
            }
        }
    }
}
