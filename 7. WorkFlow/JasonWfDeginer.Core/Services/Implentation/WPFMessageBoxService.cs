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
// ** Desc：WPFMessageBoxService.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Windows;
using JasonWfDesigner.WPF.Services.Contracts;

namespace JasonWfDesigner.WPF.Services.Implentation
{
    /// <summary>
    ///     This class implements the IMessageBoxService for WPF purposes.
    /// </summary>
    public class WpfMessageBoxService : IMessageBoxService
    {
        #region IMessageBoxService Members

        /// <summary>
        ///     Displays an error dialog with a given message.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public void ShowError(string message)
        {
            showMessage(message, "Error", CustomDialogIcons.Stop);
        }

        /// <summary>
        ///     Displays an error dialog with a given message and caption.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        public void ShowError(string message, string caption)
        {
            showMessage(message, caption, CustomDialogIcons.Stop);
        }

        /// <summary>
        ///     Displays an error dialog with a given message.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public void ShowInformation(string message)
        {
            showMessage(message, "Information", CustomDialogIcons.Information);
        }

        /// <summary>
        ///     Displays an error dialog with a given message.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        public void ShowInformation(string message, string caption)
        {
            showMessage(message, caption, CustomDialogIcons.Information);
        }

        /// <summary>
        ///     Displays an error dialog with a given message.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public void ShowWarning(string message)
        {
            showMessage(message, "Warning", CustomDialogIcons.Warning);
        }

        /// <summary>
        ///     Displays an error dialog with a given message.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        public void ShowWarning(string message, string caption)
        {
            showMessage(message, caption, CustomDialogIcons.Warning);
        }

        /// <summary>
        ///     Displays a Yes/No dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowYesNo(string message, CustomDialogIcons icon)
        {
            return showQuestionWithButton(message, icon, CustomDialogButtons.YesNo);
        }

        /// <summary>
        ///     Displays a Yes/No dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowYesNo(string message, string caption, CustomDialogIcons icon)
        {
            return showQuestionWithButton(message, caption, icon, CustomDialogButtons.YesNo);
        }

        /// <summary>
        ///     Displays a Yes/No dialog with a default button selected, and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <param name="defaultResult">Default result for the message box</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowYesNo(string message, string caption, CustomDialogIcons icon,
            CustomDialogResults defaultResult)
        {
            return showQuestionWithButton(message, caption, icon, CustomDialogButtons.YesNo, defaultResult);
        }

        /// <summary>
        ///     Displays a Yes/No/Cancel dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowYesNoCancel(string message, CustomDialogIcons icon)
        {
            return showQuestionWithButton(message, icon, CustomDialogButtons.YesNoCancel);
        }


        /// <summary>
        ///     Displays a Yes/No/Cancel dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowYesNoCancel(string message, string caption, CustomDialogIcons icon)
        {
            return showQuestionWithButton(message, caption, icon, CustomDialogButtons.YesNoCancel);
        }

