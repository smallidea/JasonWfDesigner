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
// ** Desc：ApplicationServicesProvider.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using JasonWfDesigner.Common;
using JasonWfDesigner.WPF.Services.Contracts;
using JasonWfDesigner.WPF.Services.Implentation;

namespace JasonWfDesigner.WPF.Services
{
    /// <summary>
    ///     Simple service interface
    /// </summary>
    public interface IServiceProvider
    {
        IUIVisualizerService VisualizerService { get; }
        IMessageBoxService MessageBoxService { get; }
        IDatabaseAccessService DatabaseAccessService { get; }
    }

    /// <summary>
    ///     Simple service locator
    /// </summary>
    public class ServiceProvider : IServiceProvider
    {
        public IUIVisualizerService VisualizerService { get; } = new WPFUIVisualizerService();

        public IMessageBoxService MessageBoxService { get; } = new WpfMessageBoxService();

        public IDatabaseAccessService DatabaseAccessService { get; } = new JsonAccessService();
    }


    /// <summary>
    ///     Simple service locator helper
    /// </summary>
    public class ApplicationServicesProvider
    {
        private static readonly Lazy<ApplicationServicesProvider> instance =
            new Lazy<ApplicationServicesProvider>(() => new ApplicationServicesProvider());

        static ApplicationServicesProvider()
        {
        }

        private ApplicationServicesProvider()
        {
        }

        public IServiceProvider Provider { get; private set; } = new ServiceProvider();

        public static ApplicationServicesProvider Instance => instance.Value;

        public void SetNewServiceProvider(IServiceProvider provider)
        {
            Provider = provider;
        }
    }
}