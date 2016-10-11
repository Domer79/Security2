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
        /// Играет роль хэш-функции для определенного типа.
        /// </summary>
        /// <returns>
        /// Хэш-код для текущего объекта <see cref="T:System.Object"/>.
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
        /// Определяет, равен ли заданный объект <see cref="T:System.Object"/> текущему объекту <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true, если заданный объект равен текущему объекту; в противном случае — false.
        /// </returns>
        /// <param name="obj">Объект, который требуется сравнить с текущим объектом.</param>
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