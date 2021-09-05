using Application.Session.Models;
using System.Threading.Tasks;

namespace Application.Session
{
	public interface ISessionService
	{
		Task<SessionResponseModel> CreateAsync(SessionModel model);
		Task<bool> ExistsAsync(string sessionId);
		Task RefreshExpiryByKeyAsync(string sessionId, int? cacheTime = null);
		Task<SessionModel> GetAsync(string sessionId);
	}
}