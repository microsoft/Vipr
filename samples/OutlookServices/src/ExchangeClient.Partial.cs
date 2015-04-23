using System;
using System.Reflection;

namespace Microsoft.Office365.OutlookServices
{
    partial interface IUserFetcher
    {
        global::Microsoft.Office365.OutlookServices.IEventCollection GetCalendarView(
            global::System.DateTimeOffset startDateTime, global::System.DateTimeOffset endDateTime);
    }

    partial class UserFetcher
    {
        private const string DateTimeOffsetZuluFormat = "yyyy-MM-ddTHH:mm:ssZ";

        // tag0005
        /// <summary>
        /// There are no comments for CalendarView in the schema.
        /// </summary>
        public IEventCollection GetCalendarView(global::System.DateTimeOffset startDateTime, global::System.DateTimeOffset endDateTime)
        {
            var cvPath = GetPath("CalendarView");
            if (cvPath == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            var query = Context.CreateQuery<Event>(cvPath, true);

            query = query.AddQueryOption("startDateTime", startDateTime.ToString(DateTimeOffsetZuluFormat));

            query = query.AddQueryOption("endDateTime", endDateTime.ToString(DateTimeOffsetZuluFormat));

            return new EventCollection(query, Context, this, cvPath);
        }
    }

    partial class User
    {
        private const string DateTimeOffsetZuluFormat = "yyyy-MM-ddTHH:mm:ssZ";

        // tag0005
        /// <summary>
        /// There are no comments for CalendarView in the schema.
        /// </summary>
        public IEventCollection GetCalendarView(global::System.DateTimeOffset startDateTime, global::System.DateTimeOffset endDateTime)
        {
            var cvPath = GetPath("CalendarView");
            if (cvPath == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            var query = Context.CreateQuery<Event>(cvPath, true);

            query = query.AddQueryOption("startDateTime", startDateTime.ToString(DateTimeOffsetZuluFormat));

            query = query.AddQueryOption("endDateTime", endDateTime.ToString(DateTimeOffsetZuluFormat));

            return new EventCollection(query, Context, this, cvPath);
        }
    }

    partial interface ICalendarFetcher
    {
        global::Microsoft.Office365.OutlookServices.IEventCollection GetCalendarView(
            global::System.DateTimeOffset startDateTime, global::System.DateTimeOffset endDateTime);
    }

    partial class CalendarFetcher
    {
        private const string DateTimeOffsetZuluFormat = "yyyy-MM-ddTHH:mm:ssZ";

        // tag0005
        /// <summary>
        /// There are no comments for CalendarView in the schema.
        /// </summary>
        public IEventCollection GetCalendarView(global::System.DateTimeOffset startDateTime, global::System.DateTimeOffset endDateTime)
        {
            var cvPath = GetPath("CalendarView");
            if (cvPath == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            var query = Context.CreateQuery<Event>(cvPath, true);

            query = query.AddQueryOption("startDateTime", startDateTime.ToString(DateTimeOffsetZuluFormat));

            query = query.AddQueryOption("endDateTime", endDateTime.ToString(DateTimeOffsetZuluFormat));

            return new EventCollection(query, Context, this, cvPath);
        }
    }

    partial class Calendar
    {
        private const string DateTimeOffsetZuluFormat = "yyyy-MM-ddTHH:mm:ssZ";

        // tag0005
        /// <summary>
        /// There are no comments for CalendarView in the schema.
        /// </summary>
        public IEventCollection GetCalendarView(global::System.DateTimeOffset startDateTime, global::System.DateTimeOffset endDateTime)
        {
            var cvPath = GetPath("CalendarView");
            if (cvPath == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            var query = Context.CreateQuery<Event>(cvPath, true);

            query = query.AddQueryOption("startDateTime", startDateTime.ToString(DateTimeOffsetZuluFormat));

            query = query.AddQueryOption("endDateTime", endDateTime.ToString(DateTimeOffsetZuluFormat));

            return new EventCollection(query, Context, this, "Me/Calendar/CalendarView");
        }

    }

    public partial interface IEventFetcher
    {
        IEventCollection GetInstances(global::System.DateTimeOffset startDateTime, global::System.DateTimeOffset endDateTime);
    }

    internal partial class EventFetcher
    {
        private const string DateTimeOffsetZuluFormat = "yyyy-MM-ddTHH:mm:ssZ";

        // tag0005
        /// <summary>
        /// There are no comments for Instances in the schema.
        /// </summary>
        public IEventCollection GetInstances(global::System.DateTimeOffset startDateTime, global::System.DateTimeOffset endDateTime)
        {
            var path = GetPath("Instances");
            if (path == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            var query = Context.CreateQuery<Event>(path, true);

            query = query.AddQueryOption("startDateTime", startDateTime.ToString(DateTimeOffsetZuluFormat));

            query = query.AddQueryOption("endDateTime", endDateTime.ToString(DateTimeOffsetZuluFormat));

            return new EventCollection(query, Context, this, path);
        }
    }

    public partial class Event
    {
        private const string DateTimeOffsetZuluFormat = "yyyy-MM-ddTHH:mm:ssZ";

        // tag0005
        /// <summary>
        /// There are no comments for InstancesAsync in the schema.
        /// </summary>
        public IEventCollection GetInstances(global::System.DateTimeOffset startDateTime, global::System.DateTimeOffset endDateTime)
        {
            var path = GetPath("Instances");
            if (path == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            var query = Context.CreateQuery<Event>(path, true);

            query = query.AddQueryOption("startDateTime", startDateTime.ToString(DateTimeOffsetZuluFormat));

            query = query.AddQueryOption("endDateTime", endDateTime.ToString(DateTimeOffsetZuluFormat));

            return new EventCollection(query, Context, this, path);
        }
    }

    public partial class OutlookServicesClient
    {
        partial void OnContextCreated()
        {
            Context.BuildingRequest += (sender, args) => args.Headers.Add("X-ClientService-ClientTag", String.Format("Office 365 API Tools {0}", "1.0.33.0"));
        }
    }
}
