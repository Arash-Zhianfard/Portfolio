using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Service.Implementation
{
    public static class Utils
    {
        public static string ToErrorMessage(this ModelStateDictionary state) => string.Join(", ", state.Values.Select(x => string.Join(", ", x.Errors.Select(z => z.ErrorMessage))));
    }
}
