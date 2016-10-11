using System;
using Itis.ProxyDataPlatform.Interfaces;

namespace Itis.ProxyDataPlatform.Base
{
    public class EntityBll : IEntity
    {
        protected EntityBll()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// ������ ���� ���-������� ��� ������������� ����.
        /// </summary>
        /// <returns>
        /// ���-��� ��� �������� ������� <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            if (Id == Guid.Empty)
                return base.GetHashCode();

            if (DateUpdated != null)
                return Id.GetHashCode() ^ DateUpdated.GetHashCode();

            return Id.GetHashCode();
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
            if (obj == null)
                return false;

            try
            {
                var entity = (EntityBll) obj;

                return Id == entity.Id && DateUpdated == entity.DateUpdated;
            }
            catch
            {
            }

            return false;
        }
    }
}