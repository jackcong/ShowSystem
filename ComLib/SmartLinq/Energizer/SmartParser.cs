using System;
using System.Data.Entity;
using System.Linq.Expressions;
using ComLib.Converter;

namespace ComLib.SmartLinq.Energizer
{
    public static partial class SmartLinq
    {
        private static class SmartParser
        {

            // TODO: Unit test this method.
            public static Expression ParsePropertyChain<T>(string param)
            {
                string[] chains = param.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                Expression exp = Expression.Parameter(typeof (DbContext), "c");
                foreach(string c in chains)
                {
                    exp = Expression.PropertyOrField(exp, c.Trim());
                }
                return exp;
            }

            public static object ParseOperand<T>(string type, string operand)
            {
                switch(type.ToLower())
                {
                    case "int":
                    case "int32":
                        return operand.ToInt32();
                    case "double":
                        return operand.ToDouble();
                    case "string":
                        return operand;
                    case "propchain":
                        return ParsePropertyChain<T>(operand);
                    default:
                        throw new System.Exception("Data type not supported.");
                }
            }
        }
    }
}
