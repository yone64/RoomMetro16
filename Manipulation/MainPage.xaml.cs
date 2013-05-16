using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Manipulation
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。Parameter 
        /// プロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }



        private void Rectangle_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var transform = ((UIElement)sender).RenderTransform as CompositeTransform;
            var diffX = e.Position.X - transform.CenterX;
            var diffY = e.Position.Y - transform.CenterY;

            var rad = Math.Atan2(diffY, diffX);
            var d = rad + transform.Rotation * Math.PI / 180;
            var dist = Math.Sqrt(diffX * diffX + diffY * diffY);
            var newX = dist * Math.Cos(d);
            var newY = dist * Math.Sin(d);

            transform.CenterX = e.Position.X;
            transform.CenterY = e.Position.Y;

            transform.ScaleX *= e.Delta.Scale;
            transform.ScaleY *= e.Delta.Scale;

            transform.Rotation += e.Delta.Rotation;

            transform.TranslateX += e.Delta.Translation.X + diffX * (transform.ScaleX - 1) - (diffX - newX) * transform.ScaleX;
            transform.TranslateY += e.Delta.Translation.Y + diffY * (transform.ScaleY - 1) - (diffY - newY) * transform.ScaleY;

            //transform.TranslateX += e.Delta.Translation.X - diffX + newX * transform.ScaleX;
            //transform.TranslateY += e.Delta.Translation.Y - diffY + newY * transform.ScaleY;
        }

        private void Ellipse_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (e.IsInertial)
            {
                e.Complete();
            }
            var group = ((UIElement)sender).RenderTransform as TransformGroup;

            var matrix = group.Children[0] as MatrixTransform;
            var composit = group.Children[1] as CompositeTransform;

            matrix.Matrix = group.Value;

            var center = matrix.TransformPoint(new Point(e.Position.X, e.Position.Y));
            composit.CenterX = center.X;
            composit.CenterY = center.Y;

            composit.ScaleX = e.Delta.Scale;
            composit.ScaleY = e.Delta.Scale;

            composit.Rotation = e.Delta.Rotation;

            composit.TranslateX = e.Delta.Translation.X;
            composit.TranslateY = e.Delta.Translation.Y;

        }
    }
}
