using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriterUnitTests
{
    /// <summary>
    /// Summary description for Given_an_OdcmModel
    /// </summary>
    [TestClass]
    public class Given_an_OdcmClass_Entity_IFetcher : CodeGenTestBase
    {
        private OdcmModel _model;
        private OdcmNamespace _namespace;
        private OdcmClass _class;

        [TestInitialize]
        public void Init()
        {
            _model = new OdcmModel();

            _namespace = Any.OdcmNamespace();

            _model.Namespaces.Add(_namespace);

            _class = new OdcmClass(Any.CSharpIdentifier(), _namespace.Name, OdcmClassKind.Entity);
        }

        [TestMethod]
        public void When_the_model_class_has_primitive_properties_then_they_are_not_exposed_on_the_Fetcher()
        {
            _class.Properties.Add(Any.PrimitiveOdcmProperty());

            var interfaceName = _namespace.Name + ".I" + _class.Name + "Fetcher";

            _model.AddType(_class);

            var proxy = GetProxy(_model);

            var classType = proxy.GetClass(_class.Namespace, _class.Name);

            classType.GetInterface(interfaceName).GetMembers().Should().BeEmpty();
        }

        [TestMethod]
        public void When_the_model_class_has_complex_properties_then_they_are_not_exposed_on_the_Fetcher()
        {
            var field = Any.ComplexOdcmField();

            _class.Properties.Add(Any.PrimitiveOdcmProperty());

            var interfaceName = _namespace.Name + ".I" + _class.Name + "Fetcher";

            _model.AddType(_class);

            var proxy = GetProxy(_model);

            var classType = proxy.GetClass(_class.Namespace, _class.Name);

            classType.GetInterface(interfaceName).GetMembers().Should().BeEmpty();
        }

        [TestMethod]
        public void When_the_model_class_has_entity_properties_then_they_are_exposed_on_the_Fetcher()
        {
            var field = Any.ComplexOdcmField();

            _class.Properties.Add(Any.OdcmProperty(p => p.Type= _class));

            var interfaceName = _namespace.Name + ".I" + _class.Name + "Fetcher";

            _model.AddType(_class);

            var proxy = GetProxy(_model);

            var classType = proxy.GetClass(_class.Namespace, _class.Name);

            classType.GetInterface(interfaceName).GetMembers().Should().NotBeEmpty();
        }
    }
}
