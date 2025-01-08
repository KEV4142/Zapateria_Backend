using Modelo.Entidades;

namespace WebApi.Extensions;
public static class PoliciesConfiguration
{
    public static IServiceCollection AddPoliciesServices(
            this IServiceCollection services
        )
    {
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy(
                CustomRoles.ADMIN, policy =>
                   policy.RequireRole(CustomRoles.ADMIN)
            );

            opt.AddPolicy(
                PolicyMaster.CATEGORIA_READ, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.CATEGORIA_READ
                    )
                   )
            );

            opt.AddPolicy(
                PolicyMaster.CATEGORIA_CREATE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.CATEGORIA_CREATE
                    )
                   )
            );

            opt.AddPolicy(
                PolicyMaster.CATEGORIA_UPDATE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.CATEGORIA_UPDATE
                    )
                   )
            );
            opt.AddPolicy(
              PolicyMaster.IMAGEN_READ, policy =>
                 policy.RequireAssertion(
                  context => context.User.HasClaim(
                  c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.IMAGEN_READ
                  )
                 )
          );
            opt.AddPolicy(
             PolicyMaster.IMAGEN_UPDATE, policy =>
                policy.RequireAssertion(
                 context => context.User.HasClaim(
                 c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.IMAGEN_UPDATE
                 )
                )
         );
            opt.AddPolicy(
                PolicyMaster.IMAGEN_CREATE, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.IMAGEN_CREATE
                    )
                    )
            );
            opt.AddPolicy(
                PolicyMaster.PRODUCTOS_READ, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.PRODUCTOS_READ
                    )
                    )
            );
            opt.AddPolicy(
                PolicyMaster.PRODUCTOS_UPDATE, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.PRODUCTOS_UPDATE
                    )
                    )
            );
            opt.AddPolicy(
                PolicyMaster.PRODUCTOS_CREATE, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.PRODUCTOS_CREATE
                    )
                    )
                );
            opt.AddPolicy(
                PolicyMaster.USUARIO_CREATE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.USUARIO_CREATE
                    )
                   )
            );
        }
        );
        return services;
    }
}
