﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EduBotCore.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
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
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EduBotCore.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отправьте файл .zip.
        /// </summary>
        internal static string AddCase {
            get {
                return ResourceManager.GetString("AddCase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка при загрузке кейса.
        /// </summary>
        internal static string AddCaseFail {
            get {
                return ResourceManager.GetString("AddCaseFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Кейс успешно загружен.
        /// </summary>
        internal static string AddCaseSuccess {
            get {
                return ResourceManager.GetString("AddCaseSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отправьте строку в формате: *ID* *Имя* *Фамилия* *Группа*
        ///Например: 3256357 Василий Пупкин 5565834-10002.
        /// </summary>
        internal static string AddGroupLeader {
            get {
                return ResourceManager.GetString("AddGroupLeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отправтье файл excel. Формат названия &lt;1ч.группы&gt;-&lt;2ч.группы&gt;. Таблица имеет 3 столбца (Имя, фамилия, telegramID).
        /// </summary>
        internal static string AddNewGroupOfUsers {
            get {
                return ResourceManager.GetString("AddNewGroupOfUsers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Это меню администратора.
        /// </summary>
        internal static string AdminMenu {
            get {
                return ResourceManager.GetString("AdminMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для ответа на этот вопрос отправьте .
        /// </summary>
        internal static string AnswerGuidePt1 {
            get {
                return ResourceManager.GetString("AnswerGuidePt1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Максимальный размер файла - 20Мб.
        /// </summary>
        internal static string AnswerGuidePt2 {
            get {
                return ResourceManager.GetString("AnswerGuidePt2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выберите курс из списка.
        /// </summary>
        internal static string ChoosingCourse {
            get {
                return ResourceManager.GetString("ChoosingCourse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Это меню старосты.
        /// </summary>
        internal static string ClassLeaderMenu {
            get {
                return ResourceManager.GetString("ClassLeaderMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Добавьте файл формата электроной таблицы, которая содержит материалы курса по заданному шаблону.
        /// </summary>
        internal static string CreateCase {
            get {
                return ResourceManager.GetString("CreateCase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введите пароль.
        /// </summary>
        internal static string EnterPassword {
            get {
                return ResourceManager.GetString("EnterPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введите /start для продолжения.
        /// </summary>
        internal static string EnterStart {
            get {
                return ResourceManager.GetString("EnterStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введите новые имя и фамилию через пробел
        ///Привет: Василий Пупкин.
        /// </summary>
        internal static string EnterUserInfo {
            get {
                return ResourceManager.GetString("EnterUserInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Возникла неизвестная ошибка. Наберите /start.
        /// </summary>
        internal static string Error {
            get {
                return ResourceManager.GetString("Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Файл caseinfo.case не найден.
        /// </summary>
        internal static string FileCaseNotFound {
            get {
                return ResourceManager.GetString("FileCaseNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пароль для группы:.
        /// </summary>
        internal static string GroupPassword {
            get {
                return ResourceManager.GetString("GroupPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пользователь был сделан старостой.
        /// </summary>
        internal static string MadeGroupLeader {
            get {
                return ResourceManager.GetString("MadeGroupLeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пользователь добавлен и сделан старостой.
        /// </summary>
        internal static string MadeNewGroupLeader {
            get {
                return ResourceManager.GetString("MadeNewGroupLeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вход совершён успешно, переходим в главное меню.
        /// </summary>
        internal static string RightPassword {
            get {
                return ResourceManager.GetString("RightPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Именно видео.
        /// </summary>
        internal static string SendVideo {
            get {
                return ResourceManager.GetString("SendVideo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выберите группу для добавления на курс.
        /// </summary>
        internal static string ShowAllGroups {
            get {
                return ResourceManager.GetString("ShowAllGroups", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Группа выбрана. Теперь выберите тип ответа.
        /// </summary>
        internal static string ShowAnswersTypes {
            get {
                return ResourceManager.GetString("ShowAnswersTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выберите группу.
        /// </summary>
        internal static string ShowGroups {
            get {
                return ResourceManager.GetString("ShowGroups", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Список пользователей группы.
        /// </summary>
        internal static string ShowUsers {
            get {
                return ResourceManager.GetString("ShowUsers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Операция добавления прошла успешно.
        /// </summary>
        internal static string SuccessAddGroup {
            get {
                return ResourceManager.GetString("SuccessAddGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Данные успешно изменены.
        /// </summary>
        internal static string SuccessEditing {
            get {
                return ResourceManager.GetString("SuccessEditing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Это ваш ваш персональный телеграм айди.
        /// </summary>
        internal static string TelegramId {
            get {
                return ResourceManager.GetString("TelegramId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У вас больше нет попыток.
        /// </summary>
        internal static string ThereIsNotAttempts {
            get {
                return ResourceManager.GetString("ThereIsNotAttempts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Кейс ещё не был загружен.
        /// </summary>
        internal static string ThereIsNotCase {
            get {
                return ResourceManager.GetString("ThereIsNotCase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Можно вернуться в меню.
        /// </summary>
        internal static string ToMenu {
            get {
                return ResourceManager.GetString("ToMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Файл должен быть меньше 20МБ.
        /// </summary>
        internal static string TooBigFile {
            get {
                return ResourceManager.GetString("TooBigFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Это ваша персональная карточка.
        /// </summary>
        internal static string UserCard {
            get {
                return ResourceManager.GetString("UserCard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Это меню пользователя.
        /// </summary>
        internal static string UserMenu {
            get {
                return ResourceManager.GetString("UserMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Здравствуйте, вы добавлены в систему.
        /// </summary>
        internal static string WelcomeKnown {
            get {
                return ResourceManager.GetString("WelcomeKnown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Приветствуем, вы администратор, как неожиданно и приятно!).
        /// </summary>
        internal static string WelcomeKnownAdmin {
            get {
                return ResourceManager.GetString("WelcomeKnownAdmin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Приветствуем! Вы не зарегистрированы в системе, но можете проходить курсы в качестве гостя.
        /// </summary>
        internal static string WelcomeUnknown {
            get {
                return ResourceManager.GetString("WelcomeUnknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ты чё скинул....
        /// </summary>
        internal static string WrongArgumentMessage {
            get {
                return ResourceManager.GetString("WrongArgumentMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Неправильный формат, посмотрите ещё раз на образец.
        /// </summary>
        internal static string WrongFormat {
            get {
                return ResourceManager.GetString("WrongFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка при сохранении файла. Возможно, не тот формат.
        /// </summary>
        internal static string WrongFormatFile {
            get {
                return ResourceManager.GetString("WrongFormatFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Неверный формат группы.
        /// </summary>
        internal static string WrongFormatGroup {
            get {
                return ResourceManager.GetString("WrongFormatGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Неверный формат ID. Должен быть целым положительным числом.
        /// </summary>
        internal static string WrongFormatID {
            get {
                return ResourceManager.GetString("WrongFormatID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Неверный формат имени.
        /// </summary>
        internal static string WrongFormatName {
            get {
                return ResourceManager.GetString("WrongFormatName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Неверный формат фамилии.
        /// </summary>
        internal static string WrongFormatSurname {
            get {
                return ResourceManager.GetString("WrongFormatSurname", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пароль неверный, попробуйте ещё раз.
        /// </summary>
        internal static string WrongPassword {
            get {
                return ResourceManager.GetString("WrongPassword", resourceCulture);
            }
        }
    }
}