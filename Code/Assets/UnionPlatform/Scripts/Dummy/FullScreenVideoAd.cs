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
    /// The full screen video Ad.
    /// </summary>
    public sealed class FullScreenVideoAd
    {
         public void Dispose()
        {
        }
        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetFullScreenVideoAdInteractionListener(
            IFullScreenVideoAdInteractionListener listener)
        {
        }

        /// <summary>
        /// Sets the listener for the Ad download.
        /// </summary>
        public void SetDownloadListener(IAppDownloadListener listener)
        {
        }

        /// <summary>
        /// Gets the interaction type.
        /// </summary>
        public int GetInteractionType()
        {
            return 0;
        }

        /// <summary>
        /// Show the full screen video.
        /// </summary>
        public void ShowFullScreenVideoAd()
        {
        }

        /// <summary>
        /// Set to show the download bar.
        /// </summary>
        public void SetShowDownLoadBar(bool show)
        {
        }
    }
#endif
}
