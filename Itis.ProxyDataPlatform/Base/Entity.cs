using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Itis.ProxyDataPlatform.Interfaces;

namespace Itis.ProxyDataPlatform.Base
{
    public class Entity : IEntity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public bool Equals(Entity other)
        {
            return other != null && other.Id == this.Id && other.DateUpdated == DateUpdated;
        }

        public static bool Equals(Entity x, Entity y)
        {
            return x != null && y != null && x.Id == y.Id && x.DateUpdated == y.DateUpdated;
        }

        public int GetHashCode(Entity obj)
        {
            if (DateUpdated != null)
                return obj.Id.GetHashCode() ^ DateUpdated.GetHashCode();

            return obj.Id.GetHashCode();
        }

        /// <summary>
        /// ������ ���� ���-������� ��� ������������� ����.
        /// </summary>
        /// <returns>
        /// ���-��� ��� �������� ������� <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        /// <summary>
        /// ����������, ����� �� �������� ������ <see cref="T:System.Object"/> �������� ������� <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true, ���� �������� ������ ����� �������� �������; � ��������� �����堗 false.
        /// </returns>
        /// <param name="obj">������, ������� ��������� �������� � ������� ��������.</param>
        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
                return false;

            return Equals((Entity)obj);
        }
    }
}
