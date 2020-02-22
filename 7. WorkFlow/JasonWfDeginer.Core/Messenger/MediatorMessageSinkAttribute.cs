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
// ** Desc：MediatorMessageSinkAttribute.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;

namespace JasonWfDesigner.Core
{
    /// <summary>
    ///     This attribute allows a method to be targeted as a recipient for a message.
    ///     It requires that the Type is registered with the MessageMediator through the
    ///     <seealso cref="MessageMediator.Register" /> method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MediatorMessageSinkAttribute : Attribute
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public MediatorMessageSinkAttribute()
        {
            MessageKey = null;
        }

        /// <summary>
        ///     Constructor that takes a message key
        /// </summary>
        /// <param name="messageKey">Message Key</param>
        public MediatorMessageSinkAttribute(string messageKey)
        {
            MessageKey = messageKey;
        }

        /// <summary>
        ///     Message key
        /// </summary>
        public object MessageKey { get; }
    }
}