using Microsoft.Extensions.Logging;

namespace Writings.Application.Extensions
{
    public static partial class LoggerExtensions
    {
        #region WritingLogs

        [LoggerMessage(Level = LogLevel.Information, EventId = 1, Message = "Writing id {WritingId} created by user id {UserId}")]
        public static partial void LogWritingCreation(this ILogger logger, Guid writingId, Guid? userId);

        [LoggerMessage(Level = LogLevel.Information, EventId = 2, Message = "Writing id {WritingId} updated by user id {UserId}")]
        public static partial void LogWritingUpdate(this ILogger logger, Guid writingId, Guid? userId);

        [LoggerMessage(Level = LogLevel.Information, EventId = 3, Message = "Writing id {WritingId} deleted by user id {UserId}")]
        public static partial void LogWritingDeletion(this ILogger logger, Guid writingId, Guid? userId);

        [LoggerMessage(Level = LogLevel.Error, EventId = 6, Message = "Failed to create writing. Exception: {Exception}")]
        public static partial void LogWritingCreationFailure(this ILogger logger, string exception);

        [LoggerMessage(Level = LogLevel.Error, EventId = 7, Message = "Failed to update writing. Exception: {Exception}")]
        public static partial void LogWritingUpdateFailure(this ILogger logger, string exception);

        [LoggerMessage(Level = LogLevel.Error, EventId = 8, Message = "Failed to delete writing. Exception: {Exception}")]
        public static partial void LogWritingDeletionFailure(this ILogger logger, string exception);

        #endregion

        #region TagLogs

        [LoggerMessage(Level = LogLevel.Information, EventId = 4, Message = "Tag id {TagId} created by user id {UserId}")]
        public static partial void LogTagCreation(this ILogger logger, Guid tagId, Guid? userId);

        [LoggerMessage(Level = LogLevel.Information, EventId = 5, Message = "Tag id {TagId} deleted by user id {UserId}")]
        public static partial void LogTagDeletion(this ILogger logger, Guid tagId, Guid? userId);

        [LoggerMessage(Level = LogLevel.Error, EventId = 9, Message = "Failed to create tag. Exception: {Exception}")]
        public static partial void LogTagCreationFailure(this ILogger logger, string exception);

        [LoggerMessage(Level = LogLevel.Error, EventId = 10, Message = "Failed to delete tag. Exception: {Exception}")]
        public static partial void LogTagDeletionFailure(this ILogger logger, string exception);

        #endregion
    }
}
