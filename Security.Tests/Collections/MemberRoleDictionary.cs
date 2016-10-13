using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Security.Tests.Model;

namespace Security.Tests.Collections
{
    public class MemberRoleDictionary : IEnumerable<MemberRole>
    {
        private readonly List<MemberRole> _memberRoles = new List<MemberRole>();

        public void Link(Member member, Role role)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var memberRole = new MemberRole {Member = member, Role = role};

            _memberRoles.Add(memberRole);
        }

        public List<Role> this[Member member]
        {
            get { return _memberRoles.Where(mr => mr.Member == member).Select(mr => mr.Role).ToList(); }
        }

        public List<Member> this[Role role]
        {
            get { return _memberRoles.Where(mr => mr.Role == role).Select(mr => mr.Member).ToList(); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<MemberRole> GetEnumerator()
        {
            return _memberRoles.GetEnumerator();
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
}