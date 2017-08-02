using System;
using System.Collections;
using System.Collections.Generic;
using Security.EntityDal.Base;
using Tools.Extensions;

namespace Security.EntityDal.Infrastructure
{
    /// <summary>
    /// Класс, представляющий собой коллекцию классов информаторов контекстов БД
    /// </summary>
    public class ContextInfoCollection : IEnumerable<ContextInfo>
    {
        private readonly Dictionary<Type, ContextInfo> _dictionary = new Dictionary<Type, ContextInfo>();

        /// <summary>
        /// По типу контекста создает новый класс информатор и добавляет его в коллекцию, если такой тип контекста уже зарегистрирован ничего не делает
        /// </summary>
        /// <param name="contextType">Тип контекста</param>
        /// <exception cref="ArgumentNullException">Если <see cref="contextType"/> равен null или, если один из базовых типов 
        /// не является типом <see cref="RepositoryDataContext"/></exception>
        public void Add(Type contextType)
        {
            if (contextType == null || !contextType.Is<RepositoryDataContext>()) 
                throw new ArgumentNullException("contextType");

            if (_dictionary.ContainsKey(contextType))
                return;

            var contextInfo = new ContextInfo(contextType);
            _dictionary.Add(contextType, contextInfo);
        }

        /// <summary>
        /// Возвращает информатор по типу контекста
        /// </summary>
        /// <param name="contextType">Тип контекста</param>
        /// <returns>Информатор контекста</returns>
        public ContextInfo this[Type contextType]
        {
            get
            {
                Add(contextType);

                return _dictionary[contextType];
            }
        }

        /// <summary>
        /// Возвращает перечислитель, выполняющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Интерфейс <see cref="T:System.Collections.Generic.IEnumerator`1"/>, который может использоваться для перебора элементов коллекции.
        /// </returns>
        public IEnumerator<ContextInfo> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        /// <summary>
        /// Возвращает перечислитель, осуществляющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Объект <see cref="T:System.Collections.IEnumerator"/>, который может использоваться для перебора коллекции.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}