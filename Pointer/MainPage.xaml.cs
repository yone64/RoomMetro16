using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Input;
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

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pointer
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

        /// <summary>
        /// PointerIdと描画中の線を紐づけるDictionary
        /// </summary>
        private Dictionary<uint, Polyline> dic = new Dictionary<uint, Polyline>();

        // Pointerが押されたときのイベント
        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // PointerPointの取得
            var canvas = (Canvas)sender;
            var pointerPoint = e.GetCurrentPoint(canvas);
            
            // 赤色でPolylineを描画
            var line = new Polyline 
            {
                StrokeThickness = 3,
                Stroke = new SolidColorBrush(Colors.Red)
            };

            // Polylineに頂点を追加
            line.Points.Add(pointerPoint.Position);

            // PointerIdとPolylineを紐づけ
            dic[pointerPoint.PointerId] = line;

            // キャンバスに描画する線を追加
            canvas.Children.Add(line);

            // MoveイベントがCanvas上で発生するようにPointerをキャプチャ
            canvas.CapturePointer(e.Pointer);
        }

        // Pointerが動いた時のイベント
        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // PointerPointの取得
            var pointerPoint = e.GetCurrentPoint((UIElement)sender);
            var pid = pointerPoint.PointerId;

            // Moveイベントは押下中じゃなくても発生するので
            // 描画中かどうかの判断を行う。
            if (dic.ContainsKey(pid))
            {
                // 描画中のPolylineに頂点を追加する
                dic[pid].Points.Add(pointerPoint.Position);
            }
        }

        // Pointerが離された時のイベント
        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // PointerPointの取得
            var canvas = (Canvas)sender;
            var pointerPoint = e.GetCurrentPoint(canvas);

            // PointerIdとPolylineを紐づけを解除し、描画を終了。
            dic.Remove(pointerPoint.PointerId);

            // Pointerのキャプチャも終了する
            canvas.ReleasePointerCapture(e.Pointer);
        }
    }
}
