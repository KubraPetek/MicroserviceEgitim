// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        //Bunlar metot değil get propertisi
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resorce_catalog"){Scopes={"catalog_fullpermission"}},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermission"}},
            new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
            new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},
            new ApiResource("resource_order"){Scopes={"order_fullpermission"}},
            new ApiResource("resource_payment"){Scopes={"payment_fullpermission"}},
            new ApiResource("resource_gateway"){Scopes={"gateway_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        public static IEnumerable<IdentityResource> IdentityResources => //burası kullanıcı ile ilgili erişilebilecek  işlemleri tanımlar
                   new IdentityResource[]
                   {
                
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new []{"role"} }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
               new ApiScope("catalog_fullpermission","Catalog API için full erişim"),
               new ApiScope("photo_stock_fullpermission","Photo Stock API için full erişim"),
               new ApiScope("basket_fullpermission","BasketAPI için full erişim"),
               new ApiScope("discount_fullpermission","DiscountAPI için full erişim"),
               new ApiScope("order_fullpermission","OrderAPI için full erişim"),
               new ApiScope("payment_fullpermission","Payment için full erişim"),
               new ApiScope("gateway_fullpermission","Gateway için full erişim"),
               new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
             new Client //user olmayan kullanıcıların erişimi için 
             {
                 ClientId="WebMvcClient",
                 ClientName="Asp.Net Core MVC",
                 ClientSecrets={new Secret("secret".Sha256())},//şifrelemek için
                 AllowedGrantTypes=GrantTypes.ClientCredentials,//izin verilen izin tipleri
                 AllowedScopes={ "catalog_fullpermission","photo_stock_fullpermission", "gateway_fullpermission", IdentityServerConstants.LocalApi.ScopeName }//izin verilen scopelar
             },
             new Client //kulanıcı adı ve şifre ile erişim için oluşturulan client
             {
                 //Kullanıcı giriş yaptıktan sonra 60 gün içinde hiç giriş ektranı ile karşılaşmayacak ama 60 gün içinde herhangi bir zamanda giriş yapmayıp 61.gün de girerse tekra giriş ekranına gidecek --çünkü refresh token ömrü dolmuş olacak 
                 ClientId="WebMvcClientForUsers",
                 ClientName="Asp.Net Core MVC",
                 AllowOfflineAccess=true,
                 ClientSecrets={new Secret("secret".Sha256())},
                 AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,//refresh token da oluşturur
                 AllowedScopes={"basket_fullpermission","order_fullpermission","gateway_fullpermission"
                     ,IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId
                     , IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.LocalApi.ScopeName
                     , IdentityServerConstants.StandardScopes.OfflineAccess },//Offlineaccess refresh token üretir -offline olsa dahi
                 AccessTokenLifetime=1*60*60,//1 saat kullanım belirledik -->token için
                 RefreshTokenExpiration=TokenExpiration.Absolute, //verilen süre sonunda refresh tokenin ömrü dolmuş olacak
                 AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,//60 günlük saniye verdik-->refresh tokenın ömrü 
                 RefreshTokenUsage=TokenUsage.ReUse//Token sonradan tekrar kullanılabilir olsun
             },
             new Client //user olmayan kullanıcıların erişimi için 
             {
                 ClientId="TokenExchangeClient",
                 ClientName="Token Exchange Client",
                 ClientSecrets={new Secret("secret".Sha256())},//şifrelemek için
                 AllowedGrantTypes= new []{"urn:ietf:params:oauth:grant-type:token-exchange" },
                 AllowedScopes={ "discount_fullpermission", "payment_fullpermission" , IdentityServerConstants.StandardScopes.OpenId }//izin verilen scopelar
             }
            };
    }
}