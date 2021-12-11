//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The listener for video Ad.
    /// </summary>
    public interface IVideoAdListener
    {
        /// <summary>
        /// Invoke when the video loaded.
        /// </summary>
        void OnVideoLoad(FeedAd feedAd);

        /// <summary>
        /// Invoke when the video error.
        /// </summary>
        void OnVideoError(int var1, int var2);

        /// <summary>
        /// Invoke when the video Ad start to play.
        /// </summary>
        void OnVideoAdStartPlay(FeedAd feedAd);

        /// <summary>
        /// Invoke when the video Ad paused.
        /// </summary>
        void OnVideoAdPaused(FeedAd feedAd);

        /// <summary>
        /// Invoke when the video continue to play.
        /// </summary>
        void OnVideoAdContinuePlay(FeedAd feedAd);
    }
}
