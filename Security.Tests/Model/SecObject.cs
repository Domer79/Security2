using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Tests.Model
{
    public class SecObject : ISecObject
    {
        public int IdSecObject { get; set; }

        public string ObjectName { get; set; }

        public HashSet<Grant> Grants { get; set; }

        IList<IGrant> ISecObject.Grants
        {
            get
            {
                return new List<IGrant>(Grants);
            }
        }

        /// <summary>
        /// ���������� ������, ������� ������������ ������� ������.
        /// </summary>
        /// <returns>
        /// ������, �������������� ������� ������.
        /// </returns>
        public override sealed string ToString()
        {
            return ObjectName;
        }
    }
}
