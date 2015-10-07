using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LuaInterpreter_WPF
{
    class Timeout : INotifyPropertyChanged, IDataErrorInfo
    {
        private static volatile Timeout instance;
        private static object syncRoot = new Object();

        public event PropertyChangedEventHandler PropertyChanged;

        private Timeout()
        {
        }

        public static Timeout Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Timeout();
                    }
                }

                return instance;
            }
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int _value = 500;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Will be called for each and every property when ever it's value is changed
        /// </summary>
        /// <param name="columnName">Name of the property whose value is changed</param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Name")
                {
                    if (Value <= 0 || Value >= 99)
                        result = "Please enter a valid timeout";
                }
                return result;
            }
        }

        #endregion IDataErrorInfo Members
    }
}
