﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Res {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class About {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal About() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PhotoCache.Web.Resources.About", typeof(About).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step 1.
        /// </summary>
        public static string LabelStep1 {
            get {
                return ResourceManager.GetString("LabelStep1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step 2.
        /// </summary>
        public static string LabelStep2 {
            get {
                return ResourceManager.GetString("LabelStep2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step 3.
        /// </summary>
        public static string LabelStep3 {
            get {
                return ResourceManager.GetString("LabelStep3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This will be a long paragraph about PhotoCache..
        /// </summary>
        public static string ParaAbout {
            get {
                return ResourceManager.GetString("ParaAbout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Download the PhotoCache app for your mobile device..
        /// </summary>
        public static string ParaStep1 {
            get {
                return ResourceManager.GetString("ParaStep1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Take a picture of the scenery using the PhotoCache mobile application..
        /// </summary>
        public static string ParaStep2 {
            get {
                return ResourceManager.GetString("ParaStep2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Post the picture to PhotoCache and have other players find the same spot. You get points based on the number of people that takes the same picture!.
        /// </summary>
        public static string ParaStep3 {
            get {
                return ResourceManager.GetString("ParaStep3", resourceCulture);
            }
        }
    }
}