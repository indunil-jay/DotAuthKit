namespace Application.Permissions;

public static class Permissions
{
    public static class Users
    {
        public const string View = "users:view";
        public const string Create = "users:create";
        public const string Update = "users:update";
        public const string Delete = "users:delete";
        public const string AssignRoles = "users:assign-roles";
    }

    public static class Roles
    {
        public const string View = "roles:view";
        public const string Create = "roles:create";
        public const string Update = "roles:update";
        public const string Delete = "roles:delete";
        public const string AssignPermissions = "roles:assign-permissions";
    }

    public static class Todos
    {
        public const string View = "todos:view";
        public const string Create = "todos:create";
        public const string Update = "todos:update";
        public const string Delete = "todos:delete";
    }

    public static IReadOnlyList<string> All { get; } = typeof(Permissions)
        .GetNestedTypes()
        .SelectMany(t => t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
        .Where(f => f.FieldType == typeof(string))
        .Select(f => (string)f.GetValue(null)!)
        .ToList()
        .AsReadOnly();
}
