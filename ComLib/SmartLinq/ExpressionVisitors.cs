using System.Linq.Expressions;

namespace ComLib.SmartLinq
{
    class ExpressionVisitors
    {
        internal class AggregateConditionsVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _s;
            public AggregateConditionsVisitor(ParameterExpression substitutor)
            {
                _s = substitutor;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _s;
            }
        }
    }
}
