using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Security.EntityDal;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;
using Security.Model;
using System.Data.Entity;

namespace Security.EntityFramework.Collections
{
    /// <summary>
    /// Класс, представляющий собой коллекцию участников безопасности
    /// </summary>
    public class MemberCollection : BaseCollection<Member>, IMemberCollection
    {
        internal MemberCollection(SecurityContext context) : base(context)
        {
            
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IMember> GetEnumerator()
        {
            return ((IQueryable<Member>)Context.Members.AsNoTracking()).GetEnumerator();
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

        public IQueryable<IMember> WithRoles()
        {
            return this.Include("Roles");
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </returns>
        public Expression Expression => ((IQueryable<Member>) Context.Members).Expression;

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
        /// </returns>
        public Type ElementType => ((IQueryable<Member>) Context.Members).ElementType;

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.
        /// </returns>
        public IQueryProvider Provider => ((IQueryable<Member>) Context.Members).Provider;
    }
}