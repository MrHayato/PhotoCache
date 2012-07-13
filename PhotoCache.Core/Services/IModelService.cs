using System;
using System.Collections.Generic;
using FluentValidation.Results;
using PhotoCache.Core.Models;
using Raven.Client.Linq;

namespace PhotoCache.Core.Services
{
    public interface IModelService<T> where T : IModel
    {
        ValidationResult Validate(T model);
        ValidationResult Validate(T model, string[] properties);

        bool Create(T model);
        bool Update(T model);
        IRavenQueryable<T> Query();
        T Load(Guid id);
        List<T> LoadAll();
        void Delete(Guid id);
    }
}