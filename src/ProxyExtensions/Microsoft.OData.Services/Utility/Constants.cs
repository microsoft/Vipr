using System;

namespace Microsoft.OData.Services.Utility
{
    public class Constants
    {
        /**
         * UTF-8 Encoding name
         */
        public const string UTF8_NAME = "UTF-8";

        /**
         * UTF-8 Charset instance
         */
        //public const Charset UTF8 = Charset.forName(UTF8_NAME);

        /**
         * The constant SDK_VERSION.
         */
        public const String SDK_VERSION = "0.12.1";

        /**
         * The constant USER_AGENT_HEADER.
         */
        public const String USER_AGENT_HEADER = "User-Agent";

        /**
         * The constant TELEMETRY_HEADER.
         */
        public const String TELEMETRY_HEADER = "X-ClientService-ClientTag";

        /**
         * The constant CONTENT_TYPE_HEADER.
         */
        public const String CONTENT_TYPE_HEADER = "Content-Type";

        /**
         * The constant JSON_CONTENT_TYPE.
         */
        public const String JSON_CONTENT_TYPE = "application/json";

        /**
         * The constant MULTIPART_BOUNDARY_NAME.
         */
        public const String MULTIPART_BOUNDARY_NAME = "MultiPartBoundary";

        /**
         * The constant HTTP_NEW_LINE.
         */
        public const String HTTP_NEW_LINE = "\r\n";

        /**
         * The constant MULTIPART_CONTENT_TYPE.
         */
        public const String MULTIPART_CONTENT_TYPE = "multipart/form-data; boundary=" + MULTIPART_BOUNDARY_NAME;

        /**
         * The constant ACCEPT_HEADER.
         */
        public const String ACCEPT_HEADER = "Accept";

        /**
         * The constant IF_MATCH_HEADER.
         */
        public const String IF_MATCH_HEADER = "If-Match";

        /**
         * The constant ODATA_VERSION_HEADER.
         */
        public const String ODATA_VERSION_HEADER = "OData-Version";

        /**
         * The constant ODATA_VERSION.
         */
        public const String ODATA_VERSION = "4.0";

        /**
         * The constant ODATA_MAXVERSION_HEADER.
         */
        public const String ODATA_MAXVERSION_HEADER = "OData-MaxVersion";

        /**
         * The constant ODATA_MAXVERSION.
         */
        public const String ODATA_MAXVERSION = "4.0";

        /**
         * The constant ODATA_DATA_TYPE_JSON_PROPERTY
         */
        public const String ODATA_TYPE_JSON_PROPERTY = "@odata.type";

        /**
         * The constant ODATA_TYPE_PROPERTY_NAME
         */
        public const String ODATA_TYPE_PROPERTY_NAME = "$$__ODataType";

        /**
         * The constant PROPERTY_NAME_RESERVED_PREFIX
         */
        public const String PROPERTY_NAME_RESERVED_PREFIX = "$$__$$";

        /**
         * The constant ODATA_ENTITY_BASE_CLASS_NAME
         */
        public const String ODATA_ENTITY_BASE_CLASS_NAME = "ODataBaseEntity";

        /**
         * The constant MUST_STREAM_RESPONSE_CONTENT
         */
        public const String MUST_STREAM_RESPONSE_CONTENT = "MUST_STREAM_RESPONSE_CONTENT";
    }
}
