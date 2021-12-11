#if !UNITY_WINRT || UNITY_EDITOR || UNITY_WP8
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.ObservableSupport
{
    public interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}

#endif