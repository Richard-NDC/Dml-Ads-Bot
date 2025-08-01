using AutoHotkey.Interop;
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
        static bool needExHandling = false;
        private int ThuTuDao = 1;
        private volatile bool _running = false;
        private string scriptpath = @"C:\Users\RCN\Desktop\AHK";

        // Khai báo cho RegisterHotKey và UnregisterHotKey
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Định nghĩa modifier key
        public const int MOD_NONE = 0x0000; // Không cần giữ Alt, Ctrl, Shift
        public const int WM_HOTKEY = 0x0312;

        private const int HOTKEY_ID = 9000; // số bất kỳ > 0, không trùng lặp
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint uFlags);

        // Định nghĩa các tham số
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
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

        private static void ClickImage(string image)
        {
            string position = BackgroundClicker.FindImagePosition(image);
            if (position != "0")
            {
                int X = Convert.ToInt32(position.Split('|')[0]);
                int Y = Convert.ToInt32(position.Split('|')[1]);
                BackgroundClicker.ClickAtScreen(X, Y);
            }
        }

        private static bool CheckImage(string image)
        {
            string closeposition = BackgroundClicker.FindImagePosition(image);
            if (closeposition != "0")
            {
                return true;
            }
            return false;
        }

        private void RunAHKScript(string name)
        {
            string ahkExePath = @"C:\Program Files\AutoHotkey\AutoHotkey.exe";
            string ahkScriptPath = Path.Combine(scriptpath, name);
            if (System.IO.File.Exists(ahkScriptPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = ahkExePath,
                    Arguments = $"\"{ahkScriptPath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process process = Process.Start(startInfo);
                process.WaitForExit();
            }
            else
            {
                MessageBox.Show("Không tìm thấy tệp AHK.");
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

        private void CloseAllPopup(int second)
        {
            DateTime now = DateTime.Now;
            TimeSpan duration = TimeSpan.FromSeconds(second);
            while (DateTime.Now - now < duration)
            {
                if (CheckImage("closefb"))
                {
                    ClickImage("closefb");
                }
                if (CheckImage("closedo"))
                {
                    ClickImage("closedo");
                }
                if (!hatching.Checked)
                {
                    if (CheckImage("closevang"))
                    {
                        ClickImage("closevang");
                    }
                }
                if (!otto.Checked)
                {
                    if (CheckImage("Close"))
                    {
                        ClickImage("Close");
                        Thread.Sleep(500);
                        CloseVang();
                    }
                }
                if (CheckImage("closetask"))
                {
                    ClickImage("closetask");
                }
            }
        }

        private void ReturnToBase()
        {
            RunAHKScript("8-1.ahk");
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
                SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
                Thread.Sleep(1000);
                SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
                _running = true;
                while (true)
                {
                    if (otto.Checked)
                    {
                        if (CheckImage("Otto_ads") || CheckImage("Spin"))
                        {
                            if (CheckImage("Spin"))
                            {
                                ClickImage("Spin");
                                if (BackgroundClicker.WaitForImageAndClick("Spin", false, 0.9, 20))
                                {
                                    continue;
                                }
                                if (BackgroundClicker.WaitForImageAndClick("Otto_ads", false, 0.9, 20))
                                {
                                    continue;
                                }
                            }
                            ClickImage("Otto_ads");
                            if (!BackgroundClicker.WaitForImageAndClick("Auto_close_ads", false, 0.9, 5))
                            {
                                CloseAds();
                            }
                            if (BackgroundClicker.WaitForImageAndClick("Spin"))
                            {
                                if (CheckImage("Spin"))
                                {
                                    ClickImage("Spin");
                                }
                                if (BackgroundClicker.WaitForImageAndClick("Otto_ads", false, 0.9, 20))
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
                    else if (hatching.Checked)
                    {
                        if (CheckImage("skip30m"))
                        {
                            ClickImage("skip30m");
                            Thread.Sleep(200);
                            ClickImage("OK");
                            Thread.Sleep(1000);
                            if (!BackgroundClicker.WaitForImageAndClick("Auto_close_ads", false, 0.9, 5))
                            {
                                if (BackgroundClicker.WaitForImageAndClick("ads_ex"))
                                {
                                    ClickImage("ads_ex");
                                }
                                else
                                {
                                    ClickImage("ads_ex_2");
                                }
                            }
                            if (BackgroundClicker.WaitForImageAndClick("Claim", false))
                            {
                                ClickImage("Claim");
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
                    else if (earngems.Checked)
                    {
                        CloseAllPopup(3);
                        if (CheckImage("Gold") && !needExHandling)
                        {
                            CollectGold();
                        }
                        if (CheckImage("Food") && !needExHandling)
                        {
                            CollectFood();
                        }
                        if (CheckImage("talis") && !needExHandling)
                        {
                            CollectTalis();
                        }
                        if (CheckImage("stone") && !needExHandling)
                        {
                            CollectStone();
                        }
                    IL_RE:
                        if (CheckImage("Gems") || CheckImage("Claim") || CheckImage("gems1") || CheckImage("gemsads") || CheckImage("gems2"))
                        {
                            if (CheckImage("Close"))
                            {
                                ClickImage("Close");
                                Thread.Sleep(500);
                            }
                            if (CheckImage("OK"))
                            {
                                ClickImage("OK");
                            }
                            ClickImage("Gems");
                            ClickImage("gems1");
                            ClickImage("gems2");
                            Thread.Sleep(500);
                            if (CheckImage("Close"))
                            {
                                ClickImage("Close");
                                Thread.Sleep(500);
                                needExHandling = true;
                            }
                            else
                            {
                                ClickImage("OK");
                                Thread.Sleep(1000);
                                if (!BackgroundClicker.WaitForImageAndClick("Auto_close_ads", false, 0.9, 12))
                                {
                                    if (BackgroundClicker.WaitForImageAndClick("ads_ex"))
                                    {
                                        ClickImage("ads_ex");
                                    }
                                    else
                                    {
                                        ClickImage("ads_ex_2");
                                    }
                                    Thread.Sleep(500);
                                }
                                if (BackgroundClicker.WaitForImageAndClick("Claim"))
                                {
                                    if (CheckImage("Claim"))
                                    {
                                        ClickImage("Claim");
                                    }
                                    Thread.Sleep(500);
                                }
                                ClickOthers();
                            }
                        }
                        if (needExHandling || !(CheckImage("Gems") || CheckImage("Claim") || CheckImage("gems1")))
                        {
                            needExHandling = false;
                            if (CheckImage("Food"))
                            {
                                CollectFood();
                            }
                            if (CheckImage("centre") && ThuTuDao != 1)
                            {
                                ClickImage("centre");
                                Thread.Sleep(1000);
                                if (CheckImage("gemsads"))
                                {
                                    goto IL_RE;
                                }
                                if (BackgroundClicker.WaitForImageAndClick("other", false))
                                {
                                    ClickImage("other");
                                    Thread.Sleep(200);
                                }
                                ThuTuDao = 1;
                                continue;
                            }
                            if (ThuTuDao == 8)
                            {
                                ReturnToBase();
                                if (CheckImage("centre"))
                                {
                                    ClickImage("centre");
                                    Thread.Sleep(1000);
                                    if (CheckImage("gemsads"))
                                    {
                                        goto IL_RE;
                                    }
                                    if (BackgroundClicker.WaitForImageAndClick("other", false))
                                    {
                                        ClickImage("other");
                                        Thread.Sleep(200);
                                    }
                                }
                                ThuTuDao = 1;
                                continue;
                            }
                            else
                            {
                                switch (ThuTuDao)
                                {
                                    case 1:
                                        CloseVang();
                                        RunAHKScript("1-2.ahk");
                                        break;
                                    case 2:
                                        CloseVang();
                                        RunAHKScript("2-3.ahk");
                                        break;
                                }
                                ThuTuDao++;
                            }
                        }
                    }
                    else if (spamcardads.Checked)
                    {
                        if (CheckImage("card_ads"))
                        {
                            ClickImage("card_ads");
                            if (!BackgroundClicker.WaitForImageAndClick("Auto_close_ads", false, 0.9, 5))
                            {
                                CloseAds();
                            }
                            while (BackgroundClicker.WaitForImageAndClick("open_card", true, 0.9, 1))
                            {
                                ClickImage("open_card");
                            }
                            if (BackgroundClicker.WaitForImageAndClick("claim_card", false, 0.9, 5))
                            {
                                ClickImage("claim_card");
                            }    
                        }
                    }
                    else
                    {
                        if (CheckImage("chest_ads")||CheckImage("claim_chest"))
                        {
                            if (CheckImage("claim_chest"))
                            {
                                ClickImage("claim_chest");
                            }    
                            ClickImage("chest_ads");
                            if (!BackgroundClicker.WaitForImageAndClick("Auto_close_ads", false, 0.9, 5))
                            {
                                CloseAds();
                            }
                            if (BackgroundClicker.WaitForImageAndClick("claim_chest", true, 0.9, 30))
                            {
                                ClickImage("claim_chest");
                            }
                            Thread.Sleep(800);
                        }

                    }
                }
            }
        }
        private void ClickOthers()
        {
            switch (ThuTuDao)
            {
                case 1:
                    BackgroundClicker.WaitForImageAndClick("other", true, 0.9, 2);
                    break;
                case 2:
                    BackgroundClicker.WaitForImageAndClick("2", true, 0.9, 2);
                    break;
                case 3:
                    BackgroundClicker.WaitForImageAndClick("3", true, 0.9, 2);
                    break;
                case 4:
                    BackgroundClicker.WaitForImageAndClick("4", true, 0.9, 2);
                    break;
                case 5:
                    BackgroundClicker.WaitForImageAndClick("5", true, 0.9, 2);
                    break;
                case 6:
                    BackgroundClicker.WaitForImageAndClick("6", true, 0.9, 2);
                    break;
                case 7:
                    BackgroundClicker.WaitForImageAndClick("7", true, 0.9, 2);
                    break;
            }
        }

        private void CloseAds()
        {
            if (BackgroundClicker.WaitForImageAndClick("ads_ex"))
            {
                ClickImage("ads_ex");
            }
            else if (BackgroundClicker.WaitForImageAndClick("ads_ex_2"))
            {
                ClickImage("ads_ex_2");
            }
            else
            {
                ClickImage("ads_ex_3");
            }
        }

        private void CloseVang()
        {
            if (CheckImage("closevang"))
            {
                ClickImage("closevang");
                Thread.Sleep(500);
            }
        }

        private void CollectFood()
        {
            BackgroundClicker.CountAndClickAllImages("Food");
        }

        public static bool CheckCloseAndClickEx()
        {
            if (CheckImage("Close"))
            {
                ClickImage("Close");
                Thread.Sleep(500);
                needExHandling = true;
                return true;
            }
            return false;
        }

        public void CheckCloseAndClick()
        {
            if (CheckImage("Close"))
            {
                ClickImage("Close");
                Thread.Sleep(500);
            }
        }

        private void CollectGold()
        {
            BackgroundClicker.CountAndClickAllImages("Gold");
        }

        private void CollectTalis()
        {
            BackgroundClicker.CountAndClickAllImages("talis");
        }

        private void CollectStone()
        {
            BackgroundClicker.CountAndClickAllImages("stone");
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

        private void radioButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng đưa vị trí tầm nhìn về đảo 1 (Đảo được tặng ban đầu)");
        }
    }
}
