//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    #if !UNITY_ANDROID
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.iOS.Xcode;

    /// <summary>
    /// The post processor for xcode.
    /// </summary>
    internal static class XCodePostProcess
    {
        [PostProcessBuild(700)]
        public static void OnPostProcessBuild(
            BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }
    
            var projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var proj = new PBXProject();
            proj.ReadFromFile(projPath);
            var targetGUID = proj.TargetGuidByName("Unity-iPhone");
            proj.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC");
            proj.AddFrameworkToProject(targetGUID, "libresolv.9.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "libc++.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "libz.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "CoreLocation.framework", false);
            proj.AddFrameworkToProject(targetGUID, "AVKit.framework", false);
            proj.AddFrameworkToProject(targetGUID, "ImageIO.framework", false);
            proj.AddFrameworkToProject(targetGUID, "MediaPlayer.framework", false);
            proj.AddFrameworkToProject(targetGUID, "StoreKit.framework", false);
            proj.AddFrameworkToProject(targetGUID, "SystemConfiguration.framework", false);
            proj.AddFrameworkToProject(targetGUID, "AdSupport.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CoreMotion.framework", false);
            proj.AddFrameworkToProject(targetGUID, "Photos.framework", false);
            proj.AddFrameworkToProject(targetGUID, "WebKit.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CoreTelephony.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CoreServices.framework", false);
            proj.AddFrameworkToProject(targetGUID, "Security.framework", false);
            proj.AddFrameworkToProject(targetGUID, "Accelerate.framework", false);
            proj.AddFrameworkToProject(targetGUID, "libsqlite3.tbd", false);
            proj.WriteToFile(projPath); 
        }
    }
    #endif
}
