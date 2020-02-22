// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Common
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:55
// ** Desc：NodeCommunicationBase.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

namespace JasonWfDesigner.Common
{
    /// <summary>
    ///     通讯交互
    /// </summary>
    public abstract class NodeCommunicationBase
    {
        /// <summary>
        ///     通讯类型枚举
        /// </summary>
        public enum CommunicationTypeEnum
        {
            /// <summary>
            ///     轮询
            /// </summary>
            Loop = 1,

            /// <summary>
            ///     仅请求
            /// </summary>
            OnlyRequest = 2,

            /// <summary>
            ///     请求及回馈
            /// </summary>
            RequestAndResponse = 4
        }

        /// <summary>
        ///     通讯的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public int? PlcDbNum { get; set; }

        /// <summary>
        ///     通讯类型
        /// </summary>
        public CommunicationTypeEnum Type { get; set; }

        /// <summary>
        /// </summary>
        public int? MesDbNum { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="product"></param>
        /// <param name="triggernodeKey"></param>
        /// <returns></returns>
        public abstract NodeCommunicationResult GetTargetNode(ProductBase product, string triggernodeKey);

        /// <summary>
        ///     获取滚筒速度（毫秒）
        /// </summary>
        /// <returns></returns>
        public abstract int GetRollSpeed(object product);

        protected NodeCommunicationResult AllowGo(string targetNode = null)
        {
            return new NodeCommunicationResult {IsAllowGo = true, TargetNode = targetNode};
        }

        protected NodeCommunicationResult NotAllowGo()
        {
            return new NodeCommunicationResult {IsAllowGo = false, TargetNode = null};
        }
    }
}