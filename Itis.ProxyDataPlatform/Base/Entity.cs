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
        /// Играет роль хэш-функции для определенного типа.
        /// </summary>
        /// <returns>
        /// Хэш-код для текущего объекта <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        /// <summary>
        /// Определяет, равен ли заданный объект <see cref="T:System.Object"/> текущему объекту <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true, если заданный объект равен текущему объекту; в противном случае — false.
        /// </returns>
        /// <param name="obj">Объект, который требуется сравнить с текущим объектом.</param>
        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
                return false;

            return Equals((Entity)obj);
        }
    }
}
