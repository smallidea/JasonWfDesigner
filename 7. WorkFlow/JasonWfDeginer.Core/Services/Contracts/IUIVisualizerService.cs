// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:55
// ** Desc：IUIVisualizerService.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

namespace JasonWfDesigner.WPF.Services.Contracts
{
    /// <summary>
    ///     This interface defines a UI contnode which can be used to display dialogs
    ///     in either modal form from a ViewModel.
    /// </summary>
    public interface IUIVisualizerService
    {
        /// <summary>
        ///     This method displays a modal dialog associated with the given key.
        /// </summary>
        /// <param name="dataContextForPopup">Object state to associate with the dialog</param>
        /// <param name="title"></param>
        /// <returns>True/False if UI is displayed.</returns>
        bool? ShowDialog(object dataContextForPopup, string title = null);
    }
}