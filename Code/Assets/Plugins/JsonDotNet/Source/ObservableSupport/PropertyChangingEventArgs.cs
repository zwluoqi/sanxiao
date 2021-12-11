#if !UNITY_WINRT || UNITY_EDITOR || UNITY_WP8
using System;

namespace Newtonsoft.Json.ObservableSupport
{
    public class PropertyChangingEventArgs : EventArgs
    {
        public PropertyChangingEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        public virtual string PropertyName { get; set; }
    }
}

#endif