        /// <summary>
        ///     Displays a Yes/No/Cancel dialog with a default button selected, and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <param name="defaultResult">Default result for the message box</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowYesNoCancel(string message, string caption, CustomDialogIcons icon,
            CustomDialogResults defaultResult)
        {
            return showQuestionWithButton(message, caption, icon, CustomDialogButtons.YesNoCancel, defaultResult);
        }

        /// <summary>
        ///     Displays a OK/Cancel dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowOkCancel(string message, CustomDialogIcons icon)
        {
            return showQuestionWithButton(message, icon, CustomDialogButtons.OKCancel);
        }

        /// <summary>
        ///     Displays a OK/Cancel dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowOkCancel(string message, string caption, CustomDialogIcons icon)
        {
            return showQuestionWithButton(message, caption, icon, CustomDialogButtons.OKCancel);
        }

        /// <summary>
        ///     Displays a OK/Cancel dialog with a default result selected, and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <param name="defaultResult">Default result for the message box</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowOkCancel(string message, string caption, CustomDialogIcons icon,
            CustomDialogResults defaultResult)
        {
            return showQuestionWithButton(message, caption, icon, CustomDialogButtons.OKCancel, defaultResult);
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Shows a standard System.Windows.MessageBox using the parameters requested
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The heading to be displayed</param>
        /// <param name="icon">The icon to be displayed.</param>
        private void showMessage(string message, string caption, CustomDialogIcons icon)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, getImage(icon));
        }


        /// <summary>
        ///     Shows a standard System.Windows.MessageBox using the parameters requested
        ///     but will return a translated result to enable adhere to the IMessageBoxService
        ///     implementation required.
        ///     This abstraction allows for different frameworks to use the same ViewModels but supply
        ///     alternative implementations of core service interfaces
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <param name="button"></param>
        /// <returns>CustomDialogResults results to use</returns>
        private CustomDialogResults showQuestionWithButton(string message,
            CustomDialogIcons icon, CustomDialogButtons button)
        {
            var result = MessageBox.Show(message, "Please confirm...",
                getButton(button), getImage(icon));
            return getResult(result);
        }


        /// <summary>
        ///     Shows a standard System.Windows.MessageBox using the parameters requested
        ///     but will return a translated result to enable adhere to the IMessageBoxService
        ///     implementation required.
        ///     This abstraction allows for different frameworks to use the same ViewModels but supply
        ///     alternative implementations of core service interfaces
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <param name="button"></param>
        /// <returns>CustomDialogResults results to use</returns>
        private CustomDialogResults showQuestionWithButton(string message, string caption,
            CustomDialogIcons icon, CustomDialogButtons button)
        {
            var result = MessageBox.Show(message, caption,
                getButton(button), getImage(icon));
            return getResult(result);
        }

        /// <summary>
        ///     Shows a standard System.Windows.MessageBox using the parameters requested
        ///     but will return a translated result to enable adhere to the IMessageBoxService
        ///     implementation required.
        ///     This abstraction allows for different frameworks to use the same ViewModels but supply
        ///     alternative implementations of core service interfaces
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption of the message box window</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <param name="button"></param>
        /// <param name="defaultResult">Default result for the message box</param>
        /// <returns>CustomDialogResults results to use</returns>
        private CustomDialogResults showQuestionWithButton(string message, string caption,
            CustomDialogIcons icon, CustomDialogButtons button, CustomDialogResults defaultResult)
        {
            var result = MessageBox.Show(message, caption,
                getButton(button), getImage(icon), getResult(defaultResult));
            return getResult(result);
        }


        /// <summary>
        ///     Translates a CustomDialogIcons into a standard WPF System.Windows.MessageBox MessageBoxImage.
        ///     This abstraction allows for different frameworks to use the same ViewModels but supply
        ///     alternative implementations of core service interfaces
        /// </summary>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>A standard WPF System.Windows.MessageBox MessageBoxImage</returns>
        private MessageBoxImage getImage(CustomDialogIcons icon)
        {
            var image = MessageBoxImage.None;

            switch (icon)
            {
                case CustomDialogIcons.Information:
                    image = MessageBoxImage.Information;
                    break;
                case CustomDialogIcons.Question:
                    image = MessageBoxImage.Question;
                    break;
                case CustomDialogIcons.Exclamation:
                    image = MessageBoxImage.Exclamation;
                    break;
                case CustomDialogIcons.Stop:
                    image = MessageBoxImage.Stop;
                    break;
                case CustomDialogIcons.Warning:
                    image = MessageBoxImage.Warning;
                    break;
            }

            return image;
        }


        /// <summary>
        ///     Translates a CustomDialogButtons into a standard WPF System.Windows.MessageBox MessageBoxButton.
        ///     This abstraction allows for different frameworks to use the same ViewModels but supply
        ///     alternative implementations of core service interfaces
        /// </summary>
        /// <param name="btn">The button type to be displayed.</param>
        /// <returns>A standard WPF System.Windows.MessageBox MessageBoxButton</returns>
        private MessageBoxButton getButton(CustomDialogButtons btn)
        {
            var button = MessageBoxButton.OK;

            switch (btn)
            {
                case CustomDialogButtons.OK:
                    button = MessageBoxButton.OK;
                    break;
                case CustomDialogButtons.OKCancel:
                    button = MessageBoxButton.OKCancel;
                    break;
                case CustomDialogButtons.YesNo:
                    button = MessageBoxButton.YesNo;
                    break;
                case CustomDialogButtons.YesNoCancel:
                    button = MessageBoxButton.YesNoCancel;
                    break;
            }

            return button;
        }


        /// <summary>
        ///     Translates a standard WPF System.Windows.MessageBox MessageBoxResult into a
        ///     CustomDialogIcons.
        ///     This abstraction allows for different frameworks to use the same ViewModels but supply
        ///     alternative implementations of core service interfaces
        /// </summary>
        /// <param name="result">The standard WPF System.Windows.MessageBox MessageBoxResult</param>
        /// <returns>CustomDialogResults results to use</returns>
        private CustomDialogResults getResult(MessageBoxResult result)
        {
            var customDialogResults = CustomDialogResults.None;

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    customDialogResults = CustomDialogResults.Cancel;
                    break;
                case MessageBoxResult.No:
                    customDialogResults = CustomDialogResults.No;
                    break;
                case MessageBoxResult.None:
                    customDialogResults = CustomDialogResults.None;
                    break;
                case MessageBoxResult.OK:
                    customDialogResults = CustomDialogResults.OK;
                    break;
                case MessageBoxResult.Yes:
                    customDialogResults = CustomDialogResults.Yes;
                    break;
            }

            return customDialogResults;
        }

        /// <summary>
        ///     Translates a CustomDialogResults into a standard WPF System.Windows.MessageBox MessageBoxResult
        ///     This abstraction allows for different frameworks to use the same ViewModels but supply
        ///     alternative implementations of core service interfaces
        /// </summary>
        /// <param name="result">The CustomDialogResults</param>
        /// <returns>The standard WPF System.Windows.MessageBox MessageBoxResult results to use</returns>
        private MessageBoxResult getResult(CustomDialogResults result)
        {
            var customDialogResults = MessageBoxResult.None;

            switch (result)
            {
                case CustomDialogResults.Cancel:
                    customDialogResults = MessageBoxResult.Cancel;
                    break;
                case CustomDialogResults.No:
                    customDialogResults = MessageBoxResult.No;
                    break;
                case CustomDialogResults.None:
                    customDialogResults = MessageBoxResult.None;
                    break;
                case CustomDialogResults.OK:
                    customDialogResults = MessageBoxResult.OK;
                    break;
                case CustomDialogResults.Yes:
                    customDialogResults = MessageBoxResult.Yes;
                    break;
            }

            return customDialogResults;
        }

        #endregion
    }
}