using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ningji.PlcSimulator.Core.ViewModels;

namespace Ningji.PlcSimulator.WPF.ViewModels
{
    public class DictionaryEntry : INPCBase
    {
        string _key;
        object _value;

        public string Key
        {
            get {return  this._key; }
            set { this._key = value; }
        }

        public object Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                NotifyChanged("Value");
            }
        }
    }


}
