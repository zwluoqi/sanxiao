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
    /// Set the interaction Ad.
    /// </summary>
    public sealed class InteractionAd
    {
        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetAdInteractionListener(
            IInteractionAdInteractionListener listener)
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
        /// Show this interaction Ad.
        /// </summary>
        public void ShowInteractionAd()
        {
        }
    }
#endif
}
