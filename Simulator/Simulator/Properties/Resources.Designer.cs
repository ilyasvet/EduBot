﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Simulator.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Simulator.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Ищет локализованную строку, похожую на Для регистрации Вам необходимо ввести свои имя и фамилию (не менее 2х символов, только кириллица).
        /// </summary>
        internal static string RegistrationGuide {
            get {
                return ResourceManager.GetString("RegistrationGuide", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите имя.
        /// </summary>
        internal static string RegistrationName {
            get {
                return ResourceManager.GetString("RegistrationName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Отлично, теперь фамилия.
        /// </summary>
        internal static string RegistrationNameSuccess {
            get {
                return ResourceManager.GetString("RegistrationNameSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите фамилию.
        /// </summary>
        internal static string RegistrationSurname {
            get {
                return ResourceManager.GetString("RegistrationSurname", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Заявка отправлена администратору.
        /// </summary>
        internal static string RegistrationSurnameSuccess {
            get {
                return ResourceManager.GetString("RegistrationSurnameSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Снова приветствуем. Ваша заявка ещё в рассмотрении....
        /// </summary>
        internal static string WeclomeUnregistered {
            get {
                return ResourceManager.GetString("WeclomeUnregistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на С возвращением! Давайте скорей продолжим :).
        /// </summary>
        internal static string WelcomeKnown {
            get {
                return ResourceManager.GetString("WelcomeKnown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Приветствуем! Мы ещё не знакомы..
        /// </summary>
        internal static string WelcomeUnknown {
            get {
                return ResourceManager.GetString("WelcomeUnknown", resourceCulture);
            }
        }
    }
}