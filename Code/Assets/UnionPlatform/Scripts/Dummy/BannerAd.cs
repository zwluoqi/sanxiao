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
    /// The banner Ad.
    /// </summary>
    public sealed class BannerAd
    {
        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetBannerInteractionListener(
            IBannerAdInteractionListener listener)
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
        /// Sets the show dislike icon.
        /// </summary>
        public void SetShowDislikeIcon(IDislikeInteractionListener listener)
        {
        }

        /// <summary>
        /// Gets the dislike dislog.
        /// </summary>
        public AdDislike GetDislikeDialog(IDislikeInteractionListener listener)
        {
            var dislike = new AdDislike();
            dislike.SetDislikeInteractionCallback(listener);
            return dislike;
        }

        /// <summary>
        /// Sets the slide interval time.
        /// </summary>
        public void SetSlideIntervalTime(int interval)
        {
        }
    }
#endif
}
