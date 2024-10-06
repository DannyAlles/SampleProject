using FluentValidation;
using SampleMVVM_WPF.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM_WPF.EventBus.Validators
{
    public class CreateNewHumanEventValidator : AbstractValidator<CreateNewHumanEvent>
    {
        public CreateNewHumanEventValidator()
        {
            RuleFor(x => x.FirstName).NotNull().WithMessage("Field FirstName is required");
            RuleFor(x => x.LastName).NotNull().WithMessage("Field LastName is required");
            RuleFor(x => x.MiddleName).NotNull().WithMessage("Field MiddleName is required");
            RuleFor(x => x.DateOfBirth).NotNull().WithMessage("Field DateOfBirth is required");
        }
    }
}
