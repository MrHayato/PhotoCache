using FluentValidation;

namespace PhotoCache.Validation
{
    public interface IMethodValidator<in T> : IValidator<T>
    {
        ValidationMethod Method { get; set; }
    }
}
