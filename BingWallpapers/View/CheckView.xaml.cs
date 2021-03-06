﻿using BingWallpapers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BingWallpapers.View
{
    /// <summary>
    /// CheckView.xaml 的交互逻辑
    /// </summary>
    public partial class CheckView : Page
    {
        public CheckView()
        {
            InitializeComponent();
            DataContext = new CheckViewModel(this);
        }
        public void SetProgress(double progress)
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation(progress, TimeSpan.FromMilliseconds(400));

            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, progressBar);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Value"));
            animation.EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut };

            storyboard.Begin();
        }
    }
}
