class InTechSignalRClient {
	constructor() {
		this._init();
	}
	_init = () => {
		var _sessionId;
		var sessionExpiryDuration = 900;
		var inTechHubConnection = null;

		$("#createSessionForm").validate();

		$("#createSessionForm").submit(function (e) {
			e.preventDefault();
			var form = $(this);
			if (form.valid()) {

				var url = "Session/Create";
				var data = {
					userId: $("#userId").val(),
					userName: $("#userName").val()
				}

				$.ajax({
					type: "POST",
					dataType: "json",
					contentType: "application/json",
					url: url,
					data: JSON.stringify(data),
					success: function (data) {
						if (data && data.session) {
							setSessionId(data.session);
						}
					},
					error: function (xhr, status, error) {
						var response = xhr.responseJSON;
						if (response && response.message) {
							alert(response.message);
						}
					}
				});
			}
		});

		function setSessionId(sessionId) {
			if (sessionId) {
				_sessionId = sessionId;
				sessionExpiryDuration = 900;
				var sessionIdRenderBlock = $("#sessionIdBox");
				sessionIdRenderBlock.text("Your session id is " + sessionId);
				startSessionIdExpiryTimer();
				initHubConnection(sessionId)
			}
			else {
				logConnectionStatus("Session Id is not set.");
			}
		}

		function startSessionIdExpiryTimer() {
			$("#expiryTimerBox").text(sessionExpiryDuration + " s");
			sessionExpiryDuration--;
			if (sessionExpiryDuration <= 0) {
				$("#expiryTimerBox").text("Expired!");
				_sessionId = undefined;
				return;
			}
			setTimeout(startSessionIdExpiryTimer, 1000);
		}

		function initHubConnection(sessionId) {
			inTechHubConnection = new signalR.HubConnectionBuilder().withUrl("/ws?sessionId=" + sessionId).build();

			inTechHubConnection.on("Pong", function (connectionId) {
				logConnectionStatus("Your Connection Id is " + connectionId);
			});

			inTechHubConnection.on("ErrorHandler", function (data) {
				if (data && data.message) {
					logConnectionStatus(data.message);
				}
			});

			inTechHubConnection.on("ForceStopConnection", function (data) {
				inTechHubConnection.stop();
				if (data) {
					logConnectionStatus("Connection with " + data + " Id is closed.");
				}
			});

			$("#connectBtn").click(function (e) {
				e.preventDefault();

				if (!_sessionId) {
					logConnectionStatus("Please, At first get Session Id to Connect.");
					return;
				}

				inTechHubConnection.start().then(function () {

					logConnectionStatus("Connected!");
					logConnectionStatus("Your Session Id is " + _sessionId);

					inTechHubConnection.invoke("Ping").catch(function (err) {
					});
				}).catch(function (err) {
				});
			})

			$("#disconnectBtn").click(function (e) {
				e.preventDefault();
				inTechHubConnection.stop();
				logConnectionStatus("Connection is closed.")
			})
		}

		function logConnectionStatus(log) {
			var trackerPanel = $("#connectionTrackerPanel");
			var content = trackerPanel.html();
			trackerPanel.html(content += ('<br />' + log));
		}

	}
}