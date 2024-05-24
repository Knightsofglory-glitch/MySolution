using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Daystar.Util.Data
{

    public static class Validator
    {
        public static bool IsValid(object obj, out List<ValidationResult> validationResultList)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            validationResultList = new List<ValidationResult>();
            return(System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, validationContext, validationResultList, true));
        }
    }
}
