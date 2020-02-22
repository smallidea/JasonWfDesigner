// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Core
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:54
// ** Desc：WeakINPCEventHandler.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.ComponentModel;
using System.Reflection;

namespace JasonWfDesigner.Core.Helpers
{
    //[DebuggerNonUserCode]
    public sealed class WeakINPCEventHandler
    {
        private readonly MethodInfo _method;
        private readonly WeakReference _targetReference;

        public WeakINPCEventHandler(PropertyChangedEventHandler callback)
        {
            _method = callback.Method;
            _targetReference = new WeakReference(callback.Target, true);
        }

        //[DebuggerNonUserCode]
        public void Handler(object sender, PropertyChangedEventArgs e)
        {
            var target = _targetReference.Target;
            if (target != null)
            {
                var callback =
                    (Action<object, PropertyChangedEventArgs>) Delegate.CreateDelegate(
                        typeof(Action<object, PropertyChangedEventArgs>), target, _method, true);
                if (callback != null) callback(sender, e);
            }
        }
    }
}