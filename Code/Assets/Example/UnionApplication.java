//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

package com.bytedance.android;

import android.app.Application;
import com.bytedance.sdk.openadsdk.TTAdConfig;
import com.bytedance.sdk.openadsdk.TTAdConstant;
import com.bytedance.sdk.openadsdk.TTAdSdk;
import com.bytedance.sdk.openadsdk.TTCustomController;
import com.bytedance.sdk.openadsdk.TTLocation;

public final class UnionApplication extends Application {
    @Override
    public void onCreate() {
        super.onCreate();

        TTAdConfig config = new TTAdConfig.Builder()
            .appId("5063809")
            .useTextureView(false) //使用TextureView控件播放视频,默认为SurfaceView,当有SurfaceView冲突的场景，可以使用TextureView
            .appName("呆萌消消看")
            .titleBarTheme(TTAdConstant.TITLE_BAR_THEME_DARK)
            .allowShowNotify(true) //是否允许sdk展示通知栏提示
            .allowShowPageWhenScreenLock(true) //是否在锁屏场景支持展示广告落地页
            .debug(false) //测试阶段打开，可以通过日志排查问题，上线时去除该调用
            .directDownloadNetworkType(TTAdConstant.NETWORK_STATE_WIFI, TTAdConstant.NETWORK_STATE_3G) //允许直接下载的网络状态集合
            .supportMultiProcess(false) //是否支持多进程，true支持
            .customController(getController())//控制隐私数据
            .build();

        // 强烈建议在应用对应的Application#onCreate()方法中调用，避免出现content为null的异常
        TTAdSdk.init(this, config);

        //如果明确某个进程不会使用到广告SDK，可以只针对特定进程初始化广告SDK的content
        //if (PROCESS_NAME_XXXX.equals(processName)) {
        //   TTAdSdk.init(this, config);
        //}
    }

     private static TTCustomController getController() {
        MyTTCustomController customController = new MyTTCustomController();
        return customController;
    }

    private static class MyTTCustomController extends TTCustomController{
        @Override
        public boolean isCanUseLocation() {
            return super.isCanUseLocation();
        }

        @Override
        public TTLocation getTTLocation() {
            return super.getTTLocation();
        }

        @Override
        public boolean alist() {
            return super.alist();
        }

        @Override
        public boolean isCanUsePhoneState() {
            return super.isCanUsePhoneState();
        }

        @Override
        public String getDevImei() {
            return super.getDevImei();
        }

        @Override
        public boolean isCanUseWifiState() {
            return super.isCanUseWifiState();
        }

        @Override
        public boolean isCanUseWriteExternal() {
            return super.isCanUseWriteExternal();
        }
    }
}
