using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Text;

namespace Security.EntityDal
{
    internal class SecurityEntityValidationResult
    {
        public SecurityEntityValidationResult(DbEntityValidationResult validationResult)
        {
            Entry = new SecurityEntityEntry(validationResult.Entry);

            foreach (var validationError in validationResult.ValidationErrors)
            {
                ValidationErrors.Add(new SecurityDbValidationError(validationError));
            }

            IsValid = validationResult.IsValid;
        }

        public SecurityEntityEntry Entry { get; set; }

        public ICollection<SecurityDbValidationError> ValidationErrors { get; set; } = new List<SecurityDbValidationError>();

        public bool IsValid { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Entry: {Entry}");
            sb.AppendLine($"IsValid: {IsValid}");
            sb.AppendLine("Validation Errors: ");
            foreach (var validationError in ValidationErrors)
            {
                sb.AppendLine($"    {validationError}");
            }

            return sb.ToString();
        }
    }
}