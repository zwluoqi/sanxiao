//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The listener for reward Ad.
    /// </summary>
    public interface IRewardVideoAdListener
    {
        /// <summary>
        /// Invoke when load Ad error.
        /// </summary>
        void OnError(int code, string message);

        /// <summary>
        /// Invoke when the Ad load success.
        /// </summary>
        void OnRewardVideoAdLoad(RewardVideoAd ad);

        /// <summary>
        /// The Ad loaded locally, user can play local video directly.
        /// </summary>
        void OnRewardVideoCached();

        /// <summary>
        /// Invoke when the Ad load success.
        /// </summary>
        void OnExpressRewardVideoAdLoad(ExpressRewardVideoAd ad);
    }
}
