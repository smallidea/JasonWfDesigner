// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2020-02-21 14:30
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 16:19
// ** Desc：Window1.xaml.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using JasonWfDesigner.WPF.ViewModels;

namespace JasonWfDesigner.WPF
{
    public partial class MainWindow : Window
    {
        private static readonly object _lock = new object();

        private readonly StringBuilder _sb = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();

            // 检查是否有多个实例运行
            checkOldProcesses();

            // 绑定数据
            var window1ViewModel = new MainWindowViewModel();
            DataContext = window1ViewModel;

            // 窗体事件
            Loaded += Window1_Loaded;
            Closing += (sender, args) => { window1ViewModel.CloseWindowCommand.Execute(null); };
        }

        /// <summary>
        ///     This shows you how you can create diagram items in code, which means you can
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            /*            // 添加节点
                        nodeDesignerItemViewModel item0 = new nodeDesignerItemViewModel
                        {
                            Key = "k1",
                            Parent = _window1ViewModel.DiagramViewModel,
                            Left = 100,
                            Top = 100
                        };
                        nodeDesignerItemViewModel item3 = new nodeDesignerItemViewModel
                        {
                            Key = "k2",
                            Parent = _window1ViewModel.DiagramViewModel,
                            Left = 200,
                            Top = 100
                        };
                        nodeDesignerItemViewModel item4 = new nodeDesignerItemViewModel
                        {
                            Key = "k3",
                            Parent = _window1ViewModel.DiagramViewModel,
                            Left = 300,
                            Top = 100
                        };
                        _window1ViewModel.DiagramViewModel.Items.Add(item0);
                        _window1ViewModel.DiagramViewModel.Items.Add(item3);
                        _window1ViewModel.DiagramViewModel.Items.Add(item4);

                        // 添加路由
                        ConnectorViewModel con1 = new ConnectorViewModel(item0.RightConnector, item3.LeftConnector);
                        con1.Parent = _window1ViewModel.DiagramViewModel;
                        _window1ViewModel.DiagramViewModel.Items.Add(con1);

                        ConnectorViewModel con2 = new ConnectorViewModel(item3.RightConnector, item4.LeftConnector);
                        con2.Parent = _window1ViewModel.DiagramViewModel;
                        _window1ViewModel.DiagramViewModel.Items.Add(con2);*/

            // var runningService = new RunningServices(_window1ViewModel.DiagramViewModel);
        }

        // 检查是否有多个实例运行
        private void checkOldProcesses()
        {
            var processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (processes.Length > 1)
            {
                var confirmToDel = MessageBox.Show("程序正在运行中! " + Environment.NewLine + "是否关闭旧程序！", "警告",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmToDel == MessageBoxResult.Yes)
                    foreach (var process in processes)
                    {
                        if (process.Id == Process.GetCurrentProcess().Id) continue;

                        MessageBox.Show(string.Join(";", processes.Select(a => a.Id).ToArray()) + "_" +
                                        Process.GetCurrentProcess().Id);
                        process.Kill();
                        //NjEventLog.Instance.WriteError($"关闭多余的 {Process.GetCurrentProcess().ProcessName}！");
                    }
                else
                    Application.Current.Shutdown(); // 关闭当前应用程序
            }
        }
    }
}