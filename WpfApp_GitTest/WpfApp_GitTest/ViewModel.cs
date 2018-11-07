using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Runtime.CompilerServices;


namespace WpfApp_GitTest
{
    public class ViewModel2 : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        private string inputString;
        public string InputString
        {
            get { return inputString; }
            set
            {
                inputString = value;
                NotifyPropertyChanged();
                ValidateProperty("InputString", value);
            }
        }

        private string testString;
        public string TestString
        {
            get { return testString; }
            set
            {
                testString = value;
                NotifyPropertyChanged();
            }
        }

        protected void ValidateProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "InputString":
                    if (!File.Exists(this.InputString))
                        AddError("InputString", "存在しないファイルです");
                    else
                        RemoveError("InputString");
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region 発生中のエラーを保持する処理を実装
        readonly Dictionary<string, List<string>> _currentErrors = new Dictionary<string, List<string>>();

        protected void AddError(string propertyName, string error)
        {
            if (!_currentErrors.ContainsKey(propertyName))
                _currentErrors[propertyName] = new List<string>();

            if (!_currentErrors[propertyName].Contains(error))
            {
                _currentErrors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        public void RemoveError(string propertyName)
        {
            if (_currentErrors.ContainsKey(propertyName))
                _currentErrors.Remove(propertyName);

            OnErrorsChanged(propertyName);
        }
        #endregion

        private void OnErrorsChanged(string propertyName)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #region INotifyDataErrorInfoの実装
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) ||
                !_currentErrors.ContainsKey(propertyName))
                return null;

            return _currentErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _currentErrors.Count > 0; }
        }
        #endregion
    }
}
