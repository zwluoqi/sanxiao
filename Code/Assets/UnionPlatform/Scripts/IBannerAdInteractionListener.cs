//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The banner Ad interaction listener.
    /// </summary>
    public interface IBannerAdInteractionListener
    {
        /// <summary>
        /// Invoke when the Ad is clicked.
        /// </summary>
        void OnAdClicked(int var2);

        /// <summary>
        /// Invoke when the Ad is shown.
        /// </summary>
        void OnAdShow(int var2);
    }
}
