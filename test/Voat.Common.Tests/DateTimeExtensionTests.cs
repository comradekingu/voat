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
using System.Globalization;
using Voat.Common;
using Xunit;

namespace Voat.Common.Tests
{
    
    public class DateTimeExtensionTests 
    {
        private Calendar calendar = CultureInfo.InvariantCulture.Calendar;

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestEndOfDay()
        {
            var current = DateTime.UtcNow;
            var processed = current.ToEndOfDay();

            Assert.Equal(current.Year, processed.Year);
            Assert.Equal(current.Month, processed.Month);
            Assert.Equal(current.Day, processed.Day);
            Assert.Equal(23, processed.Hour);
            Assert.Equal(59, processed.Minute);
            Assert.Equal(59, processed.Second);
            Assert.Equal(999, processed.Millisecond);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestEndOfHour()
        {
            var current = DateTime.UtcNow;
            var processed = current.ToEndOfHour();

            Assert.Equal(current.Year, processed.Year);
            Assert.Equal(current.Month, processed.Month);
            Assert.Equal(current.Day, processed.Day);
            Assert.Equal(current.Hour, processed.Hour);
            Assert.Equal(59, processed.Minute);
            Assert.Equal(59, processed.Second);
            Assert.Equal(999, processed.Millisecond);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestEndOfMonth()
        {
            var current = DateTime.UtcNow;
            var processed = current.ToEndOfMonth();

            Assert.Equal(current.Year, processed.Year);
            Assert.Equal(current.Month, processed.Month);
            Assert.Equal(calendar.GetDaysInMonth(current.Year, current.Month), processed.Day);
            Assert.Equal(23, processed.Hour);
            Assert.Equal(59, processed.Minute);
            Assert.Equal(59, processed.Second);
            Assert.Equal(999, processed.Millisecond);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestEndOfWeek()
        {
            var current = DateTime.UtcNow;
            var processed = current.ToEndOfWeek();

            Assert.Equal(DayOfWeek.Saturday, calendar.GetDayOfWeek(processed));
            Assert.True(calendar.GetDayOfYear(current) <= calendar.GetDayOfYear(processed));
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestMonthRangeEquality()
        {
            //Quarter 1
            var current = new DateTime(2015, 12, 1, 13, 3, 21);
            var range = current.ToMonthRange();
            var start = range.Item1;
            var end = range.Item2;

            var current2 = new DateTime(2015, 12, 31, 2, 50, 21);
            var range2 = current2.ToMonthRange();
            var start2 = range.Item1;
            var end2 = range.Item2;

            Assert.Equal(start, start2);
            Assert.Equal(end, end2);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestQuarterRange()
        {
            //Quarter 1
            string desc = "Quarter 1";
            var current = new DateTime(2015, 2, 14, 13, 3, 21);
            var range = current.ToQuarterRange();
            var start = range.Item1;
            var end = range.Item2;

            //year
            Assert.Equal(start.Year, end.Year);
            //month
            Assert.Equal(1, start.Month);
            Assert.Equal(3, end.Month);
            //day
            Assert.Equal(1, start.Day);
            Assert.Equal(calendar.GetDaysInMonth(current.Year, end.Month), end.Day);

            Assert.Equal(0, start.Minute);
            Assert.Equal(0, start.Second);
            Assert.Equal(0, start.Millisecond);
            Assert.Equal(59, end.Minute);
            Assert.Equal(59, end.Second);
            Assert.Equal(999, end.Millisecond);

            desc = "Quarter 2";
            current = new DateTime(2015, 4, 1, 13, 3, 21);
            range = current.ToQuarterRange();
            start = range.Item1;
            end = range.Item2;

            //year
            Assert.Equal(start.Year, end.Year);
            //month
            Assert.Equal(4, start.Month);
            Assert.Equal(6, end.Month);
            //day
            Assert.Equal(1, start.Day);
            Assert.Equal(calendar.GetDaysInMonth(current.Year, end.Month), end.Day);

            Assert.Equal(0, start.Minute);
            Assert.Equal(0, start.Second);
            Assert.Equal(0, start.Millisecond);
            Assert.Equal(59, end.Minute);
            Assert.Equal(59, end.Second);
            Assert.Equal(999, end.Millisecond);

            desc = "Quarter 3";
            current = new DateTime(2015, 7, 30, 13, 3, 21);
            range = current.ToQuarterRange();
            start = range.Item1;
            end = range.Item2;

            //year
            Assert.Equal(start.Year, end.Year);
            //month
            Assert.Equal(7, start.Month);
            Assert.Equal(9, end.Month);
            //day
            Assert.Equal(1, start.Day);
            Assert.Equal(calendar.GetDaysInMonth(current.Year, end.Month), end.Day);

            Assert.Equal(0, start.Minute);
            Assert.Equal(0, start.Second);
            Assert.Equal(0, start.Millisecond);
            Assert.Equal(59, end.Minute);
            Assert.Equal(59, end.Second);
            Assert.Equal(999, end.Millisecond);

            desc = "Quarter 4";
            current = new DateTime(2015, 12, 31, 13, 3, 21);
            range = current.ToQuarterRange();
            start = range.Item1;
            end = range.Item2;

            //year
            Assert.Equal(start.Year, end.Year);
            //month
            Assert.Equal(10, start.Month);
            Assert.Equal(12, end.Month);
            //day
            Assert.Equal(1, start.Day);
            Assert.Equal(calendar.GetDaysInMonth(current.Year, end.Month), end.Day);

            Assert.Equal(0, start.Minute);
            Assert.Equal(0, start.Second);
            Assert.Equal(0, start.Millisecond);
            Assert.Equal(59, end.Minute);
            Assert.Equal(59, end.Second);
            Assert.Equal(999, end.Millisecond);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestQuarterRangeEquality()
        {
            //Quarter 1
            var current = new DateTime(2015, 2, 14, 13, 3, 21);
            var range = current.ToQuarterRange();
            var start = range.Item1;
            var end = range.Item2;

            var current2 = new DateTime(2015, 3, 1, 2, 50, 21);
            var range2 = current2.ToQuarterRange();
            var start2 = range.Item1;
            var end2 = range.Item2;

            Assert.Equal(start, start2);
            Assert.Equal(end, end2);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestQuarterRangeEquality2()
        {
            //Quarter 1
            var current = new DateTime(2015, 12, 14, 13, 3, 21);
            var range = current.ToQuarterRange();
            var start = range.Item1;
            var end = range.Item2;

            var current2 = new DateTime(2015, 10, 1, 2, 50, 21);
            var range2 = current2.ToQuarterRange();
            var start2 = range.Item1;
            var end2 = range.Item2;

            Assert.Equal(start, start2);
            Assert.Equal(end, end2);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestStartOfDay()
        {
            var current = DateTime.UtcNow;
            var processed = current.ToStartOfDay();

            Assert.Equal(current.Year, processed.Year);
            Assert.Equal(current.Month, processed.Month);
            Assert.Equal(current.Day, processed.Day);
            Assert.Equal(0, processed.Hour);
            Assert.Equal(0, processed.Minute);
            Assert.Equal(0, processed.Second);
            Assert.Equal(0, processed.Millisecond);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestStartOfHour()
        {
            var current = DateTime.UtcNow;
            var processed = current.ToStartOfHour();

            Assert.Equal(current.Year, processed.Year);
            Assert.Equal(current.Month, processed.Month);
            Assert.Equal(current.Day, processed.Day);
            Assert.Equal(current.Hour, processed.Hour);
            Assert.Equal(0, processed.Minute);
            Assert.Equal(0, processed.Second);
            Assert.Equal(0, processed.Millisecond);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestStartOfMonth()
        {
            var current = DateTime.UtcNow;
            var processed = current.ToStartOfMonth();

            Assert.Equal(current.Year, processed.Year);
            Assert.Equal(current.Month, processed.Month);
            Assert.Equal(1, processed.Day);
            Assert.Equal(0, processed.Hour);
            Assert.Equal(0, processed.Minute);
            Assert.Equal(0, processed.Second);
            Assert.Equal(0, processed.Millisecond);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestStartOfWeek()
        {
            var current = DateTime.UtcNow;
            var processed = current.ToStartOfWeek();

            Assert.Equal(DayOfWeek.Sunday, calendar.GetDayOfWeek(processed));
            Assert.True(calendar.GetDayOfYear(current) >= calendar.GetDayOfYear(processed) ||
                (
                    current.Year > processed.Year && current.Month == 1 && current.DayOfYear <= 6 //Add exclusion for new year
                ));
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestWeekRange()
        {
            var current = new DateTime(2015, 12, 14, 13, 3, 21);
            var processed = current.ToWeekRange();
            var start = processed.Item1;
            var end = processed.Item2;

            Assert.Equal(DayOfWeek.Sunday, calendar.GetDayOfWeek(start));
            Assert.Equal(DayOfWeek.Saturday, calendar.GetDayOfWeek(end));

            Assert.Equal(13, start.Day);
            Assert.Equal(19, end.Day);

            Assert.Equal(end.Month, start.Month);
            Assert.Equal(end.Year, start.Year);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_TestWeekRangeEquality()
        {
            //Quarter 1
            var current = new DateTime(2015, 12, 14, 13, 3, 21);
            var range = current.ToWeekRange();
            var start = range.Item1;
            var end = range.Item2;

            var current2 = new DateTime(2015, 12, 17, 2, 50, 21);
            var range2 = current2.ToWeekRange();
            var start2 = range.Item1;
            var end2 = range.Item2;

            Assert.Equal(start, start2);
            Assert.Equal(end, end2);
        }

        [Fact]
        [Trait("Category", "Utility"), Trait("Category", "Utility.DateTime")]
        public void DateTimeExt_RelavtiveRanges()
        {
            var current = DateTime.UtcNow;
            var range = current.ToRelativeRange(SortSpan.Hour);
            Assert.Equal(TimeSpan.FromHours(1), range.Item2 - range.Item1);

            range = current.ToRelativeRange(SortSpan.Day);
            Assert.Equal(TimeSpan.FromHours(24), range.Item2 - range.Item1);

            range = current.ToRelativeRange(SortSpan.Week);
            Assert.Equal(TimeSpan.FromDays(7), range.Item2 - range.Item1);

            range = current.ToRelativeRange(SortSpan.Month);
            Assert.Equal(TimeSpan.FromDays(30), range.Item2 - range.Item1);

            range = current.ToRelativeRange(SortSpan.Quarter);
            Assert.Equal(TimeSpan.FromDays(90), range.Item2 - range.Item1);

            range = current.ToRelativeRange(SortSpan.Year);
            Assert.Equal(TimeSpan.FromDays(365), range.Item2 - range.Item1);

        }
    }
}
