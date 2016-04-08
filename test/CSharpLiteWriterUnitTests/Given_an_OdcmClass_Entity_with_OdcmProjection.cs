using Microsoft.Its.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;
using Vipr.Writer.CSharp.Lite;
using Xunit;
using Type = System.Type;
using FluentAssertions;
using System.Linq.Expressions;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_with_OdcmProjection : EntityTestBase
    {
        private Type m_FetcherInterface;
        private Type m_CollectionInterface;
        private OdcmClass m_TargetClass;

        private void Init(OdcmProjection projection)
        {            
            base.Init(
                m =>
                {
                    m_TargetClass = m.Namespaces[0].Classes.First();                    
                    m_TargetClass.Properties.Add(Any.PrimitiveOdcmProperty(p => p.Class = Class));
                    
                    projection.Type = m_TargetClass;
#if false
                    if (!m_TargetClass.Projections.Contains(projection))
                    {
                        m_TargetClass.Projections.Add(projection);
                    }
#else
                    m_TargetClass.AddProjection(projection.Capabilities);
#endif
                });


            var identifier = NamesService.GetFetcherInterfaceName(m_TargetClass, projection);
            m_FetcherInterface = Proxy.GetInterface(m_TargetClass.Namespace, identifier.Name);

            identifier = NamesService.GetCollectionInterfaceName(m_TargetClass, projection);
            m_CollectionInterface = Proxy.GetInterface(m_TargetClass.Namespace, identifier.Name);
        }

        private OdcmProjection AnyOdcmProjection(OdcmType odcmType)
        {
#if true
            return odcmType.AnyOdcmProjection();
#else
            var projection = new OdcmProjection
            {
                Type = odcmType,
                Capabilities = new List<OdcmCapability>()
            };

            var capabilityTerms = projection
                    .GetBooleanCapabilityTerms().RandomSequence()
                    .Distinct();

            foreach (var term in capabilityTerms)
            {
                projection.Capabilities.Add(new OdcmBooleanCapability(Any.Bool(), term));
            }

            return projection;
#endif
        }

        [Fact]
        public void The_Fetcher_Interface_for_the_projection_has_only_supported_capabilities_encoded_in_its_name()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            Init(projection);

            var expectedFetcherInterfaceName = "I" + m_TargetClass.Name + "Fetcher" + GetProjectionEncoding(projection);
            m_FetcherInterface.Name.Should().Be(expectedFetcherInterfaceName);
        }

        [Fact]
        public void The_Collection_Interface_for_the_projection_has_only_supported_capabilities_encoded_in_its_name()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            Init(projection);

            var expectedCollectionInterfaceName = "I" + m_TargetClass.Name + "Collection" + GetProjectionEncoding(projection);
            m_CollectionInterface.Name.Should().Be(expectedCollectionInterfaceName);
        }

        [Fact]        
        public void When_projection_supports_Expand_then_fetcher_interface_exposes_Expand_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var expandCapability = projection.SetBooleanCapability(TermNames.Expand, true);

            Init(projection);

            var expandMethod = m_FetcherInterface.GetMethod("Expand");

            expandMethod.Should().NotBeNull("Because entity has Expand Capability");

            expandMethod.IsPublic.Should().BeTrue();

            expandMethod.GetGenericArguments().Count()
                .Should().Be(1);

            var genericArgType = expandMethod.GetGenericArguments()[0];

            var expectedParamType =
                typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(ConcreteInterface, genericArgType));

            expandMethod.GetParameters().Count()
                .Should().Be(1);

            expandMethod.GetParameters()[0].ParameterType
                .Should().Be(expectedParamType);

            expandMethod.ReturnType
                .Should().Be(m_FetcherInterface);

            m_FetcherInterface.Name.Should().Contain(expandCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_Expand_then_fetcher_interface_does_not_expose_Expand_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var expandCapability = projection.SetBooleanCapability(TermNames.Expand, false);

            Init(projection);

            m_FetcherInterface.Should().NotHaveMethod("Expand", "Because entity does not have Expand Capability");
            m_FetcherInterface.Name.Should().NotContain(expandCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_Delete_then_fetcher_interface_exposes_DeleteAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var deleteCapability = projection.SetBooleanCapability(TermNames.Delete, true);

            Init(projection);

            m_FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task),
                "DeleteAsync",
                new Type[] { ConcreteInterface, typeof(bool) },
                "Because entity has Delete Capability");

            m_FetcherInterface.Name.Should().Contain(deleteCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_Delete_then_fetcher_interface_does_not_expose_DeleteAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var deleteCapability = projection.SetBooleanCapability(TermNames.Delete, false);

            Init(projection);

            m_FetcherInterface.Should().NotHaveMethod(                
                "DeleteAsync", "Because entity does not have Delete Capability");

            m_FetcherInterface.Name.Should().NotContain(deleteCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_DeleteLink_then_fetcher_interface_exposes_DeleteLinkAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var deleteLinkCapability = projection.SetBooleanCapability(TermNames.DeleteLink, true);

            Init(projection);

            m_FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task),
                "DeleteLinkAsync",
                new Type[] { ConcreteInterface, typeof(bool) },
                "Because entity has DeleteLink Capability");

            m_FetcherInterface.Name.Should().Contain(deleteLinkCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_DeleteLink_then_fetcher_interface_does_not_expose_DeleteLinkAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var deleteLinkCapability = projection.SetBooleanCapability(TermNames.DeleteLink, false);

            Init(projection);

            m_FetcherInterface.Should().NotHaveMethod(
                "DeleteLinkAsync", "Because entity does not have DeleteLink Capability");

            m_FetcherInterface.Name.Should().NotContain(deleteLinkCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_Update_then_fetcher_interface_exposes_UpdateAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateCapability = projection.SetBooleanCapability(TermNames.Update, true);

            Init(projection);

            m_FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task),
                "UpdateAsync",
                new Type[] { ConcreteInterface, typeof(bool) },
                "Because entity has Update Capability");

            m_FetcherInterface.Name.Should().Contain(updateCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_Update_then_fetcher_interface_exposes_SetAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateCapability = projection.SetBooleanCapability(TermNames.Update, true);

            Init(projection);

            m_FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task),
                "SetAsync",
                new System.Type[] { typeof(object), ConcreteInterface, typeof(bool) },
                "Because entity has Update Capability");

            m_FetcherInterface.Name.Should().Contain(updateCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_Update_then_fetcher_interface_does_not_expose_UpdateAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateCapability = projection.SetBooleanCapability(TermNames.Update, false);

            Init(projection);

            m_FetcherInterface.Should().NotHaveMethod(
                "UpdateAsync", "Because entity does not have Update Capability");

            m_FetcherInterface.Name.Should().NotContain(updateCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_Update_then_fetcher_interface_does_not_expose_SetAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateCapability = projection.SetBooleanCapability(TermNames.Update, false);

            Init(projection);

            m_FetcherInterface.Should().NotHaveMethod(
                "SetAsync", "Because entity does not have Update Capability");

            m_FetcherInterface.Name.Should().NotContain(updateCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_UpdateLink_then_fetcher_interface_exposes_UpdateLinkAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateLinkCapability = projection.SetBooleanCapability(TermNames.UpdateLink, true);

            Init(projection);

            m_FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task),
                "UpdateLinkAsync",
                new System.Type[] { typeof(object), ConcreteInterface, typeof(bool) },
                "Because entity has UpdateLink Capability");

            m_FetcherInterface.Name.Should().Contain(updateLinkCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_UpdateLink_then_fetcher_interface_does_not_expose_UpdateLinkAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateLinkCapability = projection.SetBooleanCapability(TermNames.UpdateLink, false);

            Init(projection);

            m_FetcherInterface.Should().NotHaveMethod(
                "UpdateLinkAsync", "Because entity does not have UpdateLink Capability");

            m_FetcherInterface.Name.Should().NotContain(updateLinkCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_Delete_then_collection_interface_exposes_DeleteAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var deleteCapability = projection.SetBooleanCapability(TermNames.Delete, true);

            Init(projection);

            m_CollectionInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "DeleteAsync",
                new System.Type[] { ConcreteInterface, typeof(bool) },
                "Because entity has Delete Capability");

            m_FetcherInterface.Name.Should().Contain(deleteCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_Delete_then_collection_interface_does_not_expose_DeleteAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var deleteCapability = projection.SetBooleanCapability(TermNames.Delete, false);

            Init(projection);

            m_CollectionInterface.Should().NotHaveMethod(
                "DeleteAsync", "Because entity does not have Delete Capability");

            m_FetcherInterface.Name.Should().NotContain(deleteCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_Update_then_collection_interface_exposes_UpdateAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateCapability = projection.SetBooleanCapability(TermNames.Update, true);

            Init(projection);

            m_CollectionInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "UpdateAsync",
                new System.Type[] { ConcreteInterface, typeof(bool) },
                "Because entity has Update Capability");

            m_FetcherInterface.Name.Should().Contain(updateCapability.GetShortName());

        }

        [Fact]
        public void When_projection_does_not_support_Update_then_collection_interface_does_not_expose_UpdateAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateCapability = projection.SetBooleanCapability(TermNames.Update, false);

            Init(projection);

            m_CollectionInterface.Should().NotHaveMethod(
                "UpdateAsync", "Because entity does not have Update Capability");

            m_FetcherInterface.Name.Should().NotContain(updateCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_Insert_then_collection_interface_exposes_AddAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var insertCapability = projection.SetBooleanCapability(TermNames.Insert, true);

            Init(projection);

            m_CollectionInterface.Should()
                .HaveMethod(
                    CSharpAccessModifiers.Public,
                    typeof(Task),
                    "Add" + m_TargetClass.Name + "Async",
                    new[] { ConcreteInterface, typeof(bool) },
                    "Because entity has Insert Capability");

            m_FetcherInterface.Name.Should().Contain(insertCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_Insert_then_collection_interface_does_not_expose_AddAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var insertCapability = projection.SetBooleanCapability(TermNames.Insert, false);

            Init(projection);

            m_CollectionInterface.Should().NotHaveMethod(
                "Add" + m_TargetClass.Name + "Async", "Because entity does not have Insert Capability");

            m_FetcherInterface.Name.Should().NotContain(insertCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_UpdateLink_then_collection_interface_exposes_AddLinkAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateLinkCapability = projection.SetBooleanCapability(TermNames.UpdateLink, true);

            Init(projection);

            m_CollectionInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "AddLinkAsync",
                new System.Type[] { typeof(object), ConcreteInterface, typeof(bool) },
                "Because entity has UpdateLink Capability");

            m_FetcherInterface.Name.Should().Contain(updateLinkCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_UpdateLink_then_collection_interface_does_not_expose_AddLinkAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var updateLinkCapability = projection.SetBooleanCapability(TermNames.UpdateLink, false);

            Init(projection);

            m_CollectionInterface.Should().NotHaveMethod(
                "AddLinkAsync", "Because entity does not have UpdateLink Capability");

            m_FetcherInterface.Name.Should().NotContain(updateLinkCapability.GetShortName());
        }

        [Fact]
        public void When_projection_supports_DeleteLink_then_collection_interface_exposes_RemoveLinkAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var deleteLinkCapability = projection.SetBooleanCapability(TermNames.DeleteLink, true);

            Init(projection);

            m_CollectionInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "RemoveLinkAsync",
                new System.Type[] { typeof(object), ConcreteInterface, typeof(bool) },
                "Because entity has DeleteLink Capability");

            m_FetcherInterface.Name.Should().Contain(deleteLinkCapability.GetShortName());
        }

        [Fact]
        public void When_projection_does_not_support_DeleteLink_then_collection_interface_does_not_expose_RemoveLinkAsync_Method()
        {
            var projection = AnyOdcmProjection(m_TargetClass);
            var deleteLinkCapability = projection.SetBooleanCapability(TermNames.DeleteLink, false);

            Init(projection);

            m_CollectionInterface.Should().NotHaveMethod(
                "RemoveLinkAsync", "Because entity does not have DeleteLink Capability");

            m_FetcherInterface.Name.Should().NotContain(deleteLinkCapability.GetShortName());
        }

        private string GetProjectionEncoding(OdcmProjection projection)
        {
            var suffix = projection.GetProjectionShortForm();

            if (!string.IsNullOrEmpty(suffix))
            {
                suffix = "_" + suffix;
            }

            return suffix;
        }
        
    }

}
