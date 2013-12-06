using System;
using AntMicro.Migrant;
using AntMicro.Migrant.Customization;

namespace AntMicro.Migrant
{
    public class VersionTolerancePolicy
    {
        public VersionToleranceLevel VersionToleranceLevel { get; private set; }

        public bool AllowFieldRemoval
        {
            get
            {
                return VersionToleranceLevel == VersionToleranceLevel.FieldRemoval ||
                       VersionToleranceLevel == VersionToleranceLevel.FieldAdditionAndRemoval;
            }
        }

        public bool AllowFieldAddition
        {
            get
            {
                return VersionToleranceLevel == VersionToleranceLevel.FieldAddition ||
                       VersionToleranceLevel == VersionToleranceLevel.FieldAdditionAndRemoval;
            }
        }

        private readonly bool _serializeBuiltInTypesForVersionTolerance;

        public VersionTolerancePolicy(
            bool serializeBuiltInTypesForVersionTolerance,
            VersionToleranceLevel versionToleranceLevel = 0)
        {
            _serializeBuiltInTypesForVersionTolerance = serializeBuiltInTypesForVersionTolerance;
            VersionToleranceLevel = versionToleranceLevel;
        }

        internal bool ShouldSerializeForVersionTolerance(CollectionMetaToken token)
        {
            return
                _serializeBuiltInTypesForVersionTolerance &&
                IsSpecialVersionTolerantType(token.ActualType) &&
                // TODO: For now only framework collection types are treated specially as version tolerant
                token.IsCollection; 
        }

        private bool IsSpecialVersionTolerantType(Type type)
        {
            // HACK: Quick and dirty way to segergate framework types from custom types
            return type.Assembly.CodeBase.Contains(@"Microsoft.NET/Framework");
        }
    }
}