using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace InkSample
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // 描画中の線を保持する
        private Polyline polyline;

        // 文字認識に使用するInkManager
        private InkManager inkmanager = new InkManager();

        // 現在入力中のPointerIdを保持する。
        // 入力がない場合は、nullとする。
        private uint? pid;

        public MainPage()
        {
            this.InitializeComponent();

            // 文字識別用のInkRecognizerを取得する。
            var recongnizer =
                inkmanager.GetRecognizers()
                .FirstOrDefault(r => r.Name.Contains("日本語"));

            // 取得できた場合はInkManagerに設定
            if (recongnizer != null)
            {
                inkmanager.SetDefaultRecognizer(recongnizer);
            }
            
        }

        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。Parameter 
        /// プロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        // 描画開始
        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 入力中のPointerがある場合は、新しい入力は開始しない。
            if (pid != null)
            {
                return;
            }

            // PointerPointの取得
            var canvas = sender as Canvas;
            var point = e.GetCurrentPoint(canvas);

            // 線の初期化
            polyline = new Polyline
            {
                StrokeThickness = 3,
                Stroke = new SolidColorBrush(Colors.Red)
            };

            // 線に頂点を追加
            polyline.Points.Add(point.Position);

            // Canvasに登録して描画
            canvas.Children.Add(polyline);

            // MoveイベントがCanvasで発生するようにキャプチャ
            canvas.CapturePointer(e.Pointer);

            // InkManagerに描画が始まったことを通知
            inkmanager.ProcessPointerDown(point);

            // PointerIdを保持
            pid = point.PointerId;
        }

        // 描画中
        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // PointerPointの取得
            var canvas = sender as Canvas;
            var point = e.GetCurrentPoint(canvas);

            // 開始した描画ポイントととこなる場合は、処理しない。
            if (pid != point.PointerId) return;

            // InkManagerに描画ポイントを追加
            polyline.Points.Add(point.Position);

            // InkManagerに描画ポイントの追加
            inkmanager.ProcessPointerUpdate(point);
        }

        // 描画終了
        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // PointerPointの取得
            var canvas = (Canvas)sender;
            var point = e.GetCurrentPoint(canvas);

            // 現在描画中の入力デバイス以外からのイベントは受け取らない
            if (pid != point.PointerId)
            {
                return;
            }

            // マウスをリリース
            canvas.ReleasePointerCapture(e.Pointer);

            // InkManagerに描画が終わったことを通知
            inkmanager.ProcessPointerUp(point);
            pid = null;
        }

        // 文字認識を行う
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rec = await inkmanager.
                    RecognizeAsync(InkRecognitionTarget.All);

                textBox1.Text =
                    string.Join(",",
                        rec.FirstOrDefault().GetTextCandidates()
                    );
            }
            catch (Exception)
            {

                textBox1.Text = "認識失敗";
            }
        }

    }
}
