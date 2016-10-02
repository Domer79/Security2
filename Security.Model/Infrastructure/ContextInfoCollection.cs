using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Security.Model.Base;
using Tools.Extensions;

namespace Security.Model.Infrastructure
{
    public class ContextInfoCollection : IEnumerable<ContextInfo>
    {
        private readonly Dictionary<Type, ContextInfo> _dictionary = new Dictionary<Type, ContextInfo>();

        public void Add(Type contextType)
        {
            if (contextType == null || !contextType.Is<RepositoryDataContext>()) 
                throw new ArgumentNullException("contextType");

            if (_dictionary.ContainsKey(contextType))
                return;

            var contextInfo = new ContextInfo(contextType);
            _dictionary.Add(contextType, contextInfo);
        }

        public ContextInfo this[Type contextType]
        {
            get
            {
                if (contextType == null || !contextType.Is<RepositoryDataContext>()) 
                    throw new ArgumentException("contextType");

                Add(contextType);

                return _dictionary[contextType];
            }
        }

        /// <summary>
        /// ���������� �������������, ����������� �������� � ���������.
        /// </summary>
        /// <returns>
        /// ��������� <see cref="T:System.Collections.Generic.IEnumerator`1"/>, ������� ����� �������������� ��� �������� ��������� ���������.
        /// </returns>
        public IEnumerator<ContextInfo> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        /// <summary>
        /// ���������� �������������, �������������� �������� � ���������.
        /// </summary>
        /// <returns>
        /// ������ <see cref="T:System.Collections.IEnumerator"/>, ������� ����� �������������� ��� �������� ���������.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}