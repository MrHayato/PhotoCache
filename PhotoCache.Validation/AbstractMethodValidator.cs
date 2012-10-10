using FluentValidation;

namespace PhotoCache.Validation
{
    public class AbstractMethodValidator<T> : AbstractValidator<T>, IMethodValidator<T>
    {
        public ValidationMethod Method { get; set; }
    }
}
