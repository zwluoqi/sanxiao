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
    /// The slot of a advertisement.
    /// </summary>
    public sealed class AdSlot
    {
        /// <summary>
        /// The builder used to build an Ad slot.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Sets the code ID.
            /// </summary>
            public Builder SetCodeId(string codeId)
            {
                return this;
            }

            /// <summary>
            /// Sets the image accepted size.
            /// </summary>
            public Builder SetImageAcceptedSize(int width, int height)
            {
                return this;
            }

            /// <summary>
            /// Sets the size of the express view accepted in dp.
            /// </summary>
            /// <returns>The Builder.</returns>
            /// <param name="width">Width.</param>
            /// <param name="height">Height.</param>
            public Builder SetExpressViewAcceptedSize(float width, float height)
            {
                return this;
            }

            /// <summary>
            /// Sets a value indicating wheteher the Ad support deep link.
            /// </summary>
            public Builder SetSupportDeepLink(bool support)
            {
                return this;
            }

            /// <summary>
            /// Sets the Ad count.
            /// </summary>
            public Builder SetAdCount(int count)
            {
                return this;
            }

            /// <summary>
            /// Sets the Native Ad type.
            /// </summary>
            public Builder SetNativeAdType(AdSlotType type)
            {
                return this;
            }

            /// <summary>
            /// Sets the reward name.
            /// </summary>
            public Builder SetRewardName(string name)
            {
                return this;
            }

            /// <summary>
            /// Sets the reward amount.
            /// </summary>
            public Builder SetRewardAmount(int amount)
            {
                return this;
            }

            /// <summary>
            /// Sets the user ID.
            /// </summary>
            public Builder SetUserID(string id)
            {
                return this;
            }

            /// <summary>
            /// Sets the Ad orientation.
            /// </summary>
            public Builder SetOrientation(AdOrientation orientation)
            {
                return this;
            }

            /// <summary>
            /// Sets the extra media for Ad.
            /// </summary>
            public Builder SetMediaExtra(string extra)
            {
                return this;
            }

            /// <summary>
            /// Build the Ad slot.
            /// </summary>
            public AdSlot Build()
            {
                return new AdSlot();
            }
        }
    }
#endif
}
