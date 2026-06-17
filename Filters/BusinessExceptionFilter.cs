using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SchoolManagement.Web.Services;

namespace SchoolManagement.Web.Filters
{
    /// <summary>
    /// จับ InvalidOperationException จาก Service แล้วจัดการแสดงผลให้ user
    /// แทน try/catch ซ้ำใน Controller
    /// </summary>
    public class BusinessExceptionFilter : IAsyncExceptionFilter
    {
        private readonly MajorService _majorService;

        public BusinessExceptionFilter(MajorService majorService)
        {
            _majorService = majorService;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is not InvalidOperationException ex)
                return;

            var actionName = context.RouteData.Values["action"]?.ToString();
            var controllerName = context.RouteData.Values["controller"]?.ToString();

            if (IsDeleteAction(actionName))
            {
                var tempData = context.HttpContext.RequestServices
                    .GetRequiredService<ITempDataDictionaryFactory>()
                    .GetTempData(context.HttpContext);
                tempData["Error"] = ex.Message;

                context.Result = new RedirectToActionResult("Index", controllerName, null);
                context.ExceptionHandled = true;
                return;
            }

            context.ModelState.AddModelError(string.Empty, ex.Message);

            var model = TryGetModelFromForm(context);
            var metadataProvider = context.HttpContext.RequestServices
                .GetRequiredService<IModelMetadataProvider>();
            var viewData = new ViewDataDictionary(metadataProvider, context.ModelState)
            {
                Model = model
            };

            await LoadDropdownDataAsync(controllerName, actionName, viewData);

            context.Result = new ViewResult
            {
                ViewName = actionName,
                ViewData = viewData
            };
            context.ExceptionHandled = true;
        }

        private static bool IsDeleteAction(string? actionName)
        {
            return string.Equals(actionName, "Delete", StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionName, "DeleteConfirmed", StringComparison.OrdinalIgnoreCase);
        }

        private async Task LoadDropdownDataAsync(string? controllerName, string? actionName, ViewDataDictionary viewData)
        {
            if (actionName is not ("Create" or "Edit"))
                return;

            if (string.Equals(controllerName, "Teacher", StringComparison.OrdinalIgnoreCase))
                viewData["Majors"] = await _majorService.GetAllMajors();
            else if (string.Equals(controllerName, "Major", StringComparison.OrdinalIgnoreCase))
                viewData["Faculties"] = await _majorService.GetAllFaculties();
        }

        private static object? TryGetModelFromForm(ExceptionContext context)
        {
            if (context.ActionDescriptor is not ControllerActionDescriptor descriptor)
                return null;

            if (!context.HttpContext.Request.HasFormContentType)
                return null;

            var modelParam = descriptor.Parameters
                .FirstOrDefault(p => p.ParameterType.IsClass && p.ParameterType != typeof(string));

            if (modelParam == null)
                return null;

            var modelType = modelParam.ParameterType;
            var instance = Activator.CreateInstance(modelType);
            if (instance == null)
                return null;

            var form = context.HttpContext.Request.Form;
            foreach (var prop in modelType.GetProperties())
            {
                if (!prop.CanWrite || !form.TryGetValue(prop.Name, out var value))
                    continue;

                try
                {
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    var converted = Convert.ChangeType(value.ToString(), targetType);
                    prop.SetValue(instance, converted);
                }
                catch
                {
                    // ข้าม property ที่ bind ไม่ได้
                }
            }

            return instance;
        }
    }
}
