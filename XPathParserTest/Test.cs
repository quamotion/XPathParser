using CodePlex.XPathParser;
using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace XPathParserTest
{
    public class Test
    {
        // Expressions from http://www.w3.org/TR/xpath#location-paths
        [InlineData(@"child::para")]
        [InlineData(@"child::*")]
        [InlineData(@"child::text()")]
        [InlineData(@"child::node()")]
        [InlineData(@"attribute::name")]
        [InlineData(@"attribute::*")]
        [InlineData(@"descendant::para")]
        [InlineData(@"ancestor::div")]
        [InlineData(@"ancestor-or-self::div")]
        [InlineData(@"descendant-or-self::para")]
        [InlineData(@"self::para")]
        [InlineData(@"child::chapter/descendant::para")]
        [InlineData(@"child::*/child::para")]
        [InlineData(@"/")]
        [InlineData(@"/descendant::para")]
        [InlineData(@"/descendant::olist/child::item")]
        [InlineData(@"child::para[position()=1]")]
        [InlineData(@"child::para[position()=last()]")]
        [InlineData(@"child::para[position()=last()-1]")]
        [InlineData(@"child::para[position()>1]")]
        [InlineData(@"following-sibling::chapter[position()=1]")]
        [InlineData(@"preceding-sibling::chapter[position()=1]")]
        [InlineData(@"/descendant::figure[position()=42]")]
        [InlineData(@"/child::doc/child::chapter[position()=5]/child::section[position()=2]")]
        [InlineData(@"child::para[attribute::type=""warning""]")]
        [InlineData(@"child::para[attribute::type='warning'][position()=5]")]
        [InlineData(@"child::para[position()=5][attribute::type=""warning""]")]
        [InlineData(@"child::chapter[child::title='Introduction']")]
        [InlineData(@"child::chapter[child::title]")]
        [InlineData(@"child::*[self::chapter or self::appendix]")]
        [InlineData(@"child::*[self::chapter or self::appendix][position()=last()]")]
        [Theory]
        public void CorrectTest(string expression)
        {
            RunTestString(expression);
            RunTestTree(expression);
        }

        [InlineData(@"")]
        [InlineData(@"a b")]
        [InlineData(@"a[")]
        [InlineData(@"]")]
        [InlineData(@"///")]
        [InlineData(@"fo(")]
        [InlineData(@")")]
        [InlineData(@"a[']")]
        [InlineData(@"b[""]")]
        [InlineData(@"3e8")]
        [InlineData(@"child::*[self::chapter or self::appendix][position()=last()] child::*[self::chapter or self::appendix][position()=last()]")]
        [Theory]
        public void ErrorTest(string expression)
        {
            Assert.Throws<XPathParserException>(() => RunTestTree(expression));
        }

        static void RunTestString(string xpathExpr)
        {
            Debug.WriteLine("Translated one: {0}", new XPathParser<string>().Parse(xpathExpr, new XPathStringBuilder()));
        }

        static void RunTestTree(string xpathExpr)
        {
            XElement xe = new XPathParser<XElement>().Parse(xpathExpr, new XPathTreeBuilder());
            XmlWriterSettings ws = new XmlWriterSettings();
            {
                ws.Indent = true;
                ws.OmitXmlDeclaration = true;
            }
            using (XmlWriter w = XmlWriter.Create(Console.Out, ws))
            {
                xe.WriteTo(w);
            }
        }
    }
}

