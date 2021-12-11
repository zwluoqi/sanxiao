//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The listener for splash Ad.
    /// </summary>
    public interface ISplashAdInteractionListener
    {
        /// <summary>
        /// Invoke when the Ad is clicked.
        /// </summary>
        void OnAdClicked(int var2);

        /// <summary>
        /// Invoke when the Ad is shown.
        /// </summary>
        void OnAdShow(int var2);

        /// <summary>
        /// Invoke when the Ad is skipped.
        /// </summary>
        void OnAdSkip();

        /// <summary>
        /// Invoke when the Ad time over.
        /// </summary>
        void OnAdTimeOver();
    }
}
