using System;
using System.Collections.Generic;
using System.Text;

namespace ADO.NET.Settings.DataWrapper
{
    /// <summary>
    /// Constant string references used for Data Access Wrapper Library.
    /// </summary>
    public static class DataAccessWrapperConstants
    {
        #region Public Constants

        /// <summary>
        /// The default connection string variable name through which the Connection should be initialized.
        /// </summary>
        public const string DEFAULT_CONNECTION_STRING_NAME = "";

        #endregion Public Constants

        #region Public Properties

        /// <summary>
        /// Exception message for missing connection string.
        /// </summary>
        public const string EXCEPTION_MESSAGE_CONNECTION_STRING_MISSING =
            "No connection string with name: {0} found in configuration file.";

        /// <summary>
        /// Exception message for connection creation failure.
        /// </summary>
        public const string EXCEPTION_MESSAGE_CANNOT_CREATE_CONNECTION =
            "Cannot create SQL Server connection using the specified Connection String specification.";

        /// <summary>
        /// Exception message for command creation failure.
        /// </summary>
        public const string EXCEPTION_MESSAGE_CANNOT_CREATE_COMMAND =
            "Cannot create SqlCommand. SqlCommand creation requires initialized connection with connection string.";

        /// <summary>
        /// Exception message for opening connection failure.
        /// </summary>
        public const string EXCEPTION_MESSAGE_CANNOT_OPEN_CONNECTION =
                "Cannot open a connection to SQL Server. Opening connection to SQL Server requires initialized connection with connection string.";

        /// <summary>
        /// Exception message for requiring initialized connection.
        /// </summary>
        public const string EXCEPTION_MESSAGE_REQUIRES_INITIALIZED_CONNECTION = "Requires an initialized connection.";

        /// <summary>
        /// Exception message for adding parameter failure.
        /// </summary>
        public const string EXCEPTION_MESSAGE_ADD_PARAMETER_FAILURE =
            "Cannot add parameter. Requires an initialized connection.";

        #endregion Public Properties
    }
}
