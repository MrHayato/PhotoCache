﻿using FluentValidation;
using FluentValidation.Results;
using Machine.Specifications;
using PhotoCache.Core.ReadModels;
using PhotoCache.Core.Specs.Persistence;
using PhotoCache.Core.Validators;
using PhotoCache.Validation;
using Raven.Client.Embedded;

// ReSharper disable UnusedMember.Local
namespace PhotoCache.Core.Specs.ValidatorSpecs
{
    public class UserModelValidatorSpecs
    {
        protected static FakeRavenRepository<UserModel> Repository = new FakeRavenRepository<UserModel>(new EmbeddableDocumentStore { RunInMemory = true }.Initialize());
        protected static IMethodValidator<UserModel> Validator = new UserModelValidator(Repository);
        protected static UserModel NewUser = new UserModel { UserName = "testuser", StoredUserName = "testuser", Password = "testuser", FirstName = "test", LastName = "user" };
        protected static ValidationResult Result;

        private Establish that = () =>
        {
            Repository.Store(new UserModel { UserName = "testuser", StoredUserName = "testuser", Password = "testuser", Email = "SomeUser@SomeDomain.com", FirstName = "test", LastName = "user" });
            Repository.Store(new UserModel { UserName = "testuser2", StoredUserName = "testuser2", Password = "testuser2", Email = "SomeUser2@SomeDomain.com", FirstName = "test", LastName = "user2" });
        };
    }

    class when_creating_a_user_that_already_exists : UserModelValidatorSpecs
    {
        protected static UserModel NewUser2;

        private Establish that = () =>
        {
            NewUser2 = new UserModel { UserName = "testuser" };
            Validator.Method = ValidationMethod.Create;
        };

        private Because of = () => { Result = Validator.Validate(NewUser2, "UserName"); };

        private It should_have_validation_errors = () => Result.Errors.Count.ShouldBeGreaterThan(0);
        private It should_give_an_error_for_the_username_property = () => Result.Errors[0].PropertyName.ShouldEqual("UserName");
        private It should_return_user_exists_error = () => Result.Errors[0].ErrorMessage.ShouldEqual(Res.UserModel.UsernameExists);
    }

    class when_creating_a_password_with_less_than_6_characters : UserModelValidatorSpecs
    {
        protected static UserModel NewUser2;
        
        private Establish that = () =>
        {
            NewUser2 = new UserModel { Password = "a" };
        };

        private Because of = () => { Result = Validator.Validate(NewUser2, "Password"); };

        private It should_have_validation_errors = () => Result.Errors.Count.ShouldBeGreaterThan(0);
        private It should_give_an_error_for_the_password_property = () => Result.Errors[0].PropertyName.ShouldEqual("Password");
        private It should_give_a_password_length_error = () => Result.Errors[0].ErrorMessage.ShouldContain("'Password' must be between");
    }
    
    class when_creating_a_username_with_less_than_4_characters : UserModelValidatorSpecs
    {
        protected static UserModel NewUser2;

        private Establish that = () =>
        {
            NewUser2 = new UserModel { UserName = "abc" };
        };

        private Because of = () => { Result = Validator.Validate(NewUser2, "UserName"); };

        private It should_have_validation_errors = () => Result.Errors.Count.ShouldBeGreaterThan(0);
        private It should_give_an_error_for_the_username_property = () => Result.Errors[0].PropertyName.ShouldEqual("UserName");
        private It should_give_a_username_length_error = () => Result.Errors[0].ErrorMessage.ShouldContain("'User Name' must be between");
    }
}
