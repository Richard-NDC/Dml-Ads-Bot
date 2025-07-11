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
        private string scriptpath = @"C:\Users\RCN\Desktop\macro\";

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

        private bool CheckImage(string image)
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
            try
            {
                string fullpath = Path.Combine(scriptpath, name);
                AutoHotkeyExecutor executor = new AutoHotkeyExecutor();
                executor.ExecuteScriptFromFile(fullpath);
                executor.CleanUp();
            } 
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
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
                        if (CheckImage("closefb"))
                        {
                            ClickImage("closefb");
                        }    
                        if (CheckImage("closedo"))
                        {
                            ClickImage("closedo");
                        }    
                        if (CheckImage("closevang"))
                        {
                            ClickImage("closevang");
                        }
                        if (CheckImage("Close"))
                        {
                            ClickImage("Close");
                        }    
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
                                if (!CheckImage("Auto_close_ads"))
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
                                if (!CheckImage("Auto_close_ads"))
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
                                if (BackgroundClicker.WaitForImageAndClick("Claim"))
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
                        else
                        {
                            if (CheckImage("Gems"))
                            {
                                ClickImage("Gems");
                                Thread.Sleep(200);
                                ClickImage("OK");
                                Thread.Sleep(1000);
                                if (!CheckImage("Auto_close_ads"))
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
                                Thread.Sleep(500);
                                ClickImage("Claim");
                                Thread.Sleep(500);
                            }
                            else
                            {
                                switch (ThuTuDao)
                                {
                                    case 1:
                                        RunAHKScript("1-2.AHK");
                                        break;
                                    case 2:
                                        break;
                                }
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

        private void radioButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng đưa vị trí tầm nhìn về đảo 1 (Đảo được tặng ban đầu)");
        }
    }
}
