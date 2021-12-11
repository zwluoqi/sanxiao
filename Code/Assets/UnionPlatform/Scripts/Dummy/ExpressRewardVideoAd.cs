namespace ByteDance.Union
{
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
    using System;
    using UnityEngine;

    /// <summary>
    /// The reward video Ad.
    /// </summary>
    public sealed class ExpressRewardVideoAd : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetRewardAdInteractionListener(
            IRewardAdInteractionListener listener)
        {
        }

        /// <summary>
        /// Sets the download listener.
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
        /// Show the reward video Ad.
        /// </summary>
        public void ShowRewardVideoAd()
        {
        }

        /// <summary>
        /// Sets whether to show the download bar.
        /// </summary>
        public void SetShowDownLoadBar(bool show)
        {
        }
    }
#endif
}
