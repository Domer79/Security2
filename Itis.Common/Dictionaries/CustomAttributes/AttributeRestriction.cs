using System;

namespace Itis.Common.Dictionaries.CustomAttributes
{
    /// <summary>
    /// Кастомный атрибут на члены перечисления <see cref="T:Itis.Common.Dictionaries.EDictionaryAttributeValueType"/>
    /// </summary>
    /// <remarks>Может использоваться для ограничения значений атрибутов как системных справочников так и пользовательских. 
    /// Если этот кастомный атрибут не указан для атрибута справочника, то такой атрибут будет виден для выбора на фронтенде в Карточке атрибута</remarks>
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class AttributeValueTypeDetails : Attribute
    {
        public string ValidationFailMessage { get; set; }

        /// <summary>
        /// Подсказка на фронтенде для правильного ввода, это может быть маска или просто текст
        /// </summary>
        /// <remarks>например </remarks>
        public string Hint { get; set; }

        /// <summary>
        /// Регулярное выражение для ограничения вводимых значений атрибута в Карточке элемента справочника. 
        /// </summary>
        /// <remarks>На фронтенде будет как ng-message="pattern"</remarks>
        public string Pattern { get; set; }

        /// <summary>
        /// Максимальная длина строки для ограничения вводимых значений атрибута в Карточке элемента справочника
        /// </summary>
        /// <remarks>На фронтенде будет как ng-message="md-maxlength"</remarks>
        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        /// <summary>
        /// Минимальная длина строки для ограничения вводимых значений атрибута в Карточке элемента справочника
        /// </summary>
        /// <remarks>На фронтенде будет как ng-message="md-minlength"</remarks>
        public int MinLength
        {
            get { return _minLength; }
            set { _minLength = value; }
        }

        private int _minLength = -1;
        private int _maxLength = -1;

        /// <summary>
        /// если да, тип не попадет в выпадающий список типов в Карточке атрибута для выбора пользователем. 
        /// Так например нужно когда есть кастомный тип для какого-то атрибута СИСТЕМНОГО справочника и этот тип никчему показывать на веб-морде
        /// </summary>
        public bool IsPrivate { get; set; }
        

        public AttributeValueTypeDetails(bool isPrivate, int maxLength = -1, int minLength = -1,
            string hint = null, string pattern = null, string validationFailMessage = null)
        {
            _maxLength = maxLength;
            _minLength = minLength;
            Hint = hint;
            IsPrivate = isPrivate;
            Pattern = pattern;
            ValidationFailMessage = validationFailMessage;
        }
    }
}