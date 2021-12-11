//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
    /// <summary>
    /// The native Ad.
    /// </summary>
    public class NativeAd
    {
        public void Dispose()
        {
        }
        /// <summary>
        /// Gets the title for this Ad.
        /// </summary>
        public string GetTitle()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the description for this Ad.
        /// </summary>
        public string GetDescription()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the button text.
        /// </summary>
        public string GetButtonText()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the app score.
        /// </summary>
        public int GetAppScore()
        {
            return 0;
        }

        /// <summary>
        /// Gets the app comment number.
        /// </summary>
        public int GetAppCommentNum()
        {
            return 0;
        }

        /// <summary>
        /// Gets the app size.
        /// </summary>
        public int GetAppSize()
        {
            return 0;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        public string GetSource()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the interaction type.
        /// </summary>
        public int GetInteractionType()
        {
            return 0;
        }

        /// <summary>
        /// Gets the image mode.
        /// </summary>
        public int GetImageMode()
        {
            return 0;
        }

        /// <summary>
        /// Sets the download listener.
        /// </summary>
        public void SetDownloadListener(IAppDownloadListener listener)
        {
        }

        public void SetNativeAdInteractionListener(
        IInteractionAdInteractionListener listener)
        {
        }

        public void ShowNativeAd()
        {
        }
    }
#endif
}
