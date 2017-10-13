using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace SF.Common.MVC
{
    public static class ValidationHelper
    {
        #region Public Methods

        public static IHtmlContent CleanValidationSummary(
            this IHtmlHelper htmlHelper,
            object htmlAttributes)
        {
            if (htmlHelper.ViewContext.ViewData.ModelState.Any(x => string.IsNullOrWhiteSpace(x.Key)))
            {
                return htmlHelper.ValidationSummary(true, string.Empty, htmlAttributes);
            }

            return null;
        }

        public static HtmlString IsValid<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string errorClass)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);

            var state = htmlHelper.ViewData.ModelState[fullHtmlFieldName];

            if (state == null || state.Errors.Count == 0)
            {
                return HtmlString.Empty;
            }

            return new HtmlString(errorClass);
        }

        #endregion Public Methods
    }
}