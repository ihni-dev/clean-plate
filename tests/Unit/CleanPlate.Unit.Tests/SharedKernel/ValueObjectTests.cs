using CleanPlate.SharedKernel.ValueObject;
using System;
using Xunit;

namespace CleanPlate.Unit.Tests.SharedKernel
{
    // source: https://github.com/jhewlett/ValueObject
    public class TestValue : ValueObject
    {
        public TestValue() { }

        public TestValue(int nonPublicValue)
        {
            ProtectedProperty = nonPublicValue;
            PrivateProperty = nonPublicValue;
            privateField = nonPublicValue;
            protectedField = nonPublicValue;
        }

        public string Property1 { get; set; }
        public int Property2 { get; set; }
        public int Field;
        protected int protectedField;
        private int privateField;

        protected int ProtectedProperty { get; set; }
        private int PrivateProperty { get; set; }
    }

    public class ValueObjectTests
    {
        [Fact]
        public void Equals_NullIsConsideredEqual()
        {
            var value1 = new TestValue();
            var value2 = new TestValue();

            AssertEqual(value1, value2);
        }

        [Fact]
        public void Equals_OnlyOneValueIsNull_DoesNotThrow_NotEqual()
        {
            var value1 = new TestValue();
            var value2 = new TestValue { Property1 = "value" };

            AssertNotEqual(value1, value2);
        }

        [Fact]
        public void Equals_ComparesAllPropertiesAndFields_Equal()
        {
            var value1 = new TestValue { Property1 = "test", Property2 = 10, Field = 3 };
            var value2 = new TestValue { Property1 = "test", Property2 = 10, Field = 3 };

            AssertEqual(value1, value2);
        }

        [Fact]
        public void Equals_ComparesAllPropertiesAndFields_PropertyDifferent_NotEqual()
        {
            var value1 = new TestValue { Property1 = "test", Property2 = 10 };
            var value2 = new TestValue { Property1 = "Test", Property2 = 10 };

            AssertNotEqual(value1, value2);
        }

        [Fact]
        public void Equals_ComparesAllPropertiesAndFields_FieldDifferent_NotEqual()
        {
            var value1 = new TestValue { Property1 = "test", Property2 = 10, Field = 8 };
            var value2 = new TestValue { Property1 = "test", Property2 = 10, Field = 9 };

            AssertNotEqual(value1, value2);
        }

        [Fact]
        public void Equals_IgnoresPrivatePropertiesAndFields()
        {
            var value1 = new TestValue(5);
            var value2 = new TestValue(8);

            AssertEqual(value1, value2);
        }

        [Fact]
        public void Equals_ComparingWithNull_ReturnsFalse()
        {
            var value = new TestValue { Property1 = "string" };

            Assert.False(value.Equals(null as object));
        }

        [Fact]
        public void Equals_ComparingWithWrongType_ReturnsFalse()
        {
            var value = new TestValue { Property1 = "string" };

            Assert.False(value.Equals(10));
        }

        [Fact]
        public void OperatorEquals_LeftSideNull_ReturnsFalse()
        {
            var value = new TestValue();

            Assert.False(null == value);
        }

        [Fact]
        public void OperatorEquals_RightSideNull_ReturnsFalse()
        {
            var value = new TestValue();

            Assert.False(value == null);
        }

        [Fact]
        public void OperatorEquals_BothValuesNull_ReturnsTrue()
        {
            Assert.True((TestValue)null == (TestValue)null);
        }

        [Fact]
        public void OperatorNotEquals_LeftSideNull_ReturnsTrue()
        {
            var value = new TestValue();

            Assert.True(null != value);
        }

        [Fact]
        public void OperatorNotEquals_RightSideNull_ReturnsTrue()
        {
            var value = new TestValue();

            Assert.True(value != null);
        }

        [Fact]
        public void OperatorNotEquals_BothValuesNull_ReturnsFalse()
        {
            Assert.False((TestValue)null != (TestValue)null);
        }

        [Fact]
        public void ImplementsIEquatable()
        {
            var value = new TestValue();

            Assert.IsAssignableFrom<IEquatable<ValueObject>>(value);
        }

        [Fact]
        public void GetHashCode_AlwaysEqualForEqualObjects()
        {
            var value1 = new TestValue { Property1 = "string", Property2 = 4 };
            var value2 = new TestValue { Property1 = "string", Property2 = 4 };

            Assert.Equal(value1.GetHashCode(), value2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_NotEqualForDistinctObjects_1()
        {
            var value1 = new TestValue { Property1 = "string", Property2 = 4 };
            var value2 = new TestValue { Property1 = "string", Property2 = 5 };

            Assert.NotEqual(value1.GetHashCode(), value2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_NotEqualForDistinctObjects_2()
        {
            var value1 = new TestValue { Property1 = "string", Property2 = 4 };
            var value2 = new TestValue { Property1 = "String", Property2 = 4 };

            Assert.NotEqual(value1.GetHashCode(), value2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_HandlesNull()
        {
            var value1 = new TestValue { Property2 = 2 };
            var value2 = new TestValue { Property2 = 5 };

            Assert.NotEqual(value1.GetHashCode(), value2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ConsidersPublicFields()
        {
            var value1 = new TestValue { Property2 = 2 };
            var value2 = new TestValue { Property2 = 2, Field = 4 };

            Assert.NotEqual(value1.GetHashCode(), value2.GetHashCode());
        }

        private void AssertEqual(TestValue value1, TestValue value2)
        {
            Assert.Equal(value1, value2);
            Assert.True(value1 == value2);
            Assert.False(value1 != value2);
            Assert.True(value1.Equals(value2));
        }

        private void AssertNotEqual(TestValue value1, TestValue value2)
        {
            Assert.NotEqual(value1, value2);
            Assert.True(value1 != value2);
            Assert.False(value1 == value2);
            Assert.False(value1.Equals(value2));
        }

        class Recursive : ValueObject
        {
            public Recursive Recurse { get; set; }
            public string Terminal;
        }

        [Fact]
        public void Nesting()
        {
            var value = new Recursive();
            var value2 = new Recursive();
            var nestedValue = new Recursive() { Terminal = "test" };
            var nestedValue2 = new Recursive() { Terminal = "test" };

            value.Recurse = nestedValue;
            value2.Recurse = nestedValue2;

            Assert.True(value.Equals(value2));
            Assert.Equal(value.GetHashCode(), value2.GetHashCode());
        }

        class Ignore : ValueObject
        {
            [IgnoreMember]
            public int Ignored { get; set; }
            [IgnoreMember]
            public int IgnoredField;
            public int Considered { get; set; }
        }

        [Fact]
        public void IgnoreMember_Property_DoesNotConsider()
        {
            var value1 = new Ignore { Ignored = 2, Considered = 4 };
            var value2 = new Ignore { Ignored = 3, Considered = 4 };

            Assert.True(value1.Equals(value2));
        }

        [Fact]
        public void IgnoreMember_Field_DoesNotConsider()
        {
            var value1 = new Ignore { IgnoredField = 3, Considered = 4 };
            var value2 = new Ignore { IgnoredField = 2, Considered = 4 };

            Assert.True(value1.Equals(value2));
        }

        class MyValue : ValueObject
        {
            public int Num;
        }

        class MyValue2 : MyValue
        {
        }

        [Fact]
        public void ObjectsOfDifferentTypeNotEqual_EvenIfSubclass()
        {
            var value1 = new MyValue();
            var value2 = new MyValue2();

            Assert.False(value1.Equals(value2));
        }
    }
}
