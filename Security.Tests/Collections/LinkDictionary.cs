using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using Security.Tests.Model;

namespace Security.Tests.Collections
{
    public class LinkDictionary<T1, T2> : IEnumerable<Link<T1, T2>> 
        where T1 : class 
        where T2 : class
    {
        private readonly List<Link<T1, T2>> _links = new List<Link<T1, T2>>();

        public void Link(T1 link1, T2 link2)
        {
            if (link1 == null)
                throw new ArgumentNullException(nameof(link1));

            if (link2 == null)
                throw new ArgumentNullException(nameof(link2));

            var link = new Link<T1, T2>() { Link1 = link1, Link2 = link2 };

            _links.Add(link);
        }

        public void DeleteLink(T1 link1, T2 link2)
        {
            if (link1 == null)
                throw new ArgumentNullException(nameof(link1));

            if (link2 == null)
                throw new ArgumentNullException(nameof(link2));

            var link = _links.FirstOrDefault(l => l.Link1 == link1 && l.Link2 == link2);

            _links.Remove(link);
        }

        public List<T1> this[T2 link2]
        {
            get { return _links.Where(link => link.Link2 == link2).Select(link => link.Link1).ToList(); }
        }

        public List<T2> this[T1 link1]
        {
            get { return _links.Where(link => link.Link1 == link1).Select(link => link.Link2).ToList(); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Link<T1, T2>> GetEnumerator()
        {
            return _links.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Link<T1, T2>
    {
        public T1 Link1 { get; set; }
        public T2 Link2 { get; set; }
    }
}