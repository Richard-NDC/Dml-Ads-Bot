using AutoHotkey;
using AutoHotkey.Interop;
using System;
using System.IO;

namespace WindowsFormsApp1
{
    public class AutoHotkeyExecutor
    {
        private AutoHotkeyEngine _ahkEngine;

        // Constructor
        public AutoHotkeyExecutor()
        {
            // Khởi tạo AutoHotkey engine
            _ahkEngine = new AutoHotkeyEngine();
        }

        // Đọc và thực thi script từ file AHK
        public void ExecuteScriptFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File không tồn tại!");
                return;
            }

            try
            {
                // Đọc nội dung script từ file
                string scriptContent = File.ReadAllText(filePath);

                // Thực thi script
                _ahkEngine.ExecRaw(scriptContent);
                Console.WriteLine("Script đã được thực thi thành công.");

                // Kết thúc ứng dụng sau khi script hoàn thành
                Environment.Exit(0); // Kết thúc chương trình
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi khi thực thi script: {ex.Message}");
                Environment.Exit(1); // Kết thúc với mã lỗi nếu có lỗi xảy ra
            }
        }

        // Thủ công dọn dẹp (nếu cần thiết)
        public void CleanUp()
        {
            _ahkEngine = null;  // Null đối tượng để dọn dẹp (nếu cần thiết)
        }
    }
}
