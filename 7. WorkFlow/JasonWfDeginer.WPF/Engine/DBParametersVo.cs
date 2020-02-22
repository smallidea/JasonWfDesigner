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
// ** Desc：DBParametersVo.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.WPF.Engine
{
    public class DBParametersVo : INPCBase
    {
        public ObservableCollection<DBObject> Datas { get; set; }

        /// <summary>
        /// </summary>
        public class DBObject : INPCBase
        {
            private byte[] _data;

            public int _db;

            /// <summary>
            ///     db注释
            /// </summary>
            public string _description;

            public string _group;

            /// <summary>
            ///     db块长度
            /// </summary>
            public int _size;

            public string Description
            {
                get => _description;
                set
                {
                    if (_description != value)
                    {
                        _description = value;
                        NotifyChanged("Description");
                    }
                }
            }

            public int Size
            {
                get => _size;
                set
                {
                    if (_size != value)
                    {
                        _size = value;
                        NotifyChanged("Size");
                    }
                }
            }

            public int DB
            {
                get => _db;
                set
                {
                    if (_db != value)
                    {
                        _db = value;
                        NotifyChanged("DB");
                    }
                }
            }

            public string Group
            {
                get => _group;
                set
                {
                    if (_group != value)
                    {
                        _group = value;
                        NotifyChanged("Group");
                    }
                }
            }

            public byte[] RawData
            {
                get => _data ?? (_data = new byte[Size]);
                set
                {
                    if (_data != value)
                    {
                        _data = value;
                        _size = _data.Length;
                        NotifyChanged("RawData");
                    }
                }
            }

        }
    }
}