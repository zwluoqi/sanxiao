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
    /// The Ad dislike object.
    /// </summary>
    public sealed class AdDislike
    {
        /// <summary>
        /// Show the dislike dialog.
        /// </summary>
        public void ShowDislikeDialog()
        {
        }

        /// <summary>
        /// Set the dislike interaction listener.
        /// </summary>
        public void SetDislikeInteractionCallback(
            IDislikeInteractionListener listener)
        {
            listener.OnCancel();
        }
    }
#endif
}
