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
    /// The feed Ad.
    /// </summary>
    public class FeedAd : NativeAd
    {
        /// <summary>
        /// Set the video Ad listener.
        /// </summary>
        public void SetVideoAdListener(IVideoAdListener listener)
        {
        }
    }
#endif
}
