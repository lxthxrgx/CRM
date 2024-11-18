using NUnit.Framework;
using SRMAgreement.SuppCode;

namespace TestsCRM
{
    public class DeleteSpaceTests
    {
        [Test]
        public void Deletespace_RemovesSpacesFromBothEnds()
        {
            // Arrange
            var input = "   test string   ";
            var expected = "test string";

            // Act
            var result = DeleteSpace.Deletespace(input);

            // Assert
            Assert.AreEqual(expected, result, "The method should remove spaces from both ends of the string.");
        }

        [Test]
        public void Deletespace_DeleteTabBetweenStrings()
        {
            // Arrange
            var input = "qwe \r\n \r\n asd \"zxc\"";

            var expected = "qwe asd zxc";

            // Act
            var result = DeleteSpace.Deletespace(input);

            // Assert
            Assert.AreEqual(expected, result, "The method should remove spaces from both ends of the string.");
        }

        [Test]
        public void Deletespace_ReturnsOriginalString_IfNoSpaces()
        {
            // Arrange
            var input = "teststring";
            var expected = "teststring";

            // Act
            var result = DeleteSpace.Deletespace(input);

            // Assert
            Assert.AreEqual(expected, result, "The method should return the same string if no spaces are present.");
        }

        [Test]
        public void Deletespace_ReturnsEmptyString_ForEmptyInput()
        {
            // Arrange
            var input = "   ";
            var expected = string.Empty;

            // Act
            var result = DeleteSpace.Deletespace(input);

            // Assert
            Assert.AreEqual(expected, result, "The method should return an empty string if the input is only spaces.");
        }

        [Test]
        public void Deletespace_ReturnsNull_ForNullInput()
        {
            // Arrange
            string input = null;

            // Act
            var result = DeleteSpace.Deletespace(input);

            // Assert
            Assert.IsNull(result, "The method should return null when the input is null.");
        }

        [Test]
        public void Deletespace_DoesNotThrowException_ForValidInput()
        {
            // Arrange
            var input = " test ";

            // Act & Assert
            Assert.DoesNotThrow(() => DeleteSpace.Deletespace(input), "The method should not throw an exception for valid input.");
        }
    }
}
