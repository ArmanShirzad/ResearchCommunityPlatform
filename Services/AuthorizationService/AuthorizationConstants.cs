using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ResearchCommunityPlatform.Services.AuthorizationService
{
    public static class AuthorizationConstants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";

        public static readonly OperationAuthorizationRequirement Create =
          new OperationAuthorizationRequirement { Name = CreateOperationName };
        public static readonly OperationAuthorizationRequirement Read =
          new OperationAuthorizationRequirement { Name = ReadOperationName };
        public static readonly OperationAuthorizationRequirement Update =
          new OperationAuthorizationRequirement { Name = UpdateOperationName };
        public static readonly OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = DeleteOperationName };
    }
}
