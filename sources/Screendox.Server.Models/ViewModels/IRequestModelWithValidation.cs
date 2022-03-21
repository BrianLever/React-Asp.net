using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// This interface helps to navigate from model to Validator
    /// </summary>
    /// <typeparam name="TModelValidator"></typeparam>
    public interface IRequestModelWithValidation <TModelValidator>
        where TModelValidator: IValidator
    {
    }
}
