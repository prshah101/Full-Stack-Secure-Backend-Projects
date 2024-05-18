using FastMember;
using Npgsql;
namespace robot_controller_api.Persistence
{
    // Derfine the extension methods for database operations
    public static class ExtensionMethods
    {
         // Maps data from a NpgsqlDataReader to properties of an entity
        public static void MapTo<T>(this NpgsqlDataReader dr, T entity)
        {
            if (entity == null) throw new
           ArgumentNullException(nameof(entity));
           // Create a FastMember accessor for the entity type
            var fastMember = TypeAccessor.Create(entity.GetType());
            // Extracting the property names from entity type and convert to HashSet
            var props = fastMember.GetMembers().Select(x =>
           x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

           // Iterating through the database columns
            for (int i = 0; i < dr.FieldCount; i++)
            {
                // Finding the related property name for each column
                var prop = props.FirstOrDefault(x =>
               x.Equals(Convert.ToPascalCase(dr.GetName(i)), StringComparison.OrdinalIgnoreCase));
                // If property name found, map the database value to entity property
                if (!string.IsNullOrEmpty(prop))
                    fastMember[entity, prop] = dr.IsDBNull(i) ? null :
                   dr.GetValue(i);
            }
        }
    }
}