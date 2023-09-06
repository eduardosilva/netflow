using AutoMapper;
using Netflow.Infrastructure.Databases;
using Microsoft.AspNetCore.Mvc;

namespace Netflow.Controllers;
/// <summary>
/// Represents an abstract base controller class for the application with common functionality.
/// </summary>
public abstract class AppBaseController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppBaseController"/> class.
    /// </summary>
    protected AppBaseController() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBaseController"/> class with a data context.
    /// </summary>
    /// <param name="dataContext">The data context for accessing the database.</param>
    protected AppBaseController(DataContext dataContext)
        : this(dataContext, mapper: null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBaseController"/> class with a mapper.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    protected AppBaseController(IMapper mapper)
        : this(dataContext: null, mapper)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBaseController"/> class with a data context and a mapper.
    /// </summary>
    /// <param name="dataContext">The data context for accessing the database.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    protected AppBaseController(DataContext dataContext, IMapper mapper)
    {
        DataContext = dataContext;
        Mapper = mapper;
    }

    /// <summary>
    /// Gets the data context for accessing the database.
    /// </summary>
    public DataContext DataContext { get; }

    /// <summary>
    /// Gets the AutoMapper instance for object mapping.
    /// </summary>
    public IMapper Mapper { get; }
}
