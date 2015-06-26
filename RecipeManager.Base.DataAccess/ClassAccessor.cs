using System;
using System.Linq.Expressions;
using System.Reflection;

namespace RecipeManager.Base.DataAccess {
    /// <summary>
    /// Caches method info into delegates to avoid the overhead of reflection.
    /// </summary>
    public class ClassAccessor {
        private readonly Func<object, object> getter;
        private readonly Action<object, object> setter;

        public string Name { get; set; }

        public Type PropertyType { get; set; }

        public bool IsPrimaryKey { get; set; }

        public ClassAccessor(PropertyInfo property) {
            this.Name = property.Name;
            this.PropertyType = property.PropertyType;
            this.IsPrimaryKey = false;
            this.getter = createGetter(property.GetGetMethod());
            this.setter = createSetter(property.GetSetMethod());
        }

        public object Get(object instance) {
            return this.getter(instance);
        }

        public void Set(object instance, object value) {
            this.setter(instance, value);
        }

        private Func<object, object> createGetter(MethodInfo method) {
            var instance = Expression.Parameter(typeof(object), "instance");

            Expression<Func<object, object>> expression =
                Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Call(
                            Expression.Convert(instance, method.DeclaringType),
                            method),
                        typeof(object)),
                    instance);

            return expression.Compile();
        }

        private Action<object, object> createSetter(MethodInfo method) {
            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object));

            Expression<Action<object, object>> expression =
                Expression.Lambda<Action<object, object>>(
                    Expression.Call(
                        Expression.Convert(instance, method.DeclaringType),
                        method,
                        Expression.Convert(value, method.GetParameters()[0].ParameterType)),
                    instance,
                    value);

            return expression.Compile();
        }
    }
}