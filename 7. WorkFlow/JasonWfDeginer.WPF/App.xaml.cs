// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2020-02-21 14:30
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:55
// ** Desc：App.xaml.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Windows;
using System.Windows.Threading;

namespace JasonWfDesigner.WPF
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void Current_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                //NjEventLog.Instance.WriteError(e.Exception);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                //NjEventLog.Instance.WriteError(ex);
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                // if (e.ExceptionObject is Exception exception) NjEventLog.Instance.WriteError(exception);
            }
            catch (Exception ex)
            {
                // NjEventLog.Instance.WriteError(ex);
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
            }
        }
    }
}