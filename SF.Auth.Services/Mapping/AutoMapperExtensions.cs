using AutoMapper;

namespace SF.Auth.Services.Mapping
{
    public static class AutoMapperExtensions
    {
        #region Public Methods

        public static IMappingExpression<TSource, TDestination> IgnoreAllUnmapped<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());

            return expression;
        }

        #endregion Public Methods
    }
}