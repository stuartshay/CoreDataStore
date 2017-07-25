﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace CoreDataStore.Data.Conventions
{
    /// <summary>
    /// https://blogs.msmvps.com/ricardoperes/2017/02/27/implementing-missing-features-in-entity-framework-core-part-7-entity-configuration-in-mapping-classes/
    /// </summary>
    public static class EntityTypeConfigurationExtensions
    {
        private static readonly MethodInfo EntityMethod = typeof(ModelBuilder).GetTypeInfo().GetMethods().Single(x => (x.Name == "Entity") && (x.IsGenericMethod == true) && (x.GetParameters().Length == 0));

        private static Type FindEntityType(Type type)
        {
            var interfaceType = type.GetInterfaces().First(x => (x.GetTypeInfo().IsGenericType == true) && (x.GetGenericTypeDefinition() == typeof(Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<>)));
            return interfaceType.GetGenericArguments().First();
        }

        private static readonly Dictionary<Assembly, IEnumerable<Type>> TypesPerAssembly = new Dictionary<Assembly, IEnumerable<Type>>();

        public static ModelBuilder ApplyConfiguration<T>(this ModelBuilder modelBuilder, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<T> configuration) where T : class
        {
            var entityType = FindEntityType(configuration.GetType());

            dynamic entityTypeBuilder = EntityMethod
                .MakeGenericMethod(entityType)
                .Invoke(modelBuilder, new object[0]);

            configuration.Configure(entityTypeBuilder);

            return modelBuilder;
        }

        public static ModelBuilder UseEntityTypeConfiguration(this ModelBuilder modelBuilder)
        {
            IEnumerable<Type> configurationTypes;
            var asm = Assembly.GetEntryAssembly();

            if (TypesPerAssembly.TryGetValue(asm, out configurationTypes) == false)
            {
                TypesPerAssembly[asm] = configurationTypes = asm
                    .GetExportedTypes()
                    .Where(x => (x.GetTypeInfo().IsClass == true) && (x.GetTypeInfo().IsAbstract == false) && (x.GetInterfaces().Any(y => (y.GetTypeInfo().IsGenericType == true) && (y.GetGenericTypeDefinition() == typeof(Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<>)))));
            }

            var configurations = configurationTypes.Select(Activator.CreateInstance);

            foreach (dynamic configuration in configurations)
            {
                ApplyConfiguration(modelBuilder, configuration);
            }

            return modelBuilder;
        }
    }
}