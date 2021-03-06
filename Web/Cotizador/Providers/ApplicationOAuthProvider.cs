﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cotizador.Models;
using libProduccionDataBase.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace Cotizador.Providers
{
	public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
	{
		private readonly string _publicClientId;
		public ApplicationOAuthProvider ( string publicClientId )
		{
			if ( publicClientId == null | publicClientId == "" )
			{
				throw new ArgumentNullException ( nameof ( publicClientId ) );
			}
			else
			{
				_publicClientId = publicClientId;
			}
		}
		public override async Task GrantResourceOwnerCredentials ( OAuthGrantResourceOwnerCredentialsContext context )
		{
			var userManager = context.OwinContext.GetUserManager<Models.ApplicationUserManager> ( );

			ApplicationUser user = await userManager.FindAsync ( context.UserName, context.Password );

			if ( user == null )
			{
				context.SetError ( "invalid_grant", "El nombre de usuario o la contraseña no son correctos." );
				return;
			}

			if ( user.Estatus == ApplicationUser.status.Baja )
			{
				context.SetError ( "invalid_grant", "Usuario no Autorizado." );
				return;
			}

			if ( !user.EmailConfirmed )
			{
				context.SetError ( "invalid_grant", "Debe Confirmar su Email para continuar." );
				return;
			}

			ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync ( userManager, OAuthDefaults.AuthenticationType );
			ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync ( userManager, CookieAuthenticationDefaults.AuthenticationType );

			var roles = await userManager.GetRolesAsync ( user.Id );


			AuthenticationProperties properties = CreateProperties ( user.UserName, user.Id, roles.ToArray ( ) );
			AuthenticationTicket ticket = new AuthenticationTicket ( oAuthIdentity, properties );
			context.Validated ( ticket );
			context.Request.Context.Authentication.SignIn ( cookiesIdentity );
		}

		public override Task TokenEndpoint ( OAuthTokenEndpointContext context )
		{
			foreach ( KeyValuePair<string, string> property in context.Properties.Dictionary )
			{
				context.AdditionalResponseParameters.Add ( property.Key, property.Value );
			}
			return Task.FromResult<object> ( null );
		}

		public override Task ValidateClientAuthentication ( OAuthValidateClientAuthenticationContext context )
		{
			// La credenciales de la contraseña del propietario del recurso no proporcionan un id. de cliente.
			if ( context.ClientId == null )
			{
				context.Validated ( );
			}

			return Task.FromResult<object> ( null );
		}

		public override Task ValidateClientRedirectUri ( OAuthValidateClientRedirectUriContext context )
		{
			if ( context.ClientId == _publicClientId )
			{
				Uri expectedRootUri = new Uri ( context.Request.Uri, "/" );

				if ( expectedRootUri.AbsoluteUri == context.RedirectUri )
				{
					context.Validated ( );
				}
			}

			return Task.FromResult<object> ( null );
		}

		public static AuthenticationProperties CreateProperties ( string userName, int userId = 0, string[] roles = null )
		{
			roles = roles ?? new string[] { "Usuario" };

			IDictionary<string, string> data = new Dictionary<string, string> {
				{ "userName", userName }, { "userId", userId.ToString()}, { "userRoles", string.Join(",", roles)}
			};
			return new AuthenticationProperties ( data );
		}
	}
}