﻿using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Constants;
using Freem.Storage.Migrations.Constants.Collections.Builders;
using Freem.Storage.Migrations.Constants.Injection;
using Freem.Storage.Migrations.Scripts;

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Factories;

internal static class ScriptExtractorFactory
{
    public static ScriptExtractor Create()
    {
        var constants = new ConstantValuesCollectionBuilder()
            .WithConstant(ConstantNames.SchemaName, EnvironmentNames.Schema)
            .Build();
        
        var injector = new ConstantsInjector(constants);
        return new ScriptExtractor(injector);
    }
}