using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using PhotoCache.Core.Exceptions;
using PhotoCache.Core.Persistence;
using PhotoCache.Core.ReadModels;
using PhotoCache.Validation;
using Raven.Abstractions.Data;
using Raven.Client.Linq;
using Raven.Json.Linq;

namespace PhotoCache.Core.Services
{
    public class ModelService<T> : IModelService<T> where T : IModel
    {
        private IRavenRepository<T> _repository;
        private IMethodValidator<T> _validator;

        public ModelService(IRavenRepository<T> repository, IMethodValidator<T> validator)
        {
            _repository = repository;
            _validator = validator;
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

        private bool ValidateAndStore(T model)
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

        public bool Create(T model)
        {
            _validator.Method = ValidationMethod.Create;
            return ValidateAndStore(model);
        }

        public bool Update(T model)
        {
            _validator.Method = ValidationMethod.Update;
            return ValidateAndStore(model);
        }

        public bool Patch(T model, ModelPatch[] patches)
        {
            var typeProperties = typeof(T).GetProperties();
            var invalidRequests = new List<ModelPatch>();
            var validRequests = new List<PatchRequest>();

            //Validate patch properties
            foreach (var patch in patches)
            {
                var valid = typeProperties.Any(p => p.Name == patch.Property);

                if (!valid)
                    invalidRequests.Add(patch);
                else
                    validRequests.Add(new PatchRequest
                                          {
                                              Name = patch.Property,
                                              Value = new RavenJValue(patch.Value),
                                              Type = PatchCommandType.Set
                                          });
            }

            if (invalidRequests.Count > 0)
                throw new InvalidPatchRequestException { Patches = invalidRequests.ToArray() };

            try
            {
                _repository.Patch(model.Id, validRequests.ToArray());
            }
            catch
            {
                return false;
            }
            return true;
        }

        public IRavenQueryable<T> Query() { return _repository.Query(); }
        public T Load(Guid id) { return _repository.Load(id); }
        public List<T> LoadAll() { return _repository.LoadAll(); }
        public void Delete(T model) { _repository.Delete(model); }
        public void Delete(Guid id) { _repository.Delete(id); }
    }
}
