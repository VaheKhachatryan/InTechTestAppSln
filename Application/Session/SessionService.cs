using Application.Session.Models;
using Common.Caching.Abstraction;
using System;
using System.Threading.Tasks;

namespace Application.Session
{
	public class SessionService : ISessionService
	{
		private readonly IDistributedCacheManager _cacheManager;

		public SessionService(IDistributedCacheManager cacheManager)
		{
			_cacheManager = cacheManager;
		}

		public async Task<SessionResponseModel> CreateAsync(SessionModel model)
		{
			if (string.IsNullOrEmpty(model?.UserId) || string.IsNullOrEmpty(model?.UserName))
			{
				throw new Exception("The UserId and UserName are required.");
			}

			var sessionId = GenerateSessionId();
 
			if (await _cacheManager.KeyExistsAsync(sessionId))
			{
				throw new Exception("The User session is already created.");
			}

			await _cacheManager.SetAsync(sessionId, model, 20);

			return new SessionResponseModel { Session = sessionId };
		}

		public async Task<bool> ExistsAsync(string sessionId)
		{
			if (string.IsNullOrEmpty(sessionId))
			{
				throw new ArgumentNullException();
			}

			return await _cacheManager.KeyExistsAsync(sessionId);
		}

		public async Task<SessionModel> GetAsync(string sessionId)
		{
			if (string.IsNullOrEmpty(sessionId))
			{
				throw new ArgumentNullException();
			}

			return await _cacheManager.GetAsync<SessionModel>(sessionId);
		}

		public async Task RefreshExpiryByKeyAsync(string sessionId, int? cacheTime = null)
		{
			if (string.IsNullOrEmpty(sessionId) || cacheTime is null)
			{
				throw new ArgumentNullException();
			}

			await _cacheManager.RefreshExpiryByKeyAsync(sessionId, cacheTime);
		}

		private string GenerateSessionId()
		{
			return Guid.NewGuid().ToString();
		}
	}
}