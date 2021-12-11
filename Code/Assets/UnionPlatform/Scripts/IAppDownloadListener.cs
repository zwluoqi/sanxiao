//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The listener for download in application.
    /// </summary>
    public interface IAppDownloadListener
    {
        /// <summary>
        /// Invoke when idle.
        /// </summary>
        void OnIdle();

        /// <summary>
        /// Invoke when the download process actived.
        /// </summary>
        void OnDownloadActive(
            long totalBytes, long currBytes, string fileName, string appName);

        /// <summary>
        /// Invoke when the download process paused.
        /// </summary>
        void OnDownloadPaused(
            long totalBytes, long currBytes, string fileName, string appName);

        /// <summary>
        /// Invoke when the download process failed.
        /// </summary>
        void OnDownloadFailed(
            long totalBytes, long currBytes, string fileName, string appName);

        /// <summary>
        /// Invoke when the download process finished.
        /// </summary>
        void OnDownloadFinished(
            long totalBytes, string fileName, string appName);

        /// <summary>
        /// Invoke when installed.
        /// </summary>
        void OnInstalled(string fileName, string appName);
    }
}
