// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Services
{
    using System;

    /// <summary>
    /// The application service provider.
    /// </summary>
    internal interface IAppServiceProvider : IServiceProvider
    {
        /// <summary>
        /// The get service.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of service.
        /// </typeparam>
        /// <returns>
        /// Instance of the service.
        /// </returns>
        TService GetService<TService>();

        /// <summary>
        /// The get service.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of service.
        /// </typeparam>
        /// <typeparam name="TServiceImpl">
        /// The real service implementataion.
        /// </typeparam>
        /// <returns>
        /// Instance of the service.
        /// </returns>
        TServiceImpl GetService<TService, TServiceImpl>();
    }
}