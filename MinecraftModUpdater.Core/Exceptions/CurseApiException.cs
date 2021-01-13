using System;

namespace MinecraftModUpdater.Core.Exceptions
{
    /// <summary>
    /// An exception for Curse Api
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CurseApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurseApiException"/> class.
        /// </summary>
        public CurseApiException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurseApiException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CurseApiException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurseApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public CurseApiException(string message, Exception inner) : base(message, inner) { }
    }
}
