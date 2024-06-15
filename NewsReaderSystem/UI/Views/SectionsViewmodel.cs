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
using NewsReaderSystem.Models;

namespace NewsReaderSystem.UI.Views
{
    public class SectionsViewmodel : ObservableObject
    {
        private readonly HttpRequestService httpRequestService = new HttpRequestService();

        private readonly DispatcherTimer timer;

        private readonly ReadingViewmodel readingViewmodel;

        public ICommand ExecuteMouseRequestCommand { get; set; }

        public SectionsViewmodel(ReadingViewmodel readingViewmodel)
        {
            ExecuteMouseRequestCommand = new AsyncRelayCommand(ExecuteMouseRequest);
            this.readingViewmodel = readingViewmodel;
            timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = new TimeSpan(1);
            timer.Tick += timer_Tick;
            // timer.Start();

            this.httpRequestService.FaceMousing += HttpRequestService_FaceMousing;
            Task t1 = this.httpRequestService.ConnectWebSocketAsync();
            Task.WhenAll(t1);
        }

        private void HttpRequestService_FaceMousing(object sender, Models.FaceMousingResult e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
                mouseControl(e);
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                await ExecuteMouseRequest();
            }
        }

        private async Task ExecuteMouseRequest()
        {
            var mouseObject = await httpRequestService.Request();

            mouseControl(mouseObject);
        }


        private void mouseControl(FaceMousingResult mouseObject)
        {
            if (mouseObject != null)
            {
                Console.WriteLine(mouseObject);

                switch (mouseObject.state)
                {
                    case 2:
                        MouseService.ClickAtPosition(mouseObject.x, mouseObject.y);
                        Console.WriteLine("--------------------------------- CLICK ----------------------------------");
                        break;
                    case 3:
                    case 4:
                        readingViewmodel.Scroll("Up");
                        Console.WriteLine("--------------------------------- SCROLL MODE ----------------------------------");
                        break;

                }

                MouseService.SendMouseMove(mouseObject.x, mouseObject.y);
            }
        }
    }
}
