using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XObjdetect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class BackgroundClicker
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, int nFlags);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        const uint MOUSEEVENTF_LEFTUP = 0x04;

        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        static Bitmap CaptureScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap bmp = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            }
            return bmp;
        }


        public static Point? FindImage(Bitmap sourceBmp, Bitmap templateBmp, double threshold = 0.9)
        {
            using var source = sourceBmp.ToImage<Bgr, byte>();
            using var template = templateBmp.ToImage<Bgr, byte>();
            using var result = source.MatchTemplate(template, TemplateMatchingType.CcoeffNormed);

            double minVal = 0, maxVal = 0;
            Point minLoc = Point.Empty, maxLoc = Point.Empty;
            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

            // Chỉ trả về điểm nếu độ khớp lớn hơn hoặc bằng threshold
            if (maxVal >= threshold)
                return maxLoc;
            else
                return null;
        }



        public static string FindImagePosition(string image_name)
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(baseFolder, "image", image_name + ".png");
            var templateBmp = new Bitmap(imagePath);
            var capturedBmp = CaptureScreen(); // dùng chụp toàn màn

            var foundPoint = FindImage(capturedBmp, templateBmp);
            if (foundPoint.HasValue)
            {
                return $"{foundPoint.Value.X}|{foundPoint.Value.Y}";
            }
            else
            {
                return "0";
            }
        }

        public static void CountAndClickAllImages(string imageName, double threshold = 0.9)
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(baseFolder, "image", imageName + ".png");

            using var templateBmp = new Bitmap(imagePath);
            int templateW = templateBmp.Width;
            int templateH = templateBmp.Height;

            // Chụp màn hình 1 lần
            using var screenBmp = CaptureScreen();

            // Làm việc trên 1 bản sao ảnh chụp để che vùng trùng
            var workingBmp = new Bitmap(screenBmp);

            var matchPoints = new List<Point>();

            while (true)
            {
                // Gọi lại chính FindImage bạn đã viết
                var foundPoint = FindImage(workingBmp, templateBmp, threshold);

                if (!foundPoint.HasValue)
                {
                    break;
                }

                // Tọa độ click chính giữa vùng
                int clickX = foundPoint.Value.X + templateW / 2;
                int clickY = foundPoint.Value.Y + templateH / 2;

                matchPoints.Add(new Point(clickX, clickY));

                // Che vùng vừa tìm để không lặp lại (tô đen)
                using (Graphics g = Graphics.FromImage(workingBmp))
                {
                    g.FillRectangle(Brushes.Black,
                        new Rectangle(foundPoint.Value.X, foundPoint.Value.Y, templateW, templateH));
                }
            }

            if (matchPoints.Count == 0)
            {
                Console.WriteLine("Không tìm thấy ảnh nào.");
                return;
            }

            // Click tất cả điểm đã tìm được
            foreach (var pt in matchPoints)
            {
                ClickAtScreen(pt.X, pt.Y);
                BackgroundClicker.MoveMouse();
                if (Form1.CheckCloseAndClickEx())
                {
                    return;
                }
                Thread.Sleep(500);
            }

            Console.WriteLine($"Đã tìm thấy và click {matchPoints.Count} điểm.");
        }


        public static bool WaitForImageAndClick(string image_name, bool click = true, double threshold = 0.9, int maxWaitTimeInSeconds = 60)
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(baseFolder, "image", image_name + ".png");
            var templateBmp = new Bitmap(imagePath);

            DateTime startTime = DateTime.Now;
            Point? foundPoint = null;

            // Wait for the image to appear within the given time
            while ((DateTime.Now - startTime).TotalSeconds < maxWaitTimeInSeconds)
            {
                var capturedBmp = CaptureScreen(); // Capture the screen
                foundPoint = FindImage(capturedBmp, templateBmp);

                if (foundPoint.HasValue) // If image is found
                {
                    if (click)
                    {
                        ClickAtScreen(foundPoint.Value.X, foundPoint.Value.Y);
                    }
                    return true;
                }

                Thread.Sleep(350);
            }
            return false;
        }

        public static bool WaitForImageDisappear(string image_name, double threshold = 0.9, int maxWaitTimeInSeconds = 60)
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(baseFolder, "image", image_name + ".png");
            using var templateBmp = new Bitmap(imagePath);

            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalSeconds < maxWaitTimeInSeconds)
            {
                using var capturedBmp = CaptureScreen(); // Chụp màn hình
                var foundPoint = FindImage(capturedBmp, templateBmp, threshold);

                // Nếu KHÔNG tìm thấy ảnh nữa thì return true
                if (!foundPoint.HasValue)
                {
                    return true;
                }
                Thread.Sleep(350);
            }
            // Nếu hết thời gian mà vẫn còn ảnh, return false
            return false;
        }


        public static void ClickAtScreen(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(50);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        public static void DragMouse(int startX, int startY, int endX, int endY, int steps = 50, int delayPerStepMs = 10)
        {
            // 1. Move to start and press down
            SetCursorPos(startX, startY);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(50);
            // 2. Simulate drag by moving step by step
            for (int i = 1; i <= steps; i++)
            {
                int currentX = startX + (endX - startX) * i / steps;
                int currentY = startY + (endY - startY) * i / steps;
                SetCursorPos(currentX, currentY);
                Thread.Sleep(delayPerStepMs);
            }
            // 3. Release
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        public static void MoveMouse()
        {
            GetCursorPos(out POINT currentPos);
            int newX = currentPos.X + 400;
            int newY = currentPos.Y;
            SetCursorPos(newX, newY);
        }
    }
}
