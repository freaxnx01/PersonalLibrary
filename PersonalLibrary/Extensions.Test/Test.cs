using System;
using NUnit.Framework;

namespace Extensions.Test
{
    [TestFixture]
    public class Test
    {
        #region StringExtensions
        [Test]
        public void TestIsNullOrEmpty()
        {
            string emptyString = "";
            Assert.IsTrue(emptyString.IsNullOrEmpty());

            emptyString = string.Empty;
            Assert.IsTrue(emptyString.IsNullOrEmpty());

            string nullString = null;
            Assert.IsTrue(nullString.IsNullOrEmpty());

            string guidString = Guid.NewGuid().ToString();
            Assert.IsFalse(guidString.IsNullOrEmpty());
        }
        #endregion

        #region SecureStringExtensions
        #endregion
    }
}
