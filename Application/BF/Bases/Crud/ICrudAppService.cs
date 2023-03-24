using System.Linq.Expressions;
using Application.BF.Bases.Service;
using Common.BF.Bases;
using Domain.BF.Bases.Mapper;
using Infrastructure.BF.Bases;

namespace Application.BF.Bases.Crud;

    /// <summary>
    /// Generic crud service.
    /// You can either instantiate if you want to use a basic crud, or inherit it and override some / all of its methods to add custom behaviours.
    /// </summary>
    /// <typeparam name="TEntity">The entity fetched by your service (aka table in db).</typeparam>
    /// <typeparam name="TDto">The DTO returned by the service to the controller that call it.</typeparam>
    /// <typeparam name="TMapper">The mapper that convert the entity to the specified DTO.</typeparam>
    public interface ICrudAppService<TEntity, TDto, TMapper> : IAppService 
        where TEntity : BaseEntity 
        where TDto : BaseDto
        where TMapper : BaseMapper<TDto, TEntity>
    {

        /// <summary>
        /// Fetch a list of <see cref="TEntity"/> from custom filter, a specific order and some of its properties.
        /// Be sure to read the doc in microsoft website, i'm not totally sure how it works.
        /// </summary>
        /// <param name="filter">Linq function that expresses a filter, applied on the db</param>
        /// <param name="orderBy">Linq function that expresses an order, applied to the return of the request.</param>
        /// <param name="includeProperties">string containing all properties.</param>
        /// <returns>A <see cref="IEnumerable{TDTO}"/>.</returns>
        public Task<IEnumerable<TDto>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Fetch all <see cref="TEntity"/>.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{TDTO}"/>.</returns>
        public Task<IEnumerable<TDto>> GetAllAsync();

        /// <summary>
        /// Fetch a <see cref="TEntity"/> of the specified Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="TEntity"/> needed.</param>
        /// <returns>A <see cref="TDto"/>.</returns>
        public Task<TDto?> GetByIdAsync(int id);

        /// <summary>
        /// Insert a <see cref="TEntity"/> of the specified <see cref="TDto"/>.
        /// </summary>
        /// <param name="dto">A <see cref="TDto"/> containing the data of the <see cref="TEntity"/> to be inserted.</param>
        /// <returns>An empty <see cref="Task"/>.</returns>
        public TDto Insert(TDto dto);

        /// <summary>
        /// Delete a <see cref="TEntity"/> of the specified Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="TEntity"/> to be deleted.</param>
        /// <returns>A <see cref="TDto"/>.</returns>
        public void Delete(int id);

        /// <summary>
        /// Delete a <see cref="TEntity"/> of the specified <see cref="TDto"/>.
        /// </summary>
        /// <param name="toDelete">A <see cref="TDto"/> containing the data of the <see cref="TEntity"/> to be deleted.</param>
        /// <returns>An empty <see cref="Task"/>.</returns>
        public void Delete(TDto toDelete);

        /// <summary>
        /// Update a <see cref="TEntity"/> of with specified <see cref="TDto"/> information.
        /// </summary>
        /// <param name="toUpdate">A <see cref="TDto"/> containing the data of the <see cref="TEntity"/> to be updated.</param>
        /// <returns>An empty <see cref="Task"/>.</returns>
        public TDto Update(TDto toUpdate);
    }