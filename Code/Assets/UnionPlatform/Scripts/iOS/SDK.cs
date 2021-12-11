//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if !UNITY_EDITOR && UNITY_IOS
    using System;

    /// <summary>
    /// The union platform SDK.
    /// </summary>
    public static class SDK
    {
        /// <summary>
        /// Gets the version of this SDK.
        /// </summary>
        public static string Version
        {
            get { return "1.0.0"; }
        }

        /// <summary>
        /// Create the advertisement native object.
        /// </summary>
        public static AdNative CreateAdNative()
        {
            return new AdNative();
        }

        /// <summary>
        /// Request permission if necessary on some device, for example ask
        /// for READ_PHONE_STATE.
        /// </summary>
        public static void RequestPermissionIfNecessary()
        {
        }

        /// <summary>
        /// Try to show install dialog when exit the app.
        /// </summary>
        /// <returns>True means show dialog.</returns>
        public static bool TryShowInstallDialogWhenExit(Action onExitInstall)
        {
            return false;
        }
    }
#endif
}
