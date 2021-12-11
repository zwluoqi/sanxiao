//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The interaction listener for full screen video Ad.
    /// </summary>
    public interface IFullScreenVideoAdInteractionListener
    {
        /// <summary>
        /// Invoke when the Ad is shown.
        /// </summary>
        void OnAdShow();

        /// <summary>
        /// Invoke when the Ad video var is clicked.
        /// </summary>
        void OnAdVideoBarClick();

        /// <summary>
        /// Invoke when the Ad is closed.
        /// </summary>
        void OnAdClose();

        /// <summary>
        /// Invoke when the video is complete.
        /// </summary>
        void OnVideoComplete();

        /// <summary>
        /// Invoke when the video is skipped.
        /// </summary>
        void OnSkippedVideo();

        /// <summary>
        /// Invoke when the video has an error.
        /// </summary>
        void OnVideoError();
    }
}
