using System;
using System.Data;
using Dapper;

namespace Web2bear.Dapper.Extensions.ExplicitMapping
{
    public class UnscpecifiedDateTimeHandler : SqlMapper.TypeHandler<DateTime>
    {
        private readonly TimeZoneInfo _unscpecifiedTimezone;

        public UnscpecifiedDateTimeHandler(TimeZoneInfo unscpecifiedTimezone)
        {
            _unscpecifiedTimezone = unscpecifiedTimezone;
        }

        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            parameter.Value = FixTimezone(value);
        }

        public override DateTime Parse(object value)
        {
            return FixTimezone((DateTime) value);
        }

        private DateTime FixTimezone(DateTime moment)
        {
            switch (moment.Kind)
            {
                case DateTimeKind.Utc:
                    break;
                case DateTimeKind.Local:
                    moment = ConvertMomentToUtc(moment, TimeZoneInfo.Local);
                    break;
                case DateTimeKind.Unspecified:
                    moment = ConvertMomentToUtc(moment, _unscpecifiedTimezone);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return moment;
        }

        private static DateTime ConvertMomentToUtc(DateTime moment, TimeZoneInfo sourceTimeZone)
        {
            var convertedMoment = (DateTime?) null;
            var shift = 0;
            do
            {
                try
                {
                    convertedMoment = TimeZoneInfo.ConvertTimeToUtc(moment.AddHours(shift), sourceTimeZone);
                }
                catch (ArgumentException)
                {
                    // Обход ошибки с "дырами во времени"
                    //The supplied DateTime represents an invalid time.
                    //For example, when the clock is adjusted forward, any time in the period that is skipped is invalid. Parameter name: dateTime
                    shift++;
                }
            } while (!convertedMoment.HasValue);

            return convertedMoment.Value;
        }
    }
}