using System;
using System.ComponentModel;

namespace LuaInterpreter_WPF
{
    class Timeout : ObservableObject, IDataErrorInfo
    {
        private static volatile Timeout instance;
        private static object syncRoot = new object();

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
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == nameof(Value))
                {
                    if (Value <= 0 || Value > 999)
                        result = "Please enter a valid timeout";
                }
                return result;
            }
        }

        #endregion IDataErrorInfo Members
    }
}
