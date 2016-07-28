using System.Xml.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using CodePlex.XPathParser;
using NUnit.Framework;

namespace XPathParserTest
{
    [TestFixture]
    [UseReporter(typeof(BeyondCompareReporter))]
    public class SpecificTests
    {
        [Test]
        public void Functions()
        {
            var input = "/para[substring(\"12345\", 0, 3) = \"1234\"]";
            var actual = new XPathParser<XElement>().Parse(input, new XPathTreeBuilder());
            
            Approvals.VerifyXml(actual.ToString());
        }
    }
}