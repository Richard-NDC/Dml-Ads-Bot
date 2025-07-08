using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int ThuTuDao = 1;
        private volatile bool _running = false;

        // Khai báo cho RegisterHotKey và UnregisterHotKey
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Định nghĩa modifier key
        public const int MOD_NONE = 0x0000; // Không cần giữ Alt, Ctrl, Shift
        public const int WM_HOTKEY = 0x0312;

        private const int HOTKEY_ID = 9000; // số bất kỳ > 0, không trùng lặp

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // Đăng ký phím F10 (có thể thay bằng Keys.Escape nếu muốn)
            RegisterHotKey(this.Handle, HOTKEY_ID, MOD_NONE, (int)Keys.F10);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            base.OnHandleDestroyed(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                _running = false;
                MessageBox.Show("Đã dừng tool bằng phím nóng (global hotkey)!");
            }
            base.WndProc(ref m);
        }

        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("Trước khi sử dụng, vui lòng bật sẵn game, chỉnh cam về vị trí cao nhất để tránh lỗi\n\nBefore using, please turn on the game, adjust the perspective to the highest position to avoid the error");
        }

        private void ClickImage(string image)
        {
            string position = BackgroundClicker.FindImagePosition(image);
            if (position != "0")
            {
                int X = Convert.ToInt32(position.Split('|')[0]);
                int Y = Convert.ToInt32(position.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void OpenShop()
        {
            string position = BackgroundClicker.FindImagePosition("Shop");
            if (position != "0")
            {
                int X = Convert.ToInt32(position.Split('|')[0]);
                int Y = Convert.ToInt32(position.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void CloseShop()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Close");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private bool CheckClose()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Close");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private void CollectGold()
        {
            BackgroundClicker.CountAndClickAllImages("Gold");
        }

        private void CollectFood()
        {
            BackgroundClicker.CountAndClickAllImages("Food");
        }

        private bool CheckGemsAds()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Close");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private bool CheckFood()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Food");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private bool CheckGold()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Gold");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private void ClickGemsAds()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Gems");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void ClickOttoAds()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Otto_ads");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private bool CheckOttoAds()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Otto_ads");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }


        private void ClickOK()
        {
            string closeposition = BackgroundClicker.FindImagePosition("OK");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }
        private void ClickClaim()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Claim");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void ClickSpin()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Spin");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void ClickCloseFB()
        {
            string closeposition = BackgroundClicker.FindImagePosition("closefb");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private bool CheckIfThisAdIsAutoClose()
        {
            string closeposition = BackgroundClicker.FindImagePosition("Auto_close_ads");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private bool CheckEggHatchingSkip()
        {
            string closeposition = BackgroundClicker.FindImagePosition("skip30m");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private bool CheckCloseFB()
        {
            string closeposition = BackgroundClicker.FindImagePosition("closefb");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private bool CheckClosedo()
        {
            string closeposition = BackgroundClicker.FindImagePosition("closedo");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private bool CheckClosevang()
        {
            string closeposition = BackgroundClicker.FindImagePosition("closevang");
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private bool CheckImage(string image)
        {
            string closeposition = BackgroundClicker.FindImagePosition(image);
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private void ClickEggHatchingSkip()
        {
            string closeposition = BackgroundClicker.FindImagePosition("skip30m");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void ClickClosevang()
        {
            string closeposition = BackgroundClicker.FindImagePosition("closevang");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void ClickClosedo()
        {
            string closeposition = BackgroundClicker.FindImagePosition("closedo");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void ClickDivine()
        {
            string closeposition = BackgroundClicker.FindImagePosition("divine");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void ClickScrollDown()
        {
            string closeposition = BackgroundClicker.FindImagePosition("scrolldown");
            if (closeposition != "0")
            {
                int X = Convert.ToInt32(closeposition.Split('|')[0]);
                int Y = Convert.ToInt32(closeposition.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private void ReopenGame()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.MainWindowTitle.Contains("Dragon Mania Legends"))
                {
                    process.Kill();
                }
            }
            string appUserModelId = "A278AB0D.DragonManiaLegends_h6adky7gbf63m!App";
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"shell:appsFolder\\{appUserModelId}",
                    UseShellExecute = true
                });
                Console.WriteLine("Đã bật lại Dragon Mania Legends.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể bật app: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string windowTitle = "Dragon Mania Legends";
            IntPtr hwnd = BackgroundClicker.FindWindow(null, windowTitle);
            if (hwnd == IntPtr.Zero)
            {
                MessageBox.Show("Game Not Found");
            }
            else
            {
                _running = true;
                new Thread(() =>
                {
                    while (_running)
                    {
                        if (otto.Checked)
                        {
                            if (CheckOttoAds() || CheckImage("Spin"))
                            {
                                if (CheckImage("Spin"))
                                {
                                    ClickImage("Spin");
                                    if (BackgroundClicker.WaitForImageAndClick("Otto_ads", false))
                                    {
                                        continue;
                                    }
                                }
                                ClickOttoAds();
                                if (BackgroundClicker.WaitForImageAndClick("Spin"))
                                {
                                    if (CheckImage("Spin"))
                                    {
                                        ClickImage("Spin");
                                    }
                                    if (BackgroundClicker.WaitForImageAndClick("Otto_ads", false))
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                /*ReopenGame();
                                if (BackgroundClicker.WaitForImageDisappear("loadscreen"))
                                {
                                    DateTime startTime = DateTime.Now;
                                    while ((DateTime.Now - startTime).TotalSeconds < 10000)
                                    {
                                        if (CheckCloseFB())
                                        {
                                            ClickCloseFB();
                                            Thread.Sleep(400);
                                        }
                                        if (CheckClosedo())
                                        {
                                            ClickClosedo();
                                            Thread.Sleep(400);
                                        }
                                        if (CheckClosevang())
                                        {
                                            ClickClosevang();
                                            Thread.Sleep(400);
                                        }
                                        if (CheckClose())
                                        {
                                            CloseShop();
                                            Thread.Sleep(400);
                                        }
                                    }
                                    ClickDivine();
                                    Thread.Sleep(300);
                                    while (!BackgroundClicker.WaitForImageAndClick("ottobtn", true, 0.9, 5))
                                    {
                                        ClickScrollDown();
                                    }
                                }*/
                            }
                        }
                        else
                        {
                            if (CheckEggHatchingSkip())
                            {
                                ClickEggHatchingSkip();
                                Thread.Sleep(200);
                                ClickOK();
                                if (BackgroundClicker.WaitForImageAndClick("Claim"))
                                {
                                    ClickClaim();
                                    if (BackgroundClicker.WaitForImageAndClick("skip30m", false))
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }).Start();
            }
        
        }

        private void otto_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng bật sẵn Otto's Lotto để chạy");
        }

        private void hatching_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng chọn trứng muốn skip trước khi chạy");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) // Dừng khi bấm ESC
            {
                _running = false;
            }
        }
    }
}
