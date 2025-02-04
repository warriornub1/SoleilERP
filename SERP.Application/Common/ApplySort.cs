using SERP.Domain.Common.Model;
using System.Linq.Expressions;

namespace SERP.Application.Common
{
    public static class ApplySort
    {
        public static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderByFunction<TEntity>(
                    List<Sortable> sortables) where TEntity : class
        {
            Type typeQueryable = typeof(IQueryable<TEntity>);
            ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "p");
            Type entityType = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            Expression resultExp = argQueryable;
            bool first = true;

            sortables ??= new List<Sortable>();
            foreach (var sortable in sortables)
            {
                LambdaExpression lambdaExp = sortable.FieldName.ToMemberOf<TEntity>();
                string methodName;
                if (first)
                {
                    first = false;
                    methodName = sortable.IsAscending ? "OrderBy" : "OrderByDescending";
                }
                else
                {
                    methodName = sortable.IsAscending ? "ThenBy" : "ThenByDescending";
                }

                resultExp = Expression.Call(typeof(Queryable), methodName, new[] { entityType, lambdaExp.ReturnType }, resultExp,
                    Expression.Quote(lambdaExp));
            }

            // Case empty columns: simply append a .OrderBy(x => true)
            if (first)
            {
                LambdaExpression lambdaExp = Expression.Lambda(Expression.Constant(true), arg);
                resultExp = Expression.Call(typeof(Queryable), "OrderBy", new[] { entityType, typeof(bool) }, resultExp,
                    Expression.Quote(lambdaExp));
            }

            LambdaExpression orderedLambda = Expression.Lambda(resultExp, argQueryable);
            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)orderedLambda.Compile();
        }

        private static Expression<Func<T, object>> ToMemberOf<T>(this string name) where T : class
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var propertyOrField = name.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
                var unaryExpression = Expression.MakeUnary(ExpressionType.Convert, propertyOrField, typeof(object));

                return Expression.Lambda<Func<T, object>>(unaryExpression, parameter);
            }
            catch (Exception)
            {
                return x => true;
            }
        }
    }
}
