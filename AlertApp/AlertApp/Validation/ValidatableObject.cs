using System;
using System.Collections.Generic;
using System.Linq;
using AlertApp.Infrastructure;

namespace AlertApp.Validation
{
    //public class ValidatableObject<T> : BaseViewModel, IValidity
    //{
    //    private readonly List<IValidationRule<T>> _validations;
    //    private List<string> _errors;
    //    private T _value;
    //    private bool _isValid;

    //    public List<IValidationRule<T>> Validations => _validations;

    //    public List<string> Errors
    //    {
    //        get => _errors;
    //        set => SetProperty(ref _errors, value, () => Errors);
    //    }

    //    public T Value
    //    {
    //        get => _value;
    //        set => SetProperty(ref _value, value, () => Value);
    //    }

    //    public bool IsValid
    //    {
    //        get => _isValid;
    //        set => SetProperty(ref _isValid, value, () => IsValid);
    //    }

    //    public ValidatableObject()
    //    {
    //        _isValid = true;
    //        _errors = new List<string>();
    //        _validations = new List<IValidationRule<T>>();
    //    }

    //    public bool Validate()
    //    {
    //        Errors.Clear();

    //        var errors = _validations.Where(v => !v.Check(Value)).Select(v => v.ValidationMessage);

    //        Errors = errors.ToList();
    //        IsValid = !Errors.Any();

    //        return IsValid;
    //    }

    //    public override void SetBusy(bool isBusy)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
