using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using Raven.Client.Linq;

namespace PhotoCache.Core.Services
{
    public class ModelService<T> : IModelService<T> where T : IModel
    {
        private IRavenRepository<T> _repository;
        private IValidator<T> _validator;

        public ModelService(IRavenRepository<T> repository, IValidator<T> validator)
        {
            _validator = validator;
            _repository = repository;
        }

        public ValidationResult Validate(T model)
        {
            return Validate(model, null);
        }

        public ValidationResult Validate(T model, string[] properties)
        {
            return _validator == null
                       ? new ValidationResult()
                       : (properties == null)
                            ? _validator.Validate(model)
                            : _validator.Validate(model, properties);
        }

        public bool Create(T model)
        {
            var validationResult = Validate(model);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                _repository.Store(model);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Update(T model)
        {
            return Create(model);
        }

        public IRavenQueryable<T> Query()
        {
            return _repository.Query();
        }

        public T Load(Guid id)
        {
            return _repository.Load(id);
        }

        public List<T> LoadAll()
        {
            return _repository.LoadAll();
        }

        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }
    }
}
