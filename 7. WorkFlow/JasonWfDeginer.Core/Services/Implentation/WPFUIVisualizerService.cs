﻿// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:55
// ** Desc：WPFUIVisualizerService.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Windows;
using JasonWfDesigner.WPF.Services.Contracts;

namespace JasonWfDesigner.WPF.Services.Implentation
{
    public class WPFUIVisualizerService : IUIVisualizerService
    {
        #region Public Methods

        /// <summary>
        ///     This method displays a modal dialog associated with the given key.
        /// </summary>
        /// <param name="dataContextForPopup">Object state to associate with the dialog</param>
        /// <returns>True/False if UI is displayed.</returns>
        public bool? ShowDialog(object dataContextForPopup, string title)
        {
            Window win = new PopupWindow();
            win.DataContext = dataContextForPopup;
            win.Owner = Application.Current.MainWindow;
            if (string.IsNullOrWhiteSpace(title) == false)
                win.Title = title;
            if (win != null)
                return win.ShowDialog();

            return false;
        }

        #endregion
    }
}