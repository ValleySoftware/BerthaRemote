using BerthaRemote.ViewModels;
using BerthaRemote.Views;
using Microsoft.AppCenter.Crashes;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace BerthaRemote
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private static MainViewModel _mainViewModel;
        public static Frame rootFrame;
        public ThemeListener Listener;

        public static DispatcherQueue dispatcherQueue;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        public static MainViewModel mainViewModel
        {
            get
            {
                if (_mainViewModel == null)
                {
                    _mainViewModel = new MainViewModel();
                }

                return _mainViewModel;
            }
            set
            {
                _mainViewModel = value;
            }
        }

        private void Listener_ThemeChanged(ThemeListener sender)
        {
            try
            {
                var theme = sender.CurrentTheme;

                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

                if (theme == ApplicationTheme.Dark)
                {
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.ButtonInactiveForegroundColor = Colors.White;
                    App.Current.Resources["ThemeTextForegroundOpposite"] = Colors.Black;
                }
                else
                {
                    titleBar.ButtonForegroundColor = Colors.Black;
                    titleBar.ButtonInactiveForegroundColor = Colors.Black;
                    App.Current.Resources["ThemeTextForegroundOpposite"] = Colors.White;
                }
            }
            catch (Exception tex)
            {
                //Issue getting current theme.  Noticed in WinUI 2.4.3 and Toolkit 6.1.1
                Crashes.TrackError(tex, new Dictionary<string, string>
                {
                    { "Class", this.GetType().Name },
                    { "Method", MethodBase.GetCurrentMethod().Name },
                    { "ExceptionVar", "tex"} }
                );
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            rootFrame = Window.Current.Content as Frame;
            dispatcherQueue = Windows.ApplicationModel.Core.CoreApplication.MainView.DispatcherQueue;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.ContentTransitions = new TransitionCollection();
                rootFrame.ContentTransitions.Add(new NavigationThemeTransition());

                rootFrame.NavigationFailed += OnNavigationFailed;
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
                // Register a handler for BackRequested events and set the
                // visibility of the Back button
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            try
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

                //titleBar.ButtonBackgroundColor = Colors.Transparent;
                //titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                //titleBar.ButtonBackgroundColor = Colors.Transparent;
                //titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                try
                {
                    if (Listener == null)
                    {
                        Listener = new ThemeListener();
                        Listener.ThemeChanged += Listener_ThemeChanged;
                    }

                    if (Listener.CurrentTheme == ApplicationTheme.Dark)
                    {
                        titleBar.ButtonForegroundColor = Colors.White;
                        titleBar.ButtonInactiveForegroundColor = Colors.White;
                        App.Current.Resources["ThemeTextForegroundOpposite"] = Colors.Black;
                    }
                    else
                    {
                        titleBar.ButtonForegroundColor = Colors.Black;
                        titleBar.ButtonInactiveForegroundColor = Colors.Black;
                        App.Current.Resources["ThemeTextForegroundOpposite"] = Colors.White;
                    }
                }
                catch (Exception tex)
                {
                    //Issue getting current theme.  Noticed in WinUI 2.4.3 and Toolkit 6.1.1
                    Crashes.TrackError(tex, new Dictionary<string, string>
                    {
                        { "Class", this.GetType().Name },
                        { "Method", MethodBase.GetCurrentMethod().Name },
                        { "ExceptionVar", "tex"} }
                    );
                }
            }
            catch (Exception titleException)
            {
                //Issue getting current theme.  Noticed in WinUI 2.4.3 and Toolkit 6.1.1
                Crashes.TrackError(titleException, new Dictionary<string, string>
                    {
                        { "Class", this.GetType().Name },
                        { "Method", MethodBase.GetCurrentMethod().Name },
                        { "ExceptionVar", "titleException"} }
                );
                //Issue trying to extend into the title bar on open.
            }

            if (e.PrelaunchActivated == false)
            {
                CoreApplication.EnablePrelaunch(true);

                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainView), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            //Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
