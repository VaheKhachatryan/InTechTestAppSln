using Application.Session;
using Application.Session.Models;
using Common.Infrastructure;
using InTechWebApp.Hubs.Models;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTechWebApp.Hubs
{
	[HubName("ws")]
	public class InTechHub : Hub
	{
		private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
		private readonly ISessionService _sessionService;

		public InTechHub()
		{
			_sessionService = EngineContext.Current.Resolve<ISessionService>();
		}

		public async Task Ping()
		{
			await Clients.Caller.SendAsync("Pong", Context.ConnectionId);
		}

		public override async Task OnConnectedAsync()
		{
			try
			{
				var sessionId = Context.GetHttpContext().Request?.Query["sessionId"].ToString();

				if (!(await _sessionService.ExistsAsync(sessionId)))
				{
					await Clients.Caller.SendAsync("ErrorHandler", new HubResponseModel
					{
						Statuscode = HubResponseStatus.SessionExpired,
						Message = "The session Id is expired"
					});

					Context.Abort();
					return;
				}

				string connectionId = Context.ConnectionId;
				var userInfo = await _sessionService.GetAsync(sessionId);

				var key = $"{userInfo.UserId}-{userInfo.UserName}";
				var oldConnectionIds = _connections.GetConnections(key);

				foreach (var id in oldConnectionIds)
				{
					await Clients.Client(id).SendAsync("ForceStopConnection", id);
					_connections.Remove(key, id);
				}

				_connections.Add(key, connectionId);

				await base.OnConnectedAsync();
				return;
			}
			catch (Exception ex)
			{
				await Clients.Caller.SendAsync("ErrorHandler", new HubResponseModel
				{
					Statuscode = HubResponseStatus.UnknownError,
					Message = "Unknown Error"
				});

				Context.Abort();

				return;
			}
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var sessionId = Context.GetHttpContext().Request?.Query["sessionId"].ToString();

			await _sessionService.RefreshExpiryByKeyAsync(sessionId, 15);

			await base.OnDisconnectedAsync(exception);
		}
	}
}