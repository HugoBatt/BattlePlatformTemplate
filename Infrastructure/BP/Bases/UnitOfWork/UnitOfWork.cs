using Infrastructure.BP.Bases.Entity;
using Infrastructure.BP.Bases.GenericRepository;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BP.Bases.UnitOfWork;

using Migrations;

/// <inheritdoc/>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The context of the db.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// The dictionary that contains all instances of the repositories.
        /// </summary>
        private readonly Dictionary<Type, object> _repositories;

        /// <summary>
        /// Is the instance disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Logger instance
        /// </summary>
        private readonly ILogger<UnitOfWork> _logger;

        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Instantiate the Unit of work service.
        /// </summary>
        /// <param name="context">The context of the db.</param>
        /// <param name="logger">Logger instance.</param>
        public UnitOfWork(
            DataContext context, ILogger<UnitOfWork> logger)
        {
            this._logger = logger;
            this._context = context;
            this._repositories = new Dictionary<Type, object>();
            this._loggerFactory = new LoggerFactory();
        }

        /// <summary>
        /// Commit the changes into the db.
        /// </summary>
        /// <returns>The number of affected rows.</returns>
        public int Save()
        {
            var nbOfRows = this._context.SaveChanges();
            this._logger.LogInformation("{Count} rows affected", nbOfRows);
            return nbOfRows;
        }

        /// <inheritdoc cref="IDisposable" />
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity
        {
            var type = typeof(TEntity);
            if (_repositories.TryGetValue(type, out object value)) return (IGenericRepository<TEntity>)value;
            
            this._repositories[type] = new GenericRepository<TEntity>(this._context, this._loggerFactory.CreateLogger<GenericRepository<TEntity>>());
            this._logger.LogInformation("new Repository of {TEntity} created at {DT}", type, DateTime.UtcNow.ToLongTimeString());

            return (IGenericRepository<TEntity>)this._repositories[type];

        }
    }