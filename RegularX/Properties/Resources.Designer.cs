﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegularX.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RegularX.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &lt;div class=(.*?)&lt;a href=&apos;(.*?)&apos;(.*?).
        /// </summary>
        internal static string getComplectation {
            get {
                return ResourceManager.GetString("getComplectation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &lt;div class=&apos;modelCode&apos;&gt;(.*?)href=&apos;(.*?)&apos;\s(.*?)&gt;(.*?)&lt;/a&gt;.
        /// </summary>
        internal static string getComplIdLnk {
            get {
                return ResourceManager.GetString("getComplIdLnk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &lt;div(.*?)&gt;(.*?)&lt;/div&gt;.
        /// </summary>
        internal static string getComplParam {
            get {
                return ResourceManager.GetString("getComplParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на .
        /// </summary>
        internal static string getDetailParam {
            get {
                return ResourceManager.GetString("getDetailParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на .
        /// </summary>
        internal static string getDetailsTable {
            get {
                return ResourceManager.GetString("getDetailsTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &lt;table(.*?)&gt;&lt;tbody&gt;(.*?)&lt;/tbody&gt;&lt;/table&gt;.
        /// </summary>
        internal static string getDetailTable {
            get {
                return ResourceManager.GetString("getDetailTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &lt;div class=&apos;List&apos;&gt;&lt;div class=&apos;(.*?)&apos;&gt;&lt;a href=&apos;(.*?)&apos;\s(.*?)&gt;(.*?)&lt;/a&gt;&lt;/div&gt;&lt;/div&gt;.
        /// </summary>
        internal static string getGroups {
            get {
                return ResourceManager.GetString("getGroups", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &lt;div class=&apos;name&apos;&gt;(.*?)&lt;/div&gt;(.*?)&lt;div class=&apos;id&apos;&gt;&lt;a href=(.*?) title=&apos;&apos; target=&apos;&apos;&gt;(.*?)&lt;/a&gt;&lt;/div&gt;(.*?)&lt;div class=&apos;dateRange&apos;&gt;(.*?)&lt;/div&gt;(.*?)&lt;div class=&apos;modelCode&apos;&gt;(.*?)&lt;/div&gt;.
        /// </summary>
        internal static string getParams {
            get {
                return ResourceManager.GetString("getParams", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &lt;div class=&apos;List&apos;&gt;&lt;div class=&apos;image&apos;&gt;&lt;a href=&apos;(.*?)&apos;\s(.*?)&gt;&lt;img src=&apos;(.*?)&apos;&gt;(.*?)&lt;div class=&apos;name&apos;&gt;&lt;a href=&apos;(.*?)&apos;\s(.*?)&gt;(.*?)&lt;/a&gt;&lt;/div&gt;&lt;/div&gt;.
        /// </summary>
        internal static string getSubGroups {
            get {
                return ResourceManager.GetString("getSubGroups", resourceCulture);
            }
        }
    }
}
