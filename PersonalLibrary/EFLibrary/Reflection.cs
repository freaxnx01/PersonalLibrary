using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Reflection;

namespace EFLibrary
{
    public enum PropertyKind
    {
        Simple, Lookup, Collection
    }

    public class Reflection
    {
        // Example: public DbSet<Category> Categories { get; set; }
        // Returns: typeof(Category)
        public static List<Type> GetDbSetPropertyGenericArgumentTypes(DbContext dbContext)
        {
            List<Type> typeList = new List<Type>();
            GetDbSetProperties(dbContext).ForEach(p => typeList.Add(p.PropertyType.GetGenericArguments()[0]));
            return typeList;
        }

        // Example: public DbSet<Category> Categories { get; set; }
        // Returns: typeof(DbSet<Category>)
        public static List<Type> GetDbSetPropertyTypes(DbContext dbContext)
        {
            List<Type> typeList = new List<Type>();
            GetDbSetProperties(dbContext).ForEach(p => typeList.Add(p.PropertyType));
            return typeList;
        }

        public static List<Type> GetDbSetPropertyTypesGenericArgument(DbContext dbContext)
        {
            List<Type> typeList = new List<Type>();
            GetDbSetPropertyTypes(dbContext).ForEach(t => typeList.Add(t.GetGenericArguments()[0]));
            return typeList;
        }

        // Example: public DbSet<Category> Categories { get; set; }
        // Returns: PropertyInfo DbSet<Category>
        public static List<PropertyInfo> GetDbSetProperties(DbContext dbContext)
        {
            return (from p in dbContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where p.PropertyType.IsGenericType
                    select p).ToList().Where(p => p.PropertyType.GetGenericTypeDefinition() == (typeof(DbSet<>))).ToList();
        }

        public static Assembly GetModelAssembly(DbContext dbContext)
        {
            return GetDbSetPropertyGenericArgumentTypes(dbContext)[0].Assembly;
        }

        public static bool IsBaseType(PropertyInfo propertyInfo)
        {
            return IsBaseType(propertyInfo.PropertyType);
        }

        public static bool IsBaseType(Type type)
        {
            return type.IsValueType || type == typeof(System.String) || type == typeof(DateTime);
        }

        public static bool IsCollection(PropertyInfo propertyInfo)
        {
            return IsCollection(propertyInfo.PropertyType);
        }

        public static bool IsCollection(Type type)
        {
            return type.IsGenericType && 
                ((type.GetGenericTypeDefinition() == typeof(ICollection<>)) ||
                (type.GetGenericTypeDefinition() == typeof(List<>)));
        }

        public static PropertyKind GetPropertyKind(PropertyInfo propertyInfo)
        {
            if (Reflection.IsCollection(propertyInfo))
            {
                return PropertyKind.Collection;
            }
            else if (Reflection.IsBaseType(propertyInfo))
            {
                return PropertyKind.Simple;
            }
            else
            {
                return PropertyKind.Lookup;
            }
        }

        public static Type GetComplexType(PropertyInfo propertyInfo)
        {
            if (IsCollection(propertyInfo))
            {
                return propertyInfo.PropertyType.GetGenericArguments()[0];
            }

            return propertyInfo.PropertyType;
        }
    }
}