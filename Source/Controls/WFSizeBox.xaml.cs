﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class WFSizeBox : UserControl
    {
        public WFSizeBox()
        {
            this.InitializeComponent();

            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(e.NewSize.Width < _textBlock.Width || e.NewSize.Height < _textBlock.Height)
            {
                _textBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                _textBlock.Visibility = Visibility.Visible;
            }

            _text.Text = e.NewSize.Width.ToString() + "x" + e.NewSize.Height.ToString();
        }
    }
}
