using System;

namespace MinecraftModUpdater.Core.Exceptions
{
    /// <summary>
    /// The basic exception of Minecraft Mod Updater
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class MinecraftModUpdaterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinecraftModUpdaterException"/> class.
        /// </summary>
        public MinecraftModUpdaterException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinecraftModUpdaterException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MinecraftModUpdaterException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinecraftModUpdaterException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public MinecraftModUpdaterException(string message, Exception inner) : base(message, inner) { }
    }
}
