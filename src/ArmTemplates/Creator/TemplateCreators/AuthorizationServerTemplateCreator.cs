﻿using System.Collections.Generic;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Constants;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.TemplateModels;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Templates.Abstractions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Templates.Builders.Abstractions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Creator.Models;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Creator.TemplateCreators
{
    public class AuthorizationServerTemplateCreator
    {
        readonly ITemplateBuilder templateBuilder;

        public AuthorizationServerTemplateCreator(ITemplateBuilder templateBuilder)
        {
            this.templateBuilder = templateBuilder;
        }

        public Template CreateAuthorizationServerTemplate(CreatorConfig creatorConfig)
        {
            // create empty template
            Template authorizationTemplate = this.templateBuilder.GenerateEmptyTemplate().Build();

            // add parameters
            authorizationTemplate.Parameters = new Dictionary<string, TemplateParameterProperties>
            {
                { ParameterNames.ApimServiceName, new TemplateParameterProperties(){ type = "string" } }
            };

            List<TemplateResource> resources = new List<TemplateResource>();
            foreach (AuthorizationServerTemplateProperties authorizationServerTemplateProperties in creatorConfig.authorizationServers)
            {
                // create authorization server resource with properties
                AuthorizationServerTemplateResource authorizationServerTemplateResource = new AuthorizationServerTemplateResource()
                {
                    Name = $"[concat(parameters('{ParameterNames.ApimServiceName}'), '/{authorizationServerTemplateProperties.displayName}')]",
                    Type = ResourceTypeConstants.AuthorizationServer,
                    ApiVersion = GlobalConstants.ApiVersion,
                    Properties = authorizationServerTemplateProperties,
                    DependsOn = new string[] { }
                };
                resources.Add(authorizationServerTemplateResource);
            }

            authorizationTemplate.Resources = resources.ToArray();
            return authorizationTemplate;
        }
    }
}
