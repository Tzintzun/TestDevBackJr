CREATE PROCEDURE dbo.ObtenerHorasTotalesUser
	@userId int
AS 
BEGIN
	WITH LogsOrdenados AS (
		SELECT User_id, TipoMov, fecha,
			ROW_NUMBER() OVER (PARTITION BY User_id ORDER BY fecha) AS numeroFila
		FROM ccloglogin
		WHERE User_id = @userId
	),
	logsEmparejados AS (
		SELECT login.User_id, login.fecha as FechaLogin, logout.fecha as FechaLogout, DATEDIFF(HOUR, login.fecha, logout.fecha) as tiempoLog
		FROM LogsOrdenados as login
		Join LogsOrdenados as logout
		ON login.User_id = logout.User_id
		and login.numeroFila + 1 = logout.numeroFila
		and login.TipoMov = 1
		and logout.TipoMov = 0
	)
	SELECT 
		User_id,
		SUM(tiempoLog) as horasTotales
	FROM logsEmparejados
	GROUP BY User_id
END