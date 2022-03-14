﻿using LostArkMap.Widgets;
using Microsoft.Gaming.XboxGameBar;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LostArkMap
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private XboxGameBarWidget youtubeWidget = null;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (args.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(LostArkMapWidget), args.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            XboxGameBarWidgetActivatedEventArgs widgetArgs = null;
            if (args.Kind == ActivationKind.Protocol)
            {
                var protocolArgs = args as IProtocolActivatedEventArgs;
                string scheme = protocolArgs.Uri.Scheme;
                if (scheme.Equals("ms-gamebarwidget"))
                {
                    widgetArgs = args as XboxGameBarWidgetActivatedEventArgs;
                }
            }

            if (widgetArgs == null)
                return;

            //
            // Activation Notes:
            //
            //    If IsLaunchActivation is true, this is Game Bar launching a new instance
            // of our widget. This means we have a NEW CoreWindow with corresponding UI
            // dispatcher, and we MUST create and hold onto a new XboxGameBarWidget.
            //
            // Otherwise this is a subsequent activation coming from Game Bar. We MUST
            // continue to hold the XboxGameBarWidget created during initial activation
            // and ignore this repeat activation, or just observe the URI command here and act 
            // accordingly.  It is ok to perform a navigate on the root frame to switch 
            // views/pages if needed.  Game Bar lets us control the URI for sending widget to
            // widget commands or receiving a command from another non-widget process. 
            //
            // Important Cleanup Notes:
            //    When our widget is closed--by Game Bar or us calling XboxGameBarWidget.Close()-,
            // the CoreWindow will get a closed event.  We can register for Window.Closed
            // event to know when our particular widget has shutdown, and cleanup accordingly.
            //
            // NOTE: If a widget's CoreWindow is the LAST CoreWindow being closed for the process
            // then we won't get the Window.Closed event.  However, we will get the OnSuspending
            // call and can use that for cleanup.
            if (!widgetArgs.IsLaunchActivation)
                return;

            var rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
            Window.Current.Content = rootFrame;

            if (widgetArgs.AppExtensionId == "lostarkmapwidget")
            {
                youtubeWidget = new XboxGameBarWidget(
                    widgetArgs,
                    Window.Current.CoreWindow,
                    rootFrame);
                rootFrame.Navigate(typeof(LostArkMapWidget), youtubeWidget);

                Window.Current.Closed += LostArkMapWidget_Closed; ;
            }

            Window.Current.Activate();

        }

        private void LostArkMapWidget_Closed(object sender, Windows.UI.Core.CoreWindowEventArgs e)
        {
            youtubeWidget = null;
            Window.Current.Closed -= LostArkMapWidget_Closed;
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