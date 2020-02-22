#region Header Text

// /******************************************************************
// ** Copyright：广州宁基智能系统有限公司 Copyright (c) 2017
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2018-06-28 11:43
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnglogs.com
// ** Version：v 1.0
// ** Last Modified: 2018-06-28 17:42
// ** Desc： DBVo.cs
// ******************************************************************/

#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.WPF.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class CommunicationVo : INPCBase
    {
        /// <summary>
        ///     通讯名称
        /// </summary>
        public string Name { get; set; }

        public ObservableCollection<DbVo> Datas { get; set; }
        public ObservableCollection<ParameterVo> Parameters { get; set; }
        /// <summary>
        /// DB块里的所有参数都有
        /// </summary>
        public ObservableCollection<DBObject> DBObjectVo { get; set; }

        public ObservableCollection<NjParameter> ParametersDatas { get; set; }



        public class NjParameter : INPCBase
        {

            public string _paramName;
            public PlcFieldType _type;
            public int _size;
            public object _value;
            public string _use;

            public string ParamName
            {
                get { return _paramName; }
                set
                {
                    if (_paramName != value)
                    {
                        _paramName = value;
                        NotifyChanged("ParamName");
                    }
                }
            }

            public string Use
            {
                get { return _use; }
                set
                {
                    if (_use != value)
                    {
                        _use = value;
                        NotifyChanged("Use");
                    }
                }
            }

            public PlcFieldType Type
            {
                get { return _type; }
                set
                {
                    if (_type != value)
                    {
                        _type = value;
                        NotifyChanged("Type");
                    }
                }
            }

            public int Size
            {
                get { return _size; }
                set
                {
                    if (_size != value)
                    {
                        _size = value;
                        NotifyChanged("Size");
                    }
                }
            }
            public object Value
            {
                get { return _value; }
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                        NotifyChanged("Value");
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public class DbVo : INPCBase
        {
            private int _dbNum;

            private uint _trigger;

            /// <summary>
            /// </summary>
            public int DbNum
            {
                get { return _dbNum; }
                set
                {
                    if (_dbNum != value)
                    {
                        _dbNum = value;
                        NotifyChanged("DbNum");
                    }
                }
            }

            /// <summary>
            /// </summary>
            public uint Trigger
            {
                get { return _trigger; }
                set
                {
                    if (_trigger != value)
                    {
                        _trigger = value;
                        NotifyChanged("Trigger");
                    }
                }
            }
        }

        public class ParameterVo : INPCBase
        {
            private int _value;

            public ParameterVo(string name, string sponsor)
            {
                Name = name;
                Sponsor = sponsor;
            }

            /// <summary>
            /// </summary>
            public int Value
            {
                get { return _value; }
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                        NotifyChanged("Value");
                    }
                }
            }

            public string Sponsor { get; set; }
            public string Name { get; set; }
        }


        public class DBObject : INPCBase
        {

            /// <summary>
            /// db注释
            /// </summary>
            public string _description;

            /// <summary>
            /// db块长度
            /// </summary>
            public int _size;

            public int _db;

            public string _group;

            private byte[] _data = null;

            public string Description
            {
                get { return _description; }
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
                get { return _size; }
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
                get { return _db; }
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
                get { return _group; }
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
                get
                {
                    return _data ?? (_data = new byte[Size]);
                }
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