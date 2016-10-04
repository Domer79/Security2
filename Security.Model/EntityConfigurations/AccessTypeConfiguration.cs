using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class AccessTypeConfiguration : BaseConfiguration<AccessType>
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="T:System.Object"/>.
        /// </summary>
        public AccessTypeConfiguration()
        {
            Property(e => e.Name).IsUnicode(false);

            HasMany(e => e.Grants).WithRequired(e => e.AccessType);
        }
    }
}