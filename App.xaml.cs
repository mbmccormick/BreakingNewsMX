using System;
using System.Diagnostics;
using BreakingNewsMX.API;
using BreakingNewsMX.Common;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BreakingNewsMX
{
    sealed partial class App : Application
    {
        public static ServiceClient BreakingNewsClient;

        public App()
        {
            InitializeComponent();
            this.Suspending += OnSuspending;

            BreakingNewsClient = new ServiceClient(Debugger.IsAttached);

            BreakingNewsClient.LastApplicationLaunchTime = DateTime.UtcNow;

            DebugSettings.BindingFailed += DebugSettings_BindingFailed;
        }

        void DebugSettings_BindingFailed(object sender, BindingFailedEventArgs e)
        {
            Debug.WriteLine("BINDING FAILED: " + e.Message);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    SmartDispatcher.Initialize(rootFrame.Dispatcher); 
                    
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                Window.Current.Content = rootFrame;
            }

            SmartDispatcher.Initialize(rootFrame.Dispatcher);                    

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(HubPage), e.Arguments);
            }
            
            Window.Current.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
