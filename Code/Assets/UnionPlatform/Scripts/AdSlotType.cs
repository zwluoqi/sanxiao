//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The type of AdSlot.
    /// </summary>
    public enum AdSlotType
    {
        /// <summary>
        /// The banner Ad type.
        /// </summary>
        Banner,

        /// <summary>
        /// The interaction Ad type.
        /// </summary>
        InteractionAd,

        /// <summary>
        /// The splash Ad type.
        /// </summary>
        Splash,

        /// <summary>
        /// The cached splash Ad type.
        /// </summary>
        CachedSplash,

        /// <summary>
        /// The feed Ad type.
        /// </summary>
        Feed,

        /// <summary>
        /// The reward video Ad type.
        /// </summary>
        RewardVideo,

        /// <summary>
        /// The full screen video Ad type.
        /// </summary>
        FullScreenVideo,

        /// <summary>
        /// The draw feed Ad type.
        /// </summary>
        DrawFeed,
    }
}
