using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewsReaderSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Threading;


namespace NewsReaderSystem.UI.Views
{
    public class SectionsViewmodel : ObservableObject
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);


        private readonly HttpRequestService httpRequestService = new HttpRequestService();

        private readonly DispatcherTimer timer;

        public ICommand ExecuteMouseRequestCommand { get; set; }

        public SectionsViewmodel()
        {
            ExecuteMouseRequestCommand = new AsyncRelayCommand(ExecuteMouseRequest);

            timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = new TimeSpan(10);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                var obj = await httpRequestService.Request();
                
                if (obj != null)
                {
                    Console.WriteLine(obj);
                    SetCursorPos(obj.x, obj.y);
                }
            }
        }

        private async Task ExecuteMouseRequest()
        {
            var mouseObject = await httpRequestService.Request();

            if (mouseObject != null)
            {
                Console.WriteLine(mouseObject);

                SetCursorPos(mouseObject.x, mouseObject.y);
            }
        }
    }
}
