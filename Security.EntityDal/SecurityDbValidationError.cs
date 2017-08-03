using System.Data.Entity.Validation;

namespace Security.EntityDal
{
    internal class SecurityDbValidationError
    {
        public SecurityDbValidationError(DbValidationError validationError)
        {
            ErrorMessage = validationError.ErrorMessage;
            PropertyName = validationError.PropertyName;
        }

        public string ErrorMessage { get; set; }
        public string PropertyName { get; set; }

        public override string ToString()
        {
            return $"ErrorMessage: {ErrorMessage}; PropertyName: {PropertyName}";
        }
    }
}