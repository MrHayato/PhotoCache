using System;
using System.Collections.Generic;
using FluentValidation.Results;
using PhotoCache.Core.ReadModels;
using Raven.Client.Linq;

namespace PhotoCache.Core.Services
{
    public interface IModelService<T> where T : IModel
    {
        ValidationResult Validate(T model);
        ValidationResult Validate(T model, string[] properties);

        bool Create(T model);
        bool Update(T model);
        bool Patch(T model, ModelPatch[] patches);
        IRavenQueryable<T> Query();
        T Load(Guid id);
        List<T> LoadAll();
        void Delete(T model);
        void Delete(Guid id);
    }
}