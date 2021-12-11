//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The listener for reward Ad interaction.
    /// </summary>
    public interface IRewardAdInteractionListener
    {
        /// <summary>
        /// Invoke when the Ad is shown.
        /// </summary>
        void OnAdShow();

        /// <summary>
        /// Invoke when the Ad video bar is clicked.
        /// </summary>
        void OnAdVideoBarClick();

        /// <summary>
        /// Invoke when the Ad is closed.
        /// </summary>
        void OnAdClose();

        /// <summary>
        /// Invoke when the video complete.
        /// </summary>
        void OnVideoComplete();

        /// <summary>
        /// Invoke when the video has an error.
        /// </summary>
        void OnVideoError();

        /// <summary>
        /// Invoke when the reward is verified.
        /// </summary>
        void OnRewardVerify(
            bool rewardVerify, int rewardAmount, string rewardName);
    }
}
