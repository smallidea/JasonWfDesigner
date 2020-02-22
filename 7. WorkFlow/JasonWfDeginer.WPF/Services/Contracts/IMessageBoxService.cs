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
// ** Desc：IMessageBoxService.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

namespace JasonWfDesigner.WPF.Services.Contracts
{
    /// <summary>
    ///     Available Button options.
    ///     Abstracted to allow some level of UI Agnosticness
    /// </summary>
    public enum CustomDialogButtons
    {
        OK,
        OKCancel,
        YesNo,
        YesNoCancel
    }

    /// <summary>
    ///     Available Icon options.
    ///     Abstracted to allow some level of UI Agnosticness
    /// </summary>
    public enum CustomDialogIcons
    {
        None,
        Information,
        Question,
        Exclamation,
        Stop,
        Warning
    }

    /// <summary>
    ///     Available DialogResults options.
    ///     Abstracted to allow some level of UI Agnosticness
    /// </summary>
    public enum CustomDialogResults
    {
        None,
        OK,
        Cancel,
        Yes,
        No
    }

    /// <summary>
    ///     This interface defines a interface that will allow
    ///     a ViewModel to show a messagebox
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        ///     Shows an error message
        /// </summary>
        /// <param name="message">The error message</param>
        void ShowError(string message);

        /// <summary>
        ///     Shows an error message with a custom caption
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="caption">The caption</param>
        void ShowError(string message, string caption);

        /// <summary>
        ///     Shows an information message
        /// </summary>
        /// <param name="message">The information message</param>
        void ShowInformation(string message);

        /// <summary>
        ///     Shows an information message with a custom caption
        /// </summary>
        /// <param name="message">The information message</param>
        /// <param name="caption">The caption</param>
        void ShowInformation(string message, string caption);

        /// <summary>
        ///     Shows an warning message
        /// </summary>
        /// <param name="message">The warning message</param>
        void ShowWarning(string message);

        /// <summary>
        ///     Shows an warning message with a custom caption
        /// </summary>
        /// <param name="message">The warning message</param>
        /// <param name="caption">The caption</param>
        void ShowWarning(string message, string caption);

        /// <summary>
        ///     Displays a Yes/No dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        CustomDialogResults ShowYesNo(string message, CustomDialogIcons icon);

        /// <summary>
        ///     Displays a Yes/No dialog with a custom caption, and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        CustomDialogResults ShowYesNo(string message, string caption, CustomDialogIcons icon);

        /// <summary>
        ///     Displays a Yes/No/Cancel dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        CustomDialogResults ShowYesNoCancel(string message, CustomDialogIcons icon);

        /// <summary>
        ///     Displays a Yes/No dialog with a default button selected, and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <param name="defaultResult">Default result for the message box</param>
        /// <returns>User selection.</returns>
        CustomDialogResults ShowYesNo(string message, string caption, CustomDialogIcons icon,
            CustomDialogResults defaultResult);

        /// <summary>
        ///     Displays a Yes/No/Cancel dialog with a custom caption and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        CustomDialogResults ShowYesNoCancel(string message, string caption, CustomDialogIcons icon);

        /// <summary>
        ///     Displays a Yes/No/Cancel dialog with a default button selected, and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// ///
        /// <param name="defaultResult">Default result for the message box</param>
        /// <returns>User selection.</returns>
        CustomDialogResults ShowYesNoCancel(string message, string caption, CustomDialogIcons icon,
            CustomDialogResults defaultResult);

        /// <summary>
        ///     Displays a OK/Cancel dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        CustomDialogResults ShowOkCancel(string message, CustomDialogIcons icon);

        /// <summary>
        ///     Displays a OK/Cancel dialog with a custom caption and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        CustomDialogResults ShowOkCancel(string message, string caption, CustomDialogIcons icon);

        /// <summary>
        ///     Displays a OK/Cancel dialog with a default button selected, and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <param name="defaultResult">Default result for the message box</param>
        /// <returns>User selection.</returns>
        CustomDialogResults ShowOkCancel(string message, string caption, CustomDialogIcons icon,
            CustomDialogResults defaultResult);
    }
}