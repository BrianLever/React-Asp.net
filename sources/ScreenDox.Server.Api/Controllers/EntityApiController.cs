using FrontDesk.Common.Messages;

using System.Web.Http;
using System.Web.Http.Results;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Base api controller
    /// </summary>
    /// <typeparam name="TDomainModel">Domain model type</typeparam>
    /// <typeparam name="TRequestModel">Reduced request model type used for Add and Update operations</typeparam>
    /// <typeparam name="TEnityID">Type of resource ID</typeparam>
    public abstract class EntityApiController<TDomainModel, TRequestModel, TEnityID> : ApiControllerWithValidationBase<TRequestModel>
        where TDomainModel : class
        where TRequestModel : class
        where TEnityID : struct
    {

        /// <summary>
        /// Resource named that is used for response messages
        /// </summary>
        protected abstract string ResourceName {get;}

        [HttpGet]
        public abstract TDomainModel Get([FromUri] TEnityID id);

        /// <summary>
        /// Add new resource
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>Status 201 and created entity if operation was successful</returns>
        protected abstract TDomainModel AddResource(TRequestModel requestModel);

        /// <summary>
        /// Post action to create new resource
        /// </summary>
        [HttpPost]
        public virtual IHttpActionResult Add(TRequestModel requestModel)
        {
            return new NegotiatedContentResult<TDomainModel>(
                System.Net.HttpStatusCode.Created,
                AddResource(requestModel),
                this);
        }

        /// <summary>
        /// Update operation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestModel"></param>
        [HttpPut]
        public abstract IHttpActionResult Update([FromUri] TEnityID id, [FromBody] TRequestModel requestModel);


        /// <summary>
        /// Return action result for successful update operation
        /// </summary>
        /// <returns></returns>
        protected IHttpActionResult GetSuccessfulUpdateResult()
        {
            return AcceptedContentResult<string>(CustomMessage.GetUpdateMessage(ResourceName));
        }

        /// <summary>
        /// Delete operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public abstract IHttpActionResult Delete([FromUri] TEnityID id);
    }
}