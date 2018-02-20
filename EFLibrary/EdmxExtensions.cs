using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LinqToEdmx;
using LinqToEdmx.Model.Conceptual;
using LinqToEdmx.Map;
using System.Reflection;

namespace EFLibrary
{
    public static class EdmxExtensions
    {
        public static string GetTableName(this Edmx edmx, string entityName)
        {
            return GetStorageEntityContainer(edmx).EntitySets.Where(e => e.Name == entityName).Single().Table;
        }

        public static LinqToEdmx.Model.Storage.EntityContainer GetStorageEntityContainer(this Edmx edmx)
        {
            return edmx.GetItems<LinqToEdmx.Model.Storage.EntityContainer>().Single();
        }

        public static string GetEntityKeyPropertyName(this Edmx edmx, string entityName)
        {
            return edmx.GetEntityKey(entityName).PropertyRefs.Single().Name;
        }

        public static EntityKeyElement GetEntityKey(this Edmx edmx, string entityName)
        {
            return edmx.GetEntityType(entityName).Key;
        }

        public static Dictionary<string, string> GetScalarPropertiesMappingDictionary(this Edmx edmx, string entityName)
        {
            Dictionary<string, string> mappingDictionary = new Dictionary<string, string>();

            string entityFullName = edmx.GetEntityFullName(entityName);

            foreach (ScalarProperty scalarProperty in edmx.GetEntityTypeMapping(entityFullName).MappingFragments[0].ScalarProperties)
            {
                mappingDictionary.Add(scalarProperty.Name, scalarProperty.ColumnName);
            }

            return mappingDictionary;
        }

        public static EndProperty GetParentChildRelationEndProperty(this Edmx edmx, string parentEntityName, string childPropertyName)
        {
            NavigationProperty navigationProperty = edmx.GetNavigationProperty(parentEntityName, childPropertyName);
            string relationship = GetAdjustedText(navigationProperty.Relationship);

            string fromRole = navigationProperty.FromRole;
            string toRole = navigationProperty.ToRole;

            LinqToEdmx.Model.Conceptual.Association association = edmx.GetConceptualAssociation(relationship);
            
            LinqToEdmx.Model.Conceptual.AssociationEnd fromAssociation = edmx.GetConceptualAssociationEnd(relationship, fromRole);
            LinqToEdmx.Model.Conceptual.AssociationEnd toAssociation = edmx.GetConceptualAssociationEnd(relationship, toRole);

            string childEntityName = GetAdjustedText(toAssociation.Type);
            AssociationSetMapping entityTypesMappingAssociation = edmx.GetMappingAssociationSet(childEntityName, relationship);

            return entityTypesMappingAssociation.EndProperties.Where(e => e.Name == fromRole).Single();
        }

        public static string GetAdjustedText(string relationship)
        {
            return relationship.Replace("Self.", string.Empty);
        }

        public static Dictionary<string, string> GetLookupPropertiesMappingDictionary(this Edmx edmx, Type entityClrType)
        {
            string entityName = entityClrType.Name;
            Dictionary<string, string> mappingDictionary = new Dictionary<string, string>();
            foreach (NavigationProperty navigationProperty in edmx.GetLookupProperties(entityClrType))
            {
                string relationship = GetAdjustedText(navigationProperty.Relationship);

                string fromRole = navigationProperty.FromRole;
                string toRole = navigationProperty.ToRole;

                LinqToEdmx.Model.Conceptual.Association association = edmx.GetConceptualAssociation(relationship);

                LinqToEdmx.Model.Conceptual.AssociationEnd fromAssociation = edmx.GetConceptualAssociationEnd(relationship, fromRole);
                LinqToEdmx.Model.Conceptual.AssociationEnd toAssociation = edmx.GetConceptualAssociationEnd(relationship, toRole);

                AssociationSetMapping entityTypesMappingAssociation = edmx.GetMappingAssociationSet(entityName, relationship);
                foreach (EndProperty endProperty in entityTypesMappingAssociation.EndProperties)
                {
                    if (endProperty.Name == toRole)
                    {
                        string columnName = endProperty.ScalarProperties[0].ColumnName;
                        mappingDictionary.Add(navigationProperty.Name, columnName);
                    }
                }
            }

            return mappingDictionary;
        }

        public static AssociationSetMapping GetMappingAssociationSet(this Edmx edmx, string entityName, string relationship)
        {
            return edmx.GetItems<AssociationSetMapping>().Where(a => a.StoreEntitySet == entityName && a.Name == relationship).Single();
        }

        public static LinqToEdmx.Model.Conceptual.Association GetConceptualAssociation(this Edmx edmx, string relationship)
        {
            return edmx.GetItems<LinqToEdmx.Model.Conceptual.Association>().Where(a => a.Name == relationship).Single();
        }

        public static LinqToEdmx.Model.Conceptual.AssociationEnd GetConceptualAssociationEnd(this Edmx edmx, string relationship, string role)
        {
            LinqToEdmx.Model.Conceptual.Association association = edmx.GetConceptualAssociation(relationship);
            return association.Ends.Where(e => e.Role == role).Single();
        }

        public static List<NavigationProperty> GetLookupProperties(this Edmx edmx, Type entityClrType)
        {
            List<NavigationProperty> properties = new List<NavigationProperty>();

            string entityName = entityClrType.Name;
            foreach (NavigationProperty navigationProperty in edmx.GetNavigationProperties(entityName))
	        {
                PropertyInfo propertyInfo = entityClrType.GetProperty(navigationProperty.Name);
                if (!EFLibrary.Reflection.IsCollection(propertyInfo))
                {
                    properties.Add(navigationProperty);
                }
	        }
            
            return properties;
        }

        public static List<NavigationProperty> GetListProperties(this Edmx edmx, Type entityClrType)
        {
            List<NavigationProperty> properties = new List<NavigationProperty>();

            string entityName = entityClrType.Name;
            foreach (NavigationProperty navigationProperty in edmx.GetNavigationProperties(entityName))
            {
                PropertyInfo propertyInfo = entityClrType.GetProperty(navigationProperty.Name);
                if (EFLibrary.Reflection.IsCollection(propertyInfo))
                {
                    properties.Add(navigationProperty);
                }
            }

            return properties;
        }

        public static NavigationProperty GetNavigationProperty(this Edmx edmx, string entityName, string navigationPropertyName)
        {
            return edmx.GetNavigationProperties(entityName).Where(n => n.Name == navigationPropertyName).Single();
        }

        public static List<NavigationProperty> GetNavigationProperties(this Edmx edmx, string entityName)
        {
            if (edmx.GetEntityType(entityName).NavigationProperties.Count > 0)
            {
                return edmx.GetEntityType(entityName).NavigationProperties.ToList();
            }
            return new List<NavigationProperty>();
        }

        public static string GetEntityFullName(this Edmx edmx, string entityName)
        {
            return string.Concat(edmx.GetConceptualSchemaNamespace(), ".", entityName);
        }

        public static string GetConceptualSchemaNamespace(this Edmx edmx)
        {
            return edmx.GetItems<ConceptualSchema>().Single().Namespace;
        }

        public static EntityType GetEntityType(this Edmx edmx, string entityName)
        {
            return edmx.GetItems<EntityType>().Where(e => e.Name == entityName).Single();
        }

        public static EntityTypeMapping GetEntityTypeMapping(this Edmx edmx, string entityFullName)
        {
            return edmx.GetItems<EntityTypeMapping>().Where(e => e.TypeName == entityFullName).Single();
        }
    }
}
