using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class IntervalDateTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void ComputeInvtervalStartDates()
        {
            // Assert
            Assert.AreEqual(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc), /*  */ IntervalDate.StartOfDay(new DateTime(2022, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfWeek(new DateTime(2022, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc), /**/ IntervalDate.StartOfMonth(new DateTime(2022, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfYear(new DateTime(2022, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            // start of interval
            Assert.AreEqual(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc), /*  */ IntervalDate.StartOfDay(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfWeek(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc), /**/ IntervalDate.StartOfMonth(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfYear(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc)));
            // 1st sec of interval
            Assert.AreEqual(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc), /*  */ IntervalDate.StartOfDay(new DateTime(2022, 02, 17, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfWeek(new DateTime(2022, 02, 14, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc), /**/ IntervalDate.StartOfMonth(new DateTime(2022, 02, 01, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfYear(new DateTime(2022, 01, 01, 00, 00, 01, DateTimeKind.Utc)));
            // last sec of interval
            Assert.AreEqual(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc), /*  */ IntervalDate.StartOfDay(new DateTime(2022, 02, 17, 23, 59, 59, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfWeek(new DateTime(2022, 02, 20, 23, 59, 59, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc), /**/ IntervalDate.StartOfMonth(new DateTime(2022, 02, 28, 23, 59, 59, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfYear(new DateTime(2022, 12, 31, 23, 59, 59, DateTimeKind.Utc)));

            // Last year
            Assert.AreEqual(new DateTime(2021, 02, 17, 00, 00, 00, DateTimeKind.Utc), /*  */ IntervalDate.StartOfDay(new DateTime(2021, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 15, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfWeek(new DateTime(2021, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 01, 00, 00, 00, DateTimeKind.Utc), /**/ IntervalDate.StartOfMonth(new DateTime(2021, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfYear(new DateTime(2021, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 17, 00, 00, 00, DateTimeKind.Utc), /*  */ IntervalDate.StartOfDay(new DateTime(2021, 02, 17, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 15, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfWeek(new DateTime(2021, 02, 15, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 01, 00, 00, 00, DateTimeKind.Utc), /**/ IntervalDate.StartOfMonth(new DateTime(2021, 02, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfYear(new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 17, 00, 00, 00, DateTimeKind.Utc), /*  */ IntervalDate.StartOfDay(new DateTime(2021, 02, 17, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 15, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfWeek(new DateTime(2021, 02, 15, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 01, 00, 00, 00, DateTimeKind.Utc), /**/ IntervalDate.StartOfMonth(new DateTime(2021, 02, 01, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfYear(new DateTime(2021, 01, 01, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 17, 00, 00, 00, DateTimeKind.Utc), /*  */ IntervalDate.StartOfDay(new DateTime(2021, 02, 17, 23, 59, 59, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 15, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfWeek(new DateTime(2021, 02, 21, 23, 59, 59, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 02, 01, 00, 00, 00, DateTimeKind.Utc), /**/ IntervalDate.StartOfMonth(new DateTime(2021, 02, 28, 23, 59, 59, DateTimeKind.Utc)));
            Assert.AreEqual(new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Utc), /* */ IntervalDate.StartOfYear(new DateTime(2021, 12, 31, 23, 59, 59, DateTimeKind.Utc)));
        }

        [TestMethod]
        [TestCategory("Tools")]
        public void Format_as_string()
        {
            // Assert
            Assert.AreEqual("2022-02-17", /*   */ IntervalDate.FormatAsDay(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-14", /*  */ IntervalDate.FormatAsWeek(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02", /*    */ IntervalDate.FormatAsMonth(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022", /*        */ IntervalDate.FormatAsYear(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-17", /*   */ IntervalDate.FormatAsDay(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-14", /*  */ IntervalDate.FormatAsWeek(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02", /*    */ IntervalDate.FormatAsMonth(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022", /*        */ IntervalDate.FormatAsYear(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-17", /*   */ IntervalDate.FormatAsDay(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-14", /*  */ IntervalDate.FormatAsWeek(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02", /*    */ IntervalDate.FormatAsMonth(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022", /*        */ IntervalDate.FormatAsYear(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-17", /*   */ IntervalDate.FormatAsDay(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-14", /*  */ IntervalDate.FormatAsWeek(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02", /*    */ IntervalDate.FormatAsMonth(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022", /*        */ IntervalDate.FormatAsYear(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc)));
        }

        [TestMethod]
        [TestCategory("Tools")]
        public void FormatStartOf()
        {
            // Assert
            Assert.AreEqual("2022-02-17", /*   */ IntervalDate.FormatStartOfDay(new DateTime(2022, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-14", /*  */ IntervalDate.FormatStartOfWeek(new DateTime(2022, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02", /*    */ IntervalDate.FormatStartOfMonth(new DateTime(2022, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual("2022", /*        */ IntervalDate.FormatStartOfYear(new DateTime(2022, 02, 17, 10, 44, 11, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-17", /*   */ IntervalDate.FormatStartOfDay(new DateTime(2022, 02, 17, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-14", /*  */ IntervalDate.FormatStartOfWeek(new DateTime(2022, 02, 14, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02", /*    */ IntervalDate.FormatStartOfMonth(new DateTime(2022, 02, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022", /*        */ IntervalDate.FormatStartOfYear(new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-17", /*   */ IntervalDate.FormatStartOfDay(new DateTime(2022, 02, 17, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-14", /*  */ IntervalDate.FormatStartOfWeek(new DateTime(2022, 02, 14, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02", /*    */ IntervalDate.FormatStartOfMonth(new DateTime(2022, 02, 01, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual("2022", /*        */ IntervalDate.FormatStartOfYear(new DateTime(2022, 01, 01, 00, 00, 01, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-17", /*   */ IntervalDate.FormatStartOfDay(new DateTime(2022, 02, 17, 23, 59, 59, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02-14", /*  */ IntervalDate.FormatStartOfWeek(new DateTime(2022, 02, 20, 23, 59, 59, DateTimeKind.Utc)));
            Assert.AreEqual("2022-02", /*    */ IntervalDate.FormatStartOfMonth(new DateTime(2022, 02, 28, 23, 59, 59, DateTimeKind.Utc)));
            Assert.AreEqual("2022", /*        */ IntervalDate.FormatStartOfYear(new DateTime(2022, 12, 31, 23, 59, 59, DateTimeKind.Utc)));
        }

    }
}
