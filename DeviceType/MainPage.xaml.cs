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

namespace DeviceType
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

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var message = e.PointerDeviceType + "で、TAP!";
            InsertString(message);
        }

        private void Rectangle_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var message = e.PointerDeviceType + "で、Hold " + e.HoldingState;
            InsertString(message);
        }

        private void Rectangle_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var message = e.PointerDeviceType + "で、Right TAP!";
            textBox1.Text = message + Environment.NewLine + textBox1.Text;
        }

        private void Rectangle_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var message = e.PointerDeviceType + "で、Double TAP!";
            InsertString(message);
        }

        private void InsertString(string message)
        {
            textBox1.Text = message + Environment.NewLine + textBox1.Text;
        }
    }
}
