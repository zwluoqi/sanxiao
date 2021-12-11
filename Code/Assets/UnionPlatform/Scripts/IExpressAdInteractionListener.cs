//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
	/// <summary>
	/// The listener for splash Ad.
	/// </summary>
	public interface IExpressAdInteractionListener
	{
		/// <summary>
		/// Invoke when the AdView is renderin succ.
		/// </summary>
		void OnAdViewRenderSucc(ExpressAd ad,float width,float height);

		/// <summary>
		/// Invoke when the AdView is renderin fail .
		/// <param name="code">error code.</param>
		/// <param name="message">rerror message.</param>
		/// </summary>
		void OnAdViewRenderError(ExpressAd ad, int code, string message);

		/// <summary>
		/// Invoke when the Ad is shown.
		/// </summary>
		void OnAdShow(ExpressAd ad);

		/// <summary>
		/// Invoke when the Ad is clicked.
		/// </summary>
		void OnAdClicked(ExpressAd ad);

        /// <summary>
        /// Invoke when the Ad is closed.
        /// </summary>
        void OnAdClose(ExpressAd ad);

	}
}
