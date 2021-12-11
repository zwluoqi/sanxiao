package com.bytedance.android;

import android.content.Context;
import android.graphics.Color;
import android.util.AttributeSet;
import android.view.Gravity;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.TextView;


class IntersititialView extends FrameLayout {

    private final Context mContext;
    private Button mDislikeButton;
    private Button mCreateButton;
    private Button mCloseButton;
    private ImageView mImageView;
    private TextView mTitle;



    public IntersititialView(Context context) {
        super(context);
        mContext = context;
        init();
    }

    public IntersititialView(Context context, AttributeSet attrs) {
        super(context, attrs);
        mContext = context;
        init();
    }

    public IntersititialView(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        mContext = context;
        init();
    }

    private void init() {
        mImageView = new ImageView(mContext);
        mImageView.setLayoutParams(new LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT,
                ViewGroup.LayoutParams.MATCH_PARENT));
        mImageView.setScaleType(ImageView.ScaleType.FIT_XY);
        this.addView(mImageView, new LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
        initTitle();
        initCreateButton();
        initDislike();
        initClose();
    }

    private void initDislike() {
        mDislikeButton = new Button(mContext);
        mDislikeButton.setText("不喜欢");
        mDislikeButton.setTextColor(Color.BLACK);
        mDislikeButton.setTextSize(10);
        mDislikeButton.setBackgroundColor(Color.GRAY);
        mDislikeButton.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
            }
        });

        int margin = 20;
        LayoutParams lp = new LayoutParams(150, 100);

        lp.gravity = Gravity.LEFT | Gravity.BOTTOM;
        lp.topMargin = margin;
        lp.rightMargin = margin;
        this.addView(mDislikeButton, lp);
    }
    private void initClose() {
        mCloseButton = new Button(mContext);
        mCloseButton.setText("关闭");
        mCloseButton.setTextColor(Color.BLACK);
        mCloseButton.setTextSize(10);
        mCloseButton.setBackgroundColor(Color.GRAY);
        mCloseButton.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
            }
        });

        int margin = 20;
        LayoutParams lp = new LayoutParams(110, 100);

        lp.gravity = Gravity.RIGHT | Gravity.TOP;
        lp.topMargin = margin;
        lp.rightMargin = margin;
        this.addView(mCloseButton, lp);
    }
    private void initTitle() {
        mTitle = new TextView(mContext);
        mTitle.setText("title");
        mTitle.setTextColor(Color.WHITE);

        int margin = 20;
        LayoutParams lp = new LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        lp.gravity = Gravity.LEFT | Gravity.TOP;
        lp.topMargin = margin;
        lp.rightMargin = margin;
        this.addView(mTitle, lp);
    }

    private void initCreateButton() {
        mCreateButton = new Button(mContext);
        mCreateButton.setText("查看详情");
        mCreateButton.setTextColor(Color.BLACK);
        int margin = 20;
        LayoutParams lp = new LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        lp.gravity = Gravity.RIGHT | Gravity.BOTTOM;
        lp.topMargin = margin;
        lp.rightMargin = margin;
        this.addView(mCreateButton, lp);
    }

    ImageView getImageView() {
        return mImageView;
    }
    void setTitle(String title) {
        mTitle.setText(title);
    }
    Button getCreateButton() {
        return mCreateButton;
    }
    public View getDisLikeView() {
        return mDislikeButton;
    }
    public View getClose() {
        return mCloseButton;
    }

}
