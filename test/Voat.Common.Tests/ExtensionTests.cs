#region LICENSE

/*
    
    Copyright(c) Voat, Inc.

    This file is part of Voat.

    This source file is subject to version 3 of the GPL license,
    that is bundled with this package in the file LICENSE, and is
    available online at http://www.gnu.org/licenses/gpl-3.0.txt;
    you may not use this file except in compliance with the License.

    Software distributed under the License is distributed on an
    "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express
    or implied. See the License for the specific language governing
    rights and limitations under the License.

    All Rights Reserved.

*/

#endregion LICENSE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voat.Common;
using Xunit;

namespace Voat.Common.Tests
{
    public class ExtensionTests 
    {
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void TestReverseSplit()
        {
            Assert.Equal("co.voat", "voat.co".ReverseSplit("."));
            Assert.Equal("co.voat.api", "api.voat.co".ReverseSplit("."));
            Assert.Equal("jason", "jason".ReverseSplit("."));
            Assert.Equal("", "".ReverseSplit("."));
            Assert.Equal(null, ((string)null).ReverseSplit("."));

        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void TestTrimSafe()
        {
            Assert.Equal(null, ((string)null).TrimSafe());
            Assert.Equal("", "".TrimSafe());
            Assert.Equal("", " ".TrimSafe());
            Assert.Equal(".", " . ".TrimSafe());

            Assert.Equal("jpg", ".jpg".TrimSafe("."));
            Assert.Equal("jpg", ".jpg.".TrimSafe("."));

        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void TestRelativePathingTests()
        {
            var parts = "somefile.txt".ToPathParts().ToList();
            Assert.Equal(1, parts.Count);
            Assert.Equal("somefile.txt", parts[0]);

            parts = "~/somefile.txt".ToPathParts().ToList();
            Assert.Equal(1, parts.Count);
            Assert.Equal("somefile.txt", parts[0]);

            parts = "~/folder/somefile.txt".ToPathParts().ToList();
            Assert.Equal(2, parts.Count);
            Assert.Equal("folder", parts[0]);
            Assert.Equal("somefile.txt", parts[1]);

            parts = "\\folder\\somefile.txt".ToPathParts().ToList();
            Assert.Equal(2, parts.Count);
            Assert.Equal("folder", parts[0]);
            Assert.Equal("somefile.txt", parts[1]);

            parts = "..\\folder\\somefile.txt".ToPathParts().ToList();
            Assert.Equal(3, parts.Count);
            Assert.Equal("..", parts[0]);
            Assert.Equal("folder", parts[1]);
            Assert.Equal("somefile.txt", parts[2]);

            parts = new string[] { "~/one/two", "..\\folder\\somefile.txt" }.ToPathParts().ToList();
            Assert.Equal(5, parts.Count);
            Assert.Equal("one", parts[0]);
            Assert.Equal("two", parts[1]);
            Assert.Equal("..", parts[2]);
            Assert.Equal("folder", parts[3]);
            Assert.Equal("somefile.txt", parts[4]);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void EnsureRangeTests()
        {
            Assert.Equal(10, 10.EnsureRange(0, 20));
            Assert.Equal(10, 10.EnsureRange(10, 20));
            Assert.Equal(15, 10.EnsureRange(15, 20));
            Assert.Equal(5, 10.EnsureRange(0, 5));
            Assert.Equal(-10, 10.EnsureRange(-20, -10));

            Assert.Equal(1.1, 0.9.EnsureRange(1.1, 1.9));
            Assert.Equal(1.2, 1.2.EnsureRange(1.1, 1.9));
            Assert.Equal(1.9, 2.7.EnsureRange(1.1, 1.9));

        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void Null_IsEqual_1()
        {
            string before = null;
            bool result = before.IsEqual(null);
            Assert.Equal(true, result);
        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void Null_IsEqual_2()
        {
            string before = null;
            bool result = before.IsEqual("");
            Assert.Equal(false, result);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void CaseDifference_IsEqual()
        {
            string before = "lower";
            bool result = before.IsEqual("LOWER");
            Assert.Equal(true, result);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void ContentDifference_IsEqual()
        {
            string before = "lower";
            bool result = before.IsEqual("UPPER");
            Assert.Equal(false, result);
        }


        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void Null_TrimSafe()
        {
            string before = null;
            string expected = null;
            string result = before.TrimSafe();
            Assert.Equal(expected, result);
        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void Empty_TrimSafe()
        {
            string before = "";
            string expected = "";
            string result = before.TrimSafe();
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void Padded_TrimSafe()
        {
            string before = " x ";
            string expected = "x";
            string result = before.TrimSafe();
            Assert.Equal(expected, result);
        }


        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void HasInterface_Simple()
        {
            var result = typeof(List<object>).HasInterface(typeof(IList));
            Assert.Equal(true, result);
        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void HasInterface_Simple_False()
        {
            var result = typeof(string).HasInterface(typeof(IList));
            Assert.Equal(false, result);
        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void HasInterface_Generic_Generic_Partial()
        {
            var result = typeof(HashSet<object>).HasInterface(typeof(ISet<>));
            Assert.Equal(true, result);
            
        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void HasInterface_Generic_Generic_FullyQualified()
        {
            var result = typeof(HashSet<object>).HasInterface(typeof(ISet<object>));
            Assert.Equal(true, result);
        }


        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void IsDefault_String_1()
        {
            Assert.Equal(false, "".IsDefault());
        }


        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void IsDefault_String_2()
        {
            Assert.Equal(true, ((string)null).IsDefault());
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void IsDefault_Int_1()
        {
            Assert.Equal(false, 1.IsDefault());
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void IsDefault_Int_2()
        {
            Assert.Equal(true, 0.IsDefault());
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void IsDefault_Enum_1()
        {
            Assert.Equal(false, TestEnum.Value1.IsDefault());
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void IsDefault_Enum_2()
        {
            Assert.Equal(true, TestEnum.Value0.IsDefault());
        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void IsDefault_Object_1()
        {
            TestObject t = new TestObject();
            Assert.Equal(false, t.IsDefault());
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void IsDefault_Object_2()
        {
            TestObject t = null;
            Assert.Equal(true, t.IsDefault());
        }



        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void Convert_Object_1()
        {
            TestObjectParent t = new TestObjectParent();
            TestObject x = t.Convert<TestObject, TestObjectParent>();
            Assert.NotNull(x);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        //[ExpectedException(typeof(InvalidCastException))]
        public void Convert_Object_2()
        {
            Assert.Throws<InvalidCastException>(() => {
                TestObject t = new TestObject();
                TestObjectParent x = t.Convert<TestObjectParent, TestObject>();
            });
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void Convert_int_1()
        {
            object input = (int)17;
            int output = input.Convert<int, object>();
            Assert.Equal(17, output);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void Convert_Complex_1()
        {
            var input = (object)new HashSet<int>();
            ISet<int> output = input.Convert<ISet<int>, object>();
            Assert.NotNull(output);
        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void ToQueryString()
        {

            Assert.Equal("text=Hello%2C%20I%27m%20writing%20%23text%20in%20a%20url%21%3F", (new { text = "Hello, I'm writing #text in a url!?" }).ToQueryString());
            Assert.Equal("text=http%3A%2F%2Fwww.voat.co%2Fv%2Fall%2Fnew%3Fpage%3D3%26show%3Dall", (new { text = "http://www.voat.co/v/all/new?page=3&show=all" }).ToQueryString());

            Assert.Equal("id=4", (new { id = 4 }).ToQueryString());
            Assert.Equal("id=1&id2=2&id3=3&id4=four", (new { id = 1, id2 = 2, id3 = "3", id4 = "four" }).ToQueryString());
            Assert.Equal("id=4&name=joe", (new { id = 4, name = "joe" }).ToQueryString());

            Assert.Equal("id=4", (new { id = 4, name = (string)null }).ToQueryString());
            Assert.Equal("id=4&name=", (new { id = 4, name = (string)null }).ToQueryString(true));

        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void TestEnumParsing()
        {

            Assert.Equal(TestSortEnum.Top, Voat.Common.Extensions.AssignIfValidEnumValue(-23223, TestSortEnum.Top));
            Assert.Equal(TestSortEnum.Top, Voat.Common.Extensions.AssignIfValidEnumValue(324324, TestSortEnum.Top));
            Assert.Equal(TestSortEnum.New, Voat.Common.Extensions.AssignIfValidEnumValue((int)TestSortEnum.New, TestSortEnum.Top));
            Assert.Equal(TestSortEnum.Top, Voat.Common.Extensions.AssignIfValidEnumValue(null, TestSortEnum.Top));
            Assert.Equal(TestSortEnum.Intensity, Voat.Common.Extensions.AssignIfValidEnumValue((int)TestSortEnum.Intensity, TestSortEnum.Top));

            Assert.False(Voat.Common.Extensions.IsValidEnumValue((TestSortEnum?)777));
            Assert.False(Voat.Common.Extensions.IsValidEnumValue((TestSortEnum?)null));
            Assert.True(Voat.Common.Extensions.IsValidEnumValue((TestSortEnum?)2));

            Assert.False(Voat.Common.Extensions.IsValidEnumValue((TestSortEnum)777));
            Assert.True(Voat.Common.Extensions.IsValidEnumValue((TestSortEnum)2));

        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        public void TestSafeEnum_Conversions()
        {
            var safeClass = new TestEnumClass();
            safeClass.EnumProperty = TestSortEnum.Top;

            Assert.Equal(TestSortEnum.Top,  safeClass.EnumProperty.Value);
            Assert.True(TestSortEnum.Top == safeClass.EnumProperty);
            Assert.True(safeClass.EnumProperty == TestSortEnum.Top);

            TestSortEnum x = TestSortEnum.New;
            x = safeClass.EnumProperty;
            Assert.Equal(TestSortEnum.Top, x);

            switch (safeClass.EnumProperty.Value)
            {
                case TestSortEnum.Top:
                    break;
                default:
                    throw new Exception("This is a problem");
                    break;
            }

            safeClass.EnumProperty = 4;
            Assert.Equal(TestSortEnum.Bottom, safeClass.EnumProperty.Value);

            safeClass.EnumProperty = "New";
            Assert.Equal(TestSortEnum.New, safeClass.EnumProperty.Value);

            safeClass.EnumProperty = "5";
            Assert.Equal(TestSortEnum.Intensity, safeClass.EnumProperty.Value);


        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        //[ExpectedException(typeof(TypeInitializationException))]
        public void TestSafeEnum_ConstructionError()
        {
            Assert.Throws<TypeInitializationException>(() => {
                var s = new SomeStruct();
                SafeEnum<SomeStruct> x = new SafeEnum<SomeStruct>(s);
            });

        }


        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        //[ExpectedException(typeof(TypeInitializationException))]
        public void TestSafeEnum_ConstructionError2()
        {
            Assert.Throws<TypeInitializationException>(() => {
                SafeEnum<int> x = new SafeEnum<int>(45);
            });

        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSafeEnum_Errors()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var safeClass = new TestEnumClass();
                safeClass.EnumProperty = ((TestSortEnum)(-203));
            });

        }
        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSafeEnum_Errors_2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var safeClass = new TestEnumClass();
                safeClass.EnumProperty = ((TestSortEnum)(203));
            });

        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSafeEnum_Errors_3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var safeClass = new TestEnumClass();
                safeClass.EnumProperty = 203;
            });

        }
        [Flags]
        public enum Numbers
        {
            One = 1,
            Two = 2,
            Three = One | Two,
            Four = 4,
            Five = Four | One,
            Six = Four | Two,
            Seven = Four | Two | One,
            Eight = 8,
            Nine = Eight | One,
            Ten = Eight | Two
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Extentions")]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Enum_Flag_Tests()
        {
            var check = new Action<IEnumerable<Numbers>, IEnumerable<Numbers>>((first, second) =>
            {
                Assert.Equal(first.Count(), second.Count());
                first.ToList().ForEach(x => {
                    Assert.True(second.Contains(x));
                });
            });

            var val = Numbers.Ten;
            var values = val.GetEnumFlags();
            check(values, new[] { Numbers.Eight, Numbers.Two });

            val = Numbers.Eight;
            values = val.GetEnumFlags();
            check(values, new[] { Numbers.Eight });

            val = Numbers.Seven;
            values = val.GetEnumFlags();
            check(values, new[] { Numbers.Four, Numbers.Two, Numbers.One });

            values = val.GetEnumFlagsIntersect(Numbers.Six);
            check(values, new[] { Numbers.Four, Numbers.Two});


        }
    }

    public class TestEnumClass
    {
        public SafeEnum<TestSortEnum> EnumProperty { get; set; }
    }
    public enum TestSortEnum
    {
        New = 1, //order by date
        Old, //order by date
        Top, //order by total upvotes
        Bottom, //order by most downvotes
        Intensity,
    }
    public class TestObject
    {

    }
    public class TestObjectParent : TestObject
    {

    }
    public struct SomeStruct : IConvertible
    {
        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
    public enum TestEnum
    {
        Value0,
        Value1,
        Value2
    }
}
