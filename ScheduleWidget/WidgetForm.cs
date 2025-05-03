using HtmlAgilityPack;
using Microsoft.Playwright;
using System;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Timer = System.Windows.Forms.Timer;

namespace ScheduleWidget
{
    public partial class WidgetForm : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public WidgetForm()
        {
            InitializeComponent();
        }

        private async Task Parsing()
        {
            try
            {
                using var playwright = await Playwright.CreateAsync();
                var browser = await playwright.Chromium.LaunchAsync();
                var page = await browser.NewPageAsync();

                await page.GotoAsync("https://erp.pkgh.ru/WebApp/#/Rasp/Group/12869");


                var element = await page.WaitForSelectorAsync("//*[@id=\"page-main\"]/div/div/div[7]/div/div/div[1]");

                if (element != null)
                {
                    var text = await element.InnerTextAsync();
                    /*Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine(Refactor(text));*/
                    lbMain.Text = Refactor(text);
                }
                else
                {
                    Console.WriteLine("Расписание нет");
                }

                await browser.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }


        }

        private string Refactor(string sourceDayText)
        {
            var stringArray = sourceDayText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var result = $"{stringArray[0]} - {stringArray[1]}\n";

            Dictionary<string, int> currentTimeTable = stringArray[0].ToLower() == "суббота" ? new Dictionary<string, int>
            {
                { "09:10", 1 },
                { "10:50", 2 },
                { "12:50", 3 },
                { "14:30", 4 }
            }
             : new Dictionary<string, int>
            {
                { "09:10", 1 },
                { "10:55", 2 },
                { "13:05", 3 },
                { "14:50", 4 },
                { "16:30", 5 }
            };


            for (int i = 2; i < stringArray.Length; i += 5)
            {
                string startTime = stringArray[i];
                string subject = stringArray[i + 2];
                string teacher = stringArray[i + 3];
                string audience = stringArray[i + 4].Replace("Аудитория: ", "").Trim();

                if (!currentTimeTable.TryGetValue(startTime, out int pairNumber))
                {
                    pairNumber = 0;
                }

                result += $"{pairNumber} пара - {subject}\n";
                result += $"{teacher} - {audience} аудитория\n\n";
            }

            return result.Trim();
        }


        private void lbMain_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        #region Transparency

        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern bool DwmIsCompositionEnabled();

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (DwmIsCompositionEnabled())
            {
                // Paint the glass effect.
                var margins = new MARGINS();
                margins.Top = 10000;
                margins.Left = 10000;
                DwmExtendFrameIntoClientArea(this.Handle, ref margins);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }

        #endregion

        #region Bottommost

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOACTIVATE = 0x0010;

        void ToBack()
        {
            SetWindowPos(Handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ToBack();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ToBack();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ToBack();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            ToBack();
        }

        #endregion

        #region Move
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                return;
            }

            base.OnMouseMove(e);
        }
        #endregion

        private async void WidgetForm_Load(object sender, EventArgs e)
        {
            await Parsing();
        }
    }
}
