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
    public class Given_an_OdcmClass_Entity : CodeGenTestBase
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

            _class = Any.OdcmClass(e => e.Namespace = _namespace.Name);
        }

        [TestMethod]
        public void When_proxied_then_the_proxy_class_has_an_interface()
        {
            _class = new OdcmClass(Any.CSharpIdentifier(), _namespace.Name, OdcmClassKind.Entity);

            var interfaceName = _namespace.Name + ".I" + _class.Name;

            _model.AddType(_class);

            var proxy = GetProxy(_model);

            var classType = proxy.GetClass(_class.Namespace, _class.Name);

            classType.GetInterface(interfaceName).Should().NotBeNull();
        }
        
        [TestMethod]
        public void When_proxied_then_the_proxy_class_inherits_from_EntityBase()
        {
            _class = new OdcmClass(Any.CSharpIdentifier(), _namespace.Name, OdcmClassKind.Entity);

            _model.AddType(_class);

            var proxy = GetProxy(_model);

            var classType = proxy.GetClass(_class.Namespace, _class.Name);

            var complexTypeBaseType = proxy.GetClass(_class.Namespace + ".Extensions", "EntityBase");

            classType.BaseType.Should().Be(complexTypeBaseType);
        }

        [TestMethod]
        public void When_proxied_then_the_proxy_class_has_a_Fetcher_interface()
        {
            _class = new OdcmClass(Any.CSharpIdentifier(), _namespace.Name, OdcmClassKind.Entity);

            var interfaceName = _namespace.Name + ".I" + _class.Name + "Fetcher";

            _model.AddType(_class);

            var proxy = GetProxy(_model);

            var classType = proxy.GetClass(_class.Namespace, _class.Name);

            classType.GetInterface(interfaceName).Should().NotBeNull();
        }
    }
}
