using CodePlex.XPathParser;
using NUnit.Framework;

namespace XPathParserTest
{
    [TestFixture]
    public class UnitTest
    {
        static string[] correctTests = {
             // Expressions from http://www.w3.org/TR/xpath#location-paths
            @"child::para"                                                          ,
            @"child::*"                                                             ,
            @"child::text()"                                                        ,
            @"child::node()"                                                        ,
            @"attribute::name"                                                      ,
            @"attribute::*"                                                         ,
            @"descendant::para"                                                     ,
            @"ancestor::div"                                                        ,
            @"ancestor-or-self::div"                                                ,
            @"descendant-or-self::para"                                             ,
            @"self::para"                                                           ,
            @"child::chapter/descendant::para"                                      ,
            @"child::*/child::para"                                                 ,
            @"/"                                                                    ,
            @"/descendant::para"                                                    ,
            @"/descendant::olist/child::item"                                       ,
            @"child::para[position()=1]"                                            ,
            @"child::para[position()=last()]"                                       ,
            @"child::para[position()=last()-1]"                                     ,
            @"child::para[position()>1]"                                            ,
            @"following-sibling::chapter[position()=1]"                             ,
            @"preceding-sibling::chapter[position()=1]"                             ,
            @"/descendant::figure[position()=42]"                                   ,
            @"/child::doc/child::chapter[position()=5]/child::section[position()=2]",
            @"child::para[attribute::type=""warning""]"                             ,
            @"child::para[attribute::type='warning'][position()=5]"                 ,
            @"child::para[position()=5][attribute::type=""warning""]"               ,
            @"child::chapter[child::title='Introduction']"                          ,
            @"child::chapter[child::title]"                                         ,
            @"child::*[self::chapter or self::appendix]"                            ,
            @"child::*[self::chapter or self::appendix][position()=last()]"         ,
        };

        [Test]
        [TestCaseSource(nameof(correctTests))]
        public void Unabbreviated(string input)
        {
            var result = new XPathParser<string>().Parse(input, new XPathStringBuilder());

            Assert.IsNotEmpty(result);
        }

        [Test]
        [TestCaseSource(nameof(abbreviatedTests))]
        public void Abbreviated(string input)
        {
            var result = new XPathParser<string>().Parse(input, new XPathStringBuilder());

            Assert.IsNotEmpty(result);
        }

        private static string[] abbreviatedTests =
        {
            "para", // selects the para element children of the context node
"*", // selects all element children of the context node
"text()", // selects all text node children of the context node
"@name", // selects the name attribute of the context node
"@*", // selects all the attributes of the context node
"para[1]", // selects the first para child of the context node
"para[last()]", // selects the last para child of the context node
"*/para", // selects all para grandchildren of the context node
"/doc/chapter[5]/section[2]", // selects the second section of the fifth chapter of the doc
"chapter//para", // selects the para element descendants of the chapter element children of the context node
"//para", // selects all the para descendants of the document root and thus selects all para elements in the same document as the context node
"//olist/item", // selects all the item elements in the same document as the context node that have an olist parent
".", // selects the context node
".//para", // selects the para element descendants of the context node
"..", // selects the parent of the context node
"../@lang", // selects the lang attribute of the parent of the context node
"para[@type=\"warning\"]", // selects all para children of the context node that have a type attribute with value warning
"para[@type=\"warning\"][5]", // selects the fifth para child of the context node that has a type attribute with value warning
"para[5][@type=\"warning\"]", // selects the fifth para child of the context node if that child has a type attribute with value warning
"chapter[title=\"Introduction\"]", // selects the chapter children of the context node that have one or more title children with string-value equal to Introduction
"chapter[title]", // selects the chapter children of the context node that have one or more title children
"employee[@secretary and @assistant]", // selects all the employee children of the context node that have both a secretary attribute and an assistant attribute
        };

        static string[] errorTests = {
            @""     ,
            @"a b"  ,
            @"a["   ,
            @"]"    ,
            @"///"  ,
            @"fo("  ,
            @")"    ,
            @"a[']" ,
            @"b[""]",
            @"3e8"  ,
            @"child::*[self::chapter or self::appendix][position()=last()] child::*[self::chapter or self::appendix][position()=last()]",
        };
        [Test]
        [TestCaseSource(nameof(errorTests))]
        public void Bad(string input)
        {
            Assert.Throws<XPathParserException>(() => new XPathParser<string>().Parse(input, new XPathStringBuilder()));
        }

    }
}