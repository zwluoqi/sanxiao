//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if !UNITY_EDITOR && UNITY_IOS
    /// <summary>
    /// The slot of a advertisement for iOS.
    /// </summary>
    public sealed class AdSlot
    {
        /// <summary>
        /// Gets or sets the code ID.
        /// </summary>
        internal string CodeId { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        internal string UserId { get; set; }

        internal int adCount;
        internal int width;
        internal int height;
        internal bool supportDeepLink;
        internal AdSlotType type;
        internal int viewwidth;
        internal int viewheight;

        /// <summary>
        /// The builder used to build an Ad slot.
        /// </summary>
        public class Builder
        {
            private AdSlot slot = new AdSlot();

            /// <summary>
            /// Sets the code ID.
            /// </summary>
            public Builder SetCodeId(string codeId)
            {
                this.slot.CodeId = codeId;
                return this;
            }

            /// <summary>
            /// Sets the image accepted size.
            /// </summary>
            public Builder SetImageAcceptedSize(int width, int height)
            {
                this.slot.width = width;
                this.slot.height = height;
                return this;
            }

            /// <summary>
            /// Sets the size of the express view accepted in dp
            /// </summary>
            public Builder SetExpressViewAcceptedSize(float width, float height)
            {
                this.slot.viewwidth = (int)width;
                this.slot.viewheight = (int)height;
                return this;
            }

            /// <summary>
            /// Sets a value indicating wheteher the Ad support deep link.
            /// </summary>
            public Builder SetSupportDeepLink(bool support)
            {
                this.slot.supportDeepLink = support;
                return this;
            }

            /// <summary>
            /// Sets the Ad count.
            /// </summary>
            public Builder SetAdCount(int count)
            {
                this.slot.adCount = count;
                return this;
            }

            /// <summary>
            /// Sets the Native Ad type.
            /// </summary>
            public Builder SetNativeAdType(AdSlotType type)
            {
                this.slot.type = type;
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
                this.slot.UserId = id;
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
                return this.slot;
            }
        }
    }
#endif
}
