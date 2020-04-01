﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Tye.ConfigModel;
using YamlDotNet.RepresentationModel;

namespace Tye.Serialization
{
    public static class YamlConfigApplicationHelpers
    {

        public static void HandleConfigApplication(YamlMappingNode? yamlMappingNode, ConfigApplication app)
        {
            foreach (var child in yamlMappingNode!.Children)
            {
                string key;
                switch (child.Key.NodeType)
                {
                    case YamlNodeType.Scalar:
                        key = (child.Key as YamlScalarNode)!.Value!;
                        break;
                    default:
                        // Don't support other types.
                        continue;
                }

                switch (key)
                {
                    case "name":
                        app.Name = YamlParser.GetScalarValue(key, child.Value);
                        break;
                    case "registry":
                        app.Registry = YamlParser.GetScalarValue(key, child.Value);
                        break;
                    case "ingress":
                        if (child.Value.NodeType != YamlNodeType.Sequence)
                        {
                            throw new TyeYamlException(child.Value.Start, $"Excpeted yaml sequence for key: {key}.");
                        }
                        YamlIngressHelpers.HandleIngress((child.Value as YamlSequenceNode)!, app.Ingress);
                        break;
                    case "services":
                        if (child.Value.NodeType != YamlNodeType.Sequence)
                        {
                            throw new TyeYamlException(child.Value.Start, $"Excpeted yaml sequence for key: {key}.");
                        }
                        YamlServiceHelpers.HandleServiceMapping((child.Value as YamlSequenceNode)!, app.Services);
                        break;
                }
            }
        }
    }
}