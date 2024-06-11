using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace CodeGen.DynamicLinq
{
    /// <summary>
    /// Represents a aggregate expression of Kendo DataSource.
    /// </summary>
    [DataContract(Name = "aggregate")]
    public class Aggregator
    {
        /// <summary>
        /// Gets or sets the name of the aggregated field (property).
        /// </summary>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the aggregate.
        /// </summary>
        [DataMember(Name = "aggregate")]
        public string Aggregate { get; set; }

        /// <summary>
        /// Get MethodInfo.
        /// </summary>
        /// <param name="type">Specifies the type of querable data.</param>
        /// <returns>A MethodInfo for field.</returns>
        public MethodInfo MethodInfo(Type type)
        {
            var proptype = type.GetProperty(Field).PropertyType;
            switch (Aggregate)
            {
                case "max":
                case "min":
                    return GetMethod(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Aggregate), MinMaxFunc().Method, 2).MakeGenericMethod(type, proptype);
                case "average":
                case "sum":
                    return GetMethod(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Aggregate),
                        ((Func<Type, Type[]>)this.GetType()
                                                 .GetMethod("SumAvgFunc", BindingFlags.Static | BindingFlags.NonPublic)
                                                 .MakeGenericMethod(proptype)
                                                 .Invoke(null, null)).Method, 1).MakeGenericMethod(type);
                case "count":
                    return GetMethod(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Aggregate),
                        Nullable.GetUnderlyingType(proptype) != null ? CountNullableFunc().Method : CountFunc().Method, 1).MakeGenericMethod(type);
            }
            return null;
        }

        private static MethodInfo GetMethod(string methodName, MethodInfo methodTypes, int genericArgumentsCount)
        {
            var methods = from method in typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                          let parameters = method.GetParameters()
                          let genericArguments = method.GetGenericArguments()
                          where method.Name == methodName &&
                                genericArguments.Length == genericArgumentsCount &&
                                parameters.Select(p => p.ParameterType).SequenceEqual((Type[])methodTypes.Invoke(null, genericArguments))
                          select method;
            return methods.FirstOrDefault();
        }

        private static Func<Type, Type[]> CountNullableFunc()
        {
            return (T) => new[]
            {
                typeof(IQueryable<>).MakeGenericType(T),
                typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(T, typeof(bool)))
            };
        }

        private static Func<Type, Type[]> CountFunc()
        {
            return (T) => new[]
            {
                typeof(IQueryable<>).MakeGenericType(T)
            };
        }

        private static Func<Type, Type, Type[]> MinMaxFunc()
        {
            return (T, U) => new[]
            {
                typeof (IQueryable<>).MakeGenericType(T),
                typeof (Expression<>).MakeGenericType(typeof (Func<,>).MakeGenericType(T, U))
            };
        }

        private static Func<Type, Type[]> SumAvgFunc<U>()
        {
            return (T) => new[]
            {
                typeof (IQueryable<>).MakeGenericType(T),
                typeof (Expression<>).MakeGenericType(typeof (Func<,>).MakeGenericType(T, typeof(U)))
            };
        }
    }
}

#region Notes

//Kendo.DynamicLinq - https://github.com/kendo-labs/dlinq-helpers

#endregion

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
* Copyright © 2015 Matthew David Elgert mdelgert@vessea.com, All Rights Reserved. 
*
* NOTICE: All information contained herein is, and remains the property of Matthew Elgert. The intellectual and technical concepts contained
* herein are proprietary to Matthew Elgert and may be covered by U.S. and Foreign Patents, patents in process, and are protected by trade secret or copyright law.
* Dissemination of this information or reproduction of this material is strictly forbidden unless prior written permission is obtained
* from Matthew Elgert. Access to the source code contained herein is hereby forbidden to anyone except Matthew Elgert or contractors who have executed 
* Confidentiality and Non-disclosure agreements explicitly covering such access.
*
* The copyright notice above does not evidence any actual or intended publication or disclosure of this source code, which includes
* information that is confidential and/or proprietary, and is a trade secret, of Matthew Elgert. ANY REPRODUCTION, MODIFICATION, DISTRIBUTION, PUBLIC PERFORMANCE, 
* OR PUBLIC DISPLAY OF OR THROUGH USE OF THIS SOURCE CODE WITHOUT THE EXPRESS WRITTEN CONSENT OF Matthew Elgert IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE 
* LAWS AND INTERNATIONAL TREATIES. THE RECEIPT OR POSSESSION OFTHIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT ITMAY DESCRIBE, IN WHOLE OR IN PART.
*
* Company: VESSEA, LLC.
*
* Author: Matthew David Elgert
*
* Project: CodeGen.DynamicLinq
*
* Authored date: 1/10/2015
* 
* Modified date: 1/10/2015
* 
* Notes: 
* 
* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */
