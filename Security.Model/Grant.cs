using Security.Interfaces.Model;

namespace Security.Model
{
    /// <summary>
    /// Объект разрешения
    /// </summary>
    public class Grant : IGrant
    {
        /// <summary>
        /// Идентификатор объекта безопасности
        /// </summary>
        public int IdSecObject { get; set; }

        public int IdRole { get; set; }

        public int IdAccessType { get; set; }

        /// <summary>
        /// Объект безопасности
        /// </summary>
        public SecObject SecObject { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Тип доступа
        /// </summary>
        public AccessType AccessType { get; set; }

        /// <summary>
        /// Объект безопасности
        /// </summary>
        ISecObject IGrant.SecObject
        {
            get { return SecObject; }
            set { SecObject = (SecObject)value; }
        }

        /// <summary>
        /// Роль
        /// </summary>
        IRole IGrant.Role
        {
            get { return Role; }
            set { Role = (Role)value; }
        }

        /// <summary>
        /// Тип доступа
        /// </summary>
        IAccessType IGrant.AccessType
        {
            get { return AccessType; }
            set { AccessType = (AccessType)value; }
        }
    }
}